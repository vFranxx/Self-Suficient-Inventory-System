namespace API.Models.Entities
{
    public class SystemOperator
    {
        public string Uid { get; set; }
        public string Nombre { get; set; }
        public bool Tipo { get; set; } = false;
        public string Pswd { get; set; }
        public DateTime? FechaBaja { get; set; } = null;
    }
}