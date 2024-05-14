using hubfast_frontend.Exceptions;
using hubfast_frontend.Models;
using hubfast_frontend.Services;
using hubfast_frontend.Services.Helpers;
using hubfast_frontend.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace hubfast_frontend.Controllers;

public class IntegracaoController : Controller
{
    private readonly ILogger<IntegracaoController> _logger;
    private IIntegracaoService _service;

    public IntegracaoController(ILogger<IntegracaoController> logger, IIntegracaoService service)
    {
        _logger = logger;
        _service = service;
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

            model = _service.gravarIntegracao(model);

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

            var model = _service.obterIntegracaoPorId(idIntegracao);
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

            _service.gravarIntegracao(model);
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

        _service.removerIntegracao(idIntegracao);

        return RedirectToAction("Index", "Home");
    }
}