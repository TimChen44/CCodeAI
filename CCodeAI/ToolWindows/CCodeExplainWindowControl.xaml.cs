using CCodeAI.ViewModels;
using ICSharpCode.AvalonEdit;
using System.Windows.Controls;

namespace CCodeAI
{
    public partial class CCodeExplainWindowControl : UserControl
    {
        public CCodeExplainWindowControl()
        {
#pragma warning disable CS0168
            MdXaml.MarkdownScrollViewer m;
            TextEditor c;
#pragma warning restore CS0168
            InitializeComponent();
            DataContext = VM = new CCodeExplainWindowControlViewModel();
        }

        public CCodeExplainWindowControlViewModel VM { get; }
    }

    public enum EWho
    {
        PlugIn,
        User,
        Assistant,
        Welcome,
    }
}
