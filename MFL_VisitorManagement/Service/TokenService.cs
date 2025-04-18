using MFL_VisitorManagement.Dtos;
using MFL_VisitorManagement.Entities;
using MFL_VisitorManagement.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace MFL_VisitorManagement.Service
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(UserLoginResponse UserLoginResponse)
        {
            var tokenKey = config["tokenKey"] ?? throw new Exception("Can not access token from appsetting");
            if (tokenKey.Length < 64) throw new Exception("Your token needs to be longer");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)); 

            var Claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, UserLoginResponse.Id),
                new(ClaimTypes.Name,$"{UserLoginResponse.FirstName} {UserLoginResponse.LastName}".Trim()),
                new(ClaimTypes.Role, UserLoginResponse.UserRole)
            };

            var Credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = Credentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }
    }
}
