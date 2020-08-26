using System.IO;
using System.Net;
using System.Reflection;
using Analogy.LogServer.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Analogy.LogServer
{
    public class Program
    {
        public static void Main()
        {


            CreateHostBuilder().Build().Run();
            GrpcEnvironment.KillServersAsync();

        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
               {
                   var config = new ConfigurationBuilder()
                       .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))  //location of the exe file
                       .AddJsonFile("appsettings_LogServer.json", optional: false, reloadOnChange: true).Build();
                   webBuilder.UseConfiguration(config)
                       .ConfigureKestrel((context, options) =>
                       {
                           options.Configure(context.Configuration.GetSection("Kestrel"))
                               .Endpoint("Https", listenOptions =>
                               {
                                   listenOptions.ListenOptions.Protocols = HttpProtocols.Http2;
                               });
                       })
                       .UseStartup<Startup>();
               })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<CleanUpWorker>();
                }).UseWindowsService();
    }
}
