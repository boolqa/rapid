namespace Boolqa.Rapid.PluginCore.Models;

public enum PluginType
{
    /// <summary>
    /// Backend with UI.
    /// </summary>
    Together,

    /// <summary>
    /// Only backend logic without UI.
    /// </summary>
    Backend,

    /// <summary>
    /// Only UI without backend.
    /// </summary>
    Ui
}
