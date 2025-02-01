<<<<<<< HEAD
﻿namespace BlazorFront.Models.Entities
=======
﻿namespace RESTful_API.Models.Entities
>>>>>>> 30789201ddefe3990da3d2a2783d4120496c1b5e
{
    public class OrderDetail
    {
        public int DetOcId { get; set; }
        public required int Cantidad { get; set; }
        public required string IdProd { get; set; }
        public required int IdOc { get; set; }
        public Product Products { get; set; }
<<<<<<< HEAD
        public Order Orders { get; set; }
=======
        public Order Orders { get; set; }   
>>>>>>> 30789201ddefe3990da3d2a2783d4120496c1b5e
    }
}
