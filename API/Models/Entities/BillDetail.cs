namespace API.Models.Entities
{
    public class BillDetail
    {
        public int FacDetId { get; set; }

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

        public decimal Subtotal { get; set; }

        public int IdFactura { get; set; }

        public string IdProducto { get; set; }

        public Bill Facturas { get; set; }

        public Product Productos { get; set; }
    }
}