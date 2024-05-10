using hubfast_frontend.Services.Models;

namespace hubfast_frontend.Services;

public interface IIntegracaoService
{
    IntegracaoModel gravarIntegracao(IntegracaoModel model);
}