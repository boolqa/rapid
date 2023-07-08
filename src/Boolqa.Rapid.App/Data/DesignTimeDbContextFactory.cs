using Boolqa.Rapid.App.PluginCore.Infrastructures;
using Microsoft.EntityFrameworkCore.Design;

namespace Boolqa.Rapid.App.Data;

/// <summary>
/// Факторка создания контекста базы данных.
/// Используется только при создании миграции через команду `Add-Migration`.
/// </summary>
internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
{
    public MainDbContext CreateDbContext(string[] args)
    {
        // todo fix pass null, надо создать хост приложения
        var assemblies = new PluginLoaderManager(null, null, true)
            .LoadPlugins()
            .SelectMany(p => p.LoadedAssemblies);

        return new MainDbContextFactory().CreateNew(assemblies);
    }
}
