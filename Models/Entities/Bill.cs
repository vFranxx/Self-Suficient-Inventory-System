<<<<<<< HEAD
﻿namespace BlazorFront.Models.Entities
{
    // Factura
    public class Bill
=======
﻿namespace RESTful_API.Models.Entities
{
    // Factura
    public class Bill 
>>>>>>> 30789201ddefe3990da3d2a2783d4120496c1b5e
    {
        public int FacId { get; set; }
        public required DateTime FechaHora { get; set; } = DateTime.Now;
        public required decimal Total { get; set; }
        public required string IdOp { get; set; }
        public SystemOperator Operators { get; set; }
    }
}
