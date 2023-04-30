using Boolqa.Rapid.App.Data;
using Boolqa.Rapid.PluginCore;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Boolqa.Rapid.App.PluginCore;

public class Core : ICore
{
    private readonly MainDbContextFactory _contextFactory;

    public Core(Container container)
    {
        using var scope = AsyncScopedLifestyle.BeginScope(container);
        _contextFactory = scope.GetRequiredService<MainDbContextFactory>();
    }

    public IDataContext GetNewDataContext() => new DataContext(_contextFactory.CreateNew());
}
