using Analogy.Interfaces;
using Analogy.Interfaces.WinForms;
using Analogy.LogViewer.gRPC.SelfHosting;
using Analogy.LogViewer.Template;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.gRPC.IAnalogy
{
    public class gRPCDataProvider : DataProvidersFactoryWinForms
    {
        public override Guid FactoryId { get; set; } = gRPCFactory.Id;
        public override string Title { get; set; } = "gRPC Receivers";
#if NETCOREAPP3_1 || NET
        public override IEnumerable<IAnalogyDataProviderWinForms> DataProviders { get; set; } = new List<IAnalogyDataProviderWinForms> { new gRPCServerClient(), new GRPCSelfHosting() };
#else
        public override IEnumerable<IAnalogyDataProviderWinForms> DataProviders { get; set; } = new List<IAnalogyDataProviderWinForms> { new gRPCServerClient() };
#endif
    }
}