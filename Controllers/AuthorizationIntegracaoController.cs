using hubfast_frontend.Exceptions;
using hubfast_frontend.Models;
using hubfast_frontend.Services;
using hubfast_frontend.Services.Helpers;
using hubfast_frontend.Services.Models;
using hubfast_frontend.Services.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace hubfast_frontend.Controllers;

public class AuthorizationIntegracaoController: GenericController
{
    private readonly ILogger<AuthorizationIntegracaoController> _logger;
    private readonly IViewRenderService _viewRenderService;
    private readonly IIntegracaoService _integracaoService;
    
    public AuthorizationIntegracaoController(ILogger<AuthorizationIntegracaoController> logger, IViewRenderService viewRenderService, IIntegracaoService integracaoService)
    {
        _logger = logger;
        _viewRenderService = viewRenderService;
        _integracaoService = integracaoService; 
    }
    
    [HttpGet]
    public JsonResult ObterAutorizacao(string idIntegracao)
    {
        try
        {
            
            var integracao = _integracaoService.obterIntegracaoPorId(idIntegracao);
            if (integracao == null)
                return JsonResultErro($"Não identificamos a integração com o ID: {idIntegracao}.");

            var model = new AuthorizationIntegracaoViewModel();
            model.IdIntegracao = integracao.IdIntegracao;
            model.NomeIntegracao = integracao.NomeIntegracao;
            model.VersaoIntegracao = integracao.VersaoIntegracao;
            model.CodigoTipoAutorizacao = TipoAutorizacaoEnum.NoAuth.CodigoEnum();

            var autorizacaoIntegracao = _integracaoService.obterAutorizacaoIntegracao(idIntegracao);
            if (autorizacaoIntegracao != null)
            {
                model.IdAutorizacao = autorizacaoIntegracao.IdAutorizacao;
                model.CodigoTipoAutorizacao = autorizacaoIntegracao.TipoAutorizacao.CodigoEnum();
                model.ParmsAutorizacao = autorizacaoIntegracao.ParmsAutorizacao;
            }
            
            var viewString = _viewRenderService.RenderToStringAsync("AuthorizationIntegracao/_AuthorizationPartial", model).Result;
            return JsonResultSucesso(viewString, "Authorization carregado.");

        }
        catch (NegocioException erroNegocio)
        {
            return JsonResultErro(erroNegocio.Message);
        }
        catch (Exception erro)
        {
            var mensagem = $"Erro ao iniciar configuração da Authorization da integração [{idIntegracao}].";
            _logger.LogError(erro, mensagem);
            return JsonResultErro(mensagem);
        }
    }
    
    [HttpPost]
    public JsonResult GravarAutorizacao(AuthorizationIntegracaoViewModel integracaoViewModel)
    {
        if (!ModelState.IsValid)
            return JsonResultErro(ModelState);

        try
        {
            var model = new AuthorizationIntegracaoModel();
            model.IdIntegracao = integracaoViewModel.IdIntegracao;
            model.TipoAutorizacao = EnumsHelper.EnumPorCodigo<TipoAutorizacaoEnum>(integracaoViewModel.CodigoTipoAutorizacao);
            model.ParmsAutorizacao = integracaoViewModel.ParmsAutorizacao;

            var autorizacao = _integracaoService.gravarAutorizacaoIntegracao(model);
            if (autorizacao == null)
                return JsonResultErro($"Authorization não foi gravada.");

            return JsonResultSucesso(autorizacao, "Authorization gravada com sucesso.");
            
        }
        catch (NegocioException erroNegocio)
        {
            return JsonResultErro(erroNegocio.Message);
        }
        catch (Exception erro)
        {
            var mensagem = $"Erro ao gravar a configuração da authorization da integração [{integracaoViewModel.IdIntegracao}].";
            _logger.LogError(erro, mensagem);
            return JsonResultErro(mensagem);
        }

    }
    
    [HttpGet]
    public JsonResult ObterTipoAutorizacao(string idIntegracao, int codigoTipoAutenticacao)
    {
        try
        {
            
            var integracao = _integracaoService.obterIntegracaoPorId(idIntegracao);
            if (integracao == null)
                return JsonResultErro($"Não identificamos a integração com o ID: {idIntegracao}.");

            var viewPath = "AuthorizationIntegracao/_AuthorizationGeralPartial";
            if (codigoTipoAutenticacao == TipoAutorizacaoEnum.NoAuth.CodigoEnum())
            {
                var viewStringNoAuth = _viewRenderService.RenderToStringAsync(viewPath, null).Result;
                return JsonResultSucesso(viewStringNoAuth, "Authorization type carregada.");
            }
            
            var parmsAuth = new List<ParmAutorizacaoModel>();
            var autorizacaoIntegracao = _integracaoService.obterAutorizacaoIntegracao(idIntegracao);
            if (autorizacaoIntegracao != null && 
                codigoTipoAutenticacao == autorizacaoIntegracao.TipoAutorizacao.CodigoEnum())
            {
                parmsAuth = autorizacaoIntegracao.ParmsAutorizacao;
            }
            else
            {
                parmsAuth = InitParmsTipoAutenticacao(codigoTipoAutenticacao);
            }
            
            if (codigoTipoAutenticacao == TipoAutorizacaoEnum.BasicAuth.CodigoEnum())
                viewPath = "AuthorizationIntegracao/_AuthorizationBasicAuthPartial";
            else if (codigoTipoAutenticacao == TipoAutorizacaoEnum.OAuth20.CodigoEnum())
                viewPath = "AuthorizationIntegracao/_AuthorizationOAuth20Partial";
            
            var viewString = _viewRenderService.RenderToStringAsync(viewPath, parmsAuth).Result;
            return JsonResultSucesso(viewString, "Authorization type carregada.");

        }
        catch (NegocioException erroNegocio)
        {
            return JsonResultErro(erroNegocio.Message);
        }
        catch (Exception erro)
        {
            var mensagem = $"Erro ao iniciar configuração da Authorization da integração [{idIntegracao}].";
            _logger.LogError(erro, mensagem);
            return JsonResultErro(mensagem);
        }
    }

    private List<ParmAutorizacaoModel> InitParmsTipoAutenticacao(int codigoTipoAutenticacao)
    {
        var paramsAuth = new List<ParmAutorizacaoModel>();
        if (codigoTipoAutenticacao == TipoAutorizacaoEnum.BasicAuth.CodigoEnum()) 
        {
            paramsAuth.Add(new ParmAutorizacaoModel("Username"));
            paramsAuth.Add(new ParmAutorizacaoModel("Password"));
        } 
        else if (codigoTipoAutenticacao == TipoAutorizacaoEnum.BearerToken.CodigoEnum())
        {
            paramsAuth.Add(new ParmAutorizacaoModel("Token"));
        }
        else if (codigoTipoAutenticacao == TipoAutorizacaoEnum.OAuth20.CodigoEnum())
        {
            paramsAuth.Add(new ParmAutorizacaoModel("GrantType", "1"));
            paramsAuth.Add(new ParmAutorizacaoModel("AccessTokenURL"));
            paramsAuth.Add(new ParmAutorizacaoModel("ClientID"));
            paramsAuth.Add(new ParmAutorizacaoModel("ClientSecret"));
        }
        else if (codigoTipoAutenticacao == TipoAutorizacaoEnum.APIKey.CodigoEnum())
        {
            paramsAuth.Add(new ParmAutorizacaoModel("Key"));
            paramsAuth.Add(new ParmAutorizacaoModel("Value"));
        }
        return paramsAuth;
    }
}