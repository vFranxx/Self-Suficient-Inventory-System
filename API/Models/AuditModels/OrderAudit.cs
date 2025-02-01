namespace API.Models.AuditModels
{
    public class OrderAudit
    {
        public int AuditId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string AuditAction { get; set; }
        public string UserId { get; set; }
        public int OcId { get; set; }
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "PENDIENTE";
        public string IdOp { get; set; }
        public int IdProv { get; set; }
    }
}