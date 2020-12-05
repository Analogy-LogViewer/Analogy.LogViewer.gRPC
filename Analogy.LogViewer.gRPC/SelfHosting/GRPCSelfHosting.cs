﻿using Analogy.Interfaces;
using Analogy.LogViewer.gRPC.Managers;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.gRPC.SelfHosting
{
    public class GRPCSelfHosting : Template.OnlineDataProvider
    {
        private static CancellationTokenSource _cts;
        private IHost? _hoster;
        private Task hostingTask;
        public override string OptionalTitle { get; set; } = "gRPC Self Hosting Server";
        public override Guid Id { get; set; } = new Guid("17115A81-E53F-4C6D-9504-F0D667C2FD08");
        public override Image? ConnectedLargeImage { get; set; } = null;
        public override Image? ConnectedSmallImage { get; set; } = null;
        public override Image? DisconnectedLargeImage { get; set; } = null;
        public override Image? DisconnectedSmallImage { get; set; } = null;

        public override Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            LogManager.Instance.SetLogger(logger);
            _cts = new CancellationTokenSource();
            return base.InitializeDataProviderAsync(logger);
        }

        public override async Task<bool> CanStartReceiving() => await Task.FromResult(true);


        void OnInstanceMessageReady(object s, AnalogyLogMessageArgs e) => MessageReady(s, e);
        public override Task StartReceiving()
        {
            if (_hoster == null)
            {
                _hoster = CreateHostBuilder().Build();
                gRPCReporter.Instance.OnMessageReady += OnInstanceMessageReady;
                gRPCReporter.Instance.OnDisconnected += Instance_OnDisconnected;
                hostingTask = _hoster.StartAsync(_cts.Token);
            }

            return Task.CompletedTask;
        }

        private void Instance_OnDisconnected(object sender, AnalogyDataSourceDisconnectedArgs e)
        {

            Disconnected(sender, e);
            gRPCReporter.Instance.OnDisconnected -= Instance_OnDisconnected;
        }

        public override Task StopReceiving()
        {
            gRPCReporter.Instance.OnMessageReady -= OnInstanceMessageReady;
            _cts.Cancel();
            Disconnected(this, new AnalogyDataSourceDisconnectedArgs("user disconnected", Environment.MachineName, Id));
            _cts = new CancellationTokenSource();
            return GrpcEnvironment.KillServersAsync();
        }

        private static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 7000, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });
                    });
                    webBuilder.UseStartup<Startup>();


                    //webBuilder.ConfigureKestrel((context, options) =>
                    //    {
                    //        options.Configure()
                    //            .Endpoint("Http", listenOptions =>
                    //            {
                    //                listenOptions.ListenOptions.Protocols = HttpProtocols.Http2;
                    //            });
                    //    })
                    //    .UseUrls(UserSettingsManager.UserSettings.Settings.SelfHostingServerAddress)
                    //    .UseStartup<Startup>();
                });

    }
}
