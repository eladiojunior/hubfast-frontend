namespace hubfast_frontend.Services.Models;

public class ParmAutorizacaoModel
{
    public ParmAutorizacaoModel()
    {
    }
    public ParmAutorizacaoModel(string key, string value = "")
    {
        Key = key;
        Value = value;
    }
    public string Key { get; set; }
    public string Value { get; set; }
}