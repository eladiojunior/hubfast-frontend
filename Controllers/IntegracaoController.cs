using Microsoft.AspNetCore.Mvc;

namespace hubfast_frontend.Controllers;

public class IntegracaoController : Controller
{
    // GET
    public IActionResult Nova()
    {
        return View();
    }
}