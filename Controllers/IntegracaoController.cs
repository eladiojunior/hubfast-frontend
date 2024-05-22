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
            model.TipoIntegracao = EnumsHelper.EnumPorCodigo<TipoIntegracaoEnum>(viewModel.CodigoTipoIntegracao);
            model.DescricaoIntegracao = viewModel.DescricaoIntegracao;
            model.OpcaoHealthcheck = viewModel.OpcaoHealthcheck;
            model.OpcaoAuthorization = viewModel.OpcaoAuthorization;
            model.OpcaoLogService = viewModel.OpcaoLogService;
            model.OpcaoSwagger = viewModel.OpcaoSwagger;

            model = _integracaoService.gravarIntegracao(model);

            return RedirectToAction("CarregarEditar", "Integracao", new { Idintegracao = model.IdIntegracao });
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
            viewModel.CodigoTipoIntegracao = model.TipoIntegracao.CodigoEnum();
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
            model.TipoIntegracao = EnumsHelper.EnumPorCodigo<TipoIntegracaoEnum>(viewModel.CodigoTipoIntegracao);
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
                return JsonResultErro($"Não identificamos a integração com o ID: {idIntegracao}.");

            var model = new OperacoesIntegracaoViewModel();
            model.IdIntegracao = integracao.IdIntegracao;
            model.NomeIntegracao = integracao.NomeIntegracao;
            model.VersaoIntegracao = integracao.VersaoIntegracao;
            model.Operacoes = new List<OperacaoIntegracaoModel>();

            var listaOperacoes = _integracaoService.listarOperacaoIntegracao(idIntegracao);
            if (listaOperacoes != null && listaOperacoes.Count != 0)
                model.Operacoes = listaOperacoes;

            var viewString = _viewRenderService.RenderToStringAsync("Integracao/_OperacoesIntegracaoPartial", model)
                .Result;

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

    //POST: Integracao/GravarOperacao
    [HttpPost]
    public JsonResult GravarOperacao(OperacaoIntegracaoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return JsonResultErro(ModelState);

        try
        {
            var model = new OperacaoIntegracaoModel();
            model.IdOperacao = viewModel.IdOperacao;
            model.TipoMetodoOperacao = EnumsHelper.EnumPorCodigo<TipoMetodoRestEnum>(viewModel.CodigoMetodoOperacao);
            model.NomeOperacao = viewModel.NomeOperacao;
            model.JsonRequest = viewModel.JsonRequestOperacao;
            model.AtributosRequest = ConvertJsonToAtributos(viewModel.JsonRequestOperacao);
            model.JsonResponse = viewModel.JsonResponseOperacao;
            model.AtributosResponse = ConvertJsonToAtributos(viewModel.JsonResponseOperacao);

            var operacao = _integracaoService.gravarOperacaoIntegracao(viewModel.IdIntegracao, model);
            if (operacao == null)
                return JsonResultErro($"Operação não foi gravada.");

            viewModel.IdOperacao = operacao.IdOperacao;
            viewModel.NomeOperacao = operacao.NomeOperacao;
            viewModel.JsonRequestOperacao = operacao.JsonRequest;
            viewModel.JsonResponseOperacao = operacao.JsonResponse;

            return ListarOperacoesIntegracao(viewModel.IdIntegracao);
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

    private List<AtributoOperacaoModel> ConvertJsonToAtributos(string json)
    {
        var listAtributos = new List<AtributoOperacaoModel>();
        if (string.IsNullOrEmpty(json))
            throw new NegocioException(
                "Json não informado, não é possível converter em objeto de atributos da operação.");

        var jsonDictionary = new Dictionary<string, object>();
        try
        {
            jsonDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }
        catch (Exception erro)
        {
            _logger.LogError(erro, "ConverteJsonToAtributos");
            throw new NegocioException("Não foi possível desserializar o json, verifique se ele não está inválido.");
        }

        // Iterar sobre os pares chave-valor do JSON
        foreach (var kvp in jsonDictionary)
        {
            var atributo = new AtributoOperacaoModel();

            atributo.NomeAtributo = kvp.Key;
            switch (kvp.Value)
            {
                // Verificar se o valor é um número ou texto
                case int or long or float or double or decimal:
                    atributo.TipoAtributo = TipoAtributoEnum.Numero;
                    atributo.AtributosObjeto = null;
                    atributo.ConteudoAtributo = kvp.Value.ToString();
                    break;
                case string:
                    atributo.TipoAtributo = TipoAtributoEnum.Texto;
                    atributo.AtributosObjeto = null;
                    atributo.ConteudoAtributo = kvp.Value.ToString();
                    break;
                case JArray array:
                    atributo.TipoAtributo = TipoAtributoEnum.Array;
                    foreach (var item in array)
                    {
                        atributo.AtributosObjeto = ConvertJsonToAtributos(array.ToString());
                        break; //Não precisa pegar mais que um objeto;
                    }

                    atributo.AtributosObjeto = null;
                    atributo.ConteudoAtributo = null;
                    break;
                case JObject:
                    atributo.TipoAtributo = TipoAtributoEnum.Objeto;
                    atributo.AtributosObjeto = ConvertJsonToAtributos(kvp.Value.ToString());
                    atributo.ConteudoAtributo = null;
                    break;
            }

            listAtributos.Add(atributo);
        }

        return listAtributos;
    }

    //GET: Integracao/CarregarEdicaoOperacao
    [HttpGet]
    public JsonResult CarregarEdicaoOperacao(string idIntegracao, string idOperacao)
    {
        try
        {
            var operacao = _integracaoService.obterOperacaoPorId(idOperacao);
            if (operacao == null)
                return JsonResultErro($"Não identificamos a operação da integração com o ID: {idOperacao}.");

            var integracao = _integracaoService.obterIntegracaoPorId(idIntegracao);
            if (integracao == null)
                return JsonResultErro($"Não identificamos a integração com o ID: {idIntegracao}.");

            var model = new OperacoesIntegracaoViewModel();
            model.IdIntegracao = integracao.IdIntegracao;
            model.NomeIntegracao = integracao.NomeIntegracao;
            model.VersaoIntegracao = integracao.VersaoIntegracao;

            model.IdOperacao = operacao.IdOperacao;
            model.CodigoMetodoOperacao = operacao.TipoMetodoOperacao.CodigoEnum();
            model.NomeOperacao = operacao.NomeOperacao;
            model.JsonRequestOperacao = operacao.JsonRequest;
            model.JsonResponseOperacao = operacao.JsonResponse;

            model.Operacoes = new List<OperacaoIntegracaoModel>();

            var listaOperacoes = _integracaoService.listarOperacaoIntegracao(idIntegracao);
            if (listaOperacoes != null && listaOperacoes.Count != 0)
                model.Operacoes = listaOperacoes;

            var viewString = _viewRenderService.RenderToStringAsync("Integracao/_OperacoesIntegracaoPartial", model)
                .Result;

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

    //POST: Integracao/RemoverOperacao
    [HttpPost]
    public JsonResult RemoverOperacao(string idOperacao)
    {
        try
        {
            var operacao = _integracaoService.obterOperacaoPorId(idOperacao);
            if (operacao == null)
                return JsonResultErro($"Não identificamos a operação da integração com o ID: {idOperacao}.");

            _integracaoService.removerOperacao(idOperacao);

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