using Microsoft.AspNetCore.Identity;

namespace RESTful_API.Models.Entities
{
    public class SystemOperator : IdentityUser
    {
        public DateTime? FechaBaja { get; set; } = null;
    }
}
