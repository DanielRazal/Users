using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace Users_Server.Token
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;
        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(User user)
        {
            // var secret = Environment.GetEnvironmentVariable("TokenKey");

            var secret = _configuration.GetValue<string>("TokenKey");

            if (secret == null)
            {
                throw new InvalidOperationException("JWT secret key is missing from configuration.");
            }
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName} {user.UserName} " +
                        $"{user.Email} {user.PhotoUrl} {user.Role.ToString()}")
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            );

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}