using API.Data;
using API.Models.Entities;
using API.Shared.DTOs.OrderDetail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public OrderDetailController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDetails()
        {
            return Ok(await _dbContext.OrderDetails.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailsByOrderId(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var details = await _dbContext.OrderDetails
                                    .Where(od => od.IdOc == order.OcId)
                                    .Select(d => new
                                    {
                                        d.DetOcId,
                                        d.Cantidad,
                                        d.IdOc,
                                        d.Products.ProdId,
                                        d.Products.Descripcion,
                                        d.Products.PrecioUnitario
                                    })
                                    .ToListAsync();

            return Ok(details);
        }

        [HttpPost("order/{id}/details")]
        public async Task<IActionResult> AddDetailsToOrder(List<AddOrderDetailDTO> detailDto, int id)
        {
            // Validar existencia de la factura
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound($"No se encontró el pedido de compra: {id}");
            }

            foreach (var item in detailDto)
            {
                // Validar existencia del producto
                var product = await _dbContext.Products.FindAsync(item.IdProd);
                if (product == null)
                {
                    return NotFound($"No se encontró el producto con ID {item.IdProd}");
                }

                // Crear el detalle
                var detail = new OrderDetail
                {
                    Cantidad = item.Cantidad,
                    IdProd = item.IdProd,
                    IdOc = item.IdOc
                };

                // Agregar el detalle directamente al contexto
                await _dbContext.OrderDetails.AddAsync(detail);

                await _dbContext.SaveChangesAsync();
            }

            return Ok($"Detalle añadido correctamente al pedido de compra con ID {id}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetail(int id)
        {
            var detail = await _dbContext.OrderDetails.FindAsync(id);
            if (detail == null)
            {
                return NotFound($"No se encontró el detalle {id} del peiddo de compra.");
            }

            _dbContext.OrderDetails.Remove(detail);
            await _dbContext.SaveChangesAsync();

            return Ok($"Detalle {id} eliminado correctamente");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDetail(int id, int units)
        {
            var detail = await _dbContext.OrderDetails.FindAsync(id);
            if (detail == null)
            {
                return NotFound($"No se encontró el detalle {id} del pedido de compra.");
            }

            // Actualizar los campos necesarios
            detail.Cantidad = units;

            _dbContext.OrderDetails.Update(detail);
            await _dbContext.SaveChangesAsync();

            return Ok($"Detalle {id} actualizado correctamente");
        }
    }
}