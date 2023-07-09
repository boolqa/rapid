using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Boolqa.Rapid.App.PluginCore.Infrastructures;

/// <summary>
/// Looks up files using a collection of <see cref="IFileProvider"/>.
/// </summary>
public class PluginsCompositeFileProvider : IFileProvider
{
    private readonly IFileProvider _rootFileProvider;
    private readonly CompositeFileProvider _compositeFileProvider;
    private readonly Dictionary<string, IPluginFileProvider> _pluginFileProviders;
    private readonly PathScanner _pathScanner;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeFileProvider" /> class using a collection of file provider.
    /// </summary>
    /// <param name="fileProviders">The collection of <see cref="IFileProvider" /></param>
    public PluginsCompositeFileProvider(IFileProvider rootFileProvider,
        IEnumerable<IPluginFileProvider> pluginsFileProviders)
    {
        ArgumentNullException.ThrowIfNull(rootFileProvider);
        ArgumentNullException.ThrowIfNull(pluginsFileProviders);

        _rootFileProvider = rootFileProvider;
        _compositeFileProvider = new CompositeFileProvider(pluginsFileProviders.Prepend(rootFileProvider));
        _pluginFileProviders = pluginsFileProviders.DistinctBy(p => p.PluginFolderName)
            .ToDictionary(k => k.PluginFolderName);
        _pathScanner = new PathScanner(StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Locates a file at the given path.
    /// </summary>
    /// <param name="subpath">The path that identifies the file. </param>
    /// <returns>The file information. Caller must check Exists property. This will be the first existing <see cref="IFileInfo"/> returned by the provided <see cref="IFileProvider"/> or a not found <see cref="IFileInfo"/> if no existing files is found.</returns>
    public IFileInfo GetFileInfo(string subpath)
    {
        // Для оптимизации, чтобы не расходовать лишнюю память на детекцию корневых плагин папок
        // todo: plugins тянуть из конфигуратора загрузчика плагинов
        if (_pathScanner.StartWith(subpath, "plugins", 0, out var lastScanIndex))
        {
            var pluginFolderName = _pathScanner.GetFirtSegment(subpath, lastScanIndex);

            if (_pluginFileProviders.TryGetValue(pluginFolderName, out var fProvider))
            {
                var fileInfo = fProvider.GetFileInfo(subpath);
                if (fileInfo != null && fileInfo.Exists)
                {
                    return fileInfo;
                }
            }
        }

        return _rootFileProvider.GetFileInfo(subpath);
    }

    /// <summary>
    /// Enumerate a directory at the given path, if any.
    /// </summary>
    /// <param name="subpath">The path that identifies the directory</param>
    /// <returns>Contents of the directory. Caller must check Exists property.
    /// The content is a merge of the contents of the provided <see cref="IFileProvider"/>.
    /// When there is multiple <see cref="IFileInfo"/> with the same Name property, only the first one is included on the results.</returns>
    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        return _compositeFileProvider.GetDirectoryContents(subpath);
    }

    /// <summary>
    /// Creates a <see cref="IChangeToken"/> for the specified <paramref name="pattern"/>.
    /// </summary>
    /// <param name="pattern">Filter string used to determine what files or folders to monitor. Example: **/*.cs, *.*, subFolder/**/*.cshtml.</param>
    /// <returns>An <see cref="IChangeToken"/> that is notified when a file matching <paramref name="pattern"/> is added, modified or deleted.
    /// The change token will be notified when one of the change token returned by the provided <see cref="IFileProvider"/> will be notified.</returns>
    public IChangeToken Watch(string pattern)
    {
        return _compositeFileProvider.Watch(pattern);
    }
}
