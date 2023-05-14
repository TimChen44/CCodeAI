using CCodeAI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.KernelExtensions;
using Microsoft.SemanticKernel.Orchestration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;

namespace CCodeAI.ViewModels;

public partial class CodeGenWindowViewModel : ObservableObject
{
    #region Fields
    private IAsyncRelayCommand _sendCommand;
    private RelayCommand<Window> _insertCommand;
    private LocalSemanticFunctionModel _selectedSemanticModel;
    private string _input;
    private string _output;
    #endregion

    #region Ctor
    public CodeGenWindowViewModel(string language)
    {
        KernelFactory.Init();

        var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "CCodeAI", "CCodeAISkills");

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var defaultDir = Path.Combine(
            Path.GetDirectoryName(typeof(CodeGenWindowViewModel).Assembly.Location),
            "Skills");

        SkillsProvider = new SkillsProvider(dir, defaultDir);

        SemanticFunctions = new ListCollectionView(
            SkillsProvider.GetSkills().SelectMany(p => p.SemanticFunctions).ToList());
        SemanticFunctions.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
        Language = language;
    }
    #endregion

    #region Properties
    public IKernel SKernel => KernelFactory.SKernel;

    public ISkillsProvider SkillsProvider { get; }

    public ListCollectionView SemanticFunctions { get; set; }

    public IAsyncRelayCommand SendCommand
    {
        get => _sendCommand ??= new AsyncRelayCommand(SendAsync, () => SelectedSemanticModel != null && !string.IsNullOrEmpty(Input));
    }

    public RelayCommand<Window> InsertCommand
    {
        get => _insertCommand ??= new RelayCommand<Window>(Insert, w => !string.IsNullOrWhiteSpace(Output));
    }

    public Action CloseAction { get; internal set; }

    public Action<bool?> DialogResult { get; internal set; }

    public string Language { get; }

    public bool JustCopy { get; internal set; }

    public string ActionBtnName => JustCopy ? Resources.Resources.Copy : Resources.Resources.InsertBtn;

    public LocalSemanticFunctionModel SelectedSemanticModel
    {
        get => _selectedSemanticModel; set
        {
            SetProperty(ref _selectedSemanticModel, value);
            SendCommand.NotifyCanExecuteChanged();
        }
    }

    public string Input
    {
        get => _input; set
        {
            SetProperty(ref _input, value);
            SendCommand?.NotifyCanExecuteChanged();
        }
    }

    public string Output
    {
        get => _output; set
        {
            SetProperty(ref _output, value);
            InsertCommand.NotifyCanExecuteChanged();
        }
    }
    #endregion

    #region Methods
    private async Task SendAsync(CancellationToken cancellationToken)
    {
        if (SelectedSemanticModel == null || string.IsNullOrWhiteSpace(Input))
        {
            return;
        }

        if (AzureConfig.AllowCalls() == false)
        {
            Output = AzureConfig.OverLimitMsg;
            return;
        }

        var skill = SKernel.ImportSemanticSkillFromDirectory(SelectedSemanticModel.RootDir, SelectedSemanticModel.Category);

        var variables = new ContextVariables();
        variables["input"] = Input;
        variables["language"] = Language;
        variables["culture"] = System.Globalization.CultureInfo.CurrentCulture.EnglishName;

        var result = await SKernel.RunAsync(
            variables,
            cancellationToken,
            skill[SelectedSemanticModel.Name]);

        if (result.ErrorOccurred)
        {
            await VS.MessageBox.ShowErrorAsync(result.LastErrorDescription);
        }

        Output = result.ToString().Trim();
    }

    [RelayCommand]
    private void Stop()
    {
        if (SendCommand.IsRunning)
        {
            SendCommand.Cancel();
        }
    }

    private void Insert(Window window)
    {
        window.DialogResult = true;
    }
    #endregion
}
