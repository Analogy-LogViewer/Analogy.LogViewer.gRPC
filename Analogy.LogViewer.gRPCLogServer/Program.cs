using System.IO;
using System.Net;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
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
                       .SetBasePath(Directory.GetCurrentDirectory())  //location of the exe file
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
               }).UseWindowsService();
    }
}
