using API.Models.Entities;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;

namespace API.Controllers
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
    }
}

