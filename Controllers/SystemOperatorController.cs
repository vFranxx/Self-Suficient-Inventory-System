using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public SystemOperatorController(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllOperators()
        {
            return Ok(_dbContext.SystemOperators.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetOperatorById(string id)
        {
            var systemOperator = _dbContext.SystemOperators.Find(id);

            if (systemOperator == null)
            {
                return NotFound();
            }

            return Ok(systemOperator);
        }

        [HttpPost]
        public IActionResult AddOperator(AddOperatorDto addOperatorDto)
        {
            var systemOperator = new SystemOperator()
            { 
                Uid = addOperatorDto.Uid,
                Nombre = addOperatorDto.Nombre,
                Tipo = addOperatorDto.Tipo,
                Pswd = addOperatorDto.Pswd,
            };

            _dbContext.SystemOperators.Add(systemOperator);
            _dbContext.SaveChanges();

            return Ok(systemOperator);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOperator(string id, UpdateOperatorDto updateOperatorDto)
        {
            var systemOperator = _dbContext.SystemOperators.Find(id);

            if (systemOperator == null)
            {
                return NotFound();
            }

            systemOperator.Nombre = updateOperatorDto.Nombre;
            systemOperator.Pswd = updateOperatorDto.Pswd;

            _dbContext.SaveChanges();

            return Ok(systemOperator);
        }

        [HttpDelete("{id}")]
        public IActionResult SoftDeleteOperator(string id)
        {
            var systemOperator = _dbContext.SystemOperators.Find(id);

            if (systemOperator == null)
            {
                return NotFound();
            }

            systemOperator.FechaBaja = DateTime.Now;

            _dbContext.SystemOperators.Update(systemOperator);
            _dbContext.SaveChanges();

            return Ok(systemOperator);
        }

        [HttpPost("{id}")]
        public IActionResult ActivateOperator(string id)
        {
            var systemOperator = _dbContext.SystemOperators.Find(id);

            if (systemOperator == null)
            {
                return NotFound();
            }

            systemOperator.FechaBaja = null;

            _dbContext.SystemOperators.Update(systemOperator);
            _dbContext.SaveChanges();

            return Ok(systemOperator);
        }

        [HttpPut("{id}/admin")]
        public IActionResult ChangeOperatorToAdmin(string id)
        {
            var systemOperator = _dbContext.SystemOperators.Find(id);

            if (systemOperator == null)
            {
                return NotFound();
            }

            systemOperator.Tipo = true;

            _dbContext.SystemOperators.Update(systemOperator);
            _dbContext.SaveChanges();

            return Ok(systemOperator);
        }

        [HttpPut("{id}/operator")]
        public IActionResult ChangeAdminToOperator(string id)
        {
            var systemOperator = _dbContext.SystemOperators.Find(id);

            if (systemOperator == null)
            {
                return NotFound();
            }

            systemOperator.Tipo = false;

            _dbContext.SystemOperators.Update(systemOperator);
            _dbContext.SaveChanges();

            return Ok(systemOperator);
        }
    }
}
