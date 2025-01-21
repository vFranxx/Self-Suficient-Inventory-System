using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTful_API.Data;
using RESTful_API.Models.Entities;
using Shared.DTOs.Product;

namespace RESTful_API.Controllers
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

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return Ok(_dbContext.Products.ToList());
        }

        [HttpPost]
        public IActionResult AddProduct(AddProductDto addProductDto)
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

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(AddProduct), product);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(string id)
        {
            var product = _dbContext.Products.Find(id);

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
        public IActionResult UpdateProduct(string id, UpdateProductDto updateProductoDto)
        {
            var product = _dbContext.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            product.Descripcion = updateProductoDto.Descripcion;
            product.PrecioUnitario = updateProductoDto.PrecioUnitario;
            product.Ganancia = updateProductoDto.Ganancia;
            product.Descuento = updateProductoDto.Descuento;
            product.StockMin = updateProductoDto.StockMin;

            _dbContext.SaveChanges();

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public IActionResult SoftDeleteProduct(string id)
        {
            var product = _dbContext.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            product.FechaBaja = DateTime.Now;

            _dbContext.Products.Update(product);

            _dbContext.SaveChanges();

            return Ok(product);
        }

        [HttpPost("{id}")]
        public IActionResult ActivateProduct(string id)
        {
            var product = _dbContext.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            product.FechaBaja = null;

            _dbContext.Products.Update(product);

            _dbContext.SaveChanges();

            return Ok(product);
        }

        [HttpGet("{productId}/suppliers")]
        public IActionResult GetSuppliersByProduct(string productId)
        {
            // Obtener los proveedores asociados al producto desde la tabla intermedia
            var suppliers = _dbContext.SupplierProducts
                .Where(sp => sp.IdProd == productId)
                .Include(sp => sp.Suppliers) // Incluir los proveedores relacionados
                .Select(sp => new
                {
                    SupplierId = sp.Suppliers.ProvId,
                    SupplierName = sp.Suppliers.Referencia,
                    Contact = sp.Suppliers.Contacto,
                    Mail = sp.Suppliers.Mail,
                    Direccion = sp.Suppliers.Direccion
                })
                .ToList();

            // Verificar si no se encontraron proveedores
            if (!suppliers.Any())
            {
                return NotFound($"No se encontraron proveedores para el producto con ID {productId}");
            }

            return Ok(suppliers);
        }

        [HttpPut("{id}/stock/{quantity}")]
        public IActionResult AddStock(string id, int quantity)
        {
            var product = _dbContext.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            if (quantity < 0)
            {
                return BadRequest("No está permitido restar productos.");
            }
            
            product.Stock = quantity;

            _dbContext.SaveChanges();

            return Ok(product);
        }

        [HttpPut("remove-from-stock/{id}")]
        public IActionResult RemoveFromStock(string id, UpdateProductStockDto updateProductStockDto)
        {
            var product = _dbContext.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            product.Stock = updateProductStockDto.Stock;

            _dbContext.SaveChanges();

            return Ok(product);
        }

        [HttpPost("product-list")]
        public IActionResult InsertProductList(List<AddProductDto> productsDto)
        {
            foreach (var item in productsDto) 
            {
                // Validar existencia del producto
                var product = _dbContext.Products.Find(item.ProdId);

                if (product == null)
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

                    _dbContext.Products.Add(newProduct);
                }
            }

            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
