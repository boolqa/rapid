using System;
using System.Collections.Generic;

namespace Boolqa.Rapid.PluginCore.Data;

public partial class LinkObject : CoreObject
{
    //public Guid LinkObjectId { get; set; }

    public Guid ObjectFromId { get; set; }

    public Guid ObjectToId { get; set; }

    public ObjectLinkType LinkType { get; set; }

    //public virtual CoreObject LinkObjectNavigation { get; set; } = null!;

    public virtual CoreObject ObjectFrom { get; set; } = null!;

    public virtual CoreObject ObjectTo { get; set; } = null!;

    protected LinkObject()
    {
        
    }

    public LinkObject(Guid? id, Guid userId, string name)
        : base(id, userId, "link", name)
    {
        
    }
}
