using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using hubfast_frontend.Models;
using hubfast_frontend.Services;
using hubfast_frontend.Services.Models;

namespace hubfast_frontend.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IIntegracaoService _service;

    public HomeController(ILogger<HomeController> logger, IIntegracaoService service)
    {
        _logger = logger;
        _service = service;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    public IActionResult Index()
    {
        var listaIntegracoes = _service.listarIntegracao();
        return View(listaIntegracoes);
    }

}