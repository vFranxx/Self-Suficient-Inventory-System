namespace API.Models.AuditModels
{
    public class OrderDetailAudit : AuditBase
    {
        public int DetOcId { get; set; }
        public required int Cantidad { get; set; }
        public required string IdProd { get; set; }
        public required int IdOc { get; set; }
        public OrderDetailAudit() => AuditType = "DetallePedido";
    }
}
