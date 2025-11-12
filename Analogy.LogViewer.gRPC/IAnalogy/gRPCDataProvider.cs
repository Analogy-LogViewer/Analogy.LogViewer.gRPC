using Analogy.Interfaces;
using Analogy.Interfaces.WinForms;
using Analogy.LogViewer.gRPC.SelfHosting;
using Analogy.LogViewer.Template;
using Analogy.LogViewer.Template.WinForms;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.gRPC.IAnalogy
{
    public class gRPCDataProvider : DataProvidersFactoryWinForms
    {
        public override Guid FactoryId { get; set; } = gRPCFactory.Id;
        public override string Title { get; set; } = "gRPC Receivers";
        public override IEnumerable<IAnalogyDataProviderWinForms> DataProviders { get; } = new List<IAnalogyDataProviderWinForms> { new gRPCServerClient() };
    }
}