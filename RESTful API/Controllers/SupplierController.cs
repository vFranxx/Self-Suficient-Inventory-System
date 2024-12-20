using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTful_API.Data;
using RESTful_API.Models.Entities;
using Shared.DTOs.Supplier;

namespace RESTful_API.Controllers
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
        public IActionResult GetAllSuppliers()
        {
            return Ok(_dbContext.Suppliers.ToList());
        }

        [HttpPost]
        public IActionResult AddSupplier(AddSupplierDto addSupplierDto)
        {
            var supplier = new Supplier()
            { 
                Referencia = addSupplierDto.Referencia,
                Contacto = addSupplierDto.Contacto,
                Mail = addSupplierDto.Mail,
                Direccion = addSupplierDto.Direccion
            };

            _dbContext.Suppliers.Add(supplier);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(AddSupplier), supplier);
        }

        [HttpGet("{id}")]
        public IActionResult GetSupplierById(int id)
        {
            var supplier = _dbContext.Suppliers.Find(id);

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
        public IActionResult UpdateSupplier(int id, UpdateSupplierDto updateSupplierDto)
        {
            var supplier = _dbContext.Suppliers.Find(id);

            if (supplier == null)
            {
                return NotFound();
            }

            supplier.Referencia = updateSupplierDto.Referencia;
            supplier.Contacto = updateSupplierDto.Contacto;
            supplier.Mail = updateSupplierDto.Mail;
            supplier.Direccion = updateSupplierDto.Direccion;
            
            _dbContext.SaveChanges();

            return Ok(supplier);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSupplierById(int id)
        {
            var supplier = _dbContext.Suppliers.Find(id);

            if (supplier == null)
            {
                return NotFound();
            }

            _dbContext.Suppliers.Remove(supplier);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
