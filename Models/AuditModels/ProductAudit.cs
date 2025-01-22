namespace Self_Suficient_Inventory_System.Models.AuditModels
{
    public class ProductAudit
    {
        public int AuditId { get; set; }
        public required DateTime TimeStamp { get; set; }
        public required string UserId { get; set; }
        public required string ProdId { get; set; }
        public required string Descripcion { get; set; }
        public required decimal PrecioUnitario { get; set; }
        public required decimal Ganancia { get; set; }
        public decimal? Descuento { get; set; } = null;
        public int? Stock { get; set; } = null;
        public int? StockMin { get; set; } = null;
        public DateTime? FechaBaja { get; set; } = null;
    }
}
