using Boolqa.Rapid.App;
using Boolqa.Rapid.App.PluginCore.Infrastructures;
using Boolqa.Rapid.PluginCore.Models;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Класс для расширения IMvcBuilder.
/// </summary>
public static class MvcPluginExtensions
{
    /// <summary>
    /// Добавляет поддержку UI составляющей из плагинов.
    /// </summary>
    /// <param name="mvcBuilder">Mvc Builder.</param>
    /// <param name="pluginContexts">Контексты загруженных в память плагинов.</param>
    public static IMvcBuilder AddUiPlugins(this IMvcBuilder mvcBuilder,
        ICollection<PluginLoadContext> pluginContexts)
    {
        var uiPlugins = pluginContexts.Where(p => p.Settings.PluginType != PluginType.Backend);

        foreach (var uiPlugin in uiPlugins)
        {
            mvcBuilder.AddPluginLoader(uiPlugin.Loader!);

            // For testing
            var pluginTypes = uiPlugin.LoadedAssemblies.SelectMany(a => a.GetTypes())
                .Where(t => typeof(ComponentBase).IsAssignableFrom(t) && !t.IsAbstract)
                .Where(t => t != null)
                .ToArray();

            foreach (var type in pluginTypes)
            {
                ForTest.Types.Add(type.FullName!, type);
            }
            // For testing end
        }

        return mvcBuilder;
    }
}
