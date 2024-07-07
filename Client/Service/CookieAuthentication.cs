using ApiProjectPRN231;
using ApiProjectPRN231.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Client.Data
{
    public class CookieAuthentication
    {
        public static async Task SignInAsync(LoginResponse user, HttpContext context)
        {
            var roleID = user.Role;
            string id = user.Id;
            string scheme = "cookie";

            List<Claim> claims = new List<Claim>()
            {
                new Claim("ID", id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, roleID.ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, scheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(1)
            };

            CookieOptions options = new CookieOptions
            {
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            };
            context.Response.Cookies.Append("token", user.Token, options);
            context.Response.Cookies.Append("role", user.Role, options);
            context.Response.Cookies.Append("id", user.Id, options);

            await context.SignInAsync(scheme,
                new ClaimsPrincipal(claimsIdentity), properties);
        }

        public static async Task SignOutAsync(HttpContext context)
        {
            await context.SignOutAsync("cookie");
        }
    }
}
