using API.Data;
using API.Models.Entities;
using API.Shared.DTOs.Supplier;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public SupplierController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            return Ok(await _dbContext.Suppliers.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplier(AddSupplierDto addSupplierDto)
        {
            var supplier = new Supplier()
            {
                Referencia = addSupplierDto.Referencia,
                Contacto = addSupplierDto.Contacto,
                Mail = addSupplierDto.Mail,
                Direccion = addSupplierDto.Direccion
            };

            await _dbContext.Suppliers.AddAsync(supplier);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(AddSupplier), supplier);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            var supplier = await _dbContext.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(supplier);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, UpdateSupplierDto updateSupplierDto)
        {
            var supplier = await _dbContext.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            supplier.Referencia = updateSupplierDto.Referencia;
            supplier.Contacto = updateSupplierDto.Contacto;
            supplier.Mail = updateSupplierDto.Mail;
            supplier.Direccion = updateSupplierDto.Direccion;

            await _dbContext.SaveChangesAsync();

            return Ok(supplier);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplierById(int id)
        {
            var supplier = await _dbContext.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            _dbContext.Suppliers.Remove(supplier);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{supplierId}/products")]
        public async Task<IActionResult> GetProductsBySupplier(int supplierId)
        {
            var products = await _dbContext.SupplierProducts
                .Where(sp => sp.IdProv == supplierId)
                .Include(p => p.Products)
                .Select(sp => new
                {
                    ProductId = sp.Products.ProdId,
                    Description = sp.Products.Descripcion
                })
                .ToListAsync();

            if (!products.Any())
            {
                return NotFound($"No se encontraron productos para el proveedor con ID {supplierId}");
            }

            return Ok(products);
        }

        [HttpPost("supplier-list")]
        public async Task<IActionResult> InsertSupplierList(List<AddSupplierDto> supplierDto)
        {
            foreach (var item in supplierDto)
            {
                // Validar existencia del proveedor
                var supplier = await _dbContext.Suppliers.FindAsync(item);

                if (supplier == null)
                {
                    var newSupplier = new Supplier
                    {
                        Referencia = item.Referencia,
                        Contacto = item.Contacto,
                        Direccion = item.Direccion,
                        Mail = item.Mail
                    };

                    await _dbContext.Suppliers.FindAsync(newSupplier);
                }
            }

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}