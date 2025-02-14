namespace API.Models.AuditModels
{
    public class SupplierAudit : AuditBase
    {
        public int ProvId { get; set; }
        public required string Referencia { get; set; }
        public string? Contacto { get; set; }
        public string? Mail { get; set; }
        public string? Direccion { get; set; }
        public SupplierAudit() => AuditType = "Proveedor";
    }
}
