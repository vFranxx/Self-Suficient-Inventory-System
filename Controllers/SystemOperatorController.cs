using API.Data;
using API.Models.Entities;
using API.Shared.DTOs.SystemOperator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "ADMIN")]
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

        [HttpPut("{id}")]
        [Authorize(Roles = "OPERADOR, ADMIN")]
        public async Task<IActionResult> UpdateOperator(string id, UpdateOperatorDto updateOperatorDto)
        {
            // Verificaciones
            if (User.IsInRole("OPERADOR") && User.FindFirst(ClaimTypes.NameIdentifier)?.Value != id)
            {
                return Forbid(); // Operador solo puede modificarse a sí mismo
            }

            var systemOperator = await _userManager.FindByIdAsync(id);

            if (systemOperator == null)
            {
                return NotFound("Usuario no encontrado");
            }

            if (systemOperator.UserName != updateOperatorDto.UserName)
            {
                var existingUser = await _userManager.FindByNameAsync(updateOperatorDto.UserName);
                if (existingUser != null && existingUser.Id != id)
                {
                    return BadRequest($"El nombre de usuario '{updateOperatorDto.UserName}' ya está en uso.");
                }
            }

            if (systemOperator.Email != updateOperatorDto.Email)
            {
                var existingEmail = await _userManager.FindByEmailAsync(updateOperatorDto.Email);
                if (existingEmail != null && existingEmail.Id != id)
                {
                    return BadRequest($"El email '{updateOperatorDto.Email}' ya está en uso.");
                }
            }

            // Actualizar campos básicos
            systemOperator.UserName = updateOperatorDto.UserName;
            systemOperator.PhoneNumber = updateOperatorDto.PhoneNumber;

            // Actualizar email (requiere validación)
            var emailResult = await _userManager.SetEmailAsync(systemOperator, updateOperatorDto.Email);
            if (!emailResult.Succeeded)
            {
                return BadRequest(emailResult.Errors);
            }

            // Actualizar contraseña (solo si se proporciona)
            if (!string.IsNullOrEmpty(updateOperatorDto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(systemOperator);
                var passwordResult = await _userManager.ResetPasswordAsync(systemOperator, token, updateOperatorDto.Password);

                if (!passwordResult.Succeeded)
                {
                    return BadRequest(passwordResult.Errors);
                }
            }

            // Guardar cambios
            var result = await _userManager.UpdateAsync(systemOperator);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

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
