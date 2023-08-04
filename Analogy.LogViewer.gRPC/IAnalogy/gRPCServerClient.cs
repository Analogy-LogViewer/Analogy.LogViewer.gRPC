using Analogy.Interfaces;
using Analogy.LogServer.Clients;
using Analogy.LogViewer.gRPC.Managers;
using Analogy.LogViewer.Template;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Analogy.LogViewer.Template.Managers;
using Microsoft.Extensions.Logging;

namespace Analogy.LogViewer.gRPC.IAnalogy
{
    public class gRPCServerClient : OnlineDataProvider
    {
        private static CancellationTokenSource cts;
        private Task hostingTask;
        public override string OptionalTitle { get; set; } = "Connect to gRPC Log Server";
        public override Guid Id { get; set; } = new Guid("F766707C-4FF8-4DC0-99BF-13D080266DF6");

        private AnalogyMessageConsumer consumer;
        private bool Connected { get; set; }

        public override Task InitializeDataProvider(ILogger logger)
        {
            cts = new CancellationTokenSource();
            return base.InitializeDataProvider(logger);

        }

        public override async Task<bool> CanStartReceiving() => await Task.FromResult(true);


        public override Task StartReceiving()
        {
            consumer = new AnalogyMessageConsumer(UserSettingsManager.UserSettings.Settings.GRPCAddress);
            cts = new CancellationTokenSource();
            hostingTask = Task.Factory.StartNew(async () =>
            {
                try
                {

#if NETCOREAPP3_1 || NET
                    var token = cts.Token;
                    await foreach (var message in consumer.GetMessages().WithCancellation(token))
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        MessageReady(this,
                            new AnalogyLogMessageArgs(message, Environment.MachineName, OptionalTitle, Id));
                    }
#else
                    consumer.OnNewMessage += Consumer_OnNewMessage;
#endif
                    Connected = true;
                }
                catch (Exception e)
                {
                    LogManager.Instance.LogError("Error: " + e.Message, nameof(gRPCServerClient));
                }
            });
            return Task.CompletedTask;
        }

        private void Consumer_OnNewMessage(object sender, AnalogyLogMessage message)
        {
            MessageReady(this, new AnalogyLogMessageArgs(message, Environment.MachineName, OptionalTitle, Id));
        }

        public override Task StopReceiving()
        {
            cts?.Cancel();
            Disconnected(this, new AnalogyDataSourceDisconnectedArgs("user disconnected", Environment.MachineName, Id));
            cts = new CancellationTokenSource();
#if !NETCOREAPP3_1 && !NET
            consumer.OnNewMessage -= Consumer_OnNewMessage;

#endif
            Connected = false;
            return consumer.Stop();
        }

        public override async Task ShutDown()
        {
            if (Connected)

            {
                await StopReceiving();
            }
        }
    }
}
