using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service.JwtService
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryMinutes;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? throw new 

                InvalidOperationException("JWT Key is missing")));

            _issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is missing");
            _audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is missing");
            _expiryMinutes = int.TryParse(_configuration["Jwt:ExpiryMinutes"], out int minutes) ? minutes : 60;
        }     
        public string GenerateToken(User user)
        {
            //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //    new Claim(ClaimTypes.Name, user.Username),
            //    new Claim(ClaimTypes.Email, user.Email


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.ID.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role?? "User"),
                //وقت الانشاء
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                // لمنع اعادة الاشتخدام 
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

           

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                 claims: claims,
                 issuer: _issuer,
                 audience: _audience,
                 expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
                 signingCredentials: creds

                 );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        // تحقق مخصص للتحقق من التوكن انها موقعة وصالحة ولكن يوجد تلقائي من دوت نت  useAuthentications 
        public int? ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _key,
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userIdClaim = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub);
                return int.Parse(userIdClaim.Value);
            }
            catch
            {
                return null;
            }
        }








        public TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            };
        }


    }
}
