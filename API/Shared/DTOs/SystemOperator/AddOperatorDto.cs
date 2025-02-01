namespace API.Shared.DTOs.SystemOperator
{
    public class AddOperatorDto
    {
        public string Uid { get; set; }
        public string Nombre { get; set; }
        public bool Tipo { get; set; } = false;
        public string Pswd { get; set; }
    }
}