using CCodeAI.ViewModels;
using ICSharpCode.AvalonEdit;
using System.Windows;
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

            cChatList.ScrollIntoView();
            DataContext = VM = new CCodeExplainWindowControlViewModel();
        }

        public CCodeExplainWindowControlViewModel VM { get; }

 

        private void SendFeedback_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/TimChen44/CCodeAI/issues");
        }

        private void TimChen44_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://space.bilibili.com/223212197");
        }
    }

    public static class ItemsControlExtensions
    {
        public static void ScrollIntoView(
            this ItemsControl control,
            object item)
        {
            FrameworkElement framework =
                control.ItemContainerGenerator.ContainerFromItem(item)
                as FrameworkElement;
            if (framework == null) { return; }
            framework.BringIntoView();
        }

        public static void ScrollIntoView(this ItemsControl control)
        {
            int count = control.Items.Count;
            if (count == 0) { return; }
            object item = control.Items[count - 1];
            control.ScrollIntoView(item);
        }
    }
}
