using hubfast_frontend.Models;
using hubfast_frontend.Services;
using hubfast_frontend.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace hubfast_frontend.Controllers;

public class IntegracaoController : Controller
{
    private IIntegracaoService service;
    
    public IntegracaoController(IIntegracaoService service)
    {
        this.service = service;
    }
    
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Gravar(IntegracaoViewModel viewModel)
    {
        
        if (!ModelState.IsValid)
            return View("Index", viewModel);
        
        //Carregar model para gravação...
        var model = new IntegracaoModel();
        model.IdIntegracao = viewModel.IdIntegracao;
        model.NomeIntegracao = viewModel.NomeIntegracao;
        model.TipoIntegracao = viewModel.TipoIntegracao;
        model.OpcaoHealthcheck = viewModel.OpcaoHealthcheck;
        model.OpcaoAuthorization = viewModel.OpcaoAuthorization;
        model.OpcaoLogService = viewModel.OpcaoLogService;
        model.OpcaoSwagger = viewModel.OpcaoSwagger;
        
        model = service.gravarIntegracao(model);
        
        viewModel.IdIntegracao = model.IdIntegracao;
        
        return View("Index", viewModel);
        
    }
}