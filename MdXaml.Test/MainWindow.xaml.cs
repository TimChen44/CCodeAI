using System.IO;
using System.Windows;

namespace MdXaml.Test;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Markdown = File.ReadAllText("./Readme.md");
        DataContext = this;
    }

    public string Markdown { get; set; }
}
