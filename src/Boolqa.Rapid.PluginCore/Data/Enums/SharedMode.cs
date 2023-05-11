namespace Boolqa.Rapid.PluginCore.Data
{
    /// <summary>
    /// Перечисление режимов предоставления прав доступа.
    /// </summary>
    public enum SharedMode
    {
        /// <summary>
        /// Доступ к сущности не предоставляется никому кроме владельца.
        /// </summary>
        Denied = 0,

        /// <summary>
        /// Только чтение.
        /// </summary>
        Read = 1,

        /// <summary>
        /// Чтение и запись.
        /// </summary>
        Write = 2,

        /// <summary>
        /// Чтение, запись и возможность предоставить доступ кому-либо ещё.
        /// </summary>
        Share = 3,
    }
}
