using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] userLogin userLogin)
        {
            // Basic validation
            if (userLogin.username == "admin" && userLogin.password == "12345")
            {
                return Ok(new
                {
                    Message = "Login successful",
                    token = GenerateJwtToken(userLogin.username)
                });
            }

            return Unauthorized(new
            {
                Message = "Invalid username or password"
            });
        }



        private string GenerateJwtToken(string username)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = jwtSettings["Key"]!;
            //var issuer = jwtSettings["Issuer"]!;
            //var audience = jwtSettings["Audience"]!;

            var expiryMinutes =
                Convert.ToDouble(jwtSettings["ExpiryMinutes"]);

            // Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim ("TenantId","Tenant123"),
                new Claim ("UserId","user123"),
                new Claim(JwtRegisteredClaimNames.Jti,
                          Guid.NewGuid().ToString())
            };

            // Secret key
            var securityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var credentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);

            // Create token
            var token = new JwtSecurityToken(
                //issuer: issuer,
                //audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            // Convert token to string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class userLogin
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}

