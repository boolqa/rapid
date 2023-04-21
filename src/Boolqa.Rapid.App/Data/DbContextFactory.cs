using Boolqa.Rapid.App.PluginCore.Infrastructures;
using Microsoft.EntityFrameworkCore.Design;

namespace Boolqa.Rapid.App.Data;

/// <summary>
/// Факторка для создания контекста базы данных.
/// </summary>
/// <remarks>Используется только для миграций!</remarks>
internal class DbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
{
    public MainDbContext CreateDbContext(string[] args)
    {
        var pluginManager = new PluginLoaderManager();
        var plugins = pluginManager.LoadPlugins();
        
        var services = new ServiceCollection();
        services.AddScoped<Func<MainDbContext>>((x) =>
        {
            var assablies = plugins.SelectMany(p => p.LoadedAssemblies);
            return () => new MainDbContext(assablies!);
        });

        var context = services
            .BuildServiceProvider()
            .GetService<Func<MainDbContext>>();

        return context!();
    }
}

