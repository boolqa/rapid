using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boolqa.Rapid.PluginCore.Data;

namespace Boolqa.Rapid.PluginCore.Services;
public interface IGenericObjectService<TObject> where TObject : CoreObject
{
    TObject Add(TObject @object);

    ValueTask<TObject?> Get(Guid objectId);
}
