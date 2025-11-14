using Analogy.Interfaces.WinForms;
using Analogy.Interfaces.WinForms.Factories;
using Analogy.LogViewer.gRPC.IAnalogy;
using System.Collections.Generic;

namespace Analogy.LogViewer.gRPC.WinForms
{
    public class gRPCDataProviderWinForms : gRPCDataProvider, IAnalogyDataProvidersFactoryWinForms
    {
        public  new virtual IEnumerable<IAnalogyDataProviderWinForms> DataProviders { get; set; } = new List<IAnalogyDataProviderWinForms> { new gRPCServerClientWinForms() };
    }
}