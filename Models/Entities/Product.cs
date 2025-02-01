namespace BlazorFront.Models.Entities
{
    public class Product
    {
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
