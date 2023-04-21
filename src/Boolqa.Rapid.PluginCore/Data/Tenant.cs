using System;
using System.Collections.Generic;

namespace Boolqa.Rapid.PluginCore.Data;

public partial class Tenant
{
    public Guid TenantId { get; set; }

    public Guid OwnerUserId { get; set; }

    public string Name { get; set; } = null!;

    public virtual User OwnerUser { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
