using CCodeAI.Common;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;

namespace CCodeAI
{
    [Command(PackageIds.CCodeAICommandId)]
    internal sealed class CCodeAICommandId : BaseCommand<CCodeAICommandId>
    {

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await CCodeExplainWindow.ShowAsync();
        }
    }
}
