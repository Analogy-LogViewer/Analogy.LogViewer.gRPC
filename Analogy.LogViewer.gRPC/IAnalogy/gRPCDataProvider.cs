using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.gRPC.IAnalogy
{
    public class gRPCDataProvider : IAnalogyDataProvidersFactory
    {
        public Guid FactoryId { get; } = gRPCFactory.Id;
        public string Title { get; } = "gRPC Receivers";
        public IEnumerable<IAnalogyDataProvider> DataProviders { get; } = new List<IAnalogyDataProvider> { new gRPCReceiverClient() };
    }
}
