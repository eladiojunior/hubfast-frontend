using Microsoft.AspNetCore.Mvc;

namespace hubfast_frontend.Controllers;

public class HubController: GenericController
{
    [HttpGet]
    public IActionResult Conexao()
    {
        return View();
    }
}