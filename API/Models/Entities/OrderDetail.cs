namespace API.Models.Entities
{
    public class OrderDetail
    {
        public int DetOcId { get; set; }
        public int Cantidad { get; set; }
        public string IdProd { get; set; }
        public int IdOc { get; set; }
        public Product Products { get; set; }
        public Order Orders { get; set; }
    }
}