using ICSharpCode.AvalonEdit;
using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;

namespace CCodeAI.Views;

public sealed class AvalonEditBehavior : Behavior<TextEditor>
{
    public static readonly DependencyProperty CodeTextProperty =
        DependencyProperty.Register("CodeText", typeof(string), typeof(AvalonEditBehavior),
        new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback));

    public string CodeText
    {
        get { return (string)GetValue(CodeTextProperty); }
        set { SetValue(CodeTextProperty, value); }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject != null)
            AssociatedObject.TextChanged += AssociatedObjectOnTextChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        if (AssociatedObject != null)
            AssociatedObject.TextChanged -= AssociatedObjectOnTextChanged;
    }

    private void AssociatedObjectOnTextChanged(object sender, EventArgs eventArgs)
    {
        if (sender is TextEditor textEditor)
        {
            if (textEditor.Document != null)
                CodeText = textEditor.Document.Text;
        }
    }

    private static void PropertyChangedCallback(
        DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
        var behavior = dependencyObject as AvalonEditBehavior;
        if (behavior.AssociatedObject != null)
        {
            var editor = behavior.AssociatedObject;
            if (editor.Document != null)
            {
                var caretOffset = editor.CaretOffset;
                editor.Document.Text = dependencyPropertyChangedEventArgs.NewValue.ToString();
                if (caretOffset <= editor.Document.Text.Length) editor.CaretOffset = caretOffset;
            }
        }
    }
}
