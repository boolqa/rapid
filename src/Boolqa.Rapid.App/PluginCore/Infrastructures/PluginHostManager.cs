using System.Collections;
using Boolqa.Rapid.PluginCore;

namespace Boolqa.Rapid.App.PluginCore.Infrastructures;

public class PluginHostManager
{
    private readonly ICollection<PluginLoadContext> _pluginLoadInfos;

    private readonly List<PluginHost> _pluginHosts;

    private readonly Core _core;

    public PluginHostManager(IEnumerable<PluginLoadContext> pluginLoadInfos, Core core)
    {
        _pluginLoadInfos = pluginLoadInfos.Where(p => p.Loader != null)
            .ToArray();
        _pluginHosts = new List<PluginHost>(_pluginLoadInfos.Count);
        _core = core;
    }

    public async Task Initialize()
    {
        foreach (var pluginInfo in _pluginLoadInfos)
        {
            var pluginHost = CreatePluginHost(pluginInfo);
            _pluginHosts.Add(pluginHost);

            await pluginHost.Plugin.Initialize();

            Console.WriteLine($"Initialized plugin '{pluginHost.PluginInfo?.Settings?.PluginName}'");
        }
    }

    public async Task Run()
    {
        foreach (var pluginHost in _pluginHosts)
        {
            await pluginHost.Plugin.Run();

            Console.WriteLine($"Runed plugin '{pluginHost.PluginInfo?.Settings?.PluginName}'");
        }
    }

    private PluginHost CreatePluginHost(PluginLoadContext pluginInfo)
    {
        var pluginTypes = pluginInfo.LoadedAssemblies.SelectMany(x => x.GetTypes())
                .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract)
                .ToArray();

        if (pluginTypes == null || !pluginTypes.Any())
        {
            throw new InvalidOperationException(
                $"IPlugin interface not found in plugin '{pluginInfo.Settings?.PluginName}'");
        }

        if (pluginTypes.Length > 1)
        {
            throw new InvalidOperationException(
                $"IPlugin interface can't be more 1 '{pluginInfo.Settings?.PluginName}'");
        }

        var pluginType = pluginTypes.First();

        var plugin = (IPlugin?)Activator.CreateInstance(pluginType, _core)
            ?? throw new InvalidOperationException($"IPlugin create instance failed '{pluginInfo.Settings?.PluginName}'");

        Console.WriteLine($"Created plugin instance '{pluginInfo.Settings?.PluginName}'");

        return new PluginHost(pluginInfo, plugin);
    }
}
