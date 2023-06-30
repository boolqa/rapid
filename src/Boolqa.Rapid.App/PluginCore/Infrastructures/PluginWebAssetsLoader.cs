using Microsoft.Extensions.FileProviders;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Boolqa.Rapid.App.PluginCore.Infrastructures;

public class PluginWebAssetsLoader
{
    /// <summary>
    /// Configure the <see cref="IWebHostEnvironment"/> to use static web assets.
    /// </summary>
    /// <param name="environment">The application <see cref="IWebHostEnvironment"/>.</param>
    /// <param name="configuration">The host <see cref="IConfiguration"/>.</param>
    internal ManifestStaticWebAssetFileProvider? LoadStaticWebAssets(IWebHostEnvironment environment,
        IConfiguration configuration, Assembly assembly, string pluginFolderName)
    {
        var manifest = ResolveManifest(environment, configuration, assembly);
        if (manifest == null)
        {
            return null;
        }

        using (manifest)
        {
            return GetStaticWebAssetsCore(environment, manifest, pluginFolderName);
        }
    }

    internal static ManifestStaticWebAssetFileProvider GetStaticWebAssetsCore(IWebHostEnvironment environment, 
        Stream manifest, string pluginFolderName)
    {
        var staticWebAssetManifest = ManifestStaticWebAssetFileProvider.StaticWebAssetManifest.Parse(manifest);
        var provider = new ManifestStaticWebAssetFileProvider(
            pluginFolderName,
            staticWebAssetManifest,
            (contentRoot) => new PhysicalFileProvider(contentRoot));

        return provider;
    }

    internal static Stream? ResolveManifest(IWebHostEnvironment environment, IConfiguration configuration,
        Assembly assembly)
    {
        try
        {
            var candidate = configuration[WebHostDefaults.StaticWebAssetsKey] ?? 
                ResolveRelativeToAssembly(assembly);
            if (candidate != null && File.Exists(candidate))
            {
                return File.OpenRead(candidate);
            }

            // A missing manifest might simply mean that the feature is not enabled, so we simply
            // return early. Misconfigurations will be uncommon given that the entire process is automated
            // at build time.
            return default;
        }
        catch
        {
            return default;
        }
    }

    [UnconditionalSuppressMessage("SingleFile", "IL3000:Assembly.Location",
        Justification = "The code handles if the Assembly.Location is empty by calling AppContext.BaseDirectory. Workaround https://github.com/dotnet/runtime/issues/83607")]
    private static string? ResolveRelativeToAssembly(Assembly assembly)
    {
        var assemblyLocation = assembly.Location;
        var basePath = string.IsNullOrEmpty(assemblyLocation) ? AppContext.BaseDirectory : 
            Path.GetDirectoryName(assemblyLocation);

        return Path.Combine(basePath!, $"{assembly.GetName().Name}.staticwebassets.runtime.json");
    }

    //[UnconditionalSuppressMessage("SingleFile", "IL3000:Assembly.Location",
    //    Justification = "The code handles if the Assembly.Location is empty by calling AppContext.BaseDirectory. Workaround https://github.com/dotnet/runtime/issues/83607")]
    //private static string? ResolveRelativeToAssembly(IWebHostEnvironment environment)
    //{
    //    if (string.IsNullOrEmpty(environment.ApplicationName))
    //    {
    //        return null;
    //    }
    //    var assembly = Assembly.Load(environment.ApplicationName);
    //    var assemblyLocation = assembly.Location;
    //    var basePath = string.IsNullOrEmpty(assemblyLocation) ? AppContext.BaseDirectory : Path.GetDirectoryName(assemblyLocation);
    //    return Path.Combine(basePath!, $"{environment.ApplicationName}.staticwebassets.runtime.json");
    //}
}
