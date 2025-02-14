using API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Data
{
    public class CustomUserManager : UserManager<SystemOperator>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public CustomUserManager(
        IUserStore<SystemOperator> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<SystemOperator> passwordHasher,
        IEnumerable<IUserValidator<SystemOperator>> userValidators,
        IEnumerable<IPasswordValidator<SystemOperator>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<SystemOperator>> logger,
        RoleManager<IdentityRole> roleManager)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _roleManager = roleManager;
        }

        public override async Task<IdentityResult> CreateAsync(SystemOperator user, string password)
        {
            var result = await base.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Crear rol "OPERADOR" si no existe
                if (!await _roleManager.RoleExistsAsync("OPERADOR"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("OPERADOR"));
                }

                // Asignar rol al usuario
                await AddToRoleAsync(user, "OPERADOR");
            }

            return result;
        }
    }
}
