namespace API.Models.AuditModels
{
    public class SystemOperatorAudit
    {
        public int AuditId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string AuditAction { get; set; }
        public string UserId { get; set; }
        public string Uid { get; set; }
        public string Nombre { get; set; }
        public bool Tipo { get; set; } = false;
        public string Pswd { get; set; }
        public DateTime? FechaBaja { get; set; } = null;
    }
}