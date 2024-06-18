using System.ComponentModel.DataAnnotations;

namespace hubfast_frontend.Models;

public class NovaIntegracaoViewModel
{
    [Required(ErrorMessage = "Nome da integração não informado")]
    [MaxLength(50, ErrorMessage = "Nome da integração não pode ser maior que 50 caracteres")]
    public string NomeIntegracao { get; set; }
    
    [Required(ErrorMessage = "Tipo de integração não informado")]  
    public int CodigoTipoIntegracao { get; set; }
    
    [Required(ErrorMessage = "Descrição da integração não informada")]
    [MaxLength(500, ErrorMessage = "Descrição da integração não pode ser maior que 500 caracteres")]
    public string DescricaoIntegracao { get; set; }
    
    public bool OpcaoHealthcheck { get; set; }
    public bool OpcaoLogService { get; set; }
    public bool OpcaoAuthorization { get; set; }
    public bool OpcaoSwagger { get; set; }
    
}