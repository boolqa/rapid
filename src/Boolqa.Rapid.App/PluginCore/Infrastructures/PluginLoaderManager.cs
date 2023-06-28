using System.Reflection;
using Boolqa.Rapid.PluginCore;
using Boolqa.Rapid.PluginCore.Models;
using McMaster.NETCore.Plugins;
using Microsoft.EntityFrameworkCore;

namespace Boolqa.Rapid.App.PluginCore.Infrastructures;

public class PluginLoaderManager
{
    public record PluginConfig(
        string PluginFolderPath,
        IConfigurationRoot Configuration,
        PluginSettings Settings);

    private readonly string _pluginsFolder;

    private readonly Type[] _sharedTypes;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _hostEnvironment;

    public PluginLoaderManager(IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        _pluginsFolder = Path.Combine(AppContext.BaseDirectory, "plugins");
        _sharedTypes = new Type[]
        {
            typeof(IPlugin),
            typeof(IServiceCollection),
            typeof(IEntityTypeConfiguration<>)
        };
        _configuration = configuration;
        _hostEnvironment = hostEnvironment;
    }

    /// <summary>
    /// Загружает плагины, их конфиги, сборки и ресурсы. 
    /// Но при этом не создает экземпляр класса плагина, это делает PluginHostManager.
    /// </summary>
    public List<PluginLoadContext> LoadPlugins()
    {
        var pluginSettings = new List<PluginConfig>(50);

        var pluginFolders = GetPluginsFolders();

        // Читаем конфиги плагинов
        foreach (var pluginFolderPath in pluginFolders)
        {
            if (string.IsNullOrEmpty(pluginFolderPath))
            {
                throw new InvalidOperationException($"Plugin FolderPath incorrect value: {pluginFolderPath}");
            }

            var pluginSetting = LoadConfig(pluginFolderPath);

            pluginSettings.Add(pluginSetting);
        }

        var pluginContexts = new List<PluginLoadContext>(pluginSettings.Count);

        // Формируем загрузчики плагинов
        foreach (var pluginSetting in pluginSettings)
        {
            var pluginContext = LoadPlugin(pluginSetting);

            pluginContexts.Add(pluginContext);
        }

        return pluginContexts;
    }

    private List<string> GetPluginsFolders()
    {
        var pluginFolders = new List<string>(1);

        if (Directory.Exists(_pluginsFolder))
        {
            var folders = Directory.GetDirectories(_pluginsFolder);

            if (folders != null)
            {
                pluginFolders.AddRange(folders);
            }
        }

        var pluginsPaths = _configuration.GetSection("Plugins")?.Get<string[]>();

        if (pluginsPaths != null)
        {
            foreach (var path in pluginsPaths)
            {
                var fullPath = Path.GetFullPath(path);
                pluginFolders.Add(fullPath);
            }
        }

        return pluginFolders;
    }

    /// <summary>
    /// Загружает конфиг плагина из его папки.
    /// </summary>
    /// <param name="pluginFolderPath">Путь до папки плагина.</param>
    /// <exception cref="InvalidOperationException"></exception>
    private PluginConfig LoadConfig(string pluginFolderPath)
    {
        var rootConfig = new ConfigurationBuilder()
            .SetBasePath(pluginFolderPath)
            .AddJsonFile("pluginsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("pluginsettings.Development.json", optional: true, reloadOnChange: true)
            .Build();

        if (rootConfig == null)
        {
            throw new InvalidOperationException($"Plugin load config failed");
        }

        var settings = rootConfig.Get<PluginSettings>() ??
            throw new InvalidOperationException("PluginSettings parsing failed");

        settings.PluginDll = settings.PluginDll.Replace(".dll", "", StringComparison.OrdinalIgnoreCase);
        settings.SeparateUiDll = settings.SeparateUiDll?.Replace(".dll", "", StringComparison.OrdinalIgnoreCase);

        return new PluginConfig(pluginFolderPath, rootConfig, settings);
    }

    /// <summary>
    /// Загружает плагин, его сборки и создает загрузчик плагина.
    /// </summary>
    /// <param name="pluginConfig">Конфиг плагина, загруженный из папки плагина.</param>
    private PluginLoadContext LoadPlugin(PluginConfig pluginConfig)
    {
        PluginLoader loader;

        var settings = pluginConfig.Settings;
        var loadedAssemblies = new List<Assembly>(2);

        var isUiSeparated = settings.PluginType == PluginType.Together && !string.IsNullOrEmpty(settings.SeparateUiDll);

        if (isUiSeparated)
        {
            loader = CreatePluginLoader(settings.SeparateUiDll!, pluginConfig);
            // Грузим UI сборку, т.к она считается основной, ибо зависит от backend dll
            loadedAssemblies.Add(loader.LoadDefaultAssembly());
            // Грузим backend сборку, чтобы запустить плагин через IPlugin
            var backendDllPath = GetDllPath(settings.PluginDll, pluginConfig);
            loadedAssemblies.Add(loader.LoadAssemblyFromPath(backendDllPath));
            // Возможно где-то тут надо будет подгружать сборки для реализации плагинов зависящих от других плагинов
        }
        else
        {
            loader = CreatePluginLoader(settings.PluginDll, pluginConfig);
            loadedAssemblies.Add(loader.LoadDefaultAssembly());
            // Возможно где-то тут надо будет подгружать сборки для реализации плагинов зависящих от других плагинов
        }

        var pluginContext = new PluginLoadContext()
        {
            FolderPath = pluginConfig.PluginFolderPath,
            FolderName = Path.GetFileName(pluginConfig.PluginFolderPath),
            ConfigurationRoot = pluginConfig.Configuration,
            Settings = pluginConfig.Settings,
            IsUiSeparated = isUiSeparated,
            Loader = loader,
            LoadedAssemblies = loadedAssemblies.AsReadOnly()
        };

        return pluginContext;
    }

    /// <summary>
    /// Создает загрузчик плагина.
    /// </summary>
    /// <param name="dllName">Имя сборки плагина.</param>
    /// <param name="pluginConfig">Конфиг плагина.</param>
    /// <exception cref="FileLoadException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private PluginLoader CreatePluginLoader(string dllName, PluginConfig pluginConfig)
    {
        var dllPath = GetDllPath(dllName, pluginConfig);

        if (!File.Exists(dllPath))
        {
            throw new FileLoadException("Plugin dll file not found", dllPath);
        }

        var loader = PluginLoader.CreateFromAssemblyFile(
            dllPath,
            // Тут передаются те типы, которые должны проходить сквозь изоляцию конекста, см. доку DotNetCorePlugins
            // Даже одноименные типы по умолчанию будут считаться как самостоятельный тип
            sharedTypes: _sharedTypes,
            configure: c =>
            {
                // Тут можно задать AssemblyLoadContext, если буду проблемы с загрузкой
                // плагинов которые ссылаются на плагины. Читай доку DotNetCorePlugins поиск по:
                // Overriding the Default Load Context

                // FIXPROMBLEM: Если есть непонятные проблемы взаимодействия логики ядра с плагинами, раскоментируй
                // эту строку Если это поможет, значит надо вычислить и доабвить нужные типы в _sharedTypes
                // c.PreferSharedTypes = true;
            });

        return loader ?? throw new InvalidOperationException("Plugin loader not created");
    }

    private string GetDllPath(string dllName, PluginConfig pluginLoadSetting) =>
        Path.Combine(pluginLoadSetting.PluginFolderPath, dllName + ".dll");
}
