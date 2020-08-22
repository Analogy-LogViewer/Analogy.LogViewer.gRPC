using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.LogViewer.gRPC.Managers;
using Grpc.Core;
using Microsoft.Extensions.Hosting;

namespace Analogy.LogViewer.gRPC.IAnalogy
{
    public class gRPCReceiverClient : IAnalogyRealTimeDataProvider
    {
        private static CancellationTokenSource cts;
        private Task hostingTask;
        public string OptionalTitle { get; } = "gRPC client Receiver";
        public Guid ID { get; } = new Guid("F766707C-4FF8-4DC0-99BF-13D080266DF6");
        public event EventHandler<AnalogyDataSourceDisconnectedArgs> OnDisconnected;
        public event EventHandler<AnalogyLogMessageArgs> OnMessageReady;
        public event EventHandler<AnalogyLogMessagesArgs> OnManyMessagesReady;
        private AnalogyMessageConsumer consumer;
        public IAnalogyOfflineDataProvider FileOperationsHandler { get; } = null;
        public bool UseCustomColors { get; set; } = false;
        public IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);

        public Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            LogManager.Instance.SetLogger(logger);
            cts = new CancellationTokenSource();
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
            consumer = new AnalogyMessageConsumer(UserSettingsManager.UserSettings.Settings.GRPCAddress); 
            cts = new CancellationTokenSource();
            hostingTask = Task.Factory.StartNew(async () =>
            {
                var token = cts.Token;
                await foreach (var message in consumer.GetMessages())
                {
                    if (token.IsCancellationRequested)
                        break;
                    OnMessageReady?.Invoke(this,
                        new AnalogyLogMessageArgs(message, Environment.MachineName, OptionalTitle, ID));
                }
            });
            return Task.CompletedTask;
        }

        public Task StopReceiving()
        {
            gRPCReporter.Instance.OnMessageReady -= OnInstanceOnOnMessageReady;
            cts?.Cancel();
            OnDisconnected?.Invoke(this, new AnalogyDataSourceDisconnectedArgs("user disconnected", Environment.MachineName, ID));
            cts = new CancellationTokenSource();
           return consumer.Stop();
        }
    }
}
