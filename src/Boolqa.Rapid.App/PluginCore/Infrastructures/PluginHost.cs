using Boolqa.Rapid.PluginCore;

namespace Boolqa.Rapid.App.PluginCore.Infrastructures;

public class PluginHost
{
    public PluginLoadContext Context { get; }

    public IPlugin Plugin { get; }

    public PluginHost(PluginLoadContext pluginInfo, IPlugin plugin)
    {
        Context = pluginInfo;
        Plugin = plugin;
    }
}
