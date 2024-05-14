using System.Text.RegularExpressions;

namespace hubfast_frontend.Services.Helpers;

public class ServicesHelper
{
    /**
     * Verifica se o nome da integração é valido.
     */
    public static bool validarNomeIntegracao(string nomeIntegracao)
    {
        if (string.IsNullOrEmpty(nomeIntegracao))
            return false;
        var regex = new Regex("^[a-zA-Z0-9_-]*$");
        return regex.IsMatch(nomeIntegracao);
    }
}