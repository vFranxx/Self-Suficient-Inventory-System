using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTful_API.Data;
using RESTful_API.Models.Entities;
using Shared.DTOs.SystemOperator;

namespace RESTful_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemOperatorController : ControllerBase
    {
        
        private readonly AppDbContext _dbContext;
        private readonly UserManager<SystemOperator> _userManager;
        public SystemOperatorController(AppDbContext dbContext, UserManager<SystemOperator> userManager) 
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOperators()
        {
            var users = await _userManager.Users.ToListAsync();
            var operators = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                operators.Add(new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.PhoneNumber,
                    user.FechaBaja,
                    Roles = roles
                });
            }

            return Ok(operators);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOperatorById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var operatorData = new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.PhoneNumber,
                user.FechaBaja,
                Roles = roles
            };

            return Ok(operatorData);
        }

        [HttpPost]
        public async Task<IActionResult> AddOperator(AddOperatorDto addOperatorDto)
        {
            var exists = await _userManager.FindByEmailAsync(addOperatorDto.Email);
            if (exists != null)
            {
                return BadRequest($"El usuario con el email '{addOperatorDto.Email}' ya existe.");
            }

            var newOperator = new SystemOperator
            {
                UserName = addOperatorDto.UserName ?? addOperatorDto.Email,
                Email = addOperatorDto.Email,
                PasswordHash = addOperatorDto.Password,
                PhoneNumber = addOperatorDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(newOperator, addOperatorDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(newOperator, "OPERADOR");

            return Ok(newOperator);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOperator(string id, UpdateOperatorDto updateOperatorDto)
        {
            var systemOperator = await _userManager.FindByIdAsync(id);

            if (systemOperator == null)
            {
                return NotFound();
            }

            systemOperator.UserName = updateOperatorDto.UserName;
            systemOperator.Email = updateOperatorDto.Email;
            systemOperator.PasswordHash = updateOperatorDto.Password;
            systemOperator.PhoneNumber = updateOperatorDto.PhoneNumber;

            await _dbContext.SaveChangesAsync();

            return Ok(systemOperator);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteOperator(string id)
        {
            var systemOperator = await _userManager.FindByIdAsync(id);

            if (systemOperator == null)
            {
                return NotFound();
            }

            if (systemOperator.FechaBaja != null) 
            {
                return BadRequest($"El usuario {id} ya está dado de baja.");
            }

            systemOperator.FechaBaja = DateTime.Now;

            _dbContext.SystemOperators.Update(systemOperator);
            await _dbContext.SaveChangesAsync();

            return Ok(systemOperator);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> ActivateOperator(string id)
        {
            var systemOperator = await _userManager.FindByIdAsync(id);

            if (systemOperator == null)
            {
                return NotFound();
            }

            if (systemOperator.FechaBaja == null) 
            {
                return BadRequest($"El usuario {id} ya está dado de alta");
            }

            systemOperator.FechaBaja = null;

            _dbContext.SystemOperators.Update(systemOperator);
            await _dbContext.SaveChangesAsync();

            return Ok(systemOperator);
        }
    }
}
