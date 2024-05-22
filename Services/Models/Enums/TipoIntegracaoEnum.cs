using System.ComponentModel;

namespace hubfast_frontend.Services.Models.Enums;

public enum TipoIntegracaoEnum
{
    [Description("de API para API")]
    ApiToApi=1,
    [Description("de API para SOAP")]
    ApiToSoap=2,
    [Description("de API para SQC")]
    ApiToSqc=3
}