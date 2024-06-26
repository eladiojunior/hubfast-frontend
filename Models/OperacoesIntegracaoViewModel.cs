﻿using hubfast_frontend.Services.Models;

namespace hubfast_frontend.Models;

public class OperacoesIntegracaoViewModel
{
    public string IdIntegracao { get; set; }
    public string NomeIntegracao { get; set; }
    public int VersaoIntegracao { get; set; }
    public string IdOperacao { get; set; }
    public int CodigoMetodoOperacao { get; set; }
    public string NomeOperacao { get; set; }
    public string JsonRequestOperacao { get; set; }
    public string JsonResponseOperacao { get; set; }
    public List<OperacaoIntegracaoModel> Operacoes { get; set; }
    
}