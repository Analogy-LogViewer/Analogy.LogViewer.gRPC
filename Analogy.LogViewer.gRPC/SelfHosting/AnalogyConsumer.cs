using Analogy.Interfaces;
using System;
using System.Threading.Tasks;

namespace Analogy.LogViewer.gRPC.SelfHosting
{
    public class AnalogyConsumer : ILogConsumer
    {
        public Task ConsumeLog(AnalogyGRPCLogMessage m)
        {
            Interfaces.AnalogyLogMessage msg = new Interfaces.AnalogyLogMessage()
            {

                Category = m.Category,
                Level = (AnalogyLogLevel)m.Level,
                Class = (AnalogyLogClass)m.Class,
                Date = m.Date.ToDateTime().ToLocalTime(),
                FileName = m.FileName,
                LineNumber = m.LineNumber,
                MachineName = m.MachineName,
                MethodName = m.MethodName,
                Module = m.Module,
                ProcessId = m.ProcessId,
                Source = m.Source,
                Text = m.Text,
                ThreadId = m.ThreadId,
                User = m.User
            };

            msg.Id = string.IsNullOrEmpty(m.Id)
                ? Guid.NewGuid()
                : Guid.TryParse(m.Id, out Guid id) ? id : Guid.NewGuid();
            gRPCReporter.Instance.MessageReady(msg);
            return Task.CompletedTask;
        }
    }
}
