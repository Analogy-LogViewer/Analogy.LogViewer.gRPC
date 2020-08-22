using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Analogy.LogServer;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Analogy.LogServer
{
    public class MessagesContainer
    {
        private readonly BlockingCollection<AnalogyLogMessage> messages;
        private List<(IServerStreamWriter<AnalogyLogMessage> stream, bool active)> clients;
        private ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
        private Task producer;
        private ILogger<MessagesContainer> _logger;

        public MessagesContainer(ILogger<MessagesContainer> logger)
        {
            _logger = logger;
            messages = new BlockingCollection<AnalogyLogMessage>();
            clients = new List<(IServerStreamWriter<AnalogyLogMessage> stream, bool active)>();
            producer = Task.Factory.StartNew(async () =>
            {
                foreach (var msg in messages.GetConsumingEnumerable())
                {
                    for (int i = 0; i < clients.Count; i++)
                    {
                        var (stream, active) = clients[i];
                        if (!active) continue;
                        try
                        {
                            await stream.WriteAsync(msg);
                        }
                        catch (Exception e)
                        {
                            clients[i] = (stream, false);
                            logger.LogDebug(e, "Error sending message");
                        }
                    }
                }
            });
        }
        public void AddMessage(AnalogyLogMessage m) => messages.Add(m);

        public void AddConsumer(string requestMessage, IServerStreamWriter<AnalogyLogMessage> responseStream)
        {
            try
            {
                _logger.LogInformation("Adding client with message: {message}", requestMessage);
                sync.EnterWriteLock();
                clients.Add((responseStream, true));
            }
            finally
            {
                sync.ExitWriteLock();
            }
        }
    }
}
