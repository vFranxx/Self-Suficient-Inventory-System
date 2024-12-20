namespace RESTful_API.Models.Entities
{
    public class SupplierProduct
    {
        public required int IdProv { get; set; } // ID_PROV
        public required string IdProd { get; set; } // ID_PROD

        // Relaciones con las tablas relacionadas
        public required Supplier Suppliers { get; set; } // Relación con Proveedores
        public required Product Products { get; set; } // Relación con Productos
    }
}
