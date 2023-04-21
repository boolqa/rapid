using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boolqa.Rapid.PluginCore.Data;

namespace Boolqa.Rapid.PluginCore.Services;

public interface ICoreObjectService
{
    CoreObject Add(CoreObject @object);

    T Add<T>(T @object)  where T : class;

    ValueTask<CoreObject?> Get(Guid objectId);
}
