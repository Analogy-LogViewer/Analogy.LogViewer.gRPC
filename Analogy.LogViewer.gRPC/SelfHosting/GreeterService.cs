#if NETCOREAPP3_1 || NET
using Analogy.LogServer;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.gRPC.SelfHosting
{
    public class GreeterService : Analogy.LogServer.Analogy.AnalogyBase
    {
        private readonly GRPCLogConsumer _grpcLogConsumer;
        private ILogger<GreeterService> Logger { get; }
        private MessagesContainer MessageContainer { get; }
        public GreeterService(MessagesContainer messageContainer, GRPCLogConsumer grpcLogConsumer, ILogger<GreeterService> logger)
        {
            _grpcLogConsumer = grpcLogConsumer;
            Logger = logger;
            MessageContainer = messageContainer;
        }
        public override async Task<AnalogyMessageReply> SubscribeForPublishingMessages(IAsyncStreamReader<AnalogyGRPCLogMessage> requestStream, ServerCallContext context)
        {
            string connectionInfo = context.RequestHeaders.Any()
                ? string.Join(",", context.RequestHeaders.Select(h => h.Value).ToArray())
                : string.Empty;
            Logger.LogInformation($"Client subscribe for sending messages. request info :{connectionInfo}");
            var tasks = Task.WhenAll(AwaitCancellation(context.CancellationToken), HandleClientPublishingMessages(requestStream, context, context.CancellationToken));
            Logger.LogWarning($"Client subscribe ended for: {connectionInfo}");
            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Logger.LogInformation(ex.Message);
            }
            Logger.LogInformation("Subscription finished.");
            return new AnalogyMessageReply { Message = "Reply at " + DateTime.Now };
        }

        public override async Task SubscribeForConsumingMessages(AnalogyConsumerMessage request, IServerStreamWriter<AnalogyGRPCLogMessage> responseStream, ServerCallContext context)
        {
            _grpcLogConsumer.AddGrpcConsumer(request.Message, responseStream);
            await responseStream.WriteAsync(new AnalogyGRPCLogMessage
            {
                Category = "Server Message",
                Text = "Connection Established. Streaming old messages (if Any)",
                Level = AnalogyGRPCLogLevel.Analogy,
                Date = Timestamp.FromDateTime(DateTime.UtcNow),
                FileName = "",
                Id = Guid.NewGuid().ToString(),
                LineNumber = 0,
                MachineName = Environment.MachineName,
                Module = Process.GetCurrentProcess().ProcessName,
                ProcessId = Process.GetCurrentProcess().Id,
                Source = "Server Operations",
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                User = Environment.UserName,
            });
            
            try
            {
                await AwaitCancellation(context.CancellationToken);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error");
            }
        }

        private async Task HandleClientPublishingMessages(IAsyncStreamReader<AnalogyGRPCLogMessage> requestStream,
            ServerCallContext serverCallContext, CancellationToken token)
        {
            try
            {
                await foreach (var message in requestStream.ReadAllAsync(token))
                {
                    try
                    {
                        if (message.Date == null)
                        {
                            message.Date = Timestamp.FromDateTime(DateTime.UtcNow);
                        }

                        if (string.IsNullOrEmpty(message.Id))
                        {
                            message.Id = Guid.NewGuid().ToString();
                        }

                        MessageContainer.AddMessage(message);
                    }
                    catch (Exception e)
                    {
                        Logger.LogError($"Error  receiving messages: {e}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Logger.LogError($"Consuming ended for Peer: {serverCallContext.Peer}");
            }
            catch (Exception e)
            {
                Logger.LogError($"Error: {e.Message}. Peer: {serverCallContext.Peer}");
            }
        }

        private Task AwaitCancellation(CancellationToken token)
        {
            var completion = new TaskCompletionSource<object>();
            token.Register(() => { completion.SetResult(null); });
            return completion.Task;
        }
    }
}
#endif