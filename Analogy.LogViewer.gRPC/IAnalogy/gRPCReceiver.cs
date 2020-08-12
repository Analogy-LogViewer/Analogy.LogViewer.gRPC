using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.LogViewer.gRPC.Managers;
using Microsoft.Extensions.Hosting;

namespace Analogy.LogViewer.gRPC.IAnalogy
{
    public class gRPCReceiver : IAnalogyRealTimeDataProvider
    {
        private static CancellationTokenSource cts;
        private IHost hoster;
        private Task hostingTask;
        public string OptionalTitle { get; }
        public Guid ID { get; }
        public bool IsConnected => true;
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

            return Task.CompletedTask;

        }
        public async Task<bool> CanStartReceiving() => await Task.FromResult(true);


        public void MessageOpened(Interfaces.AnalogyLogMessage message)
        {
            //nop
        }
        public void StartReceiving()
        {
            hoster = Hoster.CreateHostBuilder().Build();
            hostingTask = hoster.StartAsync(cts.Token);
        }

        public void StopReceiving()
        {
            cts.Cancel();
            OnDisconnected?.Invoke(this,
                new AnalogyDataSourceDisconnectedArgs("user disconnected", Environment.MachineName, ID));
            cts = new CancellationTokenSource();

        }
    }
}
