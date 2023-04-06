using Microsoft.VisualStudio.OLE.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CCodeAI.ToolWindows
{
    public class CodeRichTextBox : RichTextBox
    {
        public static readonly DependencyProperty CodeTextProperty =
             DependencyProperty.Register("CodeText", typeof(string), typeof(CodeRichTextBox), 
                 new PropertyMetadata("Default value"));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public string CodeText
        {
            get {
                return (string)GetValue(CodeTextProperty); 
            }
            set {
                SetValue(CodeTextProperty, value); 
            }
        }

    }
}
