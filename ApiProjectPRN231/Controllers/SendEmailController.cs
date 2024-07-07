using ApiProjectPRN231.Constants;
using ApiProjectPRN231.Models;
using ApiProjectPRN231.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiProjectPRN231.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly ApplicationDbContext _context;

        public SendEmailController(UserManager<UserApp> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize]
        [HttpGet("register/{userId}")]
        public async Task<bool> Get(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            var to = await _userManager.GetEmailAsync(user);
            string token = createToken(userId, DateTime.Now.AddMinutes(30));
            var isSended = await MailUtils.SendGmailAsync(to, "Verification Your Account", token);
            return isSended;
        }

        private string createToken(string userId, DateTime time)
        {
            string token = Guid.NewGuid().ToString();
            try
            {
                _context.EmailTokens.Add
               (
                   new EmailToken
                   {
                       UserId = userId,
                       Token = token,
                       ExpiredTime = time,
                   }
               );
                _context.SaveChanges();
                return token;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
