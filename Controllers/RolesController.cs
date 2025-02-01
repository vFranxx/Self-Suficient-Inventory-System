using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Self_Suficient_Inventory_System.Shared.DTOs.Role;

namespace Self_Suficient_Inventory_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityRole> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityRole> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost("{roleName}")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            string normalizedRoleName = roleName.ToUpper();

            if (await _roleManager.RoleExistsAsync(normalizedRoleName))
            {
                return BadRequest($"El rol '{normalizedRoleName}' ya existe.");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(normalizedRoleName));
            if (result.Succeeded)
            {
                return Ok($"Rol '{normalizedRoleName}' creado exitosamente.");
            }

            return BadRequest("Error al crear el rol.");
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }

        [HttpDelete("roles/{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            string normalizedRoleName = roleName.ToUpper();

            // No permitir eliminar el rol 'Admin'
            if (roleName.Equals("ADMIN", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("No se puede eliminar el rol 'Admin'.");
            }

            var role = await _roleManager.FindByNameAsync(normalizedRoleName);

            if (role == null)
            {
                return NotFound($"El rol '{normalizedRoleName}' no existe.");
            }

            // Verificar si el rol tiene usuarios asociados
            var usersInRole = await _userManager.GetUsersInRoleAsync(normalizedRoleName);
            if (usersInRole.Any())
            {
                return BadRequest($"No se puede eliminar el rol '{normalizedRoleName}' porque tiene usuarios asociados.");
            }

            // Eliminar el rol
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                return StatusCode(500, $"Error al eliminar el rol '{normalizedRoleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return Ok($"Rol '{normalizedRoleName}' eliminado correctamente.");
        }

        [HttpPost("assign-role")]
        //[Authorize(Roles = "Admin")] // Solo administradores pueden acceder
        public async Task<IActionResult> AssignRole(RoleDTO dto)
        {
            // Buscar usuario por ID o email
            var user = await _userManager.FindByIdAsync(dto.UserIdentifier)
                     ?? await _userManager.FindByEmailAsync(dto.UserIdentifier);

            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            // Verificar si el rol existe
            string normalizedRoleName = dto.RoleName.ToUpper();
            if (!await _roleManager.RoleExistsAsync(normalizedRoleName))
            {
                return BadRequest($"El rol '{dto.RoleName}' no existe");
            }

            // Verificar si el usuario ya tiene el rol
            if (await _userManager.IsInRoleAsync(user, normalizedRoleName))
            {
                return Conflict($"El usuario ya tiene el rol '{dto.RoleName}'");
            }

            // Asignar el rol
            var result = await _userManager.AddToRoleAsync(user, normalizedRoleName);

            return result.Succeeded
                ? Ok($"Rol '{dto.RoleName}' asignado correctamente")
                : BadRequest($"Error al asignar rol: {string.Join(", ", result.Errors)}");
        }

        [HttpPost("revoke-role")]
        //[Authorize(Roles = "Admin")] // Solo administradores pueden usar este endpoint
        public async Task<IActionResult> RevokeRole(RoleDTO dto)
        {
            // Buscar usuario por ID o email
            var user = await _userManager.FindByIdAsync(dto.UserIdentifier)
                     ?? await _userManager.FindByEmailAsync(dto.UserIdentifier);

            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            // Verificar si el rol existe
            string normalizedRoleName = dto.RoleName.ToUpper();
            if (!await _roleManager.RoleExistsAsync(normalizedRoleName))
            {
                return BadRequest($"El rol '{dto.RoleName}' no existe");
            }

            // Verificar si el usuario realmente tiene el rol
            if (!await _userManager.IsInRoleAsync(user, normalizedRoleName))
            {
                return Conflict($"El usuario no tiene el rol '{dto.RoleName}'");
            }

            // Validación especial para rol Admin
            if (normalizedRoleName == "ADMIN")
            {
                // Obtener todos los usuarios con rol Admin
                var admins = await _userManager.GetUsersInRoleAsync("ADMIN");

                // Evitar eliminar el último Admin
                if (admins.Count <= 1 && admins.Any(u => u.Id == user.Id))
                {
                    return BadRequest("No se puede revocar el último administrador");
                }
            }

            // Revocar el rol
            var result = await _userManager.RemoveFromRoleAsync(user, normalizedRoleName);

            return result.Succeeded
                ? Ok($"Rol '{dto.RoleName}' revocado exitosamente")
                : BadRequest($"Error al revocar rol: {string.Join(", ", result.Errors)}");
        }

    }
}

