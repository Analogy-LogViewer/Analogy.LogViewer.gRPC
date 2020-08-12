using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.gRPC.IAnalogy
{
    public class gRPCFactory : IAnalogyFactory
    {
        internal static readonly Guid Id = new Guid("9bd37cc2-daa7-4d17-974c-01ef3f3c79ba");
        public Guid FactoryId => Id;

        public string Title => "gRPC Receiver";

        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; } = new List<AnalogyChangeLog>
        {
            new AnalogyChangeLog("Initial version",AnalogChangeLogType.None, "Lior Banai",new DateTime(2020, 08, 12))
        };
        public IEnumerable<string> Contributors { get; } = new List<string> { "Lior Banai" };
        public string About { get; } = "Analogy gRPC Server";
    }
}
