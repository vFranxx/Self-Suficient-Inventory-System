namespace Shared.DTOs.Product
{
    public class AddProductDto
    {
        public required string ProdId { get; set; }
        public required string Descripcion { get; set; }
        public required decimal PrecioUnitario { get; set; }
    }
}
