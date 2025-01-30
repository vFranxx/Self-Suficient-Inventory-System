namespace Self_Suficient_Inventory_System.Shared.DTOs.OrderDetail
{
    public class AddOrderDetailDTO
    {
        public required int Cantidad { get; set; }
        public required string IdProd { get; set; }
        public required int IdOc { get; set; }
    }
}
