using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Boolqa.Rapid.App.Data;

/// <summary>
/// Предоставляет простую API для регистрации сущностей из плагинов в <see cref="DbContext" />.
/// </summary>
public class DynamicEntityRegister
{
    private readonly IEnumerable<Assembly> _assemblies;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DynamicEntityRegister" />, который применяем набор сборок.
    /// </summary>
    /// <param name="assemblies">Сборки плагинов, для которых необходимо зарегистрировать сущности.</param>
    public DynamicEntityRegister(IEnumerable<Assembly> assemblies)
    {
        _assemblies = assemblies;
    }

    /// <summary>
    /// Регистрирует сущности из плагинов в контексте базы данных.
    /// </summary>
    /// <typeparam name="BaseModel">Тип базовой модели сущностей.</typeparam>
    /// <param name="modelBuilder"></param>
    public void RegisterEntitiesInPlugins<BaseModel>(ModelBuilder modelBuilder)
    {
        var types = _assemblies
            .SelectMany(a => a.GetExportedTypes())
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(BaseModel).IsAssignableFrom(c));

        foreach (var type in types)
        {
            _ = modelBuilder.Entity(type);
        }

        foreach (var asm in _assemblies)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(asm);
        }
    }
}
