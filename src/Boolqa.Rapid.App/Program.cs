using Boolqa.Rapid.App;
using Boolqa.Rapid.App.Data;
using Boolqa.Rapid.App.PluginCore;
using Boolqa.Rapid.App.PluginCore.Infrastructures;
using Boolqa.Rapid.PluginCore;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

var siContainer = new Container();
siContainer.Options.DefaultLifestyle = Lifestyle.Scoped;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mvcBuilder = builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();


var pluginManager = new PluginLoaderManager();
var plugins = pluginManager.LoadPlugins();

builder.Services.AddScoped<Func<MainDbContext>>((x) =>
{
    var assablies = plugins.SelectMany(p => p.LoadedAssemblies);
    return () => new MainDbContext(assablies!);
});
builder.Services.AddScoped<Core>();

mvcBuilder.AddUiPlugins(plugins);

// Sets up the basic configuration that for integrating Simple Injector with
// ASP.NET Core by setting the DefaultScopedLifestyle, and setting up auto
// cross wiring.
builder.Services.AddSimpleInjector(siContainer, options =>
{
    // AddAspNetCore() wraps web requests in a Simple Injector scope and
    // allows request-scoped framework services to be resolved.
    //options.AddAspNetCore().AddControllerActivation();

    // Optionally, allow application components to depend on the non-generic
    // ILogger (Microsoft.Extensions.Logging) or IStringLocalizer
    // (Microsoft.Extensions.Localization) abstractions.
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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// todo: создавать Core вручную и передавать в него контейнер, он сом внутри разрулит по зависямостям
var scope = app.Services.CreateScope();
var core = scope.ServiceProvider.GetService<Core>();

var pluginHostManager = new PluginHostManager(plugins, core!);
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
