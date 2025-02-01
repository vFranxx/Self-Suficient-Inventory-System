<<<<<<< HEAD
﻿namespace BlazorFront.Models.Entities
=======
﻿namespace RESTful_API.Models.Entities
>>>>>>> 30789201ddefe3990da3d2a2783d4120496c1b5e
{
    public class SystemOperator
    {
        public required string Uid { get; set; }
        public required string Nombre { get; set; }
        public required bool Tipo { get; set; } = false;
        public required string Pswd { get; set; }
        public DateTime? FechaBaja { get; set; } = null;
    }
}
