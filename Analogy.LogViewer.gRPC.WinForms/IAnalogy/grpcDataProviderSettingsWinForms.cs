using Analogy.LogViewer.gRPC.Managers;
using Analogy.LogViewer.Template.WinForms;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analogy.LogViewer.gRPC.WinForms.IAnalogy
{
    public class grpcDataProviderSettingsWinForms : TemplateUserSettingsFactoryWinForms
    {
        public override string Title { get; set; } = "gRPC settings";
        public override UserControl DataProviderSettings { get; set; }

        public override Guid FactoryId { get; set; } = gRPCFactoryWinForms.Id;
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