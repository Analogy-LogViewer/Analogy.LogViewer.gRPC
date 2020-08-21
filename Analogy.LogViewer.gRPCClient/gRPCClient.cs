using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.LogViewer.gRPC;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using AnalogyLogMessage = Analogy.LogViewer.gRPC.AnalogyLogMessage;

namespace Analogy.LogViewer.gRPCClient
{
    public class GRpcClient
    {
        private gRPC.Analogy.AnalogyClient client { get; set; }
        private GrpcChannel channel;
        private static string _currentProcessName = Process.GetCurrentProcess().ProcessName;
        private static int _currentProcessId = Process.GetCurrentProcess().Id;
        private AsyncClientStreamingCall<AnalogyLogMessage, AnalogyMessageReply> streamToServer;
        private bool connected;

        public GRpcClient()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            channel = GrpcChannel.ForAddress("http://localhost:5001");
        }
        public async Task<bool> InitClientAndSendTestMessage()
        {
            try
            {
                // The port number(5001) must match the port of the gRPC server.
                client = new Analogy.LogViewer.gRPC.Analogy.AnalogyClient(channel);
                streamToServer = client.Subscribe();
                var m = new AnalogyLogMessage
                {
                    Text = "Test Message (Init)",
                    Category = "",
                    Class = AnalogyLogClass.General.ToString(),
                    Date = Timestamp.FromDateTime(DateTime.UtcNow),
                    FileName = "",
                    Id = Guid.NewGuid().ToString(),
                    Level = AnalogyLogLevel.Event.ToString(),
                    LineNumber = 0,
                    MachineName = Environment.MachineName,
                    MethodName = "",
                    Module = _currentProcessName,
                    ProcessId = _currentProcessId,
                    ThreadId = Thread.CurrentThread.ManagedThreadId,
                    Source = "None",
                    User = Environment.UserName
                };
                await streamToServer.RequestStream.WriteAsync(m);
                connected = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                connected = false;
                return false;
            }

            return true;
        }

        public async Task SendLogToGRPCService(string text, AnalogyLogLevel logLevel, int line, string source)
        {
            if (connected)
            {
                var m = new AnalogyLogMessage
                {
                    Text = text,
                    Category = "",
                    Class = AnalogyLogClass.General.ToString(),
                    Date = Timestamp.FromDateTime(DateTime.UtcNow),
                    FileName = "",
                    Id = Guid.NewGuid().ToString(),
                    Level = logLevel.ToString(),
                    LineNumber = line,
                    MachineName = Environment.MachineName,
                    MethodName = "",
                    Module = _currentProcessName,
                    ProcessId = _currentProcessId,
                    ThreadId = Thread.CurrentThread.ManagedThreadId,
                    Source = source,
                    User = Environment.UserName
                };
                await streamToServer.RequestStream.WriteAsync(m);
            }
        }

        public void CloseClient()
        {
            channel.Dispose();
            client = null;

        }

    }
}
