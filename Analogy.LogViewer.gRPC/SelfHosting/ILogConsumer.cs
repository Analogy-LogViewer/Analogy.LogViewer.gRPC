using Analogy.LogServer;
using System.Threading.Tasks;
namespace Analogy.LogViewer.gRPC.SelfHosting
{
    public interface ILogConsumer
    {
        Task ConsumeLog(AnalogyGRPCLogMessage msg);
    }
}