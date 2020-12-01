using Raccoon.DevKits.BinaryPackageService.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.DevKits.BinaryDeployer.Shared.Models
{
    public class BinaryDeployService
    {
        private readonly IServiceManager _serviceManager;
        public BinaryDeployService(IServiceManager serviceManager)
        { _serviceManager = serviceManager; }
        public void Deploy(BinaryPackage package)
        {
            string serviceName = package.ServiceName;
            if (_serviceManager.IsRunning(serviceName)) { _serviceManager.Stop(serviceName); }

            ServiceDefinition? serviceDefinition = _serviceManager.GetServiceDefinition(serviceName);
            if(serviceDefinition is null)
            { throw new ArgumentException($"{serviceName} is not registered!"); }

            string[] preservedFiles = serviceDefinition.PreservedFiles ?? Array.Empty<string>();
            string[] preservedDirectories = serviceDefinition.PreservedDirectories ?? Array.Empty<string>();
            string? preservedZip = CreateZipArchiveInTempDirectory(
                preservedFiles.Concat(preservedDirectories), "Preserved.zip");

            string deployDirectory = _serviceManager.GetDeployDirectory(serviceName);
            if (Directory.Exists(deployDirectory)) { Directory.Delete(deployDirectory, true); }
            Directory.CreateDirectory(deployDirectory);

            using MemoryStream stream = new(Convert.FromBase64String(package.Base64Content));
            ZipArchive zipArchive = new(stream, ZipArchiveMode.Read);
            zipArchive.ExtractToDirectory(deployDirectory);

            if (preservedZip is not null)
            { ZipFile.ExtractToDirectory(preservedZip, deployDirectory, true); }

            _serviceManager.Start(serviceName);
        }
        private string? CreateZipArchiveInTempDirectory(IEnumerable<string> paths, string zipFileName)
        {
            if (paths.Count() == 0) { return null; }
            string tempDirectory = Path.GetTempPath();
            foreach (string path in paths)
            {
                if (File.Exists(path))
                { File.Move(path, Path.Combine(tempDirectory, Path.GetFileName(path))); }
                else if (Directory.Exists(path))
                { Directory.Move(path, Path.Combine(tempDirectory, Path.GetDirectoryName(path)!)); }
            }
            string zipFilePath = Path.Combine(tempDirectory, zipFileName);
            ZipFile.CreateFromDirectory(tempDirectory, zipFilePath);
            return zipFilePath;
        }
    }
}
