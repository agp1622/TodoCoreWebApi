using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TodoCoreWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var scheme = Environment.GetEnvironmentVariable("SCHEME");
            if (string.IsNullOrWhiteSpace(scheme))
            {
                scheme = "http";
            }

            var port = Environment.GetEnvironmentVariable("PORT");
            if (string.IsNullOrWhiteSpace(port))
            {
                port = "5000";
            }

            var host = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls($"{scheme}://0.0.0.0:{port}")
                .Build();
            
            return host;
        }
            
    }
}
