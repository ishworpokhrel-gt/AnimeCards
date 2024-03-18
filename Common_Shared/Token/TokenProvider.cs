using Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Common_Shared.Token
{
    public class TokenProvider
    {
        private readonly IConfiguration _config;
        private readonly string _key;

        public TokenProvider(IConfiguration config)
        {
            _config = config;
            _key = _config["Jwt:Key"];

        }

        public Tuple<string, int> createadmintoken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim("userId",user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            _ = int.TryParse(_config["Jwt:JWTExpiresInMinutes"], out var time);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddDays(time)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new(tokenHandler.WriteToken(token), time * 60);

        }

        public Tuple<int,string> AdminRefreshToken(ApplicationUser user)
        {
            var randomByte = new byte[64];
            var randomNumbers = RandomNumberGenerator.Create();
            randomNumbers.GetBytes(randomByte);

            var refToken = Convert.ToBase64String(randomByte);
            _ = int.TryParse(_config["Jwt:JWTAdminRefreshExpiresInMinutes"], out var expireTIme);

            int totalRefreshTime = expireTIme * 60;

            return new(totalRefreshTime, refToken);
        }

    }
}
