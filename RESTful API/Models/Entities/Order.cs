namespace RESTful_API.Models.Entities
{
    public class Order
    { 
        public int OcId { get; set; }
        public required DateTime FechaSolicitud{ get; set; } = DateTime.Now;
        public string Estado { get; set; } = "PENDIENTE";
        public required string IdOp { get; set; }
        public required int IdProv { get; set; }
        public required SystemOperator Operators { get; set; }
        public required Supplier Suppliers { get; set; }
    }
}
