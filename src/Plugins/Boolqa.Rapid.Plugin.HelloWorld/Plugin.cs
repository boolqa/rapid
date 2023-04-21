using Boolqa.Rapid.PluginCore;

namespace Boolqa.Rapid.Plugin.HelloWorld;

public class Plugin : IPlugin
{
    private readonly ICore _core;

    public Plugin(ICore core)
    {
        _core = core;

        // Создает синглтон кторый будет доступен далее ForTest.Instance
        _ = new ForTest(core);
    }

    public async Task Initialize()
    {
        Console.WriteLine("Initialized Boolqa.Rapid.Plugin.HelloWorld");
    }

    // todo: подумать над тем, как правильно вызвать и передать контейнер
    public async Task Configure()
    {
        //_core.DataContextConfigure(typeof(HWObject));

        Console.WriteLine("Configured Boolqa.Rapid.Plugin.HelloWorld");
    }

    public async Task Run()
    {
        Console.WriteLine("runned Boolqa.Rapid.Plugin.HelloWorld");

        using var data = _core.GetNewDataContext();

        data.CoreObjectService.Value.Add(new HWObject(null, 
            new Guid("63D26DF9-0E3F-4E00-8187-1A5F7B000001"), "Test object HelloWorld", "HelloWorld text")
        {
            Description = "Test object descript"
        });

        var resSave = await data.SaveChanges();

        await ForTest.Instance.CreateTestObject("run");
    }
}
