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

        [HttpGet("Exception")]
        public IActionResult GetException()
        {
            throw new NotImplementedException();
        }

        [HttpGet("ProductAudit-results")]
        public async Task<IActionResult> GetProductResults()
        {
            return Ok(await _dbContext.ProductAudits.ToListAsync());
        }

        [HttpGet("SupplierAudit-results")]
        public async Task<IActionResult> GetSupplierResults()
        {
            return Ok(await _dbContext.SupplierAudits.ToListAsync());
        }

        [HttpGet("SystemOperatorAudit-results")]
        public async Task<IActionResult> GetSystemOperatorResults()
        {
            return Ok(await _dbContext.SystemOperatorAudits.ToListAsync());
        }

        [HttpGet("BillAudit-results")]
        public async Task<IActionResult> GetBillResults()
        {
            return Ok(await _dbContext.BillAudits.ToListAsync());
        }

        [HttpGet("BillDetailAudit-results")]
        public async Task<IActionResult> GetBillDetailResults()
        {
            return Ok(await _dbContext.BillDetails.ToListAsync());
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
    }
}
