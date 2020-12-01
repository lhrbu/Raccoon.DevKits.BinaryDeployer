using Microsoft.Extensions.Configuration;
using Raccoon.DevKits.BinaryDeployer.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Raccoon.DevKits.BinaryDeployer.Server.Services
{
    public class JsonFileServiceProvider:IServiceManagerProvider
    {
        private readonly IConfiguration _configuration;
        public IServiceManager ServiceManager { get; private set; }
        public JsonFileServiceProvider(
            IServiceManager serviceManager,
            IConfiguration configuration)
        {
            ServiceManager = serviceManager;
            _configuration = configuration;
            Reload();
        }

        public void Reload()
        {
            string filePath = _configuration["ServiceDefinitions"] ?? "ServiceDefinitions.json";
           
            using Stream stream = File.Open(filePath,FileMode.OpenOrCreate);
            byte[] buffer = new byte[stream.Length];
            try
            {
                IEnumerable<ServiceDefinition> serviceDefinitions = JsonSerializer
                    .Deserialize<IEnumerable<ServiceDefinition>>(buffer.AsSpan());
                foreach (ServiceDefinition serviceDefinition in serviceDefinitions)
                { ServiceManager.RegisterService(serviceDefinition); }
            }
            catch (JsonException) { }
        }

        public async ValueTask SaveChangeAsync()
        {
            string filePath = _configuration["ServiceDefinitions"] ?? "ServiceDefinitions.json";
            if (File.Exists(filePath)) { File.Delete(filePath); }
            using Stream stream = File.OpenWrite(filePath);
            await JsonSerializer.SerializeAsync(stream, ServiceManager.ServiceDefinitions.Values);
        }
    }
}
