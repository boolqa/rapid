using System.Reflection;
using Boolqa.Rapid.App;
using Boolqa.Rapid.App.PluginCore.Infrastructures;
using Boolqa.Rapid.PluginCore.Models;
using McMaster.NETCore.Plugins;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

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
            mvcBuilder.AddPluginLoader2(uiPlugin.Loader!);

            // For testing
            var pluginTypes = uiPlugin.LoadedAssemblies.SelectMany(a => a.GetTypes())
                .Where(t => typeof(ComponentBase).IsAssignableFrom(t) && !t.IsAbstract)
                .Where(t => t != null)
                .ToArray();

            // For testing2
            var pluginTypes2 = uiPlugin.LoadedAssemblies.SelectMany(a => a.GetTypes()).ToList();

            Console.WriteLine(string.Join(Environment.NewLine, pluginTypes2.Select(x => x.FullName)));

            var pluginTypes22 = pluginTypes2.Where(t => t.BaseType?.Name.Contains("ComponentBase", 
                StringComparison.OrdinalIgnoreCase) == true && !t.IsAbstract)
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

    public static IMvcBuilder AddPluginLoader2(this IMvcBuilder mvcBuilder, PluginLoader pluginLoader)
    {
        var pluginAssembly = pluginLoader.LoadDefaultAssembly();

        // This loads MVC application parts from plugin assemblies
        var partFactory = ApplicationPartFactory.GetApplicationPartFactory(pluginAssembly);
        foreach (var part in partFactory.GetApplicationParts(pluginAssembly))
        {
            mvcBuilder.PartManager.ApplicationParts.Add(part);
        }

        // This piece finds and loads related parts, such as MvcAppPlugin1.Views.dll.
        var relatedAssembliesAttrs = pluginAssembly.GetCustomAttributes<RelatedAssemblyAttribute>();
        foreach (var attr in relatedAssembliesAttrs)
        {
            var assembly = pluginLoader.LoadAssembly(attr.AssemblyFileName);
            partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
            foreach (var part in partFactory.GetApplicationParts(assembly))
            {
                mvcBuilder.PartManager.ApplicationParts.Add(part);
            }
        }

        return mvcBuilder;
    }
}
