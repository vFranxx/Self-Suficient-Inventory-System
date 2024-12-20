using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTful_API.Data;

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
    }
}
