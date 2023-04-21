using Boolqa.Rapid.App.Data;
using Boolqa.Rapid.PluginCore;

namespace Boolqa.Rapid.App.PluginCore;

public class Core : ICore
{
    // todo: написать факторку для генерации контекста
    private readonly Func<MainDbContext> _contextFactory;

    public Core(Func<MainDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public IDataContext GetNewDataContext()
    {
        var dataContext = new DataContext(_contextFactory());
        return dataContext;
    }
}
