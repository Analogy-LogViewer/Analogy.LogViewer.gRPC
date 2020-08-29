using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Analogy.LogServer.Interfaces;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Analogy.LogServer
{
    public class GRPCLogConsumer : ILogConsumer
    {
        private readonly ILogger<GRPCLogConsumer> _logger;
        private List<(IServerStreamWriter<AnalogyLogMessage> stream, bool active)> clients;
        private readonly ReaderWriterLockSlim _sync = new ReaderWriterLockSlim();

        public GRPCLogConsumer(ILogger<GRPCLogConsumer> logger)
        {
            _logger = logger;
            clients = new List<(IServerStreamWriter<AnalogyLogMessage> stream, bool active)>();
        }

        public void AddGrpcConsumer(string requestMessage, IServerStreamWriter<AnalogyLogMessage> responseStream)
        {
            try
            {
                _logger.LogInformation("Adding client with message: {message}", requestMessage);
                _sync.EnterWriteLock();
                clients.Add((responseStream, true));
            }
            finally
            {
                _sync.ExitWriteLock();
            }
        }

        public async Task ConsumeLog(AnalogyLogMessage msg)
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
                    _logger.LogDebug(e, "Error sending message");
                }
            }
        }

        public override string ToString() => $"gRPC consumer";
    }
}
