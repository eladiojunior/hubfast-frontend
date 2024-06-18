using hubfast_frontend.Exceptions;
using hubfast_frontend.Services.Models;

namespace hubfast_frontend.Services;

public class OperacaoIntegracaoService: IOperacaoIntegracaoService
{
    private static Dictionary<string, List<OperacaoIntegracaoModel>> _listOperacoesIntegracaoTemp = new Dictionary<string, List<OperacaoIntegracaoModel>>();

    
    public List<OperacaoIntegracaoModel> listarOperacaoIntegracao(string idIntegracao)
    {
        if (string.IsNullOrEmpty(idIntegracao))
            return null;
        return _listOperacoesIntegracaoTemp.GetValueOrDefault(idIntegracao);
    }

    public OperacaoIntegracaoModel gravarOperacaoIntegracao(OperacaoIntegracaoModel model)
    {
        
        if (model == null)
            throw new NegocioException($"Nenhuma informação da operação informada.");
        if (string.IsNullOrEmpty(model.IdIntegracao)) 
            throw new NegocioException($"Id da Integração não informado.");
        if (string.IsNullOrEmpty(model.NomeOperacao)) 
            throw new NegocioException($"Nome da operação não informado.");
        if (string.IsNullOrEmpty(model.JsonRequest)) 
            throw new NegocioException($"Informações da requisição de entrada (Request) não informado.");
        if (model.AtributosRequest == null || model.AtributosRequest.Count == 0) 
            throw new NegocioException($"Atributos da requisição de entrada (Request) não carregados do Json.");
        if (string.IsNullOrEmpty(model.JsonResponse)) 
            throw new NegocioException($"Informações da requisição de saída (Response) não informado.");
        if (model.AtributosResponse == null || model.AtributosResponse.Count == 0) 
            throw new NegocioException($"Atributos da requisição de saída (Response) não carregados do Json.");
        
        var listaOperacoes = listarOperacaoIntegracao(model.IdIntegracao);
        if (listaOperacoes != null)
        {
            if (string.IsNullOrEmpty(model.IdOperacao))
            {//Novo
                var operacao = listaOperacoes.FirstOrDefault(w => w.NomeOperacao == model.NomeOperacao);
                if (operacao != null)
                    throw new NegocioException($"Já existe uma operação com o nome [{model.NomeOperacao}], não é permitido duplicidade.");
                model.IdOperacao = Guid.NewGuid().ToString();
                listaOperacoes.Add(model);
            }
            else
            {// Atualizar
                var operacao = listaOperacoes.FirstOrDefault(w => w.IdOperacao == model.IdOperacao);
                if (operacao == null)
                    listaOperacoes.Add(model);
                else
                {
                    listaOperacoes.Remove(operacao);
                    listaOperacoes.Add(model);
                }
            }
            return model;
        }

        model.IdOperacao = Guid.NewGuid().ToString();
        listaOperacoes = new List<OperacaoIntegracaoModel>();
        listaOperacoes.Add(model);
        
        //Criar objeto...
        _listOperacoesIntegracaoTemp.Add(model.IdIntegracao, listaOperacoes);

        return model;
        
    }

    public void removerOperacaoIntegracao(string idIntegracao, string idOperacao)
    {
        var listarOperacoes = listarOperacaoIntegracao(idIntegracao);
        if (listarOperacoes == null) 
            return;
        var operacao = listarOperacoes.FirstOrDefault(w => w.IdOperacao == idOperacao);
        if (operacao != null)
            listarOperacoes.Remove(operacao);
    }

    public OperacaoIntegracaoModel obterOperacaoIntegracaoPorId(string idOperacao)
    {
        return string.IsNullOrEmpty(idOperacao) ? null : 
            _listOperacoesIntegracaoTemp.Select(kvp => kvp.Value
                    .FirstOrDefault(obj => obj.IdOperacao == idOperacao))
                .FirstOrDefault(item => item != null);
    }

    public void removerOperacaoIntegracao(string idOperacao)
    {
        foreach (var kvp in _listOperacoesIntegracaoTemp)
        {
            var item = kvp.Value.FirstOrDefault(obj => obj.IdOperacao == idOperacao);
            if (item == null) continue;
            kvp.Value.Remove(item);
            break;
        }
    }
    
}