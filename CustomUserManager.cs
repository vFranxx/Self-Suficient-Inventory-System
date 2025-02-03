using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Self_Suficient_Inventory_System
{
    public class CustomUserManager<TUser> : UserManager<TUser> where TUser : IdentityUser
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public CustomUserManager(
            IUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger,
            RoleManager<IdentityRole> roleManager)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _roleManager = roleManager;
        }

        public override async Task<IdentityResult>
    }
}
