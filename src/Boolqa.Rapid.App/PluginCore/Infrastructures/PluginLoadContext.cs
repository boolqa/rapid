using System.Collections.ObjectModel;
using System.Reflection;
using Boolqa.Rapid.PluginCore.Models;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.FileProviders;

namespace Boolqa.Rapid.App.PluginCore.Infrastructures;

/// <summary>
/// Конекст загрузки плагина.
/// </summary>
public record PluginLoadContext
{
    /// <summary>
    /// Путь до папки плагина.
    /// </summary>
    public required DirectoryInfo FolderPath { get; init; }

    /// <summary>
    /// Имя папки плагина.
    /// </summary>
    public required string FolderName { get; init; }

    /// <summary>
    /// Корневой конфиг плагина.
    /// </summary>
    public required IConfigurationRoot ConfigurationRoot { get; init; }

    /// <summary>
    /// Десериализованные базовые настройки плагина.
    /// </summary>
    public required PluginSettings Settings { get; init; }

    /// <summary>
    /// Отделена ли UI сборка от основной сборкой (backend) с логикой.
    /// </summary>
    public required bool IsUiSeparated { get; init; }

    /// <summary>
    /// Загрузчик плагина.
    /// </summary>
    public required PluginLoader Loader { get; init; }

    /// <summary>
    /// Основная сборка являющаяся точкой входа в плагин.
    /// </summary>
    public required Assembly MainLoadedAssembly { get; init; }

    /// <summary>
    /// Загруженные сборки плагина.
    /// </summary>
    public required ReadOnlyCollection<Assembly> LoadedAssemblies { get; init; }

    /// <summary>
    /// Провайдер для загрузки ресурсов плагина.
    /// </summary>
    internal IPluginFileProvider? ResourceFileProvider { get; init; }
}
