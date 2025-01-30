using RESTful_API.Models.Entities;

namespace Self_Suficient_Inventory_System.Models.AuditModels
{
    public class OrderAudit
    {
        public int AuditId { get; set; }
        public required DateTime TimeStamp { get; set; }
        public required string AuditAction { get; set; }
        public required string UserId { get; set; }
        public int OcId { get; set; }
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "PENDIENTE";
        public required string IdOp { get; set; }
        public required int IdProv { get; set; }
    }
}
