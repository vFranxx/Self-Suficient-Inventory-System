namespace API.Shared.DTOs.Bill
{
    public class BillDto
    {
        public DateTime FechaHora { get; set; }
        public required int Total { get; set; }
        public required string IdOp { get; set; }
    }
}
