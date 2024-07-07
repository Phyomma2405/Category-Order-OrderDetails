using Azure.Core;
using ApiProjectPRN231.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using ApiProjectPRN231.Models;

namespace ApiProjectPRN231.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<UserApp> _signInManager;

        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public AuthenticateController(
            UserManager<UserApp> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<UserApp> signInManager,
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _httpClient = httpClient;

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, lockoutOnFailure: true);
            if (result.IsLockedOut)
            {
                return StatusCode(423, new { message = "User account locked out" });
            }
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    var _userService = new UserService(_userManager, _configuration, _httpClient, HttpContext);
                    var rolesList = await _userManager.GetRolesAsync(user);
                    string userRole = "User";
                    foreach (var role in rolesList) 
                    {
                        if (role.Contains("Admin"))
                        {
                            userRole = role;
                            break;
                        }
                    }
                    var token = await _userService.GetToken(user, DateTime.Now.AddHours(3));

                    return Ok(new LoginResponse
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Role = userRole,
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                    });
                }
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            UserApp user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            var _userService = new UserService(_userManager, _configuration, _httpClient, HttpContext);
            if (await _userService.CallApiPost(user, "api/SendEmail/register", DateTime.Now.AddMinutes(30))) { 
                return Ok(new Response { Status = "Success", Message = "User created successfully!" });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            UserApp user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }

            var _userService = new UserService(_userManager, _configuration, _httpClient, HttpContext);
            if (await _userService.CallApiPost(user, "api/SendEmail/register", DateTime.Now.AddMinutes(30)))
            {
                return Ok(new Response { Status = "Success", Message = "User created successfully!" });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
        }
    }
}
