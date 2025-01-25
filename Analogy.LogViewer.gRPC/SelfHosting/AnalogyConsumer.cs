using Analogy.Interfaces;
using Analogy.LogServer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Analogy.LogViewer.gRPC.SelfHosting
{
    public class AnalogyConsumer : ILogConsumer
    {
        public Task ConsumeLog(AnalogyGRPCLogMessage m)
        {
            AnalogyLogMessage msg = new AnalogyLogMessage
            {
                Level = (AnalogyLogLevel)m.Level,
                Class = (AnalogyLogClass)m.Class,
                Date = m.Date.ToDateTimeOffset(),
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
            msg.AddOrReplaceAdditionalProperty("Category", m.Category);
            foreach (KeyValuePair<string, string> pair in m.AdditionalInformation)
            {
                msg.AddOrReplaceAdditionalProperty(pair.Key, pair.Value);
            }

            gRPCReporter.Instance.MessageReady(msg);
            return Task.CompletedTask;
        }
    }
}