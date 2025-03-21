using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaxPayerWeb.Dtos;

namespace TaxPayerWeb.Controllers
{

    public class ProfilesPage : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<Auth> _logger;

        public ProfilesPage(UserManager<ApplicationUser> userManager, ILogger<Auth> logger, SignInManager<ApplicationUser> signInManger)
        {
            _userManager = userManager;
            _signInManager = signInManger;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            return View(user);
        }
        public async Task<IActionResult> ChangePass(ChangePassDto changePassDto)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
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
        public async Task<IActionResult> ChangeProfile(IFormFile Img)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                _logger.LogError("no user");
            }
            _logger.LogInformation("Yes user");
            MemoryStream ms = new MemoryStream();
            Img.CopyTo(ms);
            user.ImageData = ms.ToArray();
            ms.Close();
            ms.Dispose();

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // You can add a success message or redirect
                _logger.LogInformation("WORKED");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> SignOutUser()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
