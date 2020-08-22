using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace Analogy.LogServer.Clients
{
    public class AnalogyMessageProducer
    {
        private static int processId = Process.GetCurrentProcess().Id;
        private static string processName = Process.GetCurrentProcess().ProcessName;
        private static Analogy.AnalogyClient client { get; set; }
        private GrpcChannel channel;
        private AsyncClientStreamingCall<AnalogyLogMessage, AnalogyMessageReply> stream;
        static AnalogyMessageProducer()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        }
        public AnalogyMessageProducer()
        {
            channel = GrpcChannel.ForAddress("http://localhost:6000");
            client = new Analogy.AnalogyClient(channel);
            stream = client.SubscribeForSendMessages();
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
            await stream.RequestStream.WriteAsync(m);
        }
        public void StopReceiving()
        {
            channel.Dispose();
            GrpcEnvironment.ShutdownChannelsAsync();
        }
    }
}
