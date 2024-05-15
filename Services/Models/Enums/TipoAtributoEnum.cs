using System.ComponentModel;

namespace hubfast_frontend.Services.Models.Enums;

public enum TipoAtributoEnum
{
    [Description("Alfanumerico")]
    Alfanumerico=1,
    [Description("Numerico")]
    Numerico=2,
    [Description("Objeto")]
    Objeto=3
}