global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using CCodeAI.Models;
using EnvDTE80;
using System.Runtime.InteropServices;
using System.Threading;

namespace CCodeAI
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.CCodeAIString)]
    [ProvideToolWindow(typeof(CCodeExplainWindow.Pane))]
    [ProvideOptionPage(typeof(ConfigOptions), "CCodeAI", "Config", 0, 0, true)]
    public sealed class CCodePackage : ToolkitPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            this.RegisterToolWindows();
            await this.RegisterCommandsAsync();

            var options = (ConfigOptions)GetDialogPage(typeof(ConfigOptions));
            if (string.IsNullOrEmpty( options.Endpoint)==false && string.IsNullOrEmpty(options.AppKey) == false)
            {
                AzureConfig.SetConfiguration(options.Endpoint, options.AppKey);

            }
            else
            {
                AzureConfig.DefaultConfiguration();
            }


        }
    }
}