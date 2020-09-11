using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.LogServer.Clients;
using Analogy.LogViewer.gRPC.Managers;
using Grpc.Core;
using Microsoft.Extensions.Hosting;

namespace Analogy.LogViewer.gRPC.IAnalogy
{
    public class gRPCReceiverClient : IAnalogyRealTimeDataProvider
    {
        private static CancellationTokenSource cts;
        private Task hostingTask;
        public virtual string OptionalTitle { get; set; } = "Connect to gRPC Log Server";
        public virtual Guid Id { get; set; } = new Guid("F766707C-4FF8-4DC0-99BF-13D080266DF6");

        public virtual Image ConnectedLargeImage { get; set; } = null;
        public virtual Image ConnectedSmallImage { get; set; } = null;
        public virtual Image DisconnectedLargeImage { get; set; } = null;
        public virtual Image DisconnectedSmallImage { get; set; } = null;

        public virtual event EventHandler<AnalogyDataSourceDisconnectedArgs> OnDisconnected;
        public virtual event EventHandler<AnalogyLogMessageArgs> OnMessageReady;
        public virtual event EventHandler<AnalogyLogMessagesArgs> OnManyMessagesReady;
        private AnalogyMessageConsumer consumer;
        public virtual IAnalogyOfflineDataProvider FileOperationsHandler { get; set; } = null;
        public virtual bool UseCustomColors { get; set; } = false;
        public virtual IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public virtual (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);

        public virtual Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            LogManager.Instance.SetLogger(logger);
            cts = new CancellationTokenSource();
            return Task.CompletedTask;

        }
        public virtual async Task<bool> CanStartReceiving() => await Task.FromResult(true);


        public virtual void MessageOpened(Interfaces.AnalogyLogMessage message)
        {
            //nop
        }
        void OnInstanceOnOnMessageReady(object s, AnalogyLogMessageArgs e) => OnMessageReady?.Invoke(s, e);
        public virtual Task StartReceiving()
        {
            consumer = new AnalogyMessageConsumer(UserSettingsManager.UserSettings.Settings.GRPCAddress);
            cts = new CancellationTokenSource();
            hostingTask = Task.Factory.StartNew(async () =>
            {
                try
                {
                    var token = cts.Token;
                    await foreach (var message in consumer.GetMessages().WithCancellation(token))
                    {
                        if (token.IsCancellationRequested)
                            break;
                        OnMessageReady?.Invoke(this,
                            new AnalogyLogMessageArgs(message, Environment.MachineName, OptionalTitle, Id));
                    }
                }
                catch (Exception e)
                {
                    LogManager.Instance.LogError("Error: " + e.Message,nameof(gRPCReceiverClient));
                }
            });
            return Task.CompletedTask;
        }

        public virtual Task StopReceiving()
        {
            gRPCReporter.Instance.OnMessageReady -= OnInstanceOnOnMessageReady;
            cts?.Cancel();
            OnDisconnected?.Invoke(this, new AnalogyDataSourceDisconnectedArgs("user disconnected", Environment.MachineName, Id));
            cts = new CancellationTokenSource();
            return consumer.Stop();
        }
    }
}
