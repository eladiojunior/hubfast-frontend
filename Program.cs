using hubfast_frontend.Services;
using hubfast_frontend.Views.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IIntegracaoService, IntegracaoService>();
builder.Services.AddScoped<IOperacaoIntegracaoService, OperacaoIntegracaoService>();
builder.Services.AddScoped<IViewRenderService, ViewRenderService>();

var app = builder.Build();

// Inicializar a classe estática com IConfiguration
ConfigurationHelper.Initialize(app.Services.GetRequiredService<IConfiguration>());

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();