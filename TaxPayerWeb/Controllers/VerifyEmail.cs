using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TaxPayerWeb.Controllers
{

    public class VerifyEmail : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<VerifyEmail> _logger;

        public VerifyEmail(UserManager<IdentityUser> userManager, ILogger<VerifyEmail> logger, SignInManager<IdentityUser> signInManger)
        {
            _userManager = userManager;
            _signInManager = signInManger;
            _logger = logger;
        }


        public async Task<IActionResult> Index(string VerificationToken)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user != null && (await _userManager.IsEmailConfirmedAsync(user)))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Token"] = VerificationToken;
                return View();
            } 
        }

        public async Task<IActionResult> Verify(string VerificationToken)
        {   
            var user = await _userManager.GetUserAsync(User);
            if(user == null) { return Ok("super mario error"); }
            var ver = await _userManager.ConfirmEmailAsync(user,VerificationToken);
            _logger.LogError("VerToken" + VerificationToken);
            if (ver.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in ver.Errors)
                {
                    _logger.LogError("Email confirmation failed: {Code} - {Description}", error.Code, error.Description);
                }
                return Ok("super mario error");
            }              
        }

    }
}
