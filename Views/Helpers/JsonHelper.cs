using hubfast_frontend.Exceptions;
using hubfast_frontend.Services.Models;
using hubfast_frontend.Services.Models.Enums;
using Newtonsoft.Json.Linq;

namespace hubfast_frontend.Views.Helpers;

public static class JsonHelper
{
    
    /// <summary>
    /// Converte conteúdo Json (string) em objeto AtributoOperacaoModel para facilitar no armazenamento em banco.
    /// </summary>
    /// <param name="json">Conteúdo em JSON formato String</param>
    /// <returns></returns>
    /// <exception cref="NegocioException">Identificado erro de negócio essa exception será lançada.</exception>
    public static List<AtributoOperacaoModel> ConvertJsonToAtributos(string json)
    {
        var listAtributos = new List<AtributoOperacaoModel>();
        if (string.IsNullOrEmpty(json))
            throw new NegocioException(
                "Json não informado, não é possível converter em objeto de atributos da operação.");

        var jsonDictionary = new Dictionary<string, object>();
        try
        {
            jsonDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }
        catch (Exception erro)
        {
            throw new NegocioException("Não foi possível desserializar o json, verifique se ele não está inválido.");
        }

        // Iterar sobre os pares chave-valor do JSON
        foreach (var kvp in jsonDictionary)
        {
            var atributo = new AtributoOperacaoModel();

            atributo.NomeAtributo = kvp.Key;
            switch (kvp.Value)
            {
                // Verificar se o valor é um número ou texto
                case int or long or float or double or decimal:
                    atributo.TipoAtributo = TipoAtributoEnum.Numero;
                    atributo.AtributosObjeto = null;
                    atributo.ConteudoAtributo = kvp.Value.ToString();
                    break;
                case string:
                    atributo.TipoAtributo = TipoAtributoEnum.Texto;
                    atributo.AtributosObjeto = null;
                    atributo.ConteudoAtributo = kvp.Value.ToString();
                    break;
                case JArray array:
                    atributo.TipoAtributo = TipoAtributoEnum.Array;
                    foreach (var item in array)
                    {
                        atributo.AtributosObjeto = ConvertJsonToAtributos(array.ToString());
                        break; //Não precisa pegar mais que um objeto;
                    }

                    atributo.AtributosObjeto = null;
                    atributo.ConteudoAtributo = null;
                    break;
                case JObject:
                    atributo.TipoAtributo = TipoAtributoEnum.Objeto;
                    atributo.AtributosObjeto = ConvertJsonToAtributos(kvp.Value.ToString());
                    atributo.ConteudoAtributo = null;
                    break;
            }

            listAtributos.Add(atributo);
        }

        return listAtributos;
    }
}