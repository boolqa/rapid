using Microsoft.Extensions.FileProviders;

namespace Boolqa.Rapid.App.PluginCore.Infrastructures;

public interface IPluginFileProvider : IFileProvider
{
    public string PluginFolderName { get; }
}
