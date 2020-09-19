using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.LogViewer.gRPC.SelfHosting;

namespace Analogy.LogViewer.gRPC.IAnalogy
{
    public class gRPCDataProvider : IAnalogyDataProvidersFactory
    {
        public Guid FactoryId { get; set; } = gRPCFactory.Id;
        public string Title { get; set; } = "gRPC Receivers";
        public IEnumerable<IAnalogyDataProvider> DataProviders { get; } = new List<IAnalogyDataProvider> { new gRPCReceiverClient() };
    }
}
