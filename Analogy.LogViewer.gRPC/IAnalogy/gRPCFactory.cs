using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.LogViewer.gRPC.Properties;

namespace Analogy.LogViewer.gRPC.IAnalogy
{
    public class gRPCFactory : IAnalogyFactory
    {
        internal static readonly Guid Id = new Guid("9bd37cc2-daa7-4d17-974c-01ef3f3c79ba");
        public void RegisterNotificationCallback(INotificationReporter notificationReporter)
        {
            
        }

        public Guid FactoryId { get; set; } = Id;

        public string Title { get; set; } = "gRPC Receiver";

        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = new List<AnalogyChangeLog>
        {
            new AnalogyChangeLog("Initial version",AnalogChangeLogType.None, "Lior Banai",new DateTime(2020, 08, 12))
        };
        public IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public string About { get; set; } = "Analogy gRPC Server";
        public Image SmallImage { get; set; } = Resources.gRPC16x16;
        public Image LargeImage { get; set; } = Resources.gRPC32x32;


    }
}
