namespace Shared.DTOs.BillDetail
{
    public class AddBillDetailDto
    {
        public required int Cantidad { get; set; }
        public required decimal Precio { get; set; }
        public int IdFactura { get; set; } 
        public required string IdProducto { get; set; }
    }
}
