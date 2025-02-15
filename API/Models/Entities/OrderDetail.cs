﻿namespace API.Models.Entities
{
    public class OrderDetail
    {
        public int DetOcId { get; set; }
        public required int Cantidad { get; set; }
        public required string IdProd { get; set; }
        public required int IdOc { get; set; }
        public Product Products { get; set; }
        public Order Orders { get; set; }
    }
}
