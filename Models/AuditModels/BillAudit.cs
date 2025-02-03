using RESTful_API.Models.Entities;

namespace Self_Suficient_Inventory_System.Models.AuditModels
{
    public class BillAudit : AuditBase
    {
        public required DateTime FechaHora { get; set; } = DateTime.Now;
        public required decimal Total { get; set; }
        public required string IdOp { get; set; }
        public BillAudit() => AuditType = "Factura";
    }
}
