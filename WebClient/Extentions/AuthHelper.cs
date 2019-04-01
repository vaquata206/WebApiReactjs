using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using WebClient.Core;
using WebClient.Core.Entities;

namespace WebClient.Extentions
{
    /// <summary>
    /// Auth helper
    /// </summary>
    public class AuthHelper
    {
        /// <summary>
        /// Employee Id
        /// </summary>
        private const string TypeEmployeeId = "EmployeeId";

        /// <summary>
        /// Department Id
        /// </summary>
        private const string TypeDepartmentId = "DepartmentId";

        /// <summary>
        /// Http context accessor
        /// </summary>
        private IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="httpContextAccessor">Http context accessor</param>
        public AuthHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// List claims
        /// </summary>
        public IEnumerable<Claim> Claims
        {
            get
            {
                var claims = this.httpContextAccessor.HttpContext.User.Claims;
                if (claims == null || claims.Count() == 0)
                {
                    return Enumerable.Empty<Claim>();
                }

                return claims;
            }
        }

        /// <summary>
        /// Get username of current user
        /// </summary>
        public string Username
        {
            get
            {
                return this.Claims.Where(x => x.Type == JwtRegisteredClaimNames.UniqueName).Select(x => x.Value).SingleOrDefault();
            }
        }

        /// <summary>
        /// UserId of current User
        /// </summary>
        public int UserId
        {
            get
            {
                return int.Parse(this.Claims.Where(x => x.Type == JwtRegisteredClaimNames.NameId).Select(x => x.Value).SingleOrDefault());
            }
        }

        /// <summary>
        /// Gets employeeId of current user
        /// </summary>
        public int EmployeeId
        {
            get
            {
                return int.Parse(this.Claims.Where(x => x.Type == TypeEmployeeId).Select(x => x.Value).SingleOrDefault());
            }
        }

        /// <summary>
        /// Gets departmentId of current user
        /// </summary>
        public int DepartmentId
        {
            get
            {
                return int.Parse(this.Claims.Where(x => x.Type == TypeDepartmentId).Select(x => x.Value).SingleOrDefault());
            }
        }

        /// <summary>
        /// Build a token
        /// </summary>
        /// <param name="user">Account info</param>
        /// <returns>A token</returns>
        public static string BuildToken(AccountInfo user)
        {
            var claims = new[] 
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.NameId, user.UserId.ToString()),
                new Claim(TypeEmployeeId, user.EmployeeId.ToString()),
                new Claim(TypeDepartmentId, user.DepartmentId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(WebConfig.JWTKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claimsIdentity.Claims,
                expires: DateTime.Now.AddMinutes(30), // expire time là 30 phút
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
