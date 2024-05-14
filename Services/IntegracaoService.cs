using hubfast_frontend.Exceptions;
using hubfast_frontend.Services.Helpers;
using hubfast_frontend.Services.Models;
using hubfast_frontend.Services.Models.Enums;

namespace hubfast_frontend.Services;

public class IntegracaoService: IIntegracaoService
{
    private static List<IntegracaoModel> _listTemp = new List<IntegracaoModel>();
    
    public IntegracaoModel gravarIntegracao(IntegracaoModel model)
    {
        if (string.IsNullOrEmpty(model.NomeIntegracao))
            throw new NegocioException("Nome da integração não informado.");
        if (!ServicesHelper.validarNomeIntegracao(model.NomeIntegracao))
            throw new NegocioException("Nome da integração inválido, informe um nome sem espaços ou caracteres especiais, exceto '-' e '_'.");
        
        if (string.IsNullOrEmpty(model.IdIntegracao))
        {
            
            var integracaoExistente = obterIntegracaoPorNome(model.NomeIntegracao);
            if (integracaoExistente != null)
                throw new NegocioException($"Integração com o nome [{integracaoExistente.NomeIntegracao}] já registrada na versão [{integracaoExistente.VersaoIntegracao}] e situação [{integracaoExistente.SituacaoIntegracao.DescricaoEnum()}]");

            model.IdIntegracao = Guid.NewGuid().ToString();
            model.VersaoIntegracao = 1;
            model.SituacaoIntegracao = SituacaoIntegracaoEnum.EmEdicao;
            _listTemp.Add(model);
            return model;
           
        }

        var item = _listTemp.Find(m => m.IdIntegracao == model.IdIntegracao);
        if (item != null)
            _listTemp.Remove(item);
        
        _listTemp.Add(model);
        return model;
        
    }
    public List<IntegracaoModel> listarIntegracao()
    {
        return _listTemp;
    }

    public IntegracaoModel? obterIntegracaoPorId(string idIntegracao)
    {
        if (string.IsNullOrEmpty(idIntegracao))
            return null;
        return _listTemp.FirstOrDefault(w => w.IdIntegracao == idIntegracao);
    }
    
    public IntegracaoModel? obterIntegracaoPorNome(string nomeIntegracao)
    {
        if (string.IsNullOrEmpty(nomeIntegracao))
            return null;
        return _listTemp.FirstOrDefault(w => w.NomeIntegracao == nomeIntegracao);
    }

    public void removerIntegracao(string idIntegracao)
    {
        if (string.IsNullOrEmpty(idIntegracao))
            return;
        
        var item = obterIntegracaoPorId(idIntegracao);
        if (item!=null)
            _listTemp.Remove(item);
        
    }
}