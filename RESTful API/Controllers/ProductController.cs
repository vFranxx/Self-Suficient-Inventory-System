using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                PrecioUnitario = addProductDto.PrecioUnitario
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

            _dbContext.SaveChanges();

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(string id)
        {
            var product = _dbContext.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
