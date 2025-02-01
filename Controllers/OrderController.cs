using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTful_API.Data;
using RESTful_API.Models.Entities;
using Self_Suficient_Inventory_System.Shared.DTOs.Order;

namespace Self_Suficient_Inventory_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public OrderController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _dbContext.Orders
                .Select(o => new
                {
                    o.OcId,
                    o.FechaSolicitud,
                    o.Estado,
                    o.Suppliers.Referencia,
                    o.Operators.Nombre,
                    o.IdOp
                })
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("orders-by-date-range")]
        public async Task<IActionResult> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate.Date > endDate.Date)
            {
                return BadRequest("La fecha de inicio no puede ser mayor que la fecha de fin.");
            }

            // Obtener las facturas dentro del rango
            var orders = await _dbContext.Orders
                .Where(o => o.FechaSolicitud.Date >= startDate && o.FechaSolicitud.Date <= endDate)
                .Select(o => new
                {
                    o.OcId,
                    o.FechaSolicitud,
                    o.Estado,
                    o.Suppliers.Referencia,
                    o.Operators.Nombre,
                    o.IdOp
                })
                .ToListAsync();

            if (!orders.Any())
            {
                return NotFound("No se encontraron pedidos de compras en el rango de fechas especificado.");
            }

            return Ok(orders);
        }

        [HttpGet("order/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(AddOrderDTO orderDto)
        {
            var systemOperator = await _dbContext.SystemOperators.AnyAsync(o => o.Uid == orderDto.IdOp);
            if (!systemOperator)
            {
                return NotFound($"No se encontró el operador {orderDto.IdOp}");
            }

            var supplier = await _dbContext.Suppliers.AnyAsync(s => s.ProvId == orderDto.IdProv);
            if (!supplier)
            {
                return NotFound($"No se encontró el proveedor correspondiente {orderDto.IdOp}");
            }

            var order = new Order()
            {
                IdOp = orderDto.IdOp,
                IdProv = orderDto.IdProv
            };

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return Ok($"Pedido de compra creado con ID {order.OcId}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound($"No se encontró el pedido de compra {id}.");
            }

            if (order.Estado == "FINALIZADO") 
            {
                return BadRequest("No se pueden eliminar pedidos de compra concretados");
            }

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return Ok($"Orden de compra {id} eliminada correctamente");
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> ChangeOrderStatus(int id, string status)
        {
            if (!IsStatusValid(status)) 
            {
                return BadRequest($"El estado '{status}' no es válido.");
            }

            var order = await _dbContext.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound($"No se encontró el pedido de compra {id}");
            }

            order.Estado = status;

            if (status == "FINALIZADO")
            {
                // Actualizar stock
            }

            await _dbContext.SaveChangesAsync();

            return Ok($"Estado de la orden de compra {id} actualizado a {status}");
        }

        private bool IsStatusValid(string status)
        {
            var estadosPermitidos = new List<string> { "PENDIENTE", "EN PROCESO", "CANCELADO", "FINALIZADO" };

            return estadosPermitidos.Contains(status.ToUpper());
        }
    }
}
