using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaxPayerWeb.Dtos;
namespace TaxPayerWeb.Controllers
{

    public class Auth : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<Auth> _logger;

        public Auth(UserManager<IdentityUser> userManager, ILogger<Auth> logger, SignInManager<IdentityUser> signInManger)
        {
            _userManager = userManager;
            _signInManager = signInManger;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDto dto)
        {
          
            
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
                    return RedirectToAction("Index", "Home");
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

            var user = new IdentityUser { UserName = dto.UserName, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created successfully. Signing in user: {UserName}", user.UserName);
                try
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    _logger.LogInformation("User signed in successfully: {UserName}", user.UserName);
                    return RedirectToAction("Index", "Home");

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


        public async Task<IActionResult> Logins()
        {
            var user = await _userManager.GetUserAsync(User);
            var existingUser = await _userManager.FindByNameAsync("adadl");

            // If the user is already logged in, you can either skip creating the user or redirect them
            if (User.Identity.IsAuthenticated && user != null)
            {
                _logger.LogError(User.Identity.Name);
                _logger.LogError("Redirecting");
                // Redirect to another page or show a message since the user is already logged in
                return RedirectToAction("Index", "Home");
            }
            else if (existingUser != null)
            {


                var signInResult = await _signInManager.PasswordSignInAsync("adadl",
               "Uz004@23", true,
             lockoutOnFailure: false);
                _logger.LogError("Password Sign In");

                _logger.LogError(JsonSerializer.Serialize(signInResult));
                return RedirectToAction("Index", "Home");
            }
            _logger.LogInformation("Index action called.");

            user = new IdentityUser { UserName = "adadl", Email = "adadada@gmail.com" };
            _logger.LogInformation("Creating user with username: {UserName} and email: {Email}", user.UserName, user.Email);

            var result = await _userManager.CreateAsync(user, "Uz004@23");
            if (result.Succeeded)
            {
                _logger.LogInformation("User created successfully. Signing in user: {UserName}", user.UserName);
                try
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    _logger.LogInformation("User signed in successfully: {UserName}", user.UserName);
                    return LocalRedirect("/Home/Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error signing in user: {UserName}", user.UserName);
                }
            }

            foreach (var error in result.Errors)
            {
                _logger.LogError("Error creating user: {Code} - {Description}", error.Code, error.Description);
            }

            _logger.LogInformation("Returning view due to user creation failure.");
            return Ok("FAILURE");
        }
    }
}
