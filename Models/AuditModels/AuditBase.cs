namespace Self_Suficient_Inventory_System.Models.AuditModels
{
    public class AuditBase
    {
        public int AuditId { get; set; }
        public required DateTime TimeStamp { get; set; }
        public required string AuditAction { get; set; }
        public required string UserId { get; set; }

        // Campo discriminador
        public string AuditType { get; protected set; } = null!;
    }
}
