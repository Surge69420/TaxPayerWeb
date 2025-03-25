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
            if (user == null)
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
                ViewData["ErrorMessage"] = "User does not exist";
            }
            else
            {
                if (await _userManager.CheckPasswordAsync(user, changePassDto.CurrentPassword))
                {
                    try
                    {
                        await _userManager.ChangePasswordAsync(user, changePassDto.CurrentPassword, changePassDto.NewPassword);
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception e)
                    {
                        ViewData["ErrorMessage"] = "Failed Changing Password " + e;
                    }
                }
                else
                {
                    ViewData["ErrorMessage"] = "Current Password Is Invalid ";
                }
            }
            return View("Index");
        }
        public async Task<IActionResult> ChangeProfile(IFormFile Img)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ViewData["ErrorMessage"] = "User does not exist";
            }
            else
            {
                try
                {
                    MemoryStream ms = new MemoryStream();
                    Img.CopyTo(ms);
                    user.ImageData = ms.ToArray();
                    ms.Close();
                    ms.Dispose();

                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        var errorcodes = "";
                        foreach (var error in result.Errors)
                        {
                            errorcodes += error.Code + "" + error.Description;

                        }
                        ViewData["ErrorMessage"] = "Changing Profile Failed " + errorcodes;
                    }
                }
                catch (Exception e)
                {
                    ViewData["ErrorMessage"] = "Failed Changing Profile " + e;
                }
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
