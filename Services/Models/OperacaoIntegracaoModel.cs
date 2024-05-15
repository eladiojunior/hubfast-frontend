using hubfast_frontend.Services.Models.Enums;

namespace hubfast_frontend.Services.Models;

public class OperacaoIntegracaoModel
{
    public string IdOperacao { get; set; }
    public string NomeOperacao { get; set; }
    public TipoMetodoRestEnum TipoMetodoOperacao { get; set; }
    public List<AtributoOperacaoModel> AtributosRequest { get; set; }
    public List<AtributoOperacaoModel> AtributosResponse { get; set; }
}