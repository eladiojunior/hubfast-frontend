using System.ComponentModel;

namespace hubfast_frontend.Services.Models.Enums;

public enum TipoAutorizacaoEnum
{
    [Description("No Auth")]
    NoAuth=0,
    [Description("Basic Auth")]
    BasicAuth=1,
    [Description("Bearer Token")]
    BearerToken=2,
    [Description("OAuth 2.0")]
    OAuth20=3,
    [Description("API Key")]
    APIKey=4
}