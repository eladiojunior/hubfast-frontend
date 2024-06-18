using hubfast_frontend.Services.Models;

namespace hubfast_frontend.Models;

public class HealthcheckViewModel
{
    public string IdIntegracao { get; set; }
    public int VersaoIntegracao { get; set; }
    public string NomeIntegracao { get; set; }
    
    public int CodigoSituacaoIntegracao { get; set; }
    
    public List<ApiBackendModel> ApisBackend { get; set; }
}