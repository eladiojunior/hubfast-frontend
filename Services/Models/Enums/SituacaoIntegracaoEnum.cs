using System.ComponentModel;
using static hubfast_frontend.Services.Helpers.EnumsHelper;
namespace hubfast_frontend.Services.Models.Enums;

public enum SituacaoIntegracaoEnum
{
    [Description("Em Edição")]
    EmEdicao=1,
    [Description("Publicada")]
    Publicada=2,
    [Description("Desativada")]
    Desativada=3
}