using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaxPayerWeb.Dtos;

namespace TaxPayerWeb.Controllers
{

    public class ProfilesPage : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<Auth> _logger;

        public ProfilesPage(UserManager<IdentityUser> userManager, ILogger<Auth> logger, SignInManager<IdentityUser> signInManger)
        {
            _userManager = userManager;
            _signInManager = signInManger;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            IdentityUser? user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            return View(user);
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
