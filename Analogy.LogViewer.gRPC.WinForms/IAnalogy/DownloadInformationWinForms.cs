using Analogy.LogViewer.gRPC.IAnalogy;
using Analogy.LogViewer.gRPC.WinForms.IAnalogy;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;

namespace Analogy.LogViewer.gRPC.WinForms
{
    public class DownloadInformationWinForms : Analogy.LogViewer.Template.AnalogyDownloadInformation
    {
        protected override string RepositoryURL { get; set; } = "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.gRPC";
        public override TargetFrameworkAttribute CurrentFrameworkAttribute { get; set; } = (TargetFrameworkAttribute)Assembly.GetExecutingAssembly().GetCustomAttribute(typeof(TargetFrameworkAttribute));

        public override Guid FactoryId { get; set; } = gRPCFactoryWinForms.Id;
        public override string Name { get; set; } = "gRPC Log Server Data Provider";

        private string? _installedVersionNumber;
        public override string InstalledVersionNumber
        {
            get
            {
                if (_installedVersionNumber != null)
                {
                    return _installedVersionNumber;
                }
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                _installedVersionNumber = fvi.FileVersion;
                return _installedVersionNumber;
            }
        }
    }
}