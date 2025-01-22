namespace Self_Suficient_Inventory_System.Models.AuditModels
{
    public class SupplierAudit
    {
        public int AuditId { get; set; }
        public required DateTime TimeStamp { get; set; }
        public required string UserId { get; set; }
        public int ProvId { get; set; }
        public required string Referencia { get; set; }
        public string? Contacto { get; set; }
        public string? Mail { get; set; }
        public string? Direccion { get; set; }
    }
}
