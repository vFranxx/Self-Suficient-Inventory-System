namespace RESTful_API.Models.Entities
{
    public class BillDetail
    {
        public int FacDetId { get; set; }

        public required int Cantidad { get; set; }

        public required decimal Precio { get; set; }

        public required decimal Subtotal { get; set; }

        public required int IdFactura { get; set; }

        public required string IdProducto { get; set; }

        public Bill Facturas { get; set; }

        public Product Productos { get; set; }
    }
}
