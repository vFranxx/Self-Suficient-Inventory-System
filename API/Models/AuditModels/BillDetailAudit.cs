namespace API.Models.AuditModels
{
    public class BillDetailAudit
    {
        public int AuditId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string AuditAction { get; set; }
        public string UserId { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
        public int IdFactura { get; set; }
        public string IdProducto { get; set; }
    }
}