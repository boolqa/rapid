using System;
using System.Collections.Generic;

namespace Boolqa.Rapid.PluginCore.Data;

public partial class EntityHistory
{
    public Guid EntityHistoryId { get; set; }

    public Guid? EntityId { get; set; }

    public string Type { get; set; } = null!;

    public string FieldName { get; set; } = null!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }
}
