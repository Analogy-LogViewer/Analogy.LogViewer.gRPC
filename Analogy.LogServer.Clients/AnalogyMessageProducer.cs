using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;

namespace Analogy.LogServer.Clients
{
    public class AnalogyMessageProducer
    {
        private static int processId = Process.GetCurrentProcess().Id;
        private static string processName = Process.GetCurrentProcess().ProcessName;
        private static Analogy.AnalogyClient client { get; set; }
        private GrpcChannel channel;
        private AsyncClientStreamingCall<AnalogyLogMessage, AnalogyMessageReply> stream;
        private ILogger _logger;
        static AnalogyMessageProducer()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        }

        public AnalogyMessageProducer(ILogger logger)
        {
            _logger = logger;
            try
            {
                channel = GrpcChannel.ForAddress("http://localhost:6000");
                client = new Analogy.AnalogyClient(channel);
                stream = client.SubscribeForSendMessages();
            }
            catch (Exception e)
            {
                logger?.LogError(e, "Error creating gRPC Connection");
            }

        }

        public async Task Log(string text, string source, AnalogyLogLevel level, string category = "", [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = "")
        {
            var m = new AnalogyLogMessage()
            {
                Text = text,
                Category = category,
                Class = AnalogyLogClass.General.ToString(),
                Date = Timestamp.FromDateTime(DateTime.UtcNow),
                FileName = filePath,
                Id = Guid.NewGuid().ToString(),
                Level = level.ToString(),
                LineNumber = lineNumber,
                MachineName = Environment.MachineName,
                MethodName = memberName,
                Module = processName,
                ProcessId = processId,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Source = source,
                User = Environment.UserName
            };
            try
            {
                await stream.RequestStream.WriteAsync(m);

            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Error sending message to gRPC Server");
            }
        }
        public void StopReceiving()
        {
            try
            {
                channel.Dispose();
                GrpcEnvironment.ShutdownChannelsAsync();
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Error closing  gRPC connection to Server");

            }

        }
    }
}
