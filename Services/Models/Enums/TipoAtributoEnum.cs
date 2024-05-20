using System.ComponentModel;

namespace hubfast_frontend.Services.Models.Enums;

public enum TipoAtributoEnum
{
    [Description("Texto")]
    Texto=1,
    [Description("Número")]
    Numero=2,
    [Description("Array")]
    Array=3,
    [Description("Objeto")]
    Objeto=4
}