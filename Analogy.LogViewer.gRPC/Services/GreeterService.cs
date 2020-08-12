using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Analogy.LogViewer.gRPC
{
    public class GreeterService : Analogy.AnalogyBase
    {
        private readonly ILogger<GreeterService> _logger;

        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<AnalogyMessageReply> SendMessage(AnalogyLogMessage message, ServerCallContext context)
        {
            return Task.FromResult(new AnalogyMessageReply
            {
                Message = "Received at " + DateTime.Now

            });
        }
    }
}
