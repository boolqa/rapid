namespace Boolqa.Rapid.PluginCore.Models
{
    /// <summary>
    /// Настройки плагина.
    /// </summary>
    public class PluginSettings
    {
        /// <summary>
        /// Уникальный GUID идентификатор плагина. Нужен для идентификации плагина и различия между одноименными плагинами.
        /// </summary>
        public Guid PluginId { get; set; }

        /// <summary>
        /// Имя плагина для отображения.
        /// </summary>
        public string PluginName { get; set; } = null!;

        /// <summary>
        /// Описание предназначения плагина.
        /// </summary>
        public string? PluginDescription { get; set; }

        /// <summary>
        /// Имя автора/разработчика плагина.
        /// </summary>
        public string PluginAuthor { get; set; } = null!;

        /// <summary>
        /// Тип плагина, необходимо корректно указать для корректной инициализации плагина.
        /// </summary>
        public PluginType PluginType { get; set; }

        /// <summary>
        /// Название dll плагина, необходим для загрузки плагина. Тут должна быть указана та сборка, 
        /// где реализуется IPlugin интерфейс.
        /// </summary>
        public string PluginDll { get; set; } = null!;

        /// <summary>
        /// Название dll ui части плагина, если она находится в отдельной сборке.
        /// </summary>
        public string? SeparateUiDll { get; set; }
    }
}
