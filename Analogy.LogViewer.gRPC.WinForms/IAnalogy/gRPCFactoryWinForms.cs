using Analogy.Interfaces;
using Analogy.Interfaces.WinForms.Factories;
using Analogy.LogViewer.gRPC.IAnalogy;
using Analogy.LogViewer.gRPC.WinForms.Properties;
using Analogy.LogViewer.Template.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Analogy.LogViewer.gRPC.WinForms.IAnalogy
{
    public class gRPCFactoryWinForms : gRPCFactory, IAnalogyFactoryWinForms
    {
        internal static readonly Guid Id = new Guid("9bd37cc2-daa7-4d17-974c-01ef3f3c79ba");
        public override IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = new List<AnalogyChangeLog>
        {
            new AnalogyChangeLog("Add Self hosting of Analogy Log Server #46", Interfaces.DataTypes.AnalogChangeLogType.Improvement,
                "Lior Banai", new DateTime(2020, 12, 5), ""),
            new AnalogyChangeLog("Initial version", Interfaces.DataTypes.AnalogChangeLogType.None, "Lior Banai",
                new DateTime(2020, 08, 12), ""),
        };
        public override IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public override string About { get; set; } = "Analogy gRPC Server";
        public virtual Image? SmallImage { get; set; } = Resources.gRPC16x16;
        public virtual Image? LargeImage { get; set; } = Resources.gRPC32x32;
    }
}