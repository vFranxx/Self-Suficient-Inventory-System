using API.Data;
using API.Models.Entities;
using Shared.DTO.SystemOperator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class SystemOperatorController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<SystemOperator> _userManager;
        public SystemOperatorController(AppDbContext dbContext, UserManager<SystemOperator> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOperators()
        {
            var users = await _userManager.Users.ToListAsync();
            var operators = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                operators.Add(new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.PhoneNumber,
                    user.FechaBaja,
                    Roles = roles
                });
            }

            return Ok(operators);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOperatorById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var operatorData = new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.PhoneNumber,
                user.FechaBaja,
                Roles = roles
            };

            return Ok(operatorData);
        }

        [HttpPut("{id}/username")]
        [Authorize(Roles = "OPERADOR, ADMIN")]
        public async Task<IActionResult> UpdateUsername(string id, [FromBody] string newUsername)
        {
            // Verificaciones
            if (User.IsInRole("OPERADOR") && User.FindFirst(ClaimTypes.NameIdentifier)?.Value != id)
            {
                return Forbid(); // Operador solo puede modificarse a sí mismo
            }

            var systemOperator = await _userManager.FindByIdAsync(id);
            if (systemOperator == null)
            {
                return NotFound("Usuario no encontrado");
            }

            // Verificar que el nuevo nombre no esté en uso
            var existingUser = await _userManager.FindByNameAsync(newUsername);
            if (existingUser != null && existingUser.Id != id)
            {
                return BadRequest($"El nombre de usuario '{newUsername}' ya está en uso.");
            }

            systemOperator.UserName = newUsername;
            systemOperator.NormalizedUserName = _userManager.NormalizeName(newUsername);

            var result = await _userManager.UpdateAsync(systemOperator);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(systemOperator);
        }

        [HttpPut("{id}/email")]
        [Authorize(Roles = "OPERADOR, ADMIN")]
        public async Task<IActionResult> UpdateEmail(string id, [FromBody] string newEmail)
        {
            // Verificaciones
            if (User.IsInRole("OPERADOR") && User.FindFirst(ClaimTypes.NameIdentifier)?.Value != id)
            {
                return Forbid(); // Operador solo puede modificarse a sí mismo
            }

            var systemOperator = await _userManager.FindByIdAsync(id);
            if (systemOperator == null)
            {
                return NotFound("Usuario no encontrado");
            }

            // Verificar que el nuevo email no esté en uso
            var existingEmail = await _userManager.FindByEmailAsync(newEmail);
            if (existingEmail != null && existingEmail.Id != id)
            {
                return BadRequest($"El email '{newEmail}' ya está en uso.");
            }

            systemOperator.Email = newEmail;
            systemOperator.NormalizedEmail = _userManager.NormalizeEmail(newEmail);

            var result = await _userManager.UpdateAsync(systemOperator);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(systemOperator);
        }

        [HttpPut("{id}/phone")]
        [Authorize(Roles = "OPERADOR, ADMIN")]
        public async Task<IActionResult> UpdatePhone(string id, [FromBody] string newPhoneNumber)
        {
            // Verificaciones
            if (User.IsInRole("OPERADOR") && User.FindFirst(ClaimTypes.NameIdentifier)?.Value != id)
            {
                return Forbid(); // Operador solo puede modificarse a sí mismo
            }

            var systemOperator = await _userManager.FindByIdAsync(id);
            if (systemOperator == null)
            {
                return NotFound("Usuario no encontrado");
            }

            if (!string.IsNullOrEmpty(newPhoneNumber))
            {
                systemOperator.PhoneNumber = newPhoneNumber;
            }

            var result = await _userManager.UpdateAsync(systemOperator);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(systemOperator);
        }

        [HttpPut("{id}/password")]
        [Authorize(Roles = "OPERADOR, ADMIN")]
        public async Task<IActionResult> UpdatePassword(string id, [FromBody] string newPassword)
        {
            // Verificaciones
            if (User.IsInRole("OPERADOR") && User.FindFirst(ClaimTypes.NameIdentifier)?.Value != id)
            {
                return Forbid(); // Operador solo puede modificarse a sí mismo
            }

            var systemOperator = await _userManager.FindByIdAsync(id);
            if (systemOperator == null)
            {
                return NotFound("Usuario no encontrado");
            }

            if (!string.IsNullOrEmpty(newPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(systemOperator);
                var passwordResult = await _userManager.ResetPasswordAsync(systemOperator, token, newPassword);

                if (!passwordResult.Succeeded)
                {
                    return BadRequest(passwordResult.Errors);
                }
            }

            var result = await _userManager.UpdateAsync(systemOperator);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(systemOperator);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteOperator(string id)
        {
            var systemOperator = await _userManager.FindByIdAsync(id);

            if (systemOperator == null)
            {
                return NotFound();
            }

            if (systemOperator.FechaBaja != null)
            {
                return BadRequest($"El usuario {id} ya está dado de baja.");
            }

            systemOperator.FechaBaja = DateTime.UtcNow;

            _dbContext.SystemOperators.Update(systemOperator);
            await _dbContext.SaveChangesAsync();

            return Ok(systemOperator);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> ActivateOperator(string id)
        {
            var systemOperator = await _userManager.FindByIdAsync(id);

            if (systemOperator == null)
            {
                return NotFound();
            }

            if (systemOperator.FechaBaja == null)
            {
                return BadRequest($"El usuario {id} ya está dado de alta");
            }

            systemOperator.FechaBaja = null;

            _dbContext.SystemOperators.Update(systemOperator);
            await _dbContext.SaveChangesAsync();

            return Ok(systemOperator);
        }


        [HttpDelete("Reject/{id}")]
        public async Task<IActionResult> RejectOperator(string id)
        {
            var systemOperator = await _userManager.FindByIdAsync(id);
            if (systemOperator == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(systemOperator);
            if (roles.Any())
            {
                return BadRequest($"El usuario {id} tiene roles asignados y no puede ser eliminado.");
            }

            var result = await _userManager.DeleteAsync(systemOperator);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok($"El usuario {id} ha sido eliminado.");
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingOperators()
        {
            var users = await _userManager.Users.Where(u => u.FechaBaja == null).ToListAsync();
            var operators = new List<object>();
            
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Any())
                {
                    operators.Add(new
                    {
                        user.Id,
                        user.UserName,
                        user.Email,
                        user.PhoneNumber,
                        user.FechaBaja,
                        Roles = roles
                    });
                }
            }
            
            return Ok(operators);
        }
    }
}
