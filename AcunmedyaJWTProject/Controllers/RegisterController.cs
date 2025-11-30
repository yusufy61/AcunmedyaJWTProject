using AcunmedyaJWTProject.DTOs;
using AcunmedyaJWTProject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcunmedyaJWTProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterDTO registerDTO)
        {
            var user = new AppUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                return Ok("Kullanıcı başarıyla oluşturuldu.");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
