using hubfast_frontend.Services.Models.Enums;

namespace hubfast_frontend.Services.Models;

public class AtributoOperacaoModel
{
    public string IdAtributo { get; set; }
    public string NomeAtributo { get; set; }
    public TipoAtributoEnum TipoAtributo { get; set; }
    public Object ConteudoAtributo { get; set; }
}