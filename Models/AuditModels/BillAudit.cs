using RESTful_API.Models.Entities;

namespace Self_Suficient_Inventory_System.Models.AuditModels
{
    public class BillAudit
    {
        public int AuditId { get; set; }
        public required DateTime TimeStamp { get; set; }
        public required string AuditAction { get; set; }
        public required string UserId { get; set; }
        public required DateTime FechaHora { get; set; } = DateTime.Now;
        public required decimal Total { get; set; }
        public required string IdOp { get; set; }
    }
}
