using hubfast_frontend.Services.Models.Enums;

namespace hubfast_frontend.Services.Models;

public class AtributoOperacaoModel
{
    public string IdAtributo { get; set; }
    public string NomeAtributo { get; set; }
    public TipoAtributoEnum TipoAtributo { get; set; }
    public string ConteudoAtributo { get; set; }
    public List<AtributoOperacaoModel> AtributosObjeto { get; set; }
}