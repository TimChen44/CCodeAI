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
            ICSharpCode.AvalonEdit.TextEditor c;
#pragma warning restore CS0168
            InitializeComponent();
            DataContext = VM = new CCodeExplainWindowControlViewModel();
        }

        public CCodeExplainWindowControlViewModel VM { get; }
    }

    public class ChatData
    {
        public EWho Who { get; set; }

        public string WhoText => Who.ToString();

        public string Content { get; set; }

        public int Tokens { get; set; }

        public string ToPrompt()
        {
            return $"<|im_start|>{Who.ToString().ToLower()}{Content}<|im_end|>";
        }
    }

    public enum EWho
    {
        PlugIn,
        User,
        Assistant
    }
}
