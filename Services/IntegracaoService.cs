using hubfast_frontend.Exceptions;
using hubfast_frontend.Services.Helpers;
using hubfast_frontend.Services.Models;
using hubfast_frontend.Services.Models.Enums;

namespace hubfast_frontend.Services;

public class IntegracaoService: IIntegracaoService
{
    private static List<IntegracaoModel> _listIntegracoesTemp = new List<IntegracaoModel>();
    private static Dictionary<string, AuthorizationIntegracaoModel> _listAutorIntegracoesTemp = new Dictionary<string, AuthorizationIntegracaoModel>();
    
    public IntegracaoModel gravarIntegracao(IntegracaoModel model)
    {
        
        if (string.IsNullOrEmpty(model.NomeIntegracao))
            throw new NegocioException("Nome da integração não informado.");
        
        if (!ServicesHelper.validarNomeIntegracao(model.NomeIntegracao))
            throw new NegocioException("Nome da integração inválido, informe um nome sem espaços ou caracteres especiais, exceto '-' e '_'.");

        var integracaoExistente = obterIntegracaoPorNome(model.NomeIntegracao);
        if (integracaoExistente != null && 
            integracaoExistente.VersaoIntegracao != model.VersaoIntegracao &&
            integracaoExistente.IdIntegracao != model.IdIntegracao)
            throw new NegocioException($"Integração com o nome [{integracaoExistente.NomeIntegracao}] já registrada na versão [{integracaoExistente.VersaoIntegracao}] e situação [{integracaoExistente.SituacaoIntegracao.DescricaoEnum()}]");

        if (string.IsNullOrEmpty(model.IdIntegracao))
        {
            model.IdIntegracao = Guid.NewGuid().ToString();
            model.VersaoIntegracao = 1;
            model.SituacaoIntegracao = SituacaoIntegracaoEnum.EmEdicao;
            _listIntegracoesTemp.Add(model);
            return model;
        }

        //Remover o item para UPDATE se existir, REMOVER quando tiver banco.
        var item = _listIntegracoesTemp.Find(m => m.IdIntegracao == model.IdIntegracao);
        if (item != null)
            _listIntegracoesTemp.Remove(item);
        
        item.NomeIntegracao = model.NomeIntegracao;
        item.TipoIntegracao = model.TipoIntegracao;
        item.DescricaoIntegracao = model.DescricaoIntegracao;
        item.OpcaoHealthcheck = model.OpcaoHealthcheck;
        item.OpcaoAuthorization = model.OpcaoAuthorization;
        item.OpcaoSwagger = model.OpcaoSwagger;
        item.OpcaoLogService = model.OpcaoLogService;
        
        _listIntegracoesTemp.Add(item);
        return item;
        
    }
    public List<IntegracaoModel> listarIntegracao()
    {
        return _listIntegracoesTemp;
    }

    public IntegracaoModel? obterIntegracaoPorId(string idIntegracao)
    {
        if (string.IsNullOrEmpty(idIntegracao))
            return null;
        return _listIntegracoesTemp.FirstOrDefault(w => w.IdIntegracao == idIntegracao);
    }
    
    public IntegracaoModel? obterIntegracaoPorNome(string nomeIntegracao)
    {
        if (string.IsNullOrEmpty(nomeIntegracao))
            return null;
        return _listIntegracoesTemp.FirstOrDefault(w => w.NomeIntegracao == nomeIntegracao);
    }

    public void removerIntegracao(string idIntegracao)
    {
        if (string.IsNullOrEmpty(idIntegracao))
            return;
        
        var item = obterIntegracaoPorId(idIntegracao);
        if (item!=null)
            _listIntegracoesTemp.Remove(item);
        
    }

    public AuthorizationIntegracaoModel? obterAutorizacaoIntegracao(string idIntegracao)
    {
        return _listAutorIntegracoesTemp.GetValueOrDefault(idIntegracao);
    }

    public AuthorizationIntegracaoModel gravarAutorizacaoIntegracao(AuthorizationIntegracaoModel model)
    {
        if (model == null)
            throw new NegocioException("Authorization da integração não informado.");
        if (string.IsNullOrEmpty(model.IdIntegracao))
            throw new NegocioException("Identificado da integração não informado.");
        if (model.TipoAutorizacao != TipoAutorizacaoEnum.NoAuth && (model.ParmsAutorizacao == null || model.ParmsAutorizacao.Count == 0)) 
            throw new NegocioException("Nenhum parametro de autenticação informado para o tipo de autenticação.");

        var integracao = obterIntegracaoPorId(model.IdIntegracao);
        if  (integracao == null)
            throw new NegocioException($"Integração com o Id [{model.IdIntegracao}] não encontrada.");

        var autorizacao = obterAutorizacaoIntegracao(model.IdIntegracao);
        if (autorizacao != null)
            _listAutorIntegracoesTemp.Remove(model.IdIntegracao);
        else
            model.IdAutorizacao = Guid.NewGuid().ToString();
        
        _listAutorIntegracoesTemp.Add(model.IdIntegracao, model);
        return model;
        
    }

    public void removerAutorizacaoIntegracao(string idIntegracao)
    {
        if (string.IsNullOrEmpty(idIntegracao))
            throw new NegocioException("Identificado da integração não informado.");

        var integracao = obterIntegracaoPorId(idIntegracao);
        if  (integracao == null)
            throw new NegocioException($"Integração com o Id [{idIntegracao}] não encontrada.");

        var autorizacao = obterAutorizacaoIntegracao(idIntegracao);
        if (autorizacao != null)
            _listAutorIntegracoesTemp.Remove(idIntegracao);
    }
    
}