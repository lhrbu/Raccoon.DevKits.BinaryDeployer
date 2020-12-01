using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.DevKits.BinaryPackageService.Shared.Models
{
    public class BinaryPackage
    {
        public string ServiceName { get; init; } = null!;
        public string Base64Content { get; init; } = null!;
    }
}
