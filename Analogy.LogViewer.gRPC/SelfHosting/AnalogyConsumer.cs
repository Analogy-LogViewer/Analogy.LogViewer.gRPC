using Analogy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Analogy.LogServer;

namespace Analogy.LogViewer.gRPC.SelfHosting
{
    public class AnalogyConsumer : ILogConsumer
    {
        public Task ConsumeLog(AnalogyGRPCLogMessage m)
        {
            AnalogyLogMessage msg = new AnalogyLogMessage
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
                User = m.User,
                Id = string.IsNullOrEmpty(m.Id)
                    ? Guid.NewGuid()
                    : Guid.TryParse(m.Id, out Guid id) ? id : Guid.NewGuid(),
            };
            msg.AdditionalInformation = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in m.AdditionalInformation)
            {
                msg.AdditionalInformation.Add(pair.Key, pair.Value);
            }

            gRPCReporter.Instance.MessageReady(msg);
            return Task.CompletedTask;
        }
    }
}
