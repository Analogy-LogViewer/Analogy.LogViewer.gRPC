namespace Analogy.LogViewer.gRPC
{
    public class GRPCSettings
    {
        public string GRPCAddress { get; set; }
        public string SelfHostingServerAddress { get; set; }

        public GRPCSettings()
        {
            GRPCAddress = "http://localhost:6000";
            SelfHostingServerAddress = "http://localhost:7000";
        }
    }
}
