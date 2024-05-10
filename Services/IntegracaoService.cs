using hubfast_frontend.Services.Models;

namespace hubfast_frontend.Services;

public class IntegracaoService: IIntegracaoService
{
    private List<IntegracaoModel> _listTemp = new List<IntegracaoModel>();
    
    public IntegracaoModel gravarIntegracao(IntegracaoModel model)
    {
        
        if (string.IsNullOrEmpty(model.IdIntegracao))
        {
            model.IdIntegracao = Guid.NewGuid().ToString();
            _listTemp.Add(model);
            return model;
        }

        var item = _listTemp.Find(m => m.IdIntegracao == model.IdIntegracao);
        if (item != null)
            _listTemp.Remove(item);
        
        _listTemp.Add(model);
        return model;
        
    }
}