namespace Boolqa.Rapid.PluginCore.Data
{
    /// <summary>
    /// Перечисление типов данных.
    /// </summary>
    public enum VariableType
    {
        /// <summary>
        /// Текст.
        /// </summary>
        String = 0,

        /// <summary>
        /// Логический тип.
        /// </summary>
        /// <remarks>Может принимать одно из двух значений: <see langword="true"/> или <see langword="false"/>.</remarks>
        Bool = 1,

        /// <summary>
        /// Целое число.
        /// </summary>
        Integer = 2,

        /// <summary>
        /// Число с плавающей запятой.
        /// </summary>
        Float = 3,

        /// <summary>
        /// Время.
        /// </summary>
        /// <remarks>Формат: HH:mm:ss</remarks>
        Time = 4,

        /// <summary>
        /// Дата и время.
        /// </summary>
        /// <remarks>Формат: dd.MM.yyyy HH:mm:ss</remarks>
        DateTime = 5,

        /// <summary>
        /// Перечисление константных значений.
        /// </summary>
        Enum = 6,

        /// <summary>
        /// Cron выражение.
        /// </summary>
        Cron = 7,
    }
}
