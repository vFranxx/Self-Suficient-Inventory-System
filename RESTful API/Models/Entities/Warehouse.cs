namespace RESTful_API.Models.Entities
{
    public class Warehouse
    {
        public int DepoId { get; set; }
        public required int Stock { get; set; }
        public required int StockMin { get; set; }
        public required decimal Ganancia { get; set; }
        public decimal? Descuento { get; set; }
        public required string IdProducto { get; set; }
        public required Product Products { get; set; }
    }
}
