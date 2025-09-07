using BookVerse.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookVerse.Web.Controllers;

public class HomeController : BaseController
{
    [AllowAnonymous]
    public IActionResult Index()
    {
        if (IsUserAuthenticated()) return RedirectToAction(nameof(Index), "Book");

        return View();
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