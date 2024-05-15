using hubfast_frontend.Exceptions;
using hubfast_frontend.Models;
using hubfast_frontend.Services;
using hubfast_frontend.Services.Helpers;
using hubfast_frontend.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace hubfast_frontend.Controllers;

public class IntegracaoController : GenericController
{
    private readonly ILogger<IntegracaoController> _logger;
    private readonly IIntegracaoService _integracaoService;
    private readonly IViewRenderService _viewRenderService;
    
    public IntegracaoController(ILogger<IntegracaoController> logger, IIntegracaoService integracaoService, IViewRenderService viewRenderService)
    {
        _logger = logger;
        _integracaoService = integracaoService;
        _viewRenderService = viewRenderService;
    }

    public IActionResult Novo()
    {
        return View();
    }

    public IActionResult Gravar(GravarIntegracaoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View("Novo", viewModel);

        try
        {
            //Carregar model para gravação...
            var model = new IntegracaoModel();
            model.NomeIntegracao = viewModel.NomeIntegracao;
            model.TipoIntegracao = viewModel.TipoIntegracao;
            model.DescricaoIntegracao = viewModel.DescricaoIntegracao;
            model.OpcaoHealthcheck = viewModel.OpcaoHealthcheck;
            model.OpcaoAuthorization = viewModel.OpcaoAuthorization;
            model.OpcaoLogService = viewModel.OpcaoLogService;
            model.OpcaoSwagger = viewModel.OpcaoSwagger;

            model = _integracaoService.gravarIntegracao(model);

            return CarregarEditar(model.IdIntegracao);
        }
        catch (NegocioException erro_negocio)
        {
            ModelState.AddModelError("erro_negocio", erro_negocio.Message);
        }
        catch (Exception erro)
        {
            var mensagem = "Erro ao gravar uma nova integração.";
            _logger.LogError(erro, mensagem);
            ModelState.AddModelError("erro_interno", mensagem);
        }

        return View("Novo", viewModel);
    }

    public IActionResult CarregarEditar(string idIntegracao)
    {
        try
        {
            if (string.IsNullOrEmpty(idIntegracao))
            {
                _logger.LogInformation("Identficador não informado.");
                return RedirectToAction("Index", "Home");
            }

            var model = _integracaoService.obterIntegracaoPorId(idIntegracao);
            if (model == null)
            {
                _logger.LogInformation("Não encontramos nenhuma integração com esse identificador.");
                return RedirectToAction("Index", "Home");
            }

            var viewModel = new EdicaoIntegracaoViewModel();
            viewModel.IdIntegracao = model.IdIntegracao;
            viewModel.NomeIntegracao = model.NomeIntegracao;
            viewModel.TipoIntegracao = model.TipoIntegracao;
            viewModel.DescricaoIntegracao = model.DescricaoIntegracao;
            viewModel.OpcaoHealthcheck = model.OpcaoHealthcheck;
            viewModel.OpcaoAuthorization = model.OpcaoAuthorization;
            viewModel.OpcaoLogService = model.OpcaoLogService;
            viewModel.OpcaoSwagger = model.OpcaoSwagger;
            viewModel.VersaoIntegracao = model.VersaoIntegracao;
            viewModel.DescricaoSituacaoIntegracao = model.SituacaoIntegracao.DescricaoEnum();

            return View("Edicao", viewModel);
        }
        catch (NegocioException erro_negocio)
        {
            ModelState.AddModelError("erro_negocio", erro_negocio.Message);
        }
        catch (Exception erro)
        {
            var mensagem = $"Erro ao carregar uma integração [{idIntegracao}] para edição.";
            _logger.LogError(erro, mensagem);
            ModelState.AddModelError("erro_interno", mensagem);
        }

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Editar(EdicaoIntegracaoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View("Edicao", viewModel);

        try
        {
            //Carregar model para gravação...
            var model = new IntegracaoModel();
            model.IdIntegracao = viewModel.IdIntegracao;
            model.NomeIntegracao = viewModel.NomeIntegracao;
            model.TipoIntegracao = viewModel.TipoIntegracao;
            model.DescricaoIntegracao = viewModel.DescricaoIntegracao;
            model.OpcaoHealthcheck = viewModel.OpcaoHealthcheck;
            model.OpcaoAuthorization = viewModel.OpcaoAuthorization;
            model.OpcaoLogService = viewModel.OpcaoLogService;
            model.OpcaoSwagger = viewModel.OpcaoSwagger;

            _integracaoService.gravarIntegracao(model);
        }
        catch (NegocioException erro_negocio)
        {
            ModelState.AddModelError("erro_negocio", erro_negocio.Message);
        }
        catch (Exception erro)
        {
            var mensagem = $"Erro ao editar uma integração [{viewModel.IdIntegracao}].";
            _logger.LogError(erro, mensagem);
            ModelState.AddModelError("erro_interno", mensagem);
        }

        return View("Edicao", viewModel);
    }

    public IActionResult Remover(string idIntegracao)
    {
        if (string.IsNullOrEmpty(idIntegracao))
            return RedirectToAction("Index", "Home");

        _integracaoService.removerIntegracao(idIntegracao);

        return RedirectToAction("Index", "Home");
    }

    //GET: Integracao/ListarOperacoesIntegracao
    [HttpGet]
    public JsonResult ListarOperacoesIntegracao(string idIntegracao)
    {
        try
        {

            var integracao = _integracaoService.obterIntegracaoPorId(idIntegracao);
            if (integracao == null)
                return JsonResultErro($"Não idenfitificamos a integração com o ID: {idIntegracao}.");

            var model = new OperacoesIntegracaoViewModel();
            model.IdIntegracao = integracao.IdIntegracao;
            model.NomeIntegracao = integracao.NomeIntegracao;
            model.VersaoIntegracao = integracao.VersaoIntegracao;
            model.OperacoesIntegracao = new List<OperacaoIntegracaoModel>();

            var listaOperacoes = _integracaoService.listarOperacaoIntegracao(idIntegracao);
            if (listaOperacoes != null && listaOperacoes.Count != 0)
                model.OperacoesIntegracao = listaOperacoes;

            var viewString = _viewRenderService.RenderToStringAsync("Integracao/_OperacoesIntegracaoPartial", model).Result;

            return JsonResultSucesso(viewString, "Sucesso.");

        }
        catch (NegocioException erro_negocio)
        {
            return JsonResultErro(erro_negocio.Message);
        }
        catch (Exception erro)
        {
            var mensagem = $"Erro ao listar as operações da integração [{idIntegracao}].";
            _logger.LogError(erro, mensagem);
            return JsonResultErro(mensagem);
        }
    }
}