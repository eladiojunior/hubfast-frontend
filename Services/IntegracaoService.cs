using hubfast_frontend.Exceptions;
using hubfast_frontend.Services.Helpers;
using hubfast_frontend.Services.Models;
using hubfast_frontend.Services.Models.Enums;

namespace hubfast_frontend.Services;

public class IntegracaoService: IIntegracaoService
{
    private static List<IntegracaoModel> _listIntegracoesTemp = new List<IntegracaoModel>();
    private static Dictionary<String, List<OperacaoIntegracaoModel>> _listOperacoesIntegracaoTemp = new Dictionary<string, List<OperacaoIntegracaoModel>>();
    
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
            _listIntegracoesTemp.Add(model);
            return model;
           
        }

        var item = _listIntegracoesTemp.Find(m => m.IdIntegracao == model.IdIntegracao);
        if (item != null)
            _listIntegracoesTemp.Remove(item);
        
        _listIntegracoesTemp.Add(model);
        return model;
        
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

    public List<OperacaoIntegracaoModel> listarOperacaoIntegracao(string idIntegracao)
    {
        
        if (string.IsNullOrEmpty(idIntegracao))
            return null;
        return _listOperacoesIntegracaoTemp.GetValueOrDefault(idIntegracao);
        
    }

    public OperacaoIntegracaoModel gravarOperacaoIntegracao(string idIntegracao, OperacaoIntegracaoModel model)
    {
        
        if (model == null)
            throw new NegocioException($"Nenhuma informação da operação informada.");
        if (string.IsNullOrEmpty(model.NomeOperacao)) 
            throw new NegocioException($"Nome da operação não informado.");
        
        var listaOperacoes = listarOperacaoIntegracao(idIntegracao);
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
        _listOperacoesIntegracaoTemp.Add(idIntegracao, listaOperacoes);

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
}