<<<<<<< HEAD
﻿namespace BlazorFront.Models.Entities
=======
﻿namespace RESTful_API.Models.Entities
>>>>>>> 30789201ddefe3990da3d2a2783d4120496c1b5e
{
    public class Product
    {
        public required string ProdId { get; set; }
        public required string Descripcion { get; set; }
<<<<<<< HEAD
        public required decimal PrecioUnitario { get; set; }
=======
        public required decimal PrecioUnitario { get; set; }   
>>>>>>> 30789201ddefe3990da3d2a2783d4120496c1b5e
        public required decimal Ganancia { get; set; }
        public decimal? Descuento { get; set; } = null;
        public int? Stock { get; set; } = null;
        public int? StockMin { get; set; } = null;
        public DateTime? FechaBaja { get; set; } = null;
    }
}
