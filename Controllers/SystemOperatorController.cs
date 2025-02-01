using Microsoft.AspNetCore.Http;
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
        /*
        private readonly AppDbContext _dbContext;
        public SystemOperatorController(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOperators()
        {
            return Ok(await _dbContext.SystemOperators.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOperatorById(string id)
        {
            var systemOperator = await _dbContext.SystemOperators.FindAsync(id);

            if (systemOperator == null)
            {
                return NotFound();
            }

            return Ok(systemOperator);
        }

        [HttpPost]
        public async Task<IActionResult> AddOperator(AddOperatorDto addOperatorDto)
        {
            var systemOperator = new SystemOperator()
            { 
                Uid = addOperatorDto.Uid,
                Nombre = addOperatorDto.Nombre,
                Tipo = addOperatorDto.Tipo,
                Pswd = addOperatorDto.Pswd,
            };

            bool exists = await _dbContext.SystemOperators.AnyAsync(so => so.Uid == systemOperator.Uid);
            if (exists)
            {
                return BadRequest($"Un usuario con el identificador '{systemOperator.Uid}' ya existe");
            }

            await _dbContext.SystemOperators.AddAsync(systemOperator);
            await _dbContext.SaveChangesAsync();

            return Ok(systemOperator);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOperator(string id, UpdateOperatorDto updateOperatorDto)
        {
            var systemOperator = await _dbContext.SystemOperators.FindAsync(id);

            if (systemOperator == null)
            {
                return NotFound();
            }

            systemOperator.Nombre = updateOperatorDto.Nombre;
            systemOperator.Pswd = updateOperatorDto.Pswd;

            await _dbContext.SaveChangesAsync();

            return Ok(systemOperator);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteOperator(string id)
        {
            var systemOperator = await _dbContext.SystemOperators.FindAsync(id);

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
            var systemOperator = await _dbContext.SystemOperators.FindAsync(id);

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

        [HttpPut("{id}/admin")]
        public async Task<IActionResult> ChangeOperatorToAdmin(string id)
        {
            var systemOperator = await _dbContext.SystemOperators.FindAsync(id);

            if (systemOperator == null)
            {
                return NotFound();
            }

            if (systemOperator.Tipo) 
            {
                return BadRequest($"El usuario {id} ya tiene permisos de administrador.");
            }

            systemOperator.Tipo = true;

            _dbContext.SystemOperators.Update(systemOperator);
            await _dbContext.SaveChangesAsync();

            return Ok(systemOperator);
        }

        [HttpPut("{id}/operator")]
        public async Task<IActionResult> ChangeAdminToOperator(string id)
        {
            var systemOperator = await _dbContext.SystemOperators.FindAsync(id);

            if (systemOperator == null)
            {
                return NotFound();
            }

            if (!systemOperator.Tipo)
            {
                return BadRequest($"El usuario {id} no tiene permisos de administrador.");
            }

            systemOperator.Tipo = false;

            _dbContext.SystemOperators.Update(systemOperator);
            await _dbContext.SaveChangesAsync();

            return Ok(systemOperator);
        }
    */
    }
}
