namespace Boolqa.Rapid.PluginCore.Data;

/// <summary>
/// Описывает пользователя в системе.
/// </summary>
public partial class User
{
    /// <summary>
    /// Глобальный уникальный идентификатор.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Задаёт принадлежность к какому-либо "тенанту".
    /// </summary>
    /// <remarks>Не обязателен.</remarks>
    public Guid? TenantId { get; set; }

    /// <summary>
    /// Имя.
    /// </summary>
    /// <remarks>Может отсутствовать.</remarks>
    public string? Name { get; set; }

    #region Virtual props

    public virtual ICollection<CoreObject> Objects { get; set; } = new List<CoreObject>();

    public virtual ICollection<SharedObject> SharedObjects { get; set; } = new List<SharedObject>();

    public virtual Tenant? Tenant { get; set; }

    public virtual ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();

    #endregion

    protected User()
    {

    }

    /// <summary>
    /// Создаёт нового пользователя.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <remarks>
    /// Если <paramref name="id"/> передать <see langword="null"/>, то будет присвоен случайный <see cref="Guid"/>.
    /// </remarks>
    public User(Guid? id)
    {
        UserId = id ?? Guid.NewGuid();
    }
}
