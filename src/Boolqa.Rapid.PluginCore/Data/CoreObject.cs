namespace Boolqa.Rapid.PluginCore.Data;

/// <summary>
/// Базовая сущность. Описывает основные свойства, для всех других object-сущностей.
/// </summary>
/// <remarks>
/// Отдельно без object-сущностей не используется!
/// Изначально не был помечен как <see langword="abstract"/>, что давало возможность создать "безымянный" объект.
/// </remarks>
public abstract partial class CoreObject
{
    /// <summary>
    /// Глобальный уникальный идентификатор сущности.
    /// </summary>
    /// <remarks>Значение принимается извне, либо задаётся автоматически при создании экземпляра класса.</remarks>
    public Guid ObjectId { get; set; }

    /// <summary>
    /// Идентификатор пользователя, который создал/создаёт данную сущность.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Тип.
    /// </summary>
    /// <remarks>Задаётся при создании экземпляра класса.</remarks>
    public string Type { get; set; } = null!;

    /// <summary>
    /// Название.
    /// </summary>
    /// <remarks>Задаётся при создании экземпляра класса.</remarks>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Описание.
    /// </summary>
    /// <remarks>Может отсутствовать.</remarks>
    public string? Description { get; set; }

    /// <summary>
    /// Дата и время создания в UTC-формате.
    /// </summary>
    /// <remarks>Задаётся автоматически при создании экземпляра класса.</remarks>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата и время обновления/изменения в UTC-формате.
    /// </summary>
    /// <remarks>Задаётся автоматически при создании экземпляра класса.</remarks>
    public DateTime UpdatedAt { get; set; }

    #region Virtual props

    //public virtual GroupObject? GroupObject { get; set; }

    //public virtual LinkObject? LinkObjectLinkObjectNavigation { get; set; }

    //public virtual SettingObject? SettingObject { get; set; }

    //public virtual SharedObject? SharedObjectObject { get; set; }

    public virtual ICollection<LinkObject> LinkObjectObjectFrom { get; set; } = new List<LinkObject>();

    public virtual ICollection<LinkObject> LinkObjectObjectTo { get; set; } = new List<LinkObject>();

    public virtual ICollection<SharedObject> SharedObjectTargetObjects { get; set; } = new List<SharedObject>();

    public virtual User User { get; set; } = null!;

    #endregion

    protected CoreObject()
    {

    }

    /// <summary>
    /// Создаёт новый экземпляр базовой сущности.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="userId">Идентификатор пользователя, который создаёт сущность.</param>
    /// <param name="type">Тип сущности.</param>
    /// <param name="name">Название сущности.</param>
    /// <remarks>
    /// <para>
    /// Если <paramref name="id"/> передать <see langword="null"/>,
    /// то будет присвоен случайный <see cref="Guid"/>.
    /// </para>
    /// <para>
    /// Свойствам <see cref="CreatedAt"/> и <see cref="UpdatedAt"/> автоматически задаётся <see cref="DateTime.UtcNow"/>.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// Если <paramref name="id"/> передать <see cref="Guid.Empty"/>.
    /// Если <paramref name="userId"/> передать <see cref="Guid.Empty"/>.
    /// Если <paramref name="type"/> или <paramref name="name"/> передать пустое значение.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="type"/> или <paramref name="name"/> передать <see langword="null"/>.
    /// </exception>
    public CoreObject(Guid? id, Guid userId, string type, string name)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(null, nameof(id));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException(null, nameof(userId));
        }
        
        ArgumentException.ThrowIfNullOrEmpty(type);
        ArgumentException.ThrowIfNullOrEmpty(name);
        
        ObjectId = id ?? Guid.NewGuid();
        UserId = userId;

        // todo: падает ошибка в postgresql, что поле не содержит таймзону
        //CreatedAt = DateTime.UtcNow;
        //UpdatedAt = DateTime.UtcNow;

        Type = type;
        Name = name;
    }
}
