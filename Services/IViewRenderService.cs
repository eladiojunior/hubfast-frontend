namespace hubfast_frontend.Services;

public interface IViewRenderService
{
    Task<string> RenderToStringAsync(string viewName, object model);
}