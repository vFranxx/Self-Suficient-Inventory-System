using RESTful_API.Models.Entities;

namespace Self_Suficient_Inventory_System.Models.AuditModels
{
    public class OrderAudit : AuditBase
    {
        public int OcId { get; set; }
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "PENDIENTE";
        public required string IdOp { get; set; }
        public required int IdProv { get; set; }
        public OrderAudit() => AuditType = "PedidoDeCompra";
    }
}
