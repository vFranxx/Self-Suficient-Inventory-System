namespace Shared.DTOs.SystemOperator
{
    public class AddOperatorDto
    {
        public string Uid { get; set; }
        public required string Nombre { get; set; }
        public bool Tipo { get; set; } = false;
        public required string Pswd { get; set; }
    }
}
