using System;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.LogServer.Clients;
using Analogy.LogViewer.gRPC.IAnalogy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analogy.LogViewer.gRPC.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            gRPCFactory gf = new gRPCFactory();
            Assert.IsTrue(gf.FactoryId != Guid.Empty);
        }
        [TestMethod]
        public async Task TestClients()
        {
            var c = new AnalogyMessageProducer();
            for (int i = 0; i < 100000; i++)
            {
                await c.Log("test " + i, "none", AnalogyLogLevel.Event);
                await Task.Delay(500);
            }
        }
    }
}
