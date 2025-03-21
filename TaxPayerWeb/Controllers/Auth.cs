using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaxPayerWeb.Dtos;
using Data.Models;
namespace TaxPayerWeb.Controllers
{

    public class Auth : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<Auth> _logger;

        public Auth(UserManager<ApplicationUser> userManager, ILogger<Auth> logger, SignInManager<ApplicationUser> signInManger)
        {
            _userManager = userManager;
            _signInManager = signInManger;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null && !user.EmailConfirmed)
            {
                var VerToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return RedirectToAction("Index", "VerifyEmail", new { VerificationToken = VerToken });
            }
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                ViewData["ErrorMessage"] = "Invalid password or user";

                return View("Index");
            }
            else
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, dto.Password, true, lockoutOnFailure: false);
                if (signInResult.Succeeded)
                {
                    if (!user.EmailConfirmed)
                    {
                        var VerToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        return RedirectToAction("Index", "VerifyEmail", new { VerificationToken = VerToken });
                    }
                    return RedirectToAction("Index", "VerifyEmail");
                }
                else
                {
                    ViewData["ErrorMessage"] = "Invalid password or user";
                    return View("Index");
                }
            }
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            ViewData["Login"] = false;
            var CheckIFEmailExists = await _userManager.FindByEmailAsync(dto.Email);
            if (CheckIFEmailExists != null)
            {
                ViewData["ErrorMessage"] = "Email Already In Use";
                return View("Index");
            }
            var CheckIfUserExists = await _userManager.FindByNameAsync(dto.UserName);
            if (CheckIfUserExists != null)
            {
                ViewData["ErrorMessage"] = "User Name Already In Use";
                return View("Index");
            }

            var user = new ApplicationUser { UserName = dto.UserName, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created successfully. Signing in user: {UserName}", user.UserName);
                try
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    _logger.LogInformation("User signed in successfully: {UserName}", user.UserName);
                    var VerToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    return RedirectToAction("Index", "VerifyEmail", new { VerificationToken = VerToken });

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error signing in user: {UserName}", user.UserName);
                    return View("Index");
                }
            }
            else
            {
                return View("Index");
            }
        }
    }
}
