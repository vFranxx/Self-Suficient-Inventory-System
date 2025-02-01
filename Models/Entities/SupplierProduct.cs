namespace BlazorFront.Models.Entities
{
    public class SupplierProduct
    {
        public required int IdProv { get; set; } // ID_PROV
        public required string IdProd { get; set; } // ID_PROD

        // Relaciones con las tablas relacionadas
        public Supplier Suppliers { get; set; } // Relación con Proveedores
        public Product Products { get; set; } // Relación con Productos
    }
}
