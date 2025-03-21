using System.Diagnostics;
using System.Text.RegularExpressions;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaxPayerWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace TaxPayerWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DbService _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public HomeController(ILogger<HomeController> logger, DbService db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _logger = logger;
        _db = db;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [Authorize]
    public async Task<IActionResult> Index(string query)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user != null && !user.EmailConfirmed)
        {
            // If email is not confirmed, redirect them to the login page or another page
            return RedirectToAction("Index", "Auth");
        }

        Console.WriteLine(User?.Identity?.IsAuthenticated);
        var taxPayers = _db.queryDatabase();

        if (!string.IsNullOrEmpty(query))
        {
            taxPayers = taxPayers.Where(x => (x.Name?.Contains(query) ?? false) || (x.Address?.Contains(query) ?? false) || (x.PostalCode?.Contains(query) ?? false)).ToList();
        }
        return View(taxPayers);

    }



    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateTable(Dtos.TaxPayerDto taxPayer)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user != null && !user.EmailConfirmed)
        {
            // If email is not confirmed, redirect them to the login page or another page
            return RedirectToAction("Index", "Auth");
        }
        MemoryStream ms = new MemoryStream();
        taxPayer.ImageData.CopyTo(ms);
        string pcodeNum = Regex.Match(taxPayer.PostalCode.Trim(), @"^\d+").ToString();
        var city = _db.GetCity(Convert.ToInt32(pcodeNum));
        var taxp = new TaxPayer
        {
            Name = taxPayer.Name,
            Address = taxPayer.Address,
            PostalCode = taxPayer.PostalCode,
            CityName = city,
            ImageData = ms.ToArray()
        };
        ms.Close();
        ms.Dispose();
        var result = _db.CreateTable(taxp);
        Console.WriteLine(result);
        return RedirectToAction("Index");
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> EditTable(Data.Models.TaxPayer taxPayer)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user != null && !user.EmailConfirmed || String.IsNullOrEmpty(taxPayer.PostalCode))
        {
            // If email is not confirmed, redirect them to the login page or another page
            return RedirectToAction("Index", "Auth");
        }
        string pcodeNum = Regex.Match(taxPayer.PostalCode.Trim(), @"^\d+").ToString();
        taxPayer.CityName = _db.GetCity(Convert.ToInt32(pcodeNum));
        var result = _db.UpdateTable(taxPayer);
        Console.WriteLine(result);
        return RedirectToAction("Index");
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteTable(Data.Models.TaxPayer taxPayer)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user != null && !user.EmailConfirmed)
        {
            // If email is not confirmed, redirect them to the login page or another page
            return RedirectToAction("Index", "Auth");
        }
        var result = _db.DeleteTable(taxPayer);
        Console.WriteLine(result);
        return RedirectToAction("Index");
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
