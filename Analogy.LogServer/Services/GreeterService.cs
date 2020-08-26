using Analogy.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogServer.Services
{
    public class GreeterService : Analogy.AnalogyBase
    {
        private ILogger<GreeterService> Logger { get; }
        private MessagesContainer MessageContainer { get; }

        public GreeterService(MessagesContainer messageContainer, ILogger<GreeterService> logger)
        {
            Logger = logger;
            MessageContainer = messageContainer;
        }

        public override async Task<AnalogyMessageReply> SubscribeForSendMessages(
            IAsyncStreamReader<AnalogyLogMessage> requestStream, ServerCallContext context)
        {
            Logger.LogInformation("Client subscribe for sending messages");
            var tasks = Task.WhenAll(AwaitCancellation(context.CancellationToken),
                HandleClientActions(requestStream, context.CancellationToken));

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

        public override async Task SubscribeForConsumeMessages(AnalogyConsumerMessage request, IServerStreamWriter<AnalogyLogMessage> responseStream, ServerCallContext context)
        {
            MessageContainer.AddConsumer(request.Message, responseStream);
            await responseStream.WriteAsync(new AnalogyLogMessage
            {
                Category = "Server Message",
                Text = "Connection Established",
                Class = AnalogyLogClass.General.ToString(),
                Level = AnalogyLogLevel.AnalogyInformation.ToString(),
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
                Logger.LogError(e, "Error");
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

                        MessageContainer.AddMessage(message);
                    }
                    catch (Exception e)
                    {
                        Logger.LogError($"Error  receiving messages: {e}");
                    }
                }
            }
            catch (Exception e)
            {

                Logger.LogError($"Error: {e.Message}");
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

