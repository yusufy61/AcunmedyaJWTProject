using AcunmedyaJWTProject.Entities;
using AcunmedyaJWTProject.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AcunmedyaJWTProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(SignInManager<AppUser> _signInManager, IOptions<JwtTokenOptions> jwtTokenOptions, UserManager<AppUser> _userManager) : ControllerBase
    {
        private readonly JwtTokenOptions _jwtTokenOptions = jwtTokenOptions.Value;

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password)
        {
            // Kullanıcı adı ve şifre ile giriş yapmayı dene
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            // Giriş başarılı ise ilk başta kullanıcıyı bul
            var appUser = await _userManager.FindByNameAsync(username);

            // JWT token oluşturma işlemi
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.SecurityKey));

            // Kullanıcının rollerini al
            var userRoles = await _userManager.GetRolesAsync(appUser);

            // Claim'leri oluştur
            // Claim = Kullanıcı ile ilgili bilgileri tutan nesne, örneğin kullanıcı adı, kullanıcı ID'si, roller vb.
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                new Claim(ClaimTypes.Name, appUser.UserName),
                new Claim(ClaimTypes.Email, appUser.Email)
            };

            // Kullanıcının rollerini claim'lere ekle
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtTokenOptions.Issuer,
                audience: _jwtTokenOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_jwtTokenOptions.ExpireMinutes),
                signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
            );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);
            return Ok(token);
        }

    }
}
