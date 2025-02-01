namespace BlazorFront.Models.Entities
{
    public class Order
    {
        public int OcId { get; set; }
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "PENDIENTE";
        public required string IdOp { get; set; }
        public required int IdProv { get; set; }
        public SystemOperator Operators { get; set; }
        public Supplier Suppliers { get; set; }
    }
}
