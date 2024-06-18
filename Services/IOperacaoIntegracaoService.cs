using hubfast_frontend.Services.Models;

namespace hubfast_frontend.Services;

public interface IOperacaoIntegracaoService
{
    List<OperacaoIntegracaoModel> listarOperacaoIntegracao(string idIntegracao);
    OperacaoIntegracaoModel gravarOperacaoIntegracao(OperacaoIntegracaoModel model);
    void removerOperacaoIntegracao(string idIntegracao, string idOperacao);
    OperacaoIntegracaoModel obterOperacaoIntegracaoPorId(string idOperacao);
    void removerOperacaoIntegracao(string idOperacao);

}