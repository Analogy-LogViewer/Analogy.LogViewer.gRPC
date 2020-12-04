using Analogy.LogViewer.gRPC.Managers;
using System;
using System.Windows.Forms;

namespace Analogy.LogViewer.gRPC
{
    public partial class grpcUserSettingsUC : UserControl
    {
        public grpcUserSettingsUC()
        {
            InitializeComponent();
        }

        private void grpcUserSettingsUC_Load(object sender, EventArgs e)
        {
            txtbRealTimeServerURL.Text = UserSettingsManager.UserSettings.Settings.GRPCAddress;
            txtbSelfHostingServerURL.Text = UserSettingsManager.UserSettings.Settings.SelfHostingServerAddress;
        }

        private void txtbRealTimeServerURL_TextChanged(object sender, EventArgs e)
        {
            UserSettingsManager.UserSettings.Settings.GRPCAddress = txtbRealTimeServerURL.Text;

        }

        private void txtbSelfHostingServerURL_TextChanged(object sender, EventArgs e)
        {
            UserSettingsManager.UserSettings.Settings.SelfHostingServerAddress = txtbSelfHostingServerURL.Text;

        }
    }
}
