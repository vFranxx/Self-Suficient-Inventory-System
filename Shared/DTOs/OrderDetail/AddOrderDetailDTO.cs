namespace API.Shared.DTOs.OrderDetail
{
    public class AddOrderDetailDTO
    {
        public required int Cantidad { get; set; }
        public required string IdProd { get; set; }
    }
}
