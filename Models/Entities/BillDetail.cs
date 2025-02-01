<<<<<<< HEAD
﻿namespace BlazorFront.Models.Entities
=======
﻿namespace RESTful_API.Models.Entities
>>>>>>> 30789201ddefe3990da3d2a2783d4120496c1b5e
{
    public class BillDetail
    {
        public int FacDetId { get; set; }

        public required int Cantidad { get; set; }

        public required decimal Precio { get; set; }

        public required decimal Subtotal { get; set; }

        public required int IdFactura { get; set; }

        public required string IdProducto { get; set; }

        public Bill Facturas { get; set; }

        public Product Productos { get; set; }
    }
}
