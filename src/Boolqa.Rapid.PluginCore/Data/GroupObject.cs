using System;
using System.Collections.Generic;

namespace Boolqa.Rapid.PluginCore.Data;

public partial class GroupObject : CoreObject
{
    protected GroupObject()
    {
        
    }

    public GroupObject(Guid? id, Guid userId, string name)
        : base(id, userId, "group", name)
    {
        
    }
    //public Guid GroupObjectId { get; set; }

    //public virtual CoreObject GroupObjectNavigation { get; set; } = null!;
}
