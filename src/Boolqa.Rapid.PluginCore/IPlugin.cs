using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boolqa.Rapid.PluginCore;

public interface IPlugin
{
    Task Initialize();
    
    Task Configure();

    Task Run();
}
