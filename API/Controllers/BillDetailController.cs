using API.Data;
using API.Models.Entities;
using API.Shared.DTOs.BillDetail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillDetailController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public BillDetailController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDetails()
        {
            return Ok(await _dbContext.BillDetails.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailsByBillId(int id)
        {
            var bill = await _dbContext.Bills.FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }

            var details = await _dbContext.BillDetails
                                    .Where(bd => bd.IdFactura == bill.FacId)
                                    .Select(d => new
                                    {
                                        d.FacDetId,
                                        d.Cantidad,
                                        d.Subtotal,
                                        d.Productos.ProdId,
                                        d.Productos.Descripcion,
                                        d.Productos.PrecioUnitario
                                    })
                                    .ToListAsync();

            return Ok(details);
        }

        [HttpPost("bill/{id}/details")]
        public async Task<IActionResult> AddDetailsToBill(List<AddBillDetailDto> detailDto, int id)
        {
            // Validar existencia de la factura
            var bill = await _dbContext.Bills.FindAsync(id);
            if (bill == null)
            {
                return NotFound($"No se encontró la factura: {id}");
            }

            foreach (var item in detailDto)
            {
                // Validar existencia del producto
                var product = await _dbContext.Products.FindAsync(item.IdProducto);
                if (product == null)
                {
                    return NotFound($"No se encontró el producto con ID {item.IdProducto}");
                }

                // Validar stock disponible
                if (product.Stock < item.Cantidad)
                {
                    return BadRequest($"Stock insuficiente para el producto con ID {item.IdProducto}");
                }

                // Calcular subtotal
                var subtotal = item.Cantidad * product.PrecioUnitario;

                // Crear el detalle
                var detail = new BillDetail
                {
                    IdFactura = id,
                    IdProducto = product.ProdId,
                    Cantidad = item.Cantidad,
                    Precio = product.PrecioUnitario,
                    Subtotal = subtotal
                };

                if (await _dbContext.BillDetails.AnyAsync())
                {
                    detail.FacDetId = await _dbContext.BillDetails.MaxAsync(d => d.FacDetId) + 1;
                }
                else
                {
                    detail.FacDetId = 1;
                }

                // Agregar el detalle directamente al contexto
                await _dbContext.BillDetails.AddAsync(detail);

                // Actualizar el total de la factura y reducir el stock del producto
                bill.Total += subtotal;
                product.Stock -= item.Cantidad;

                await _dbContext.SaveChangesAsync();
            }

            return Ok($"Detalle añadido correctamente a la factura con ID {id}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetail(int id)
        {
            var detail = await _dbContext.BillDetails.FindAsync(id);
            if (detail == null)
            {
                return NotFound($"No se encontró el detalle {id} de factura.");
            }

            var product = await _dbContext.Products.FindAsync(detail.IdProducto);
            if (product != null)
            {
                product.Stock += detail.Cantidad;
            }

            var bill = await _dbContext.Bills.FindAsync(detail.IdFactura);
            if (bill != null)
            {
                bill.Total -= detail.Subtotal;
            }

            _dbContext.BillDetails.Remove(detail);
            await _dbContext.SaveChangesAsync();

            return Ok($"Detalle {id} eliminado correctamente");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDetail(int id, UpdateBillDetailDto updateDto)
        {
            var detail = await _dbContext.BillDetails.FindAsync(id);
            if (detail == null)
            {
                return NotFound();
            }

            var product = await _dbContext.Products.FindAsync(detail.IdProducto);
            if (product == null)
            {
                return NotFound($"No se encontró el producto con ID {detail.IdProducto}");
            }

            int stockAdjustment = detail.Cantidad - updateDto.Cantidad;
            if (product.Stock + stockAdjustment < 0)
            {
                return BadRequest($"Stock insuficiente. Stock disponible: {product.Stock}");
            }

            product.Stock += stockAdjustment;

            // Actualizar la cantidad y subtotal del detalle
            var nuevoSubtotal = updateDto.Cantidad * product.PrecioUnitario;
            detail.Cantidad = updateDto.Cantidad;
            detail.Subtotal = nuevoSubtotal;

            var bill = await _dbContext.Bills.FindAsync(detail.IdFactura);
            if (bill != null)
            {
                bill.Total += nuevoSubtotal - detail.Subtotal;
            }

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
