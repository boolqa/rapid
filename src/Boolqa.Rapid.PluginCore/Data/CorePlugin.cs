using System;
using System.Collections.Generic;

namespace Boolqa.Rapid.PluginCore.Data;

public partial class CorePlugin
{
    public Guid PluginId { get; set; }

    public string? PluginKey { get; set; }

    public string? Name { get; set; }

    public string? Version { get; set; }
}
