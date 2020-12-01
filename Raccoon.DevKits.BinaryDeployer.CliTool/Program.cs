using System;
using System.IO;
using Raccoon.DevKits.BinaryPackageService.Shared.Services;
using System.Linq;
using System.Collections.Generic;
using Raccoon.DevKits.BinaryPackageService.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Raccoon.DevKits.BinaryDeployer.CliTool
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Raccoon.DevKits.BinaryPackageService.Shared.Services.BinaryPackageService 
                packageService = new();
            try
            {
                Console.WriteLine("Start to pacakge...");
                string serviceName = GetRequiredArgumentByTag(args, 's');
                string packageDirectory = GetArgumentByTag(args, 'd')??Environment.CurrentDirectory;
                string deployDirectory = GetRequiredArgumentByTag(args, 'o');
                BinaryPackage binaryPackage = packageService.CreateFromDirectory(serviceName, deployDirectory);
                Console.WriteLine("Package Successfully!...");

                Console.WriteLine("Start to deploy...");
                using HttpClient client = new();
                HttpResponseMessage responseMessage = await client
                    .PostAsJsonAsync("http://localhost:5000", binaryPackage);

                Console.WriteLine($"Deploy server response with {responseMessage.StatusCode}");
            }catch(Exception err)
            {
                Console.WriteLine(err);
            }
            Console.ReadKey();
        }

        private static string? GetArgumentByTag(string[] args,char tag)
        {
            List<string> argsList = args.ToList();
            int index = argsList.IndexOf($"-{tag}");
            
            if(index is -1) { return null; }

            if(index>=args.Length)
            { throw new ArgumentException($"Wrong args format in -{tag}", nameof(args)); }
            return argsList.ElementAt(index + 1);
        }

        private static string GetRequiredArgumentByTag(string[] args,char tag)
        {
            string? argument = GetArgumentByTag(args, tag);
            if(argument is not null) { return argument; }
            else{ throw new ArgumentException($"Lack of required arg of -{tag}", nameof(args)); }
        }

    }
}
