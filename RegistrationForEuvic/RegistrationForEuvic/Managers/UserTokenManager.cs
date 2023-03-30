using RegistrationForEuvic.Models;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RegistrationForEuvic.Managers
{
    public class UserTokenManager
    {
        public static String GenerateToken(ref User user,ref string key)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            SigningCredentials credentials=new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Sid,user.UserId.ToString()),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim(ClaimTypes.Email,user.Email),
            };

            JwtSecurityToken token = new JwtSecurityToken(claims:claims,
                                                          expires:DateTime.Now.AddMinutes(30),
                                                          signingCredentials:credentials);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
    }
}
