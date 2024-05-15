using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace hubfast_frontend.Controllers;

public class GenericController : Controller
{
    
    /// <summary>
    /// Cria um retorno Json de Erro (Result = false) com mensagem de erro.
    /// </summary>
    /// <param name="mensagemErro"></param>
    /// <returns></returns>
    internal JsonResult JsonResultErro(string mensagemErro)
    {
        return Json(new { HasErro = true, Erros = new List<string> { mensagemErro } });
    }

    /// <summary>
    ///     Cria um retorno Json de Erro (Result = false) com mensagem de erro.
    /// </summary>
    /// <param name="mensagensErro">Lista de mensagens de erro que deve ser apresentada ao usuário.</param>
    /// <returns></returns>
    internal JsonResult JsonResultErro(List<string> mensagensErro)
    {
        return Json(new { HasErro = true, Erros = mensagensErro });
    }

    internal JsonResult JsonResultErro(object model, string mensagem = "")
    {
        return Json(new { HasErro = true, Model = model, Mensagem = mensagem });
    }

    internal JsonResult JsonResultErro(Exception ex)
    {
        return Json(new { HasErro = true, Erros = new[] { ex.Message } });
    }

    /// <summary>
    ///     Cria um retorno Json de Erro (Result = false) com mensagem de erro, com base nos erros do modelState
    /// </summary>
    /// <param name="modelState">Lista de mensagens de erro que deve ser apresentada ao usuário.</param>
    /// <returns></returns>
    internal JsonResult JsonResultErro(ModelStateDictionary modelState)
    {
        var chaves = from modelstate in modelState.AsQueryable().Where(f => f.Value.Errors.Count > 0)
            select modelstate.Key;
        var mensagens =
            from modelstate in modelState.AsQueryable().Where(f => f.Value.Errors.Count > 0)
            select modelstate.Value.Errors.FirstOrDefault(a => !string.IsNullOrEmpty(a.ErrorMessage));
        return
            Json(
                new
                {
                    HasErro = true,
                    Chaves = chaves,
                    Erros = mensagens.Where(a => a != null).Select(a => a.ErrorMessage).ToList()
                });
    }

    /// <summary>
    ///     Cria um retorno Json de Sucesso (Result = true) com mensagem para o usuário (opcional).
    /// </summary>
    /// <param name="mensagemAlerta">Mensagem de alerta que deve ser apresentada ao usuário.</param>
    /// <returns></returns>
    internal JsonResult JsonResultSucesso(string mensagemAlerta = "")
    {
        return Json(new { HasErro = false, Mensagem = mensagemAlerta });
    }

    /// <summary>
    ///     Cria um retorno Json de Sucesso (Result = true) com Model e mensagem para o usuário (opcional).
    /// </summary>
    /// <param name="model">Informações do Model para renderizar a view.</param>
    /// <param name="mensagemAlerta">Mensagem de alerta que deve ser apresentada ao usuário.</param>
    /// <returns></returns>
    internal JsonResult JsonResultSucesso(object model, string mensagemAlerta = "")
    {
        return Json(new { HasErro = false, Model = model, Mensagem = mensagemAlerta });
    }

}