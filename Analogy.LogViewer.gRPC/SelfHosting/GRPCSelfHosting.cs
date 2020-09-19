using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.LogViewer.gRPC.Managers;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Analogy.LogViewer.gRPC.SelfHosting
{
    public class GRPCSelfHosting : IAnalogyRealTimeDataProvider
    {
        private static CancellationTokenSource _cts;
        private IHost _hoster;
        private Task hostingTask;
        public string OptionalTitle { get; set; } = "gRPC Self Hosting Server";
        public Guid Id { get; set; } = new Guid("17115A81-E53F-4C6D-9504-F0D667C2FD08");
        public virtual Image ConnectedLargeImage { get; set; } = null;
        public virtual Image ConnectedSmallImage { get; set; } = null;
        public virtual Image DisconnectedLargeImage { get; set; } = null;
        public virtual Image DisconnectedSmallImage { get; set; } = null;

        public virtual event EventHandler<AnalogyDataSourceDisconnectedArgs> OnDisconnected;
        public virtual event EventHandler<AnalogyLogMessageArgs> OnMessageReady;
        public virtual event EventHandler<AnalogyLogMessagesArgs> OnManyMessagesReady;
        public IAnalogyOfflineDataProvider FileOperationsHandler { get; } = null;
        public bool UseCustomColors { get; set; } = false;
        public IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);

        public Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            LogManager.Instance.SetLogger(logger);
            _cts = new CancellationTokenSource();
            return Task.CompletedTask;

        }
        public async Task<bool> CanStartReceiving() => await Task.FromResult(true);


        public void MessageOpened(Interfaces.AnalogyLogMessage message)
        {
            //nop
        }
        void OnInstanceOnOnMessageReady(object s, AnalogyLogMessageArgs e) => OnMessageReady?.Invoke(s, e);
        public Task StartReceiving()
        {
            _hoster = CreateHostBuilder().Build();
            gRPCReporter.Instance.OnMessageReady += OnInstanceOnOnMessageReady;
            gRPCReporter.Instance.OnDisconnected += Instance_OnDisconnected;
            hostingTask = _hoster.StartAsync(_cts.Token);
            return Task.CompletedTask;
        }

        private void Instance_OnDisconnected(object sender, AnalogyDataSourceDisconnectedArgs e)
        {

            OnDisconnected?.Invoke(sender, e);
            gRPCReporter.Instance.OnDisconnected -= Instance_OnDisconnected;
        }

        public Task StopReceiving()
        {
            gRPCReporter.Instance.OnMessageReady -= OnInstanceOnOnMessageReady;
            _cts.Cancel();
            OnDisconnected?.Invoke(this,
                new AnalogyDataSourceDisconnectedArgs("user disconnected", Environment.MachineName, Id));
            _cts = new CancellationTokenSource();
            return GrpcEnvironment.KillServersAsync();
        }

        private static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var config = new ConfigurationBuilder()
                        .SetBasePath(
                            Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)) //location of the exe file
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
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
                });

    }
}
