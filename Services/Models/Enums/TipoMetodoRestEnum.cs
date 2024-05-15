using System.ComponentModel;

namespace hubfast_frontend.Services.Models.Enums;

public enum TipoMetodoRestEnum
{
    [Description("POST")]
    MetodoPost=1,
    [Description("GET")]
    MetodoGet=2,
    [Description("PUT")]
    MetodoPut=3,
    [Description("DELETE")]
    MetodoDelete=4
}