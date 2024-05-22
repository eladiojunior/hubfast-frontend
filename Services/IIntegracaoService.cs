using hubfast_frontend.Services.Models;

namespace hubfast_frontend.Services;

public interface IIntegracaoService
{
    IntegracaoModel gravarIntegracao(IntegracaoModel model);
    List<IntegracaoModel> listarIntegracao();
    IntegracaoModel? obterIntegracaoPorId(string idIntegracao);
    IntegracaoModel? obterIntegracaoPorNome(string nomeIntegracao);
    void removerIntegracao(string idIntegracao);
    List<OperacaoIntegracaoModel> listarOperacaoIntegracao(string idIntegracao);
    OperacaoIntegracaoModel gravarOperacaoIntegracao(string idIntegracao, OperacaoIntegracaoModel model);
    void removerOperacaoIntegracao(string idIntegracao, string idOperacao);
    OperacaoIntegracaoModel obterOperacaoPorId(string idOperacao);
    void removerOperacao(string idOperacao);
}