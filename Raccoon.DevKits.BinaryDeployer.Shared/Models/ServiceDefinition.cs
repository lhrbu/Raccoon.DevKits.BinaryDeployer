using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.DevKits.BinaryDeployer.Shared.Models
{
    public record ServiceDefinition
    {
        public string Name { get; init; } = null!;
        public string FilePath { get; init; } = null!;
        public string[]? Arguments { get; init; }
        public string DeployDirectory { get; init; } = null!;
        public bool AutoRestart { get; init; } = false;
        public string StdoutLog { get; init; } = "stdout.log";
        public string StderrLog { get; init; } = "stderr.log";
        public string[]? PreservedFiles { get; init; }
        public string[]? PreservedDirectories { get; init; }
    }
}
