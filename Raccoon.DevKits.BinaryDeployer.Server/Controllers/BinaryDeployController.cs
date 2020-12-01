using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raccoon.DevKits.BinaryDeployer.Shared.Models;
using Raccoon.DevKits.BinaryPackageService.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raccoon.DevKits.BinaryDeployer.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BinaryDeployController : ControllerBase
    {
        private readonly BinaryDeployService _binaryDeployService;
        public BinaryDeployController(BinaryDeployService binaryDeployService)
        { _binaryDeployService = binaryDeployService; }

        [HttpPost]
        public void Post(BinaryPackage binaryPackage) =>
            _binaryDeployService.Deploy(binaryPackage);
    }
}
