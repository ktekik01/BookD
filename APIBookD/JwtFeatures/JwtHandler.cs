using APIBookD.Models.Entities.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIBookD.JwtFeatures
{
    public class JwtHandler
    {

        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSetings;

        public JwtHandler(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtSetings = _configuration.GetSection("JwtSettings");
        }

        public string createToken(User user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }


        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSetings["Security Key"]);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private List<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSetings["ValidIssuer"],
                audience: _jwtSetings["ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSetings["ExpiryInMinutes"])),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }
    }
}
