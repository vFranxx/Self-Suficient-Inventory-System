﻿using System.ComponentModel.DataAnnotations;

namespace API.Shared.DTOs.Product
{
    public class UpdateProductDto
    {
        public required string Descripcion { get; set; }
        public required decimal PrecioUnitario { get; set; }
        public required decimal Ganancia { get; set; }

        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100.")]
        public decimal? Descuento { get; set; }

        public int? StockMin { get; set; }
    }
}
