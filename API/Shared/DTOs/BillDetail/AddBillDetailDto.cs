namespace API.Shared.DTOs.BillDetail
{
    public class AddBillDetailDto
    {
        public required int Cantidad { get; set; }
        public required string IdProducto { get; set; }
    }
}
