using System;
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
            Assert.IsTrue(gf.FactoryId!= Guid.Empty);
        }
    }
}
