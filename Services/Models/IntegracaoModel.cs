using hubfast_frontend.Services.Models.Enums;

namespace hubfast_frontend.Services.Models;

public class IntegracaoModel
{
    public string IdIntegracao { get; set; }
    public string NomeIntegracao { get; set; }
    public TipoIntegracaoEnum TipoIntegracao { get; set; }
    public int VersaoIntegracao { get; set; }
    public string DescricaoIntegracao { get; set; }
    public bool OpcaoHealthcheck { get; set; }
    public bool OpcaoLogService { get; set; }
    public bool OpcaoAuthorization { get; set; }
    public bool OpcaoSwagger { get; set; }
    public SituacaoIntegracaoEnum SituacaoIntegracao { get; set; }
}