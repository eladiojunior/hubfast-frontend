using hubfast_frontend.Exceptions;
using hubfast_frontend.Models;
using hubfast_frontend.Services;
using hubfast_frontend.Services.Helpers;
using hubfast_frontend.Services.Models;
using hubfast_frontend.Services.Models.Enums;
using hubfast_frontend.Views.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace hubfast_frontend.Controllers;

public class OperacaoIntegracaoController: GenericController
{
    private readonly ILogger<OperacaoIntegracaoController> _logger;
    private readonly IIntegracaoService _integracaoService;
    private readonly IOperacaoIntegracaoService _operacaoIntegracaoService;
    private readonly IViewRenderService _viewRenderService;

    public OperacaoIntegracaoController(ILogger<OperacaoIntegracaoController> logger, IIntegracaoService integracaoService, 
        IOperacaoIntegracaoService operacaoIntegracaoService, IViewRenderService viewRenderService)
    {
        _logger = logger;
        _integracaoService = integracaoService;
        _operacaoIntegracaoService = operacaoIntegracaoService;
        _viewRenderService = viewRenderService;
    }
    
    //GET: OperacaoIntegracao/NovaOperacao
    [HttpGet]
    public JsonResult NovaOperacao(string idIntegracao)
    {
        try
        {
            var integracao = _integracaoService.obterIntegracaoPorId(idIntegracao);
            if (integracao == null)
                return JsonResultErro($"Não identificamos a integração com o ID: {idIntegracao}.");

            var model = new OperacaoIntegracaoViewModel();
            model.IdIntegracao = integracao.IdIntegracao;
            model.NomeIntegracao = integracao.NomeIntegracao;
            model.VersaoIntegracao = integracao.VersaoIntegracao;

            var viewString = _viewRenderService.RenderToStringAsync("OperacaoIntegracao/_OperacaoIntegracaoPartial", model).Result;
            return JsonResultSucesso(viewString, "Nova operação pronta para registro.");
        }
        catch (NegocioException erro_negocio)
        {
            return JsonResultErro(erro_negocio.Message);
        }
        catch (Exception erro)
        {
            var mensagem = $"Erro ao iniciar nova operação da integração [{idIntegracao}].";
            _logger.LogError(erro, mensagem);
            return JsonResultErro(mensagem);
        }
    }
    
    //GET: OperacaoIntegracao/ListarOperacao
    [HttpGet]
    public JsonResult ListarOperacao(string idIntegracao)
    {
        try
        {
            var integracao = _integracaoService.obterIntegracaoPorId(idIntegracao);
            if (integracao == null)
                return JsonResultErro($"Não identificamos a integração com o ID: {idIntegracao}.");

            var listaOperacoes = _operacaoIntegracaoService.listarOperacaoIntegracao(idIntegracao);
            var viewString = _viewRenderService.RenderToStringAsync("OperacaoIntegracao/_ListaOperacoesIntegracaoPartial", listaOperacoes).Result;

            return JsonResultSucesso(viewString, "Lista de operações carregada com sucesso.");
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
    
    //POST: OperacaoIntegracao/GravarOperacao
    [HttpPost]
    public JsonResult GravarOperacao(OperacaoIntegracaoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return JsonResultErro(ModelState);

        try
        {
            var model = new OperacaoIntegracaoModel();
            model.IdIntegracao = viewModel.IdIntegracao;
            model.IdOperacao = viewModel.IdOperacao;
            model.TipoMetodoOperacao = EnumsHelper.EnumPorCodigo<TipoMetodoRestEnum>(viewModel.CodigoMetodoOperacao);
            model.NomeOperacao = viewModel.NomeOperacao;
            model.JsonRequest = viewModel.JsonRequestOperacao;
            model.AtributosRequest = JsonHelper.ConvertJsonToAtributos(viewModel.JsonRequestOperacao);
            model.JsonResponse = viewModel.JsonResponseOperacao;
            model.AtributosResponse = JsonHelper.ConvertJsonToAtributos(viewModel.JsonResponseOperacao);

            var operacao = _operacaoIntegracaoService.gravarOperacaoIntegracao(model);
            if (operacao == null)
                return JsonResultErro($"Operação não foi gravada.");

            return JsonResultSucesso(operacao, "Operação gravada com sucesso.");
            
        }
        catch (NegocioException erro_negocio)
        {
            return JsonResultErro(erro_negocio.Message);
        }
        catch (Exception erro)
        {
            var mensagem = $"Erro ao gravar a operação da integração [{viewModel.IdIntegracao}].";
            _logger.LogError(erro, mensagem);
            return JsonResultErro(mensagem);
        }
    }

    //GET: OperacaoIntegracao/CarregarEdicaoOperacao
    [HttpGet]
    public JsonResult CarregarEdicaoOperacao(string idIntegracao, string idOperacao)
    {
        try
        {
            var operacao = _operacaoIntegracaoService.obterOperacaoIntegracaoPorId(idOperacao);
            if (operacao == null)
                return JsonResultErro($"Não identificamos a operação da integração com o ID: {idOperacao}.");

            var integracao = _integracaoService.obterIntegracaoPorId(idIntegracao);
            if (integracao == null)
                return JsonResultErro($"Não identificamos a integração com o ID: {idIntegracao}.");

            var model = new OperacaoIntegracaoViewModel();
            model.IdIntegracao = integracao.IdIntegracao;
            model.NomeIntegracao = integracao.NomeIntegracao;
            model.VersaoIntegracao = integracao.VersaoIntegracao;

            model.IdOperacao = operacao.IdOperacao;
            model.CodigoMetodoOperacao = operacao.TipoMetodoOperacao.CodigoEnum();
            model.NomeOperacao = operacao.NomeOperacao;
            model.JsonRequestOperacao = operacao.JsonRequest;
            model.JsonResponseOperacao = operacao.JsonResponse;

            var viewString = _viewRenderService.RenderToStringAsync("OperacaoIntegracao/_OperacaoIntegracaoPartial", model).Result;

            return JsonResultSucesso(viewString, "Informações da operação carrega com sucesso.");
        }
        catch (NegocioException erro_negocio)
        {
            return JsonResultErro(erro_negocio.Message);
        }
        catch (Exception erro)
        {
            var mensagem = $"Erro ao carregar operação da integração [{idOperacao}].";
            _logger.LogError(erro, mensagem);
            return JsonResultErro(mensagem);
        }
    }

    //DELETE: OperacaoIntegracao/RemoverOperacao
    [HttpDelete]
    public JsonResult RemoverOperacao(string idOperacao)
    {
        try
        {
            var operacao = _operacaoIntegracaoService.obterOperacaoIntegracaoPorId(idOperacao);
            if (operacao == null)
                return JsonResultErro($"Não identificamos a operação da integração com o ID: {idOperacao}.");

            _operacaoIntegracaoService.removerOperacaoIntegracao(idOperacao);

            return JsonResultSucesso("Operação da integração removida com sucesso.");
        }
        catch (NegocioException erro_negocio)
        {
            return JsonResultErro(erro_negocio.Message);
        }
        catch (Exception erro)
        {
            var mensagem = $"Erro ao remover operação da integração [{idOperacao}].";
            _logger.LogError(erro, mensagem);
            return JsonResultErro(mensagem);
        }
    }
    
}