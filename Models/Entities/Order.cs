<<<<<<< HEAD
﻿namespace BlazorFront.Models.Entities
{
    public class Order
    {
        public int OcId { get; set; }
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;
=======
﻿namespace RESTful_API.Models.Entities
{
    public class Order
    { 
        public int OcId { get; set; }
        public DateTime FechaSolicitud{ get; set; } = DateTime.Now;
>>>>>>> 30789201ddefe3990da3d2a2783d4120496c1b5e
        public string Estado { get; set; } = "PENDIENTE";
        public required string IdOp { get; set; }
        public required int IdProv { get; set; }
        public SystemOperator Operators { get; set; }
        public Supplier Suppliers { get; set; }
    }
}
