﻿namespace API.Shared.DTOs.BillDetail
{
    public class UpdateBillDetailDto
    {
        public required int Cantidad { get; set; }
        public required decimal Precio { get; set; }
    }
}
