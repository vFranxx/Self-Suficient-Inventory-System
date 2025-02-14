namespace API.Shared.DTOs.Supplier
{
    public class AddSupplierDto
    {
        public required string Referencia { get; set; }
        public string? Contacto { get; set; }
        public string? Mail { get; set; }
        public string? Direccion { get; set; }
    }
}
