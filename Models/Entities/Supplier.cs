namespace BlazorFront.Models.Entities
{
    public class Supplier
    {
        public int ProvId { get; set; }
        public required string Referencia { get; set; }
        public string? Contacto { get; set; }
        public string? Mail { get; set; }
        public string? Direccion { get; set; }
    }
}
