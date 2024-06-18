using hubfast_frontend.Exceptions;
using hubfast_frontend.Models;
using hubfast_frontend.Services;
using hubfast_frontend.Services.Helpers;
using hubfast_frontend.Services.Models;
using hubfast_frontend.Services.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace hubfast_frontend.Controllers;

public class IntegracaoController : GenericController
{
    private readonly ILogger<IntegracaoController> _logger;
    private readonly IIntegracaoService _integracaoService;
    private readonly IViewRenderService _viewRenderService;

    public IntegracaoController(ILogger<IntegracaoController> logger, IIntegracaoService integracaoService,
        IViewRenderService viewRenderService)
    {
        _logger = logger;
        _integracaoService = integracaoService;
        _viewRenderService = viewRenderService;
    }

    [HttpGet]
    public IActionResult NovaIntegracao()
    {
        return View();
    }

    [HttpPost]
    public IActionResult RegistrarIntegracao(NovaIntegracaoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View("NovaIntegracao", viewModel);

        try
        {
            //Carregar model para gravação...
            var model = new IntegracaoModel();
            model.NomeIntegracao = viewModel.NomeIntegracao;
            model.TipoIntegracao = EnumsHelper.EnumPorCodigo<TipoIntegracaoEnum>(viewModel.CodigoTipoIntegracao);
            model.DescricaoIntegracao = viewModel.DescricaoIntegracao;
            model.OpcaoHealthcheck = viewModel.OpcaoHealthcheck;
            model.OpcaoAuthorization = viewModel.OpcaoAuthorization;
            model.OpcaoLogService = viewModel.OpcaoLogService;
            model.OpcaoSwagger = viewModel.OpcaoSwagger;

            model = _integracaoService.gravarIntegracao(model);

            return RedirectToAction("CarregarEdicaoIntegracao", "Integracao", new { Idintegracao = model.IdIntegracao });
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

        return View("NovaIntegracao", viewModel);
        
    }

    [HttpGet]
    public IActionResult CarregarEdicaoIntegracao(string idIntegracao)
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

            var viewModel = new EditarIntegracaoViewModel();
            viewModel.IdIntegracao = model.IdIntegracao;
            viewModel.NomeIntegracao = model.NomeIntegracao;
            viewModel.CodigoTipoIntegracao = model.TipoIntegracao.CodigoEnum();
            viewModel.DescricaoIntegracao = model.DescricaoIntegracao;
            viewModel.OpcaoHealthcheck = model.OpcaoHealthcheck;
            viewModel.OpcaoAuthorization = model.OpcaoAuthorization;
            viewModel.OpcaoLogService = model.OpcaoLogService;
            viewModel.OpcaoSwagger = model.OpcaoSwagger;
            viewModel.VersaoIntegracao = model.VersaoIntegracao;
            viewModel.DescricaoSituacaoIntegracao = model.SituacaoIntegracao.DescricaoEnum();

            return View("EditarIntegracao", viewModel);
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

    [HttpPost]
    public IActionResult GravarEdicaoIntegracao(EditarIntegracaoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View("EditarIntegracao", viewModel);

        try
        {
            //Carregar model para gravação...
            var model = new IntegracaoModel();
            model.IdIntegracao = viewModel.IdIntegracao;
            model.NomeIntegracao = viewModel.NomeIntegracao;
            model.TipoIntegracao = EnumsHelper.EnumPorCodigo<TipoIntegracaoEnum>(viewModel.CodigoTipoIntegracao);
            model.DescricaoIntegracao = viewModel.DescricaoIntegracao;
            model.OpcaoHealthcheck = viewModel.OpcaoHealthcheck;
            model.OpcaoAuthorization = viewModel.OpcaoAuthorization;
            model.OpcaoLogService = viewModel.OpcaoLogService;
            model.OpcaoSwagger = viewModel.OpcaoSwagger;

            model = _integracaoService.gravarIntegracao(model);

            viewModel.VersaoIntegracao = model.VersaoIntegracao;
            viewModel.DescricaoSituacaoIntegracao = model.SituacaoIntegracao.DescricaoEnum();
            
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

        return View("EditarIntegracao", viewModel);
    }

    //GET: Integracao/ListarIntegracoes
    [HttpGet]
    public JsonResult ListarIntegracoes()
    {
        try
        {
            var lista = _integracaoService.listarIntegracao();
            var viewString = _viewRenderService.RenderToStringAsync("Integracao/_ListaIntegracoesPartial", lista).Result;
            return JsonResultSucesso(viewString, "Lista de integrações com sucesso.");
        }
        catch (NegocioException erro_negocio)
        {
            return JsonResultErro(erro_negocio.Message);
        }
        catch (Exception erro)
        {
            var mensagem = $"Erro ao listar de integrações.";
            _logger.LogError(erro, mensagem);
            return JsonResultErro(mensagem);
        }
    }

    //DELETE: Integracao/RemoverIntegracao
    [HttpDelete]
    public JsonResult RemoverIntegracao(string idIntegracao)
    {
        try
        {
            _integracaoService.removerIntegracao(idIntegracao);
            return JsonResultSucesso("Integração removida com sucesso.");
        }
        catch (NegocioException erro_negocio)
        {
            return JsonResultErro(erro_negocio.Message);
        }
        catch (Exception erro)
        {
            var mensagem = $"Erro ao remover a integração.";
            _logger.LogError(erro, mensagem);
            return JsonResultErro(mensagem);
        }
        
    }
}