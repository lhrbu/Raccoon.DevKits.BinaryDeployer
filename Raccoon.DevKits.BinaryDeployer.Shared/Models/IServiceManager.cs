using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.DevKits.BinaryDeployer.Shared.Models
{
    public interface IServiceManager
    {
        void Stop(string serviceName);
        void Start(string serviceName);
        void Restart(string serviceName);
        bool IsRunning(string serviceName);
        string GetDeployDirectory(string serviceName);
        void RegisterService(ServiceDefinition serviceDefinition);
        void UnregisterService(string serviceName);
        ServiceDefinition? GetServiceDefinition(string serviceName);
        IReadOnlyDictionary<string, ServiceDefinition> ServiceDefinitions { get; }
        IReadOnlyDictionary<string, Process> Processes { get; }
    }
}
