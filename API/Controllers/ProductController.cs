using API.Data;
using API.Models.Entities;
using API.Shared.DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _dbContext.Products.ToListAsync());
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveProducts()
        {
            var activeProducts = _dbContext.Products
                .Where(p => p.FechaBaja == null);

            return Ok(await activeProducts.ToListAsync());
        }

        [HttpGet("inactive")]
        public async Task<IActionResult> GetInactiveProducts()
        {
            var inactiveProducts = _dbContext.Products
                .Where(p => p.FechaBaja != null);

            return Ok(await inactiveProducts.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductDto addProductDto)
        {
            var product = new Product()
            {
                ProdId = addProductDto.ProdId,
                Descripcion = addProductDto.Descripcion,
                PrecioUnitario = addProductDto.PrecioUnitario,
                Ganancia = addProductDto.Ganancia,
                Descuento = addProductDto.Descuento,
                Stock = addProductDto.Stock,
                StockMin = addProductDto.StockMin
            };

            bool exists = await _dbContext.Products.AnyAsync(p => p.ProdId == product.ProdId);
            if (exists)
            {
                return BadRequest($"El producto '{product.ProdId}' ya existe");
            }

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(AddProduct), product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(product);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, UpdateProductDto updateProductoDto)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            product.Descripcion = updateProductoDto.Descripcion;
            product.PrecioUnitario = updateProductoDto.PrecioUnitario;
            product.Ganancia = updateProductoDto.Ganancia;
            product.Descuento = updateProductoDto.Descuento;
            product.StockMin = updateProductoDto.StockMin;

            await _dbContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteProduct(string id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            if (product.FechaBaja != null)
            {
                return BadRequest($"El producto {id} se encuentra dado de baja.");
            }

            product.FechaBaja = DateTime.Now;


            await _dbContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> ActivateProduct(string id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            if (product.FechaBaja == null)
            {
                return BadRequest($"El producto {id} se encuentra dado de baja.");
            }

            product.FechaBaja = null;

            await _dbContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpGet("{productId}/suppliers")]
        public async Task<IActionResult> GetSuppliersByProduct(string productId)
        {
            // Obtener los proveedores asociados al producto desde la tabla intermedia
            var suppliers = await _dbContext.SupplierProducts
                .Where(sp => sp.IdProd == productId)
                .Include(sp => sp.Suppliers) // Incluir los proveedores relacionados
                .Select(sp => new
                {
                    SupplierId = sp.Suppliers.ProvId,
                    SupplierName = sp.Suppliers.Referencia,
                    Contact = sp.Suppliers.Contacto,
                    sp.Suppliers.Mail,
                    sp.Suppliers.Direccion
                })
                .ToListAsync();

            // Verificar si no se encontraron proveedores
            if (!suppliers.Any())
            {
                return NotFound($"No se encontraron proveedores para el producto con ID {productId}");
            }

            return Ok(suppliers);
        }

        [HttpPut("{id}/stock/{quantity}")]
        public async Task<IActionResult> AddStock(string id, int quantity)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            if (quantity < 0)
            {
                return BadRequest("No está permitido restar productos.");
            }

            product.Stock += quantity;

            await _dbContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpPut("remove-from-stock/{id}")]
        public async Task<IActionResult> RemoveFromStock(string id, UpdateProductStockDto updateProductStockDto)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            if (updateProductStockDto.Stock < 0)
            {
                return BadRequest("La cantidad a restar no puede ser negativa.");
            }

            if (product.Stock < updateProductStockDto.Stock)
            {
                return BadRequest("No hay suficiente stock para realizar esta operación.");
            }

            product.Stock -= updateProductStockDto.Stock;

            await _dbContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpPost("product-list")]
        public async Task<IActionResult> InsertProductList(List<AddProductDto> productsDto)
        {
            // Obtengo los productos existentes de la bd
            var existingProductIds = await _dbContext.Products
                .Where(p => productsDto.Select(pd => pd.ProdId).Contains(p.ProdId))
                .Select(p => p.ProdId)
                .ToListAsync();

            var newProducts = new List<Product>();

            foreach (var item in productsDto)
            {
                if (!existingProductIds.Contains(item.ProdId))
                {
                    var newProduct = new Product
                    {
                        ProdId = item.ProdId,
                        Descripcion = item.Descripcion,
                        PrecioUnitario = item.PrecioUnitario,
                        Ganancia = item.Ganancia,
                        Descuento = item.Descuento,
                        Stock = item.Stock,
                        StockMin = item.StockMin
                    };

                    newProducts.Add(newProduct);
                }
            }

            if (newProducts.Any())
            {
                await _dbContext.Products.AddRangeAsync(newProducts);
                await _dbContext.SaveChangesAsync();
            }

            return Ok(newProducts.Count);
        }
    }
}
