namespace RESTful_API.Models.Entities
{
    public class Product
    {
        public required string ProdId { get; set; }
        public required string Descripcion { get; set; }
        public required decimal PrecioUnitario { get; set; }
    }
}
