<<<<<<< HEAD
﻿namespace BlazorFront.Models.Entities
=======
﻿namespace RESTful_API.Models.Entities
>>>>>>> 30789201ddefe3990da3d2a2783d4120496c1b5e
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
