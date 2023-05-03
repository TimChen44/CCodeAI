using EnvDTE;

namespace CCodeAI;

[Command(PackageIds.CCodeAICommandId)]
internal sealed class CCodeAICommandId : BaseCommand<CCodeAICommandId>
{

    protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
    {
        await CCodeExplainWindow.ShowAsync();
    }
}
