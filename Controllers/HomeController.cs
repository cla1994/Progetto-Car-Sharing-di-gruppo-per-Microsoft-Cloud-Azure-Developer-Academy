using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Academy2023.Net.Models;
using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Identity.Web;
using NuGet.Protocol;

namespace Academy2023.Net.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DataContext _context;

    public HomeController(ILogger<HomeController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var userAuth = _context.usersData
            .Where(x => x.AuthID == this.User.Identity.Name)
            .FirstOrDefault();
        if (userAuth != null)
        {
            return RedirectToAction("Details", "UserDatas", new { Id = userAuth.UserDataID });
        }
        //ViewData["Auth"] = AuthenticationID;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
