using Analogy.Interfaces;
using Analogy.Interfaces.WinForms;
using Analogy.Interfaces.WinForms.Factories;
using Analogy.LogViewer.gRPC.IAnalogy;
using Analogy.LogViewer.gRPC.SelfHosting;
using Analogy.LogViewer.gRPC.WinForms.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Analogy.LogViewer.gRPC.WinForms
{
    public class gRPCDataProviderWinForms : gRPCDataProvider, IAnalogyDataProvidersFactoryWinForms
    {
#if NET
        public override IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } = new List<IAnalogyDataProvider> { new gRPCServerClientWinForms(), new GRPCSelfHosting() };
#else
        public  override IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } = new List<IAnalogyDataProvider> { new gRPCServerClientWinForms() };
#endif

        public Image? GetDataFactorySmallImage(Guid componentId)
        {
            return Resources.gRPC16x16;
        }

        public Image? GetDataFacoryLargeImage(Guid componentId)
        {
            return Resources.gRPC32x32;
        }
    }
}