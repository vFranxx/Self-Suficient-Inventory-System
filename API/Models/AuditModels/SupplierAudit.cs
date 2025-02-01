namespace API.Models.AuditModels
{
    public class SupplierAudit
    {
        public int AuditId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string AuditAction { get; set; }
        public string UserId { get; set; }
        public int ProvId { get; set; }
        public string Referencia { get; set; }
        public string? Contacto { get; set; }
        public string? Mail { get; set; }
        public string? Direccion { get; set; }
    }
}