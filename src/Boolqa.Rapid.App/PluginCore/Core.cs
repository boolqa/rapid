using Boolqa.Rapid.App.Data;
using Boolqa.Rapid.PluginCore;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Boolqa.Rapid.App.PluginCore;

public class Core : ICore
{
    private readonly Lazy<MainDbContextFactory> _contextFactory;

    public Core(Container container)
    {
        _contextFactory = new Lazy<MainDbContextFactory>(() =>
        {
            using var scope = AsyncScopedLifestyle.BeginScope(container);
            return scope.GetRequiredService<MainDbContextFactory>();
        });
    }

    public IDataContext GetNewDataContext() => new DataContext(_contextFactory.Value.CreateNew());
}
