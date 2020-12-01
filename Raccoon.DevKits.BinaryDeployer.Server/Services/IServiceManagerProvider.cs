using Raccoon.DevKits.BinaryDeployer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raccoon.DevKits.BinaryDeployer.Server.Services
{
    public interface IServiceManagerProvider
    {
        IServiceManager ServiceManager { get; }
        void Reload();
        ValueTask SaveChangeAsync();
    }
}
