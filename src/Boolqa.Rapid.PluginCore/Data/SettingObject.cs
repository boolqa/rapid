using System;
using System.Collections.Generic;

namespace Boolqa.Rapid.PluginCore.Data;

public partial class SettingObject : CoreObject
{
    //public Guid SettingObjectId { get; set; }

    public SettingType SettingType { get; set; }

    public VariableType VariableType { get; set; }

    public string? Key { get; set; }

    public string? Value { get; set; }

    //public virtual CoreObject SettingObjectNavigation { get; set; } = null!;

    protected SettingObject()
    {
        
    }

    public SettingObject(Guid? id, Guid userId, string name)
        : base(id, userId, "setting", name)
    {
        
    }
}
