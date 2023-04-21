using Boolqa.Rapid.PluginCore.Data;

namespace Boolqa.Rapid.Plugin.HelloWorld;

public partial class HWObject : CoreObject
{
    // todo: использовать fluent api для маппинга
    public string LogText { get; } = string.Empty;

    protected HWObject()
    {

    }

    public HWObject(Guid? id, Guid userId, string name, string logText)
        : base(id, userId, "helloworld", name)
    {
        ObjectId = id ?? Guid.NewGuid();
        UserId = userId;
        Name = name;

        LogText = logText;
    }
}
