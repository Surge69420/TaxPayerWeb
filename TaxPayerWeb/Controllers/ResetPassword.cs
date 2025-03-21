using System.Runtime.ConstrainedExecution;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaxPayerWeb.Dtos;

namespace TaxPayerWeb.Controllers
{
    public class ResetPassword : Controller
    {
        private readonly ILogger<ResetPassword> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public ResetPassword(ILogger<ResetPassword> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckUser(string email)
        {
            _logger.LogInformation(email);
            if (email == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ApplicationUser? user = _userManager.FindByEmailAsync(email).Result;
                if (user != null)
                {
                    return RedirectToAction("ResetPasswordPage", new { Email = email });
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ResetPasswordPage(string Email)
        {
            ApplicationUser? user = _userManager.FindByEmailAsync(Email).Result;
            if (string.IsNullOrEmpty(Email) || user == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var Token = await _userManager.GeneratePasswordResetTokenAsync(user);
                ViewData["Email"] = Email;
                ViewData["Token"] = Token;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordPost(ResetPassDto resetPassDto)
        {
            var user = _userManager.FindByEmailAsync(resetPassDto.Email).Result;
            if (user != null)
            {
                var res = await _userManager.ResetPasswordAsync(user, resetPassDto.Token, resetPassDto.Password);
                if (res.Succeeded) {
                    return RedirectToAction("Index", "Auth");
                    }
                else
                {
                    foreach (var error in res.Errors)
                    {
                        _logger.LogError("Pass Reset failed: {Code} - {Description}", error.Code, error.Description);

                    }
                    return RedirectToAction("Index");
                }
            }
            else { return RedirectToAction("Index"); }
        }
    }
}
