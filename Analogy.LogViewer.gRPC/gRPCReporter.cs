using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.gRPC.IAnalogy;
using System;

namespace Analogy.LogViewer.gRPC
{
    public class gRPCReporter
    {
        private static Lazy<gRPCReporter> _instance = new Lazy<gRPCReporter>(() => new gRPCReporter());

        public static gRPCReporter Instance { get; } = _instance.Value;

        public event EventHandler<AnalogyDataSourceDisconnectedArgs> OnDisconnected;
        public event EventHandler<AnalogyLogMessageArgs> OnMessageReady;
        public event EventHandler<AnalogyLogMessagesArgs> OnManyMessagesReady;

        public void MessageReady(AnalogyLogMessage m) => OnMessageReady?.Invoke(this, new AnalogyLogMessageArgs(m, Environment.MachineName, "", gRPCFactory.Id));
    }
}