using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Boolqa.Rapid.App.Data;

/// <summary>
/// Факторка для создания <see cref="MainDbContext"/>.
/// </summary>
internal class MainDbContextFactory
{
    public MainDbContext CreateNew() => new();

    public MainDbContext CreateNew(IEnumerable<Assembly> assemblies) => CreateNew(new DynamicEntityRegister(assemblies));

    public MainDbContext CreateNew(DynamicEntityRegister entityRegister) => new(entityRegister, new DbContextOptions<MainDbContext>());
}
