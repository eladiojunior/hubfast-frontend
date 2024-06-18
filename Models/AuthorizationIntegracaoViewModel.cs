
using hubfast_frontend.Services.Models;

namespace hubfast_frontend.Models;

public class AuthorizationIntegracaoViewModel
{
    public string IdIntegracao { get; set; }
    public int VersaoIntegracao { get; set; }
    public string NomeIntegracao { get; set; }
    
    public string? IdAutorizacao { get; set; }
    public int CodigoTipoAutorizacao { get; set; }
    public List<ParmAutorizacaoModel> ParmsAutorizacao { get; set; }
}