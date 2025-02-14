using Microsoft.AspNetCore.Identity;

namespace API.Models.Entities
{
    public class SystemOperator : IdentityUser
    {
        public DateTime? FechaBaja { get; set; } = null;
    }
}
