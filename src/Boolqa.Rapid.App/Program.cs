using Boolqa.Rapid.App;
using Boolqa.Rapid.App.Data;
using Boolqa.Rapid.App.PluginCore;
using Boolqa.Rapid.App.PluginCore.Infrastructures;
using Boolqa.Rapid.App.WeatherForecast;
using SimpleInjector;
using SimpleInjector.Lifestyles;

var siContainer = new Container();

siContainer.Options.DefaultLifestyle = Lifestyle.Scoped;
siContainer.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
var mvcBuilder = services.AddRazorPages();
services.AddServerSideBlazor();
services.AddSingleton<WeatherForecastService>();

var pluginManager = new PluginLoaderManager();
var plugins = pluginManager.LoadPlugins();

builder.Services.AddScoped<Func<MainDbContext>>((x) =>
{
    var assablies = plugins.SelectMany(p => p.LoadedAssemblies);
    return () => new MainDbContext(assablies!);
});

mvcBuilder.AddUiPlugins(plugins);

// см. страничку на вики SI для Blazor: https://docs.simpleinjector.org/en/latest/blazorintegration.html
services.AddServerSideBlazor();
services.AddSimpleInjector(siContainer, options =>
{
    // If you plan on adding AspNetCore as well, change the
    // ServiceScopeReuseBehavior to OnePerNestedScope as follows:
    // options.AddAspNetCore(ServiceScopeReuseBehavior.OnePerNestedScope);

    //options.AddServerSideBlazor(typeof(Program).Assembly);

    //options.AddLogging();
    //options.AddLocalization();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.Services.UseSimpleInjector(siContainer);

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

var core = new Core(siContainer);
var pluginHostManager = new PluginHostManager(plugins, core);

await pluginHostManager.Initialize();

using (var context = scope.ServiceProvider.GetService<Func<MainDbContext>>()?.Invoke())
{
    context.Database.EnsureDeleted();
    context.Database.Migrate();
}

scope.Dispose();

app.Services.UseSimpleInjector(siContainer);

siContainer.Verify();
siContainer.ApplyMigrations<MainDbContext>(isResetDb: true);

await pluginHostManager.Run();

app.Run();
