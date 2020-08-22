using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Analogy.LogViewer.gRPC
{
    public class GRPCSettings
    {
        public string GRPCAddress { get; set; }

        public GRPCSettings()
        {
            GRPCAddress = "http://localhost:6000";
        }
    }
}
