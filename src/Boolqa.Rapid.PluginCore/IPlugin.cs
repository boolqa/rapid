namespace Boolqa.Rapid.PluginCore;

public interface IPlugin
{
    Task Initialize();

    Task Run();
}
