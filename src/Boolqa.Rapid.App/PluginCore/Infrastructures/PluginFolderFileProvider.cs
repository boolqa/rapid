using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Boolqa.Rapid.App.PluginCore.Infrastructures;

public class PluginFolderFileProvider : IPluginFileProvider
{
    private readonly string _pluginFolderName;
    private readonly DirectoryInfo _rootFolderPath;
    private readonly PhysicalFileProvider _physicalFileProvider;

    public string PluginFolderName => _pluginFolderName;

    public PluginFolderFileProvider(string pluginFolderName, DirectoryInfo rootFolderPath)
    {
        _pluginFolderName = pluginFolderName;
        _rootFolderPath = rootFolderPath;
        _physicalFileProvider = new PhysicalFileProvider(rootFolderPath.FullName + "/wwwroot");
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        var path = GetRealPath(subpath);

        if (path is null)
        {
            return new NotFoundDirectoryContents();
        }

        return _physicalFileProvider.GetDirectoryContents(path);
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        Console.WriteLine($"GetFileInfo {subpath} from {_pluginFolderName}");

        var filePath = GetRealPath(subpath);

        if (filePath is null)
        {
            return new NotFoundFileInfo(subpath);
        }

        return _physicalFileProvider.GetFileInfo(filePath);
    }

    public IChangeToken Watch(string filter)
    {
        return _physicalFileProvider.Watch(filter);
    }

    private string? GetRealPath(string path)
    {
        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

        // Путь должен состоять из названия папки плагинов + папка плагина как минимум
        if (segments.Length < 2
        // todo: plugins тянуть из конфигуратора загрузчика плагинов
            || !segments[0].Equals("plugins", StringComparison.OrdinalIgnoreCase)
            || !segments[1].Equals(_pluginFolderName, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        // todo: можно оптимальнее написать
        segments = segments.Skip(2).ToArray();

        var filePath = string.Join('/', segments);

        return filePath;
    }
}
