<<<<<<< HEAD
﻿namespace BlazorFront.Models.Entities
=======
﻿namespace RESTful_API.Models.Entities
>>>>>>> 30789201ddefe3990da3d2a2783d4120496c1b5e
{
    public class Supplier
    {
        public int ProvId { get; set; }
        public required string Referencia { get; set; }
        public string? Contacto { get; set; }
        public string? Mail { get; set; }
        public string? Direccion { get; set; }
    }
}
