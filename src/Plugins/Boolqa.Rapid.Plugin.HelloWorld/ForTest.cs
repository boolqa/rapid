using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boolqa.Rapid.PluginCore;

namespace Boolqa.Rapid.Plugin.HelloWorld;
public class ForTest
{
    public static ForTest Instance { get; private set; }

    private readonly ICore _core;

    public ForTest(ICore core)
    {
        _core = core;

        if (Instance == null)
        {
            Instance = this;
        }
    }

    public async Task CreateTestObject(string name)
    {
        using var data = _core.GetNewDataContext();

        var createdObject = data.CoreObjectService.Value.Add(new HWObject(null,
            new Guid("63D26DF9-0E3F-4E00-8187-1A5F7B000001"), "object", $"Test object HelloWorld {name}")
        {
            Description = "Test object descript"
        });

        var resSave = await data.SaveChanges();

        // todo: не подгружается User после сохранения изменений
        Console.WriteLine($"Object created: {createdObject.ObjectId} for user: {createdObject.User?.Name}");
    }
}
