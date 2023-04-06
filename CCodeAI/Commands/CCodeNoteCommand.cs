using CCodeAI.Common;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using System.IO;

namespace CCodeAI
{
    [Command(PackageIds.CCodeNoteCommandId)]
    internal sealed class CCodeNoteCommand : BaseCommand<CCodeNoteCommand>
    {

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
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

            await toolWindows.CodeNote(selectedText, Path.GetExtension(docView.FilePath));
        }
    }
}
