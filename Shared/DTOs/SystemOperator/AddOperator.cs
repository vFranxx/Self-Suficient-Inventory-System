namespace Shared.DTOs.SystemOperator
{
    public class AddOperator
    {
        public string Uid { get; set; }
        public required string Nombre { get; set; }
        public required bool Tipo { get; set; } = false;
        public required string Pswd { get; set; }
    }
}
