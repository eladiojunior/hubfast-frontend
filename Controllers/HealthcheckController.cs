using hubfast_frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace hubfast_frontend.Controllers;

public class HealthcheckController: GenericController
{
    private readonly ILogger<HealthcheckController> _logger;
    private readonly IViewRenderService _viewRenderService;
    
    public HealthcheckController(ILogger<HealthcheckController> logger, IViewRenderService viewRenderService)
    {
        _logger = logger;
        _viewRenderService = viewRenderService;
    }
    
    [HttpGet]
    public JsonResult Configuracao()
    {
        var viewString = _viewRenderService.RenderToStringAsync("Healthcheck/_HealthcheckPartial", null).Result;
        return JsonResultSucesso(viewString, "Healthcheck carregado.");
    }
    
}