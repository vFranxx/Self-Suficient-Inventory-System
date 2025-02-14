using API.Data;
using API.Models.Entities;
using API.Shared.DTOs.Bill;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public BillController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBills()
        {
            var bills = await _dbContext.Bills
                .Select(b => new
                {
                    b.FacId,
                    b.Total,
                    b.FechaHora,
                    //b.Operators.Nombre,
                    b.IdOp
                })
                .ToListAsync();

            return Ok(bills);
        }

        [HttpGet("bills-by-date-range")]
        public async Task<IActionResult> GetBillsByDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate.Date > endDate.Date)
            {
                return BadRequest("La fecha de inicio no puede ser mayor que la fecha de fin.");
            }

            // Obtener las facturas dentro del rango
            var bills = await _dbContext.Bills
                .Where(b => b.FechaHora.Date >= startDate && b.FechaHora.Date <= endDate)
                .Select(b => new
                {
                    b.FacId,
                    b.FechaHora,
                    b.Total,
                    //b.Operators.Nombre,
                    b.IdOp
                })
                .ToListAsync();

            if (!bills.Any())
            {
                return NotFound("No se encontraron facturas en el rango de fechas especificado.");
            }

            return Ok(bills);
        }

        [HttpGet("bill/{id}")]
        public async Task<IActionResult> GetBillsById(int id)
        {
            var bill = await _dbContext.Bills.FindAsync(id);

            if (bill == null)
            {
                return NotFound();
            }

            return Ok(bill);
        }

        [HttpGet("bills-operator/{id}")]
        public async Task<IActionResult> GetBillsByOperator(string id)
        {
            // Validar si el operador existe
            //var systemOperator = await _dbContext.SystemOperators.FirstOrDefaultAsync(op => op.Uid == id);
            //if (systemOperator == null)
            //{
            //    return NotFound($"No se encontró el operador con ID {id}");
            //}

            var bills = await _dbContext.Bills
                .Where(b => b.IdOp == id)
                .Select(b => new
                {
                    b.FacId,
                    b.FechaHora,
                    b.Total
                })
                .ToListAsync();

            if (!bills.Any())
            {
                return NotFound($"No se encontraron facturas para el operador con ID {id}");
            }

            return Ok(new
            {
                //OperatorName = systemOperator.Nombre,
                Facturas = bills
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateBill(BillDto billDto)
        {
            //var systemOperator = await _dbContext.SystemOperators.FirstOrDefaultAsync(o => o.Uid == billDto.IdOp);
            //if (systemOperator == null)
            //{
            //    return NotFound($"No se encontró el operador {billDto.IdOp}");
            //}

            var bill = new Bill()
            {
                FechaHora = billDto.FechaHora,
                IdOp = billDto.IdOp,
                Total = 0, // Inicialmente sin detalles, el total es 0
                //Operators = systemOperatorW
            };

            if (await _dbContext.Bills.AnyAsync())
            {
                bill.FacId = await _dbContext.Bills.MaxAsync(b => b.FacId) + 1;
            }
            else
            {
                bill.FacId = 1;
            }

            await _dbContext.Bills.AddAsync(bill);
            await _dbContext.SaveChangesAsync();

            return Ok($"Factura creada con ID {bill.FacId}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var bill = await _dbContext.Bills.FindAsync(id);

            if (bill == null)
            {
                return NotFound($"No se encontró el detalle {id} de factura.");
            }

            _dbContext.Bills.Remove(bill);
            await _dbContext.SaveChangesAsync();

            return Ok($"Factura {id} eliminado correctamente");
        }
    }
}
