namespace API.Models.AuditModels
{
    public class OrderDetailAudit
    {
        public int AuditId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string AuditAction { get; set; }
        public string UserId { get; set; }
        public int DetOcId { get; set; }
        public int Cantidad { get; set; }
        public string IdProd { get; set; }
        public int IdOc { get; set; }
    }
}