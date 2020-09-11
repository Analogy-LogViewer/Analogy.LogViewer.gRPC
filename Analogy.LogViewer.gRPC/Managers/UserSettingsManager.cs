using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Analogy.LogViewer.gRPC.Managers
{
    public class UserSettingsManager
    {
        private static readonly Lazy<UserSettingsManager> _instance =
            new Lazy<UserSettingsManager>(() => new UserSettingsManager());
        public static UserSettingsManager UserSettings { get; set; } = _instance.Value;
        public string gRPCFileSetting { get; private set; } = "AnalogyGRPCSettings.json";
        public GRPCSettings Settings { get; set; }


        public UserSettingsManager()
        {
            if (File.Exists(gRPCFileSetting))
            {
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        ObjectCreationHandling = ObjectCreationHandling.Replace
                    };
                    string data = File.ReadAllText(gRPCFileSetting);
                    Settings = JsonConvert.DeserializeObject<GRPCSettings>(data, settings);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.LogException("Error loading user setting file",ex, "Analogy gRPC Parser");
                    Settings = new GRPCSettings();

                }
            }
            else
            {
                Settings = new GRPCSettings();
            }

        }
    }
}
