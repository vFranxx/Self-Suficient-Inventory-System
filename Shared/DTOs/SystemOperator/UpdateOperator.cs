namespace Shared.DTOs.SystemOperator
{
    public class UpdateOperator
    {
        public required string Nombre { get; set; }
        public required bool Tipo { get; set; } = false;
        public required string Pswd { get; set; }
    }
}
