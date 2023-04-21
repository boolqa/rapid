using System;
using System.Collections.Generic;

namespace Boolqa.Rapid.PluginCore.Data;

public partial class User
{
    public Guid UserId { get; set; }

    public Guid? TenantId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<CoreObject> Objects { get; set; } = new List<CoreObject>();

    public virtual ICollection<SharedObject> SharedObjects { get; set; } = new List<SharedObject>();

    public virtual Tenant? Tenant { get; set; }

    public virtual ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
}
