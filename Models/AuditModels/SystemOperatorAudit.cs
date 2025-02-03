namespace Self_Suficient_Inventory_System.Models.AuditModels
{
    public class SystemOperatorAudit : AuditBase
    {
        public required string Email { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? FechaBaja { get; set; } = null;
        public string Rol { get; set; }
        public SystemOperatorAudit() => AuditType = "Usuario";
    }
}
