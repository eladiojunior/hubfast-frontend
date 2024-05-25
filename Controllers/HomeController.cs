using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using hubfast_frontend.Models;
using hubfast_frontend.Services;
using hubfast_frontend.Services.Models;

namespace hubfast_frontend.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

}