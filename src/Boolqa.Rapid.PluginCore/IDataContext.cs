using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boolqa.Rapid.PluginCore.Services;

namespace Boolqa.Rapid.PluginCore;

public interface IDataContext : IDisposable
{
    Lazy<ICoreObjectService> CoreObjectService { get; }

    Task<int> SaveChanges();
}
