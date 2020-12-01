using Raccoon.DevKits.BinaryPackageService.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.DevKits.BinaryPackageService.Shared.Services
{
    public class BinaryPackageService
    {
        public BinaryPackage CreateFromDirectory(string serviceName,string directory)
        {
            string tempFile = Path.GetTempFileName();
            ZipFile.CreateFromDirectory(directory, tempFile);

            using Stream stream = File.OpenRead(tempFile);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer);

            return new BinaryPackage
            {
                ServiceName = serviceName,
                Base64Content = Convert.ToBase64String(buffer.AsSpan())
            };
        }
    }
}
