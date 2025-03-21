using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaxPayerWeb.Dtos;

namespace TaxPayerWeb.Controllers
{
    public class ChangePassword : Controller
    {
        private readonly ILogger<ResetPassword> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public ChangePassword(ILogger<ResetPassword> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ChangePass(ChangePassDto changePassDto)
        {
            IdentityUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View();
            }
            else
            {
                await _userManager.ChangePasswordAsync(user, changePassDto.CurrentPassword, changePassDto.NewPassword);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
