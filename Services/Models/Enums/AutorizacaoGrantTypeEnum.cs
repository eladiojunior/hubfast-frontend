using System.ComponentModel;

namespace hubfast_frontend.Services.Models.Enums;

public enum AutorizacaoGrantTypeEnum
{
    [Description("Client Credentials")]
    ClientCredentials=1,
    [Description("Authorization Code")]
    AuthorizationCode=2,
    [Description("Authorization Code (With PKCE)")]
    AuthorizationCodePKCE=3,
    [Description("Implicit")]
    Implicit=4,
    [Description("Password Credentials")]
    PasswordCredentials=5
}