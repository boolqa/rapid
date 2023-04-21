using Boolqa.Rapid.PluginCore;

namespace Boolqa.Rapid.App.PluginCore.Infrastructures;

public class PluginHost
{
    public PluginLoadContext PluginInfo { get; }

    public IPlugin Plugin { get; }

    public PluginHost(PluginLoadContext pluginInfo, IPlugin plugin)
    {
        PluginInfo = pluginInfo;
        Plugin = plugin;
    }
}
