using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using ApiProjectPRN231.Models;

namespace ApiProjectPRN231.Service
{
    public class UserService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly HttpContext _httpContext;
        public UserService(UserManager<UserApp> userManager, IConfiguration configuration, HttpClient httpClient, HttpContext httpContext)
        {
            _userManager = userManager;
            _configuration = configuration;
            _httpClient = httpClient;
            _httpContext = httpContext;
        }

        public async Task<JwtSecurityToken> GetToken(UserApp user, DateTime time)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: time,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }

        public async Task<bool> CallApiPost(UserApp user, string path, DateTime time)
        {
            var token = await GetToken(user, time);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(token));

            string userId = await _userManager.GetUserIdAsync(user);
            var scheme = _httpContext.Request.Scheme;
            var host = _httpContext.Request.Host;
            var url = $"{scheme}://{host}/{path}/{userId}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
