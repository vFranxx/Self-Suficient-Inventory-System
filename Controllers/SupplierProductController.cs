using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using RESTful_API.Models.Entities;
using Self_Suficient_Inventory_System.Data;
using Shared.DTOs.SupplierProduct;

namespace Self_Suficient_Inventory_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierProductController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public SupplierProductController(AppDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        private bool SupplierExists(int id)
        {
            return _dbContext.Suppliers.Any(s => s.ProvId == id);
        }

        private bool ProductExists(string id)
        {
            return _dbContext.Products.Any(p => p.ProdId == id);
        }

        private bool RelationDoesNotExist(int supplierId, string productId)
        {
            return !_dbContext.SupplierProducts.Any(sp => sp.IdProv == supplierId && sp.IdProd == productId);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _dbContext.SupplierProducts.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToSupplier(SupplierProductDto supplierProductDto)
        {
            if (!SupplierExists(supplierProductDto.IdProv))
            {
                return NotFound($"No se ha encontrado el proveedor {supplierProductDto.IdProv}");
            }

            if (!ProductExists(supplierProductDto.IdProd))
            {
                return NotFound($"No se ha encontrado el producto {supplierProductDto.IdProd}");
            }

            if (!RelationDoesNotExist(supplierProductDto.IdProv, supplierProductDto.IdProd))
            {
                return Conflict($"El producto {supplierProductDto.IdProd} ya está asociado con el proveedor {supplierProductDto.IdProv}");
            }

            var supplierProduct = new SupplierProduct
            {
                IdProv = supplierProductDto.IdProv,
                IdProd = supplierProductDto.IdProd
            };

            await _dbContext.SupplierProducts.AddAsync(supplierProduct);
            await _dbContext.SaveChangesAsync();

            return Ok($"Producto {supplierProductDto.IdProd} ha sido añadido correctamente al Proveedor {supplierProductDto.IdProv}.");
        }

        [HttpDelete("{supplierId}/product/{productId}")]
        public async Task<IActionResult> DeleteSupplierProduct(int supplierId, string productId)
        {
            if (RelationDoesNotExist(supplierId, productId))
            {
                return NotFound($"No se encontró la relación entre el proveedor {supplierId} y el producto {productId}.");
            }

            var relation = await _dbContext.SupplierProducts.FirstOrDefaultAsync(sp => sp.IdProv == supplierId && sp.IdProd == productId);

            _dbContext.SupplierProducts.Remove(relation);
            await _dbContext.SaveChangesAsync();

            return Ok($"La relación entre el proveedor {supplierId} y el producto {productId} fue eliminada correctamente.");
        }

    }
}
