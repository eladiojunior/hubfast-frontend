using hubfast_frontend.Services.Models.Enums;

namespace hubfast_frontend.Services.Models;

public class AuthorizationIntegracaoModel
{
    public string IdIntegracao { get; set; }
    public string IdAutorizacao { get; set; }
    public TipoAutorizacaoEnum TipoAutorizacao { get; set; }
    public List<ParmAutorizacaoModel> ParmsAutorizacao { get; set; }
}