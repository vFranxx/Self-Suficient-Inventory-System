using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTful_API.Data;
using Self_Suficient_Inventory_System.Models.AuditModels;

namespace Self_Suficient_Inventory_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public TestController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("exception")]
        public IActionResult GetException()
        {
            throw new NotImplementedException();
        }

        [HttpGet("audit-results/{auditType}")]
        public async Task<IActionResult> GetAuditResultsByType(string auditType)
        {
            try
            {
                var audits = await _dbContext.Audits
                                             .Where(a => a.AuditType == auditType)
                                             .ToListAsync();

                if (audits == null || !audits.Any())
                {
                    return NotFound($"No audit records found for type: {auditType}");
                }

                return Ok(audits);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("ExceptionLogEntries")]
        public async Task<IActionResult> GetExceptionLogs()
        {
            return Ok(await _dbContext.ExceptionLogEntries.ToListAsync());
        }

        [HttpGet("ResponseEntries")]
        public async Task<IActionResult> GetResponseLogs()
        {
            return Ok(await _dbContext.ResponseLogEntries.ToListAsync());
        }

        [Authorize]
        [HttpGet("sensitive-data")]
        public IActionResult GetSensitiveData()
        {
            return Ok("Información confidencial para admins");
        }
    }
}
