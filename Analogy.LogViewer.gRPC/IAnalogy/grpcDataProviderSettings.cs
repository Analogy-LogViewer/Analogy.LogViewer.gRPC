using Analogy.LogViewer.gRPC.Managers;
using Analogy.LogViewer.Template;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analogy.LogViewer.gRPC.IAnalogy
{
    public class grpcDataProviderSettings : TemplateUserSettingsFactoryWinForms
    {
        public override string Title { get; set; } = "gRPC settings";
        public override UserControl DataProviderSettings { get; set; }

        public override Guid FactoryId { get; set; } = gRPCFactory.Id;
        public override Guid Id { get; set; } = new Guid("2f366d60-fe44-484a-a86c-cbf5fe51a5e5");

        public override void CreateUserControl(ILogger logger)
        {
            DataProviderSettings = new grpcUserSettingsUC();
        }

        public override Task SaveSettingsAsync()
        {
            UserSettingsManager.UserSettings.Save();
            return Task.CompletedTask;
        }
    }
}