using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Raccoon.DevKits.BinaryDeployer.Server.Services;
using Raccoon.DevKits.BinaryDeployer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raccoon.DevKits.BinaryDeployer.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceDefinitionsController : ControllerBase
    {
        private readonly IServiceManagerProvider _serviceManagerProvider;
        public ServiceDefinitionsController(IServiceManagerProvider serviceManagerProvider)
        { _serviceManagerProvider = serviceManagerProvider; }

        [HttpGet]
        public IEnumerable<ServiceDefinition> Get() => _serviceManagerProvider
            .ServiceManager.ServiceDefinitions.Values;

        [HttpPost]
        public async ValueTask PostAsync(ServiceDefinition serviceDefinition)
        {
            _serviceManagerProvider.ServiceManager.RegisterService(serviceDefinition);
            await _serviceManagerProvider.SaveChangeAsync();
        }
    }
}
