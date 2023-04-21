using Boolqa.Rapid.App.Data;
using Boolqa.Rapid.App.PluginCore.Services;
using Boolqa.Rapid.PluginCore;
using Boolqa.Rapid.PluginCore.Services;

namespace Boolqa.Rapid.App.PluginCore;

public class DataContext : IDataContext
{
    private readonly MainDbContext _mainDbContext;

    public Lazy<ICoreObjectService> CoreObjectService { get; private set; }

    public DataContext(MainDbContext mainDbContext)
    {
        _mainDbContext = mainDbContext;
        // todo: тут нужно прокинуть все через DI
        CoreObjectService = new Lazy<ICoreObjectService>(() => new CoreObjectService(mainDbContext));
    }

    public async Task<int> SaveChanges()
    {
        return await _mainDbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _mainDbContext?.Dispose();
    }
}
