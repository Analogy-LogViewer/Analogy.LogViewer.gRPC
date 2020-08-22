using Analogy.Interfaces;
using Analogy.LogServer;
using Analogy.LogViewer.gRPC.Managers;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.gRPC.IAnalogy
{
    public class gRPCReceiver : IAnalogyRealTimeDataProvider
    {
        private static CancellationTokenSource _cts;
        private IHost _hoster;
        private Task hostingTask;
        public string OptionalTitle { get; } = "gRPC Self Hosting Server";
        public Guid ID { get; } = new Guid("F475166B-5BBA-40E4-B8A2-4F9E8C40C761");
        public event EventHandler<AnalogyDataSourceDisconnectedArgs> OnDisconnected;
        public event EventHandler<AnalogyLogMessageArgs> OnMessageReady;
        public event EventHandler<AnalogyLogMessagesArgs> OnManyMessagesReady;

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
            _hoster = Program.CreateHostBuilder().Build();
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
                new AnalogyDataSourceDisconnectedArgs("user disconnected", Environment.MachineName, ID));
            _cts = new CancellationTokenSource();
            return GrpcEnvironment.KillServersAsync();
        }
    }
}
