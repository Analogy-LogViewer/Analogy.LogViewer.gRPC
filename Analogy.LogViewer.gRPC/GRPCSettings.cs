namespace Analogy.LogViewer.gRPC
{
    public class GRPCSettings
    {
        public string GRPCAddress { get; set; }
        public int SelfHostingServerPort { get; set; }

        public GRPCSettings()
        {
            GRPCAddress = "http://localhost:6000";
            SelfHostingServerPort = 7000;
        }
    }
}
