using ApiProjectPRN231.Models;
using ApiProjectPRN231.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiProjectPRN231.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyController : ControllerBase
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly ApplicationDbContext _context;
        public VerifyController(UserManager<UserApp> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("{token}")]
        public async Task<IActionResult> Get(string token)
        {
            try
            {
                var userId = _context.EmailTokens.Where(et => et.Token == token && et.Deleted == false && et.ExpiredTime >= DateTime.Now).Select(et => et.UserId).FirstOrDefault();
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "error", Message = "token unknow" });
                }

                user.EmailConfirmed = true;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Verify Success" });

                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "error", Message = "can not update db" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "error", Message = "???" });

            }
        }


    }
}
