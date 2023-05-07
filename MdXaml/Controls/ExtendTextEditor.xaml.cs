using Community.VisualStudio.Toolkit;
using ICSharpCode.AvalonEdit;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MdXaml.Controls;

/// <summary>
/// ExtendTextEditor.xaml 的交互逻辑
/// </summary>
public partial class ExtendTextEditor : UserControl
{
    private DelegateCommand _copyCommand;
    private DelegateCommand _insertCommand;

    public ExtendTextEditor()
    {
        InitializeComponent();
        DataContext = this;
    }

    public string Text
    {
        get => _editor.Text; set
        {
            _editor.Text = value;
        }
    }

    public TextEditor TextEditor => _editor;

    public DelegateCommand CopyCommand => _copyCommand ??= new DelegateCommand(Copy);

    public DelegateCommand InsertCommand => _insertCommand ??= new DelegateCommand(InsertAsync);

    private async void InsertAsync()
    {
        try
        {
            //check
            if (string.IsNullOrWhiteSpace(Text)) { return; }

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            //get document view
            DocumentView? docView = await VS.Documents.GetActiveDocumentViewAsync();
            if (docView == null) return;
            if (docView?.TextView == null) return; //not a text window

            //get selection
            var selection = docView?.TextView?.Selection;
            if (selection == null) return;

            //insert code block
            var startIndex = selection.End.Position;
            ITextSnapshot? textSnapshot = docView?.TextBuffer?.Insert(startIndex, Text);

            if(textSnapshot == null) return;

            //Formart text
            docView.TextView.Selection.Select(new SnapshotSpan(textSnapshot, new Span(startIndex, Text.Length)),false);
            await VS.Commands.ExecuteAsync(Microsoft.VisualStudio.VSConstants.VSStd2KCmdID.FORMATSELECTION);
        }
        catch (Exception ex)
        {
            await VS.MessageBox.ShowErrorAsync(ex.Message);
        }
    }

    private void Copy(object obj)
    {
        try
        {
            Clipboard.SetDataObject(_editor.Text);
        }
        catch (Exception ex)
        {
            VS.MessageBox.ShowError(ex.Message);
        }
    }
}
