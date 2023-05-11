namespace Boolqa.Rapid.PluginCore.Data;

/// <summary>
/// Описывает предоставление права доступа к какому-либо <see cref="CoreObject"/>.
/// </summary>
public partial class SharedObject : CoreObject
{
    /// <summary>
    /// Идентификатор объекта, для которого предоставляется доступ.
    /// </summary>
    public Guid TargetObjectId { get; set; }

    /// <summary>
    /// Идентификатор пользователя, для которого предоставляется доступ.
    /// </summary>
    public Guid AccessUserId { get; set; }

    /// <summary>
    /// Режим предоставления права доступа.
    /// </summary>
    public SharedMode Mode { get; set; }

    #region Virtual props

    //public virtual CoreObject Object { get; set; } = null!;

    public virtual User? AccessUser { get; set; }

    public virtual CoreObject? TargetObject { get; set; }

    #endregion

    protected SharedObject()
    {

    }

    /// <summary>
    /// Создаёт новый объект предоставления права доступа.
    /// </summary>
    /// <param name="id">Идентификатор права.</param>
    /// <param name="userId">Идентификатор пользователя, который создаёт право.</param>
    /// <param name="name">Название права.</param>
    /// <param name="targetObjectId">Идентификатор объекта, для которого предоставляется доступ.</param>
    /// <param name="accessUserId">Идентификатор пользователя, для которого предоставляется доступ.</param>
    /// <param name="mode">Режим предоставления права доступа.</param>
    /// <remarks>Свойству <see cref="CoreObject.Type"/> автоматически задаётся "shared_object".</remarks>
    /// <exception cref="ArgumentException">
    /// Если <paramref name="targetObjectId"/> или <paramref name="accessUserId"/> передать <see cref="Guid.Empty"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если <paramref name="userId"/> равен <paramref name="accessUserId"/>.
    /// Если <paramref name="id"/> равен <paramref name="targetObjectId"/>.
    /// </exception>
    public SharedObject(Guid? id, Guid userId, string name, Guid targetObjectId, Guid accessUserId, SharedMode mode)
        : base(id, userId, "shared_object", name)
    {
        if (targetObjectId == Guid.Empty)
        {
            throw new ArgumentException(null, nameof(targetObjectId));
        }

        if (accessUserId == Guid.Empty)
        {
            throw new ArgumentException(null, nameof(accessUserId));
        }

        if (userId == accessUserId)
        {
            throw new InvalidOperationException(
                $"'{nameof(userId)}' and '{nameof(accessUserId)}' have the same values");
        }

        if (id == targetObjectId)
        {
            throw new InvalidOperationException(
                $"'{nameof(id)}' and '{nameof(targetObjectId)}' have the same values");
        }

        Mode = mode;
        TargetObjectId = targetObjectId;
        AccessUserId = accessUserId;
    }
}
