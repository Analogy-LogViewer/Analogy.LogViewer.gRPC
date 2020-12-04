using Analogy.Interfaces;
using Analogy.LogViewer.gRPC.Properties;
using Analogy.LogViewer.Template;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Analogy.LogViewer.gRPC.IAnalogy
{
    public class gRPCFactory : PrimaryFactory
    {
        internal static readonly Guid Id = new Guid("9bd37cc2-daa7-4d17-974c-01ef3f3c79ba");

        public override Guid FactoryId { get; set; } = Id;

        public override string Title { get; set; } = "gRPC Receiver";

        public override IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = new List<AnalogyChangeLog>
        {
            new AnalogyChangeLog("Add Self Hosting mode",AnalogChangeLogType.Improvement, "Lior Banai",new DateTime(2020, 12, 4)),
            new AnalogyChangeLog("Initial version",AnalogChangeLogType.None, "Lior Banai",new DateTime(2020, 08, 12))
        };
        public override IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public override string About { get; set; } = "Analogy gRPC Server";
        public override Image? SmallImage { get; set; } = Resources.gRPC16x16;
        public override Image? LargeImage { get; set; } = Resources.gRPC32x32;

    }
}
