namespace RESTful_API.Models.Entities
{
    public class OrderDetailAudit
    {
        public int AuditId { get; set; }
        public required DateTime TimeStamp { get; set; }
        public required string AuditAction { get; set; }
        public required string UserId { get; set; }
        public int DetOcId { get; set; }
        public required int Cantidad { get; set; }
        public required string IdProd { get; set; }
        public required int IdOc { get; set; }
    }
}
