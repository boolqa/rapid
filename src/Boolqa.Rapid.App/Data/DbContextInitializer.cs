using Boolqa.Rapid.App.Data;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Boolqa.Rapid.App;

internal static class DbContextInitializer
{
    /// <summary>
    /// Применяет имеющиеся миграции для <see cref="MainDbContext"/> контекста базы данных.
    /// </summary>
    /// <remarks>
    /// На <see href="Production"/> окружении не применять! Для получения sql-скрипта с изменениями базы данных,
    /// выполняем команду `Script-Migration` в консоли диспетчера пакетов - сгенерируется временный .sql файл.
    /// </remarks>
    /// <param name="isResetDb">Принимает <see langword="true"/>, если нужно обнулить базу до применения миграции; иначе <see langword="false"/>.</param>
    public static void ApplyMigrations<T>(this Container container, bool isResetDb = false) where T : DbContext
    {
        //container.ThrowIfNull(nameof(container));

        using var scope = AsyncScopedLifestyle.BeginScope(container);

        var environment = scope.GetRequiredService<IWebHostEnvironment>();

        if (environment.IsDevelopment())
        {
            var dbContext = scope.GetRequiredService<T>();

            if (isResetDb)
            {
                dbContext.Database.EnsureDeleted();
            }

            dbContext.Database.Migrate();
        }
    }
}
