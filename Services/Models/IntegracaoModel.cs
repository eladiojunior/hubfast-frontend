namespace hubfast_frontend.Services.Models;

public class IntegracaoModel
{
    public string IdIntegracao { get; set; }
    public string NomeIntegracao { get; set; }
    public string TipoIntegracao { get; set; }
    public bool OpcaoHealthcheck { get; set; }
    public bool OpcaoLogService { get; set; }
    public bool OpcaoAuthorization { get; set; }
    public bool OpcaoSwagger { get; set; }
}