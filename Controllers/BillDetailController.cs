using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTful_API.Data;
using RESTful_API.Models.Entities;
using Shared.DTOs.BillDetail;

namespace RESTful_API.Controllers
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

        [HttpGet("{id}")]
        public IActionResult GetDetailsByBillId(int id)
        {
            var bill = _dbContext.Bills.Find(id);
            if (bill == null)
            {
                return NotFound();
            }

            var details = _dbContext.BillDetails
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
                                    .ToList();

            return Ok(details);
        }

        [HttpPost("bill/{id}/details")]
        public IActionResult AddDetailsToBill(List<AddBillDetailDto> detailDto, int id)
        {
            // Validar existencia de la factura
            var bill = _dbContext.Bills.Find(id);
            if (bill == null)
            {
                return NotFound($"No se encontró la factura: {id}");
            }

            foreach (var item in detailDto)
            {
                // Validar existencia del producto
                var product = _dbContext.Products.Find(item.IdProducto);
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
                var subtotal = item.Cantidad * item.Precio;

                // Crear el detalle
                var detail = new BillDetail
                {
                    IdFactura = id,
                    IdProducto = item.IdProducto,
                    Cantidad = item.Cantidad,
                    Precio = item.Precio,
                    Subtotal = subtotal
                };

                // Agregar el detalle directamente al contexto
                _dbContext.BillDetails.Add(detail);

                // Actualizar el total de la factura y reducir el stock del producto
                bill.Total += subtotal;
                product.Stock -= item.Cantidad;

                _dbContext.SaveChanges();
            }

            return Ok($"Detalle añadido correctamente a la factura con ID {id}");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDetail(int id)
        {
            var detail = _dbContext.BillDetails.Find(id);
            if (detail == null)
            {
                return NotFound($"No se encontró el detalle {id} de factura.");
            }

            var product = _dbContext.Products.Find(detail.IdProducto);
            if (product != null)
            {
                product.Stock += detail.Cantidad;
            }

            var bill = _dbContext.Bills.Find(detail.IdFactura);
            if (bill != null)
            {
                bill.Total -= detail.Subtotal;
            }

            _dbContext.BillDetails.Remove(detail);
            _dbContext.SaveChanges();

            return Ok($"Detalle {id} eliminado correctamente");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDetail(int id, UpdateBillDetailDto updateDto)
        {
            var detail = _dbContext.BillDetails.Find(id);
            if (detail == null)
            {
                return NotFound();
            }

            var product = _dbContext.Products.Find(detail.IdProducto);
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

            var bill = _dbContext.Bills.Find(detail.IdFactura);
            if (bill != null)
            {
                bill.Total += (nuevoSubtotal - detail.Subtotal);
            }

            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
