using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTful_API.Data;
using RESTful_API.Models.Entities;
using Shared.DTOs.Bill;

namespace RESTful_API.Controllers
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
        public IActionResult GetAllBills()
        {
            var bills = _dbContext.Bills
                .Select(b => new
                {
                    b.FacId,
                    b.Total,
                    b.FechaHora,
                    b.Operators.Nombre,
                    b.IdOp
                })
                .ToList();

            return Ok(bills);
        }

        [HttpGet("bills-by-date-range")]
        public IActionResult GetBillsByDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("La fecha de inicio no puede ser mayor que la fecha de fin.");
            }

            // Obtener las facturas dentro del rango
            var bills = _dbContext.Bills
                .Where(b => b.FechaHora >= startDate && b.FechaHora <= endDate)
                .Select(b => new
                {
                    b.FacId,
                    b.FechaHora,
                    b.Total,
                    b.Operators.Nombre,
                    b.IdOp
                })
                .ToList();

            if (!bills.Any())
            {
                return NotFound("No se encontraron facturas en el rango de fechas especificado.");
            }

            return Ok(bills);
        }

        [HttpGet("bills/{id}")]
        public IActionResult GetBillsByOperator(string id)
        {
            // Validar si el operador existe
            var systemOperator = _dbContext.SystemOperators.FirstOrDefault(op => op.Uid == id);
            if (systemOperator == null)
            {
                return NotFound($"No se encontró el operador con ID {id}");
            }

            var bills = _dbContext.Bills
                .Where(b => b.IdOp == id)
                .Select(b => new
                {
                    b.FacId,
                    b.FechaHora,
                    b.Total
                })
                .ToList();

            if (!bills.Any())
            {
                return NotFound($"No se encontraron facturas para el operador con ID {id}");
            }

            return Ok(new
            {
                OperatorName = systemOperator.Nombre,
                Facturas = bills
            });
        }

        [HttpPost]
        public IActionResult CreateBill(BillDto billDto)
        {
            var systemOperator = _dbContext.SystemOperators.FirstOrDefault(o => o.Uid == billDto.IdOp);
            if (systemOperator == null)
            {
                return NotFound($"No se encontró el operador {billDto.IdOp}");
            }

            var bill = new Bill()
            {
                FechaHora = billDto.FechaHora,
                IdOp = billDto.IdOp,
                Total = 0, // Inicialmente sin detalles, el total es 0
                Operators = systemOperator
            };

            _dbContext.Bills.Add(bill);
            _dbContext.SaveChanges();

            return Ok($"Factura creada con ID {bill.FacId}");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBill(int id)
        {
            var bill = _dbContext.Bills.Find(id);
            
            if (bill == null)
            {
                return NotFound($"No se encontró el detalle {id} de factura.");
            }

            var hasDetail = _dbContext.BillDetails.Any(detail => detail.IdFactura == id);

            if (hasDetail)
            { 
                return BadRequest("No se puede eliminar una factura con detalles asociados");
            }

            _dbContext.Bills.Remove(bill);
            _dbContext.SaveChanges();

            return Ok($"Factura {id} eliminado correctamente");
        }
    }
}
