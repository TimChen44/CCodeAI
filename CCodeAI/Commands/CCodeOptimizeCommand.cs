using CCodeAI.Extensions;
using CCodeAI.ViewModels;
using EnvDTE;
using Microsoft.VisualStudio.Text;
using System.IO;

namespace CCodeAI;

[Command(PackageIds.CCodeOptimizeCommandId)]
internal sealed class CCodeOptimizeCommand : BaseCommand<CCodeOptimizeCommand>
{

    protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
    {
        try
        {
            DocumentView docView = await VS.Documents.GetActiveDocumentViewAsync();
            if (docView?.TextView == null) return; //not a text window

            var selection = docView?.TextView.Selection;
            SnapshotSpan selectedSpan = selection.StreamSelectionSpan.SnapshotSpan;
            string selectedText = selectedSpan.GetText();

            if (string.IsNullOrWhiteSpace(selectedText))
            {
                ITextSnapshotLine line = selection.Start.Position.GetContainingLine();
                selectedText = line.GetText();
            }
            var tool = await CCodeExplainWindow.ShowAsync();

            var toolWindows = ((CCodeExplainWindowControl)tool.Content);

            await toolWindows.VM.CodeSkillAsync(
                selectedText,
                CodeExtension.GetCodeType(Path.GetExtension(docView.FilePath)),
                CodeSemanticFunctions.CodeOptimize);
        }
        catch (Exception ex)
        {
        }

    }
}
