using CCodeAI.Extensions;
using CCodeAI.ViewModels;
using EnvDTE;
using Microsoft.VisualStudio.Text;
using System.IO;

namespace CCodeAI
{
    [Command(PackageIds.CCodeExplainCommandId)]
    internal sealed class CCodeExplainCommand : BaseCommand<CCodeExplainCommand>
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
                    // 获取光标所在行的文本  
                    ITextSnapshotLine line = selection.Start.Position.GetContainingLine();
                    selectedText = line.GetText();
                }
                var tool = await CCodeExplainWindow.ShowAsync();

                var toolWindows = ((CCodeExplainWindowControl)tool.Content);
                
                await toolWindows.VM.CodeSkillAsync(
                    selectedText, 
                    CodeExtension.GetCodeType(Path.GetExtension(docView.FilePath)),
                    CodeSemanticFunctions.CodeExplain);
            }
            catch (Exception ex)
            {
                await VS.MessageBox.ShowErrorAsync(ex.Message);
            }
        }
    }
}
