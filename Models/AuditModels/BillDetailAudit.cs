using RESTful_API.Models.Entities;

namespace Self_Suficient_Inventory_System.Models.AuditModels
{
    public class BillDetailAudit
    {
        public int AuditId { get; set; }
        public required DateTime TimeStamp { get; set; }
        public required string AuditAction { get; set; }
        public required string UserId { get; set; }
        public required int Cantidad { get; set; }
        public required decimal Precio { get; set; }
        public required decimal Subtotal { get; set; }
        public required int IdFactura { get; set; }
        public required string IdProducto { get; set; }
    }
}
