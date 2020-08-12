using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Google.Protobuf.WellKnownTypes;
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
        public GRpcClient()
        {
            channel = GrpcChannel.ForAddress("https://localhost:5001");
        }

        public async Task<bool> InitClientAndSendTestMessage()
        {
            try
            {

                // The port number(5001) must match the port of the gRPC server.
                client = new gRPC.Analogy.AnalogyClient(channel);
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
                await client.SendMessageAsync(m);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

    }
}
