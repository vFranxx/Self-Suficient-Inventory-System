using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
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

        [HttpGet("ProductAudit-entries")]
        public async Task<IActionResult> GetProductEntries()
        {
            return Ok(await _dbContext.ProductAudits.ToListAsync());
        }

        [HttpGet("SupplierAudit-entries")]
        public async Task<IActionResult> GetSupplierEntries()
        {
            return Ok(await _dbContext.SupplierAudits.ToListAsync());
        }

        [HttpGet("BillAudit-entries")]
        public async Task<IActionResult> GetBillEntries()
        {
            return Ok(await _dbContext.BillAudits.ToListAsync());
        }
        [HttpGet("BillDetailAudit-entries")]
        public async Task<IActionResult> GetBillDetailEntries()
        {
            return Ok(await _dbContext.BillDetailAudits.ToListAsync());
        }

        [HttpGet("OrderAudit-entries")]
        public async Task<IActionResult> GetOrderEntries()
        {
            return Ok(await _dbContext.OrderAudits.ToListAsync());
        }
        [HttpGet("OrderDetailAudit-entries")]
        public async Task<IActionResult> GetOrderDetailEntries()
        {
            return Ok(await _dbContext.OrderDetailAudits.ToListAsync());
        }
    }
}
