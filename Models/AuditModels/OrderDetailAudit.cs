using Self_Suficient_Inventory_System.Models.AuditModels;

namespace RESTful_API.Models.Entities
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
