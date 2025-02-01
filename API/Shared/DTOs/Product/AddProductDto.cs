namespace API.Shared.DTOs.Product
{
    public class AddProductDto
    {
        public string ProdId { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Ganancia { get; set; }
        public decimal? Descuento { get; set; }
        public int? Stock { get; set; }
        public int? StockMin { get; set; }
    }
}