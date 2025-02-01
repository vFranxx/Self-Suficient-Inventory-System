namespace API.Shared.DTOs.BillDetail
{
    public class AddBillDetailDto
    {
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public int IdFactura { get; set; }
        public string IdProducto { get; set; }
    }
}