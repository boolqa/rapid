namespace Boolqa.Rapid.PluginCore.Data;

/// <summary>
/// Описывает связь 2-х <see cref="CoreObject"/> (A -> B).
/// </summary>
public partial class LinkObject : CoreObject
{
    /// <summary>
    /// Идентификатор объекта, который ссылается (A -> x).
    /// </summary>
    public Guid ObjectFromId { get; set; }

    /// <summary>
    /// Идентификатор объекта, на который ссылаются (x -> B).
    /// </summary>
    public Guid ObjectToId { get; set; }

    /// <summary>
    /// Тип связи.
    /// </summary>
    public ObjectLinkType LinkType { get; set; } = ObjectLinkType.Linked; // todo: пока есть только одно значение, поэтому задал явно

    #region Virtual props

    //public virtual CoreObject LinkObjectNavigation { get; set; } = null!;

    public virtual CoreObject ObjectFrom { get; set; } = null!;

    public virtual CoreObject ObjectTo { get; set; } = null!;

    #endregion

    protected LinkObject()
    {

    }
    
    /// <summary>
    /// Создаёт новую связь.
    /// </summary>
    /// <param name="id">Идентификатор связи.</param>
    /// <param name="userId">Идентификатор пользователя, который создаёт связь.</param>
    /// <param name="name">Название связи.</param>
    /// <param name="objectFromId">Идентификатор объекта, который ссылается.</param>
    /// <param name="objectToId">Идентификатор объекта, на который ссылаются.</param>
    /// <remarks>Свойству <see cref="CoreObject.Type"/> автоматически задаётся "link".</remarks>
    /// <exception cref="ArgumentException">
    /// Если <paramref name="objectFromId"/> или <paramref name="objectToId"/> передать <see cref="Guid.Empty"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если <paramref name="objectFromId"/> равен <paramref name="objectToId"/>.
    /// А также если <paramref name="objectFromId"/> или <paramref name="objectToId"/> равны <paramref name="id"/>.
    /// </exception>
    public LinkObject(Guid? id, Guid userId, string name, Guid objectFromId, Guid objectToId)
        : base(id, userId, "link", name)
    {
        if (objectFromId == Guid.Empty)
        {
            throw new ArgumentException(null, nameof(objectFromId));
        }
        
        if (objectToId == Guid.Empty)
        {
            throw new ArgumentException(null, nameof(objectToId));
        }

        if (objectFromId == objectToId)
        {
            throw new InvalidOperationException(
                $"'{nameof(objectFromId)}' and '{nameof(objectToId)}' have the same values");
        }
        
        if (id == objectFromId || id == objectToId)
        {
            throw new InvalidOperationException(
                $"'{nameof(objectFromId)}' or '{nameof(objectToId)}' can't have value of the '{nameof(id)}'");
        }

        ObjectFromId = objectFromId;
        ObjectToId = objectToId;
    }
}
