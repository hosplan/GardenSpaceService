using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GardenSpaceService.Services
{
    public interface IJWTService
    {
        /// <summary>
        /// Jwt 생성
        /// </summary>
        /// <param name="email"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string GenerateJWT(string email, int roleId);

        /// <summary>
        /// Jwt decode
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public JwtSecurityToken DecodeJwt(string jwt);

        /// <summary>
        /// Jwt 내부의 Claimlist 얻기
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public List<Claim> GetClaimList(string jwt);

        public string GetEmail(List<Claim> claimList);

        public int GetId(List<Claim> claimList);  
        public string GetRoleId(List<Claim> claimList);
    }

    public class JWTService : IJWTService
    {
        private readonly IConfiguration _config;
        public JWTService(IConfiguration config)
        {
            _config = config;
        }

        public string GetEmail(List<Claim> claimList)
        {
            return claimList[0].Value;
        }

        public string GetRoleId(List<Claim> claimList)
        {
            return claimList[1].Value;
        }

        public int GetId(List<Claim> claimList)
        {
            try
            {
                return Convert.ToInt32(claimList[3].Value);
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return 0;
            }
        }

        public JwtSecurityToken DecodeJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(jwt);
        }

        public List<Claim> GetClaimList(string jwt)
        {
            JwtSecurityToken jwtSecurityToken = DecodeJwt(jwt);
            return jwtSecurityToken.Claims.ToList();
        }


        public string GenerateJWT(string email, int roleId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("eml", email),
                new Claim("rol", Convert.ToString(roleId)),
                new Claim("jti", Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
              );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
