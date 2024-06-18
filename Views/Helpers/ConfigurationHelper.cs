namespace hubfast_frontend.Views.Helpers;

public static class ConfigurationHelper
{
    private static IConfiguration _configuration;

    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static string GetSetting(string key)
    {
        return string.IsNullOrEmpty(key) ? string.Empty : _configuration[key];
    }
    
}