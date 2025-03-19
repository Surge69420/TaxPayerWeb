using System.Diagnostics;
using System.Text.Json.Nodes;
using Data.Services;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

using TaxPayerWeb.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Data.Models;
using System.Net;

namespace TaxPayerWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DbService _db;

    public HomeController(ILogger<HomeController> logger, DbService db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index(string query)
    {
        var taxPayers = _db.queryDatabase();

        if (!string.IsNullOrEmpty(query))
        {
            taxPayers = taxPayers.Where(x => x.Name.Contains(query) || x.Address.Contains(query) || x.PostalCode.ToString().Contains(query)).ToList();
        }
        return View(taxPayers);
    }
    [HttpPost]
    public IActionResult CreateTable(TaxPayerFormModel taxPayer)
    {
        var taxp = new TaxPayer
        {
            Name = taxPayer.Name,
            Address = taxPayer.Address,
            PostalCode = Convert.ToInt32(taxPayer.PostalCode)
        };
        var result = _db.CreateTable(taxp);
        Console.WriteLine(result);
        return RedirectToAction("Index");
    }
    [HttpPost]
    public IActionResult EditTable(Data.Models.TaxPayer taxPayer)
    {

        var result = _db.UpdateTable(taxPayer);
        Console.WriteLine(result);
        return RedirectToAction("Index");
    }
    [HttpPost]
    public IActionResult DeleteTable(Data.Models.TaxPayer taxPayer)
    {

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
