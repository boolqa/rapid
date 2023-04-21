using Boolqa.Rapid.App.Data;
using Boolqa.Rapid.PluginCore.Data;
using Boolqa.Rapid.PluginCore.Services;

namespace Boolqa.Rapid.App.PluginCore.Services;

public class CoreObjectService : ICoreObjectService
{
    private readonly MainDbContext _mainDbContext;

    public CoreObjectService(MainDbContext mainDbContext)
    {
        _mainDbContext = mainDbContext;
    }

    public CoreObject Add(CoreObject @object)
    {
        return _mainDbContext.Objects.Add(@object).Entity;
    }

    // todo: вынести в IGenericObjectService
    public T Add<T>(T @object) where T : class
    {
        return _mainDbContext.Set<T>().Add(@object).Entity;
    }

    // todo: сделать для Guid objectId strongType (не как в v2, т.к в новом .NET есть лучшее решение)
    public async ValueTask<CoreObject?> Get(Guid objectId)
    {
        return await _mainDbContext.Objects.FindAsync(objectId);
    }
}
