using CCodeAI.Extensions;
using CCodeAI.Views;
using EnvDTE;
using Microsoft.VisualStudio.Text;
using System.IO;

namespace CCodeAI;

[Command(PackageIds.CCodeCodeGenCommandId)]
internal sealed class CCodeCodeGenCommandId : BaseCommand<CCodeCodeGenCommandId>
{
    protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
    {
		try
		{
			DocumentView docView = await VS.Documents.GetActiveDocumentViewAsync();

            //programming language
			var lang = CodeExtension.GetCodeType(Path.GetExtension(docView?.FilePath));

            //selection
            var selection = docView?.TextView?.Selection;
            SnapshotSpan? selectedSpan = selection?.StreamSelectionSpan.SnapshotSpan;
            string selectedText = selectedSpan?.GetText();

            //show dialog
			var codeGenWindow = new CodeGenWindow(lang,selectedText);
            if (codeGenWindow.ShowDialog() != true)
			{
				return;			
			}
                        
            if (docView?.TextView == null) return; //not a text window

			var result = codeGenWindow.ResponseText;

            docView.TextBuffer.Insert(selection.End.Position, "\r\n" + result.Trim('`'));
        }
        catch (Exception ex)
		{
			await VS.MessageBox.ShowErrorAsync(ex.Message);
		}
    }
}
