namespace Boolqa.Rapid.PluginCore.Data;

/// <summary>
/// Описывает настройку, позволяющую пользователю изменять параметры системы.
/// </summary>
public partial class SettingObject : CoreObject
{
    /// <summary>
    /// Тип настройки.
    /// </summary>
    public SettingType SettingType { get; set; }

    /// <summary>
    /// Тип данных используемый в <see cref="Value"/>.
    /// </summary>
    public VariableType VariableType { get; set; }

    /// <summary>
    /// Ключ параметра.
    /// </summary>
    /// <remarks>Не обязателен.</remarks>
    public string? Key { get; set; }

    /// <summary>
    /// Значение параметра.
    /// </summary>
    /// <remarks>Не обязателен.</remarks>
    public string? Value { get; set; }

    #region Virtual props

    //public virtual CoreObject SettingObjectNavigation { get; set; } = null!;

    #endregion

    protected SettingObject()
    {

    }
    
    /// <summary>
    /// Создаёт новую настройку.
    /// </summary>
    /// <param name="id">Идентификатор настройки.</param>
    /// <param name="userId">Идентификатор пользователя, который создаёт настройку.</param>
    /// <param name="name">Название настройки.</param>
    /// <param name="settingType">Тип настройки.</param>
    /// <param name="variableType">Тип используемых данных.</param>
    /// <remarks>Свойству <see cref="CoreObject.Type"/> автоматически задаётся "setting".</remarks>
    public SettingObject(Guid? id, Guid userId, string name, SettingType settingType, VariableType variableType)
        : base(id, userId, "setting", name)
    {
        SettingType = settingType;
        VariableType = variableType;
    }
}
