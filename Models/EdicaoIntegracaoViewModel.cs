﻿namespace hubfast_frontend.Models;

public class EdicaoIntegracaoViewModel
{
    public string IdIntegracao { get; set; }
    public string NomeIntegracao { get; set; }
    public string TipoIntegracao { get; set; }
    public string DescricaoIntegracao { get; set; }
    public bool OpcaoHealthcheck { get; set; }
    public bool OpcaoLogService { get; set; }
    public bool OpcaoAuthorization { get; set; }
    public bool OpcaoSwagger { get; set; }
    public int? VersaoIntegracao { get; set; }
    public string? DescricaoSituacaoIntegracao { get; set; }
}