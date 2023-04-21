using System;
using System.Collections.Generic;

namespace Boolqa.Rapid.PluginCore.Data;

public partial class SharedObject : CoreObject
{
    //public Guid ObjectId { get; set; }

    public Guid? TargetObjectId { get; set; }

    public Guid? AccessUserId { get; set; }

    public SharedMode Mode { get; set; }

    public virtual User? AccessUser { get; set; }

    //public virtual CoreObject Object { get; set; } = null!;

    public virtual CoreObject? TargetObject { get; set; }

    protected SharedObject()
    {
        
    }

    public SharedObject(Guid? id, Guid userId, string name)
        : base(id, userId, "shared_object", name)
    {
        
    }
}
