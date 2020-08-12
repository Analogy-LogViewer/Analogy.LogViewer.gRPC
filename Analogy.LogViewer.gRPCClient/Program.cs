using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Analogy.LogViewer.gRPCClient
{
   public static class Program
    {
        public static async Task Main()
        {
            GRpcClient client = new GRpcClient();
            await client.InitClientAndSendTestMessage();
        }
    }
}
