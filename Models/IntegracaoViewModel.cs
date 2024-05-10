namespace hubfast_frontend.Models;

public class IntegracaoViewModel
{
    public string? IdIntegracao { get; set; }
    public string NomeIntegracao { get; set; }
    public string TipoIntegracao { get; set; }
    public bool OpcaoHealthcheck { get; set; }
    public bool OpcaoLogService { get; set; }
    public bool OpcaoAuthorization { get; set; }
    public bool OpcaoSwagger { get; set; }
}