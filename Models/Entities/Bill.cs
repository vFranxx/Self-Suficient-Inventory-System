namespace API.Models.Entities
{
    // Factura
    public class Bill
    {
        public int FacId { get; set; }
        public required DateTime FechaHora { get; set; } = DateTime.Now;
        public required decimal Total { get; set; }
        public required string IdOp { get; set; }
        public SystemOperator Operators { get; set; }
    }
}
