using hubfast_frontend.Services.Models;

namespace hubfast_frontend.Services;

public interface IIntegracaoService
{
    IntegracaoModel gravarIntegracao(IntegracaoModel model);
    List<IntegracaoModel> listarIntegracao();
    IntegracaoModel? obterIntegracaoPorId(string idIntegracao);
    IntegracaoModel? obterIntegracaoPorNome(string nomeIntegracao);
    void removerIntegracao(string idIntegracao);
    AuthorizationIntegracaoModel? obterAutorizacaoIntegracao(string idIntegracao);
    AuthorizationIntegracaoModel gravarAutorizacaoIntegracao(AuthorizationIntegracaoModel model);
    void removerAutorizacaoIntegracao(string idIntegracao);
}