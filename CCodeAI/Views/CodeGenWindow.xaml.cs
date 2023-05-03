using CCodeAI.ViewModels;
using Microsoft.VisualStudio.Shell.Interop;
using System.Windows;

namespace CCodeAI.Views;

/// <summary>
/// CodeGenWindow.xaml 的交互逻辑
/// </summary>
public partial class CodeGenWindow : Window
{
    public CodeGenWindow(
        string language,
        string input = "",
        bool justCopy = false)
    {
        InitializeComponent();

        DataContext = VM = new CodeGenWindowViewModel(language)
        {
            Input = input,
            CloseAction = () => this.Close(),
            DialogResult = (b) => { DialogResult = b; },
            JustCopy = justCopy,
        };
    }

    /// <summary>
    /// Generated code by LLM Model
    /// </summary>
    public string ResponseText => VM.Output;

    internal CodeGenWindowViewModel VM { get; }
}
