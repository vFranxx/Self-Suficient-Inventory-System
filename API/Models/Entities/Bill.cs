namespace API.Models.Entities
{
    // Factura
    public class Bill
    {
        public int FacId { get; set; }
        public DateTime FechaHora { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public string IdOp { get; set; }
        public SystemOperator Operators { get; set; }
    }
}