namespace Shared.DTOs.Product
{
    public class UpdateProductDto
    {
        public required string Descripcion { get; set; }
        public required decimal PrecioUnitario { get; set; }
    }
}
