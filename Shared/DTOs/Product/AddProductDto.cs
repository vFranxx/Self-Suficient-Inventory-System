﻿namespace Shared.DTOs.Product
{
    public class AddProductDto
    {
        public required string ProdId { get; set; }
        public required string Descripcion { get; set; }
        public required decimal PrecioUnitario { get; set; }
        public required decimal Ganancia { get; set; }
        public decimal? Descuento { get; set; }
        public int? Stock { get; set; }
        public int? StockMin { get; set; }
    }
}