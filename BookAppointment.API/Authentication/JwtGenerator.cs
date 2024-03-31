using BookAppointment.Core.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookAppointment.API.Authentication
{
    public static class JwtGenerator
    {
        private const string TokenSecret = "RizkySeptyanAhadQwiikCaseworkTestInMarch";
        private static readonly TimeSpan TokenLifeTime = TimeSpan.FromHours(8);

        public static string GenerateToken(User user, string userType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(TokenSecret);

            //var claims = new List<Claim>
            //{
            //    new(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
            //    new(JwtRegisteredClaimNames.Sub, user.UserName),
            //    new(JwtRegisteredClaimNames.Name, user.FullName),
            //    new("userType", userType)
            //};
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, userType)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenLifeTime),
                Issuer = "localhost",
                Audience = "localhost",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);
            return jwt;
        }
    }
}
