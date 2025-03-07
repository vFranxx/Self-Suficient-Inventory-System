﻿namespace API.Models.AuditModels
{
    public class BillAudit : AuditBase
    {
        public required DateTime FechaHora { get; set; } = DateTime.UtcNow;
        public required decimal Total { get; set; }
        public required string IdOp { get; set; }
        public BillAudit() => AuditType = "Factura";
    }
}
