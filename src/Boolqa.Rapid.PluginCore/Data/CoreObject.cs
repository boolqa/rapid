using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Boolqa.Rapid.PluginCore.Data;

public partial class CoreObject
{
    public Guid ObjectId { get; set; }

    public Guid UserId { get; set; }

    public string Type { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    //public virtual GroupObject? GroupObject { get; set; }

    //public virtual LinkObject? LinkObjectLinkObjectNavigation { get; set; }

    //public virtual SettingObject? SettingObject { get; set; }

    //public virtual SharedObject? SharedObjectObject { get; set; }

    public virtual ICollection<LinkObject> LinkObjectObjectFrom { get; set; } = new List<LinkObject>();

    public virtual ICollection<LinkObject> LinkObjectObjectTo { get; set; } = new List<LinkObject>();

    public virtual ICollection<SharedObject> SharedObjectTargetObjects { get; set; } = new List<SharedObject>();

    public virtual User User { get; set; } = null!;

    protected CoreObject()
    {
        
    }

    public CoreObject(Guid? id, Guid userId, string type, string name)
    {
        ObjectId = id ?? Guid.NewGuid();
        UserId = userId;
        // todo: падает ошибка в postgresql, что поле не содержит таймзону
        //CreatedAt = DateTime.UtcNow;
        //UpdatedAt = DateTime.UtcNow;
        Type = type;
        Name = name;
    }
}
