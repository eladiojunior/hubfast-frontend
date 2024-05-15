using hubfast_frontend.Services.Models;

namespace hubfast_frontend.Models;

public class OperacoesIntegracaoViewModel
{
    public string IdIntegracao { get; set; }
    public string NomeIntegracao { get; set; }
    public int VersaoIntegracao { get; set; }
    public List<OperacaoIntegracaoModel> OperacoesIntegracao { get; set; }
    
}