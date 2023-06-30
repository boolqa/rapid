using Castle.Core.Resource;

namespace Boolqa.Rapid.App.PluginCore.Infrastructures;

public class BlazorPluginRenderer
{
    private readonly PluginHostManager _pluginHoster;

    public BlazorPluginRenderer(PluginHostManager pluginHoster)
    {
        _pluginHoster = pluginHoster;
    }

    public string RenderStyleSheetLinks()
    {
        // todo: закэшировать результат, чтобы быстро отдавать строку на каждый вызов, т.к влияет на скорость рендеринга
        // todo: plugins тянуть из конфигуратора загрузчика плагинов
        var tags = _pluginHoster.OnlyUiPluginHosts.Select(ph =>
            $"<link rel=\"stylesheet\" href=\"plugins/{ph.Context.FolderName}/" +
            $"{ph.Context.MainLoadedAssembly.GetName().Name}.styles.css\" />");
        
        return string.Join(Environment.NewLine, tags);
    }
}
