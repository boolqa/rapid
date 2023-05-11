namespace Boolqa.Rapid.PluginCore.Data;

/// <summary>
/// Описывает группу связанных <see cref="CoreObject"/>.
/// </summary>
/// <remarks>Работает в связке с <see cref="LinkObject"/>.</remarks>
public partial class GroupObject : CoreObject
{
    #region Virtual props

    //public virtual CoreObject GroupObjectNavigation { get; set; } = null!;

    #endregion

    protected GroupObject()
    {

    }

    /// <summary>
    /// Создаёт новую группу.
    /// </summary>
    /// <param name="id">Идентификатор группы.</param>
    /// <param name="userId">Идентификатор пользователя, который создаёт группу.</param>
    /// <param name="name">Название группы.</param>
    /// <remarks>Свойству <see cref="CoreObject.Type"/> автоматически задаётся "group".</remarks>
    public GroupObject(Guid? id, Guid userId, string name)
        : base(id, userId, "group", name)
    {

    }
}
