namespace API.Models.AuditModels
{
    public class ProductAudit
    {
        public int AuditId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string AuditAction { get; set; }
        public string UserId { get; set; }
        public string ProdId { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Ganancia { get; set; }
        public decimal? Descuento { get; set; } = null;
        public int? Stock { get; set; } = null;
        public int? StockMin { get; set; } = null;
        public DateTime? FechaBaja { get; set; } = null;
    }
}