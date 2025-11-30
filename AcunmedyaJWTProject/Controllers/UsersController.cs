using AcunmedyaJWTProject.DTOs;
using AcunmedyaJWTProject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcunmedyaJWTProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UsersController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("addToRole")]
        public async Task<IActionResult> AddToRole(AddToRole model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return BadRequest("Kullanıcı bulunamadı.");
            }

            var existRole = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!existRole)
            {
                var createResult = await _roleManager.CreateAsync(new AppRole { Name = model.RoleName });
                if (!createResult.Succeeded)
                    return BadRequest(string.Join(" | ", createResult.Errors.Select(e => e.Description)));
            }


            var addResult = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (!addResult.Succeeded)
                return BadRequest(string.Join(" | ", addResult.Errors.Select(e => e.Description)));

            return Ok("Kullanıcı role kaydedildi");
        }

    }
}
