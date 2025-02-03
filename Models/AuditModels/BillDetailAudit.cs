using RESTful_API.Models.Entities;

namespace Self_Suficient_Inventory_System.Models.AuditModels
{
    public class BillDetailAudit : AuditBase
    {
        public required int Cantidad { get; set; }
        public required decimal Precio { get; set; }
        public required decimal Subtotal { get; set; }
        public required int IdFactura { get; set; }
        public required string IdProducto { get; set; }
        public BillDetailAudit() => AuditType = "DetalleFactura";
    }
}
