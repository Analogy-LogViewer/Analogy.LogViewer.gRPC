using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.LogServer;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Analogy.LogViewer.gRPC.SelfHosting
{
    public class GreeterService : Analogy.AnalogyBase
    {
        private readonly ILogger<GreeterService> _logger;
        private MessagesContainer MessageContainer { get; }
        private readonly GRPCLogConsumer _grpcLogConsumer;
        public GreeterService(MessagesContainer messageContainer, GRPCLogConsumer grpcLogConsumer, ILogger<GreeterService> logger)
        {
            _grpcLogConsumer = grpcLogConsumer;
            MessageContainer = messageContainer;

            _logger = logger;
        }


        public override async Task<AnalogyMessageReply> SubscribeForSendMessages(
            IAsyncStreamReader<AnalogyLogMessage> requestStream, ServerCallContext context)
        {
            _logger.LogInformation("Client subscribe for sending messages");
            var tasks = Task.WhenAll(AwaitCancellation(context.CancellationToken),
                HandleClientActions(requestStream, context.CancellationToken));

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
            _logger.LogInformation("Subscription finished.");
            return new AnalogyMessageReply { Message = "Reply at " + DateTime.Now };
        }

        public override async Task SubscribeForConsumeMessages(AnalogyConsumerMessage request, IServerStreamWriter<AnalogyLogMessage> responseStream, ServerCallContext context)
        {
            _grpcLogConsumer.AddGrpcConsumer(request.Message, responseStream);
            await responseStream.WriteAsync(new AnalogyLogMessage
            {
                Category = "Server Message",
                Text = "Connection Established",
                Class = AnalogyLogClass.General.ToString(),
                Level = AnalogyLogLevel.Analogy.ToString(),
                Date = Timestamp.FromDateTime(DateTime.UtcNow),
                FileName = "",
                Id = Guid.NewGuid().ToString(),
                LineNumber = 0,
                MachineName = Environment.MachineName,
                MethodName = nameof(SubscribeForConsumeMessages),
                Module = Process.GetCurrentProcess().ProcessName,
                ProcessId = Process.GetCurrentProcess().Id,
                Source = "Server Operations",
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                User = Environment.UserName

            });
            try
            {
                await AwaitCancellation(context.CancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error");
            }
        }

        private async Task HandleClientActions(IAsyncStreamReader<AnalogyLogMessage> requestStream, CancellationToken token)
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
                            message.Id = Guid.NewGuid().ToString();
                        MessageContainer.AddMessage(message);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Error  receiving messages: {e}");
                    }
                }
            }
            catch (Exception e)
            {

                _logger.LogError($"Error: {e.Message}");
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
