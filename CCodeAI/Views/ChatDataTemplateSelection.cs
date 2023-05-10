using CCodeAI.Models;
using System.Windows;
using System.Windows.Controls;

namespace CCodeAI.Views;

internal class ChatDataTemplateSelector:DataTemplateSelector
{
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item == null) { return  null; }

        return item switch
        {
            WelcomeChatData => WelcomeChatDataTemplate,
            _ => ChatDataTemplate
        };
    }

    public DataTemplate WelcomeChatDataTemplate { get; set; }

    public DataTemplate ChatDataTemplate { get; set; }
}
