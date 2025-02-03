using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RESTful_API.Models.Entities;
using Self_Suficient_Inventory_System.Shared.DTOs.Role;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Self_Suficient_Inventory_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")] // Solo administradores pueden acceder
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<SystemOperator> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<SystemOperator> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }

        [HttpPost("assign-role")]
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

        [HttpPost("revoke-admin-role")]
        public async Task<IActionResult> RevokeAdminRole(string userId)
        {
            // Buscar usuario por ID o email
            var user = await _userManager.FindByIdAsync(userId)
                     ?? await _userManager.FindByEmailAsync(userId);

            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            string normalizedRoleName = "ADMIN";

            // Verificar si el usuario realmente tiene el rol
            if (!await _userManager.IsInRoleAsync(user, normalizedRoleName))
            {
                return Conflict($"El usuario no tiene el rol '{normalizedRoleName}'");
            }

            // Obtener todos los usuarios con rol Admin
            var admins = await _userManager.GetUsersInRoleAsync("ADMIN");

            // Evitar eliminar el último Admin
            if (admins.Count <= 1 && admins.Any(u => u.Id == user.Id))
            {
                return BadRequest("No se puede revocar el último administrador");
            }
            
            // Revocar el rol
            var result = await _userManager.RemoveFromRoleAsync(user, normalizedRoleName);

            return result.Succeeded
                ? Ok($"Rol '{normalizedRoleName}' revocado exitosamente")
                : BadRequest($"Error al revocar rol: {string.Join(", ", result.Errors)}");
        }

    }
}

