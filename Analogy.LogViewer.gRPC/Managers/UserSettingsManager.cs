using Newtonsoft.Json;
using System;
using System.IO;
using Analogy.LogViewer.Template.Managers;
using Microsoft.Extensions.Logging;

namespace Analogy.LogViewer.gRPC.Managers
{
    public class UserSettingsManager
    {
        private static readonly Lazy<UserSettingsManager> _instance =
            new Lazy<UserSettingsManager>(() => new UserSettingsManager());
        public static UserSettingsManager UserSettings { get; set; } = _instance.Value;
        public string gRPCFileSetting { get; private set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Analogy.LogViewer", "AnalogyGRPCSettings.json");
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
                    LogManager.Instance.LogError(ex, "Error loading user setting file: {error}", ex);
                    Settings = new GRPCSettings();

                }
            }
            else
            {
                Settings = new GRPCSettings();
            }

        }
        public void Save()
        {
            try
            {
                File.WriteAllText(gRPCFileSetting, JsonConvert.SerializeObject(Settings));
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogCritical("", $"Unable to save file {gRPCFileSetting}: {ex}");

            }
        }
    }

}
