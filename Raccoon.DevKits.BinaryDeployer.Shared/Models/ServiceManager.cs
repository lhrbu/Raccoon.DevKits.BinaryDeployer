using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.DevKits.BinaryDeployer.Shared.Models
{
    public class ServiceManager : IServiceManager
    {
        public string GetDeployDirectory(string serviceName)
        {
            throw new NotImplementedException();
        }
        public bool IsRunning(string serviceName) => _processes.ContainsKey(serviceName);
        public void Restart(string serviceName)
        {
            Stop(serviceName);
            Start(serviceName);
        }
        public void Start(string serviceName)
        {
            ServiceDefinition serviceDefinition= CheckAndGetServiceStatus(serviceName);
            ProcessStartInfo startInfo = new(serviceDefinition.FilePath)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = serviceDefinition.DeployDirectory
            };
            if (serviceDefinition.Arguments is not null)
            { startInfo.Arguments = serviceDefinition.Arguments.Aggregate((prev, next) => $"{prev} {next}"); }

            Process process = new() { StartInfo = startInfo, EnableRaisingEvents = true };
            StreamWriter stdoutWriter = new(serviceDefinition.StdoutLog, true) { AutoFlush = true };
            StreamWriter stderrWriter = new(serviceDefinition.StderrLog, true) { AutoFlush = true };
            process.OutputDataReceived += (sender, e) =>stdoutWriter.WriteLine(e.Data);
            process.ErrorDataReceived += (sender, e) =>stderrWriter.WriteLine(e.Data);
            process.Exited += (sender, e) =>
            {
                stdoutWriter.Close();
                stderrWriter.Close();
                if (serviceDefinition.AutoRestart) { Start(serviceName); }
            };
            _processes.Add(serviceDefinition.Name, process);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }
        public void Stop(string serviceName)
        {
            if (_processes.TryGetValue(serviceName, out Process? process))
            {
                process?.Close();
            }
        }
        public void RegisterService(ServiceDefinition serviceDefinition)
        {
            if(!_serviceDefinitions.ContainsKey(serviceDefinition.Name))
            { _serviceDefinitions.Add(serviceDefinition.Name, serviceDefinition); }
        }
        public void UnregisterService(string serviceName)=>
            _serviceDefinitions.Remove(serviceName, out _);

        public IReadOnlyDictionary<string, ServiceDefinition> ServiceDefinitions => _serviceDefinitions;

        private readonly Dictionary<string, ServiceDefinition> _serviceDefinitions = new();

        public IReadOnlyDictionary<string, Process> Processes => _processes;
        private readonly Dictionary<string, Process> _processes = new();
        private ServiceDefinition CheckAndGetServiceStatus(string serviceName)
        {
            if (!_serviceDefinitions.ContainsKey(serviceName))
            {
                throw new ArgumentException($"{serviceName} is not registered in service manager",
                  nameof(serviceName));
            }
            else { return _serviceDefinitions.GetValueOrDefault(serviceName)!; }
        }

        public ServiceDefinition? GetServiceDefinition(string serviceName)=>
            _serviceDefinitions.GetValueOrDefault(serviceName);
        
    }
}
