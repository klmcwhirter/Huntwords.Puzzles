using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace puzzles
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseSetting(WebHostDefaults.PreventHostingStartupKey, "true")
                .ConfigureLogging((context, factory) =>
                {
                    factory.AddConfiguration(context.Configuration.GetSection("Logging"));
                    factory.AddConsole();
                    factory.AddDebug();
                })
                .UseKestrel()
                // TODO: when no longer need docker-compose remove the bind to 32000
                .UseUrls("http://*:5000/;http://*:32000")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
