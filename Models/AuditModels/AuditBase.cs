namespace Self_Suficient_Inventory_System.Models.AuditModels
{
    public abstract class AuditBase
    {
        public int AuditId { get; set; }
        // Campo discriminador
        public string AuditType { get; set; } = null!;
        public required string AuditAction { get; set; }
        public required DateTime TimeStamp { get; set; }
        public required string UserId { get; set; }
        public string? ModifiedColumns { get; set; } 
        public string? OriginalValues { get; set; }   
        public string? NewValues { get; set; }
    }
}
