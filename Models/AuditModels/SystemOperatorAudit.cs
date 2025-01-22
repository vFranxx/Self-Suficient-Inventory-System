namespace Self_Suficient_Inventory_System.Models.AuditModels
{
    public class SystemOperatorAudit
    {
        public int AuditId { get; set; }
        public required DateTime TimeStamp { get; set; }
        public required string UserId { get; set; }
        public string Uid { get; set; }
        public required string Nombre { get; set; }
        public required bool Tipo { get; set; } = false;
        public required string Pswd { get; set; }
        public DateTime? FechaBaja { get; set; } = null;
    }
}
