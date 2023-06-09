﻿using EnvDTE;
using Microsoft.VisualStudio.Text;

namespace CCodeAI;

[Command(PackageIds.CCodeAddAskCommandId)]
internal sealed class CCodeAddAskCommandId : BaseCommand<CCodeAddAskCommandId>
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

            toolWindows.VM.Question += selectedText;

        }
        catch (Exception ex)
        {
            await VS.MessageBox.ShowErrorAsync(ex.Message);
        }
    }
}
