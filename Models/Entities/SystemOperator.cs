namespace BlazorFront.Models.Entities

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
