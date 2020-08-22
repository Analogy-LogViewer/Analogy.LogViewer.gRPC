using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Analogy.LogServer.Services
{
    public class GreeterService : Analogy.AnalogyBase
    {
        private readonly ILogger<GreeterService> _logger;
        private MessagesContainer MessageContainer { get; }

        public GreeterService(MessagesContainer messageContainer, ILogger<GreeterService> logger)
        {
            _logger = logger;
            MessageContainer = messageContainer;
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
            MessageContainer.AddConsumer(request.Message, responseStream);
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
