using Boolqa.Rapid.PluginCore;

namespace Boolqa.Rapid.Plugin.SecondTestPlugin;

public class Plugin : IPlugin
{
    private readonly ICore _core;

    public Plugin(ICore core)
    {
        _core = core;
    }

    public async Task Initialize()
    {
        Console.WriteLine("Initialized Boolqa.Rapid.Plugin.SecondTestPlugin");
    }

    public async Task Run()
    {
        Console.WriteLine("runned Boolqa.Rapid.Plugin.SecondTestPlugin");
    }
}
