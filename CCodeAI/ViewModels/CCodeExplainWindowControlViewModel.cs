using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.SemanticKernel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;

namespace CCodeAI.ViewModels
{
    public partial class CCodeExplainWindowControlViewModel : ObservableObject, INotifyPropertyChanged
    {
        private AsyncRelayCommand _sendCommand;
        private string _question;

        private bool _IsLoading = false;
        public bool IsLoading { get => _IsLoading; set => SetProperty(ref _IsLoading, value); }

        public void AiLoading()
        {
            IsLoading = true;
        }

        public void AiLoaded()
        {
            IsLoading = false;
        }

        public CCodeExplainWindowControlViewModel()
        {
            ChatDatas.Add(new ChatData()
            {
                Who = EWho.PlugIn,
                Content = "遇事不决，问AI。"
            });

            KernelFactory.Init();
        }

        public string Question { get => _question; set => SetProperty(ref _question, value); }

        public ObservableCollection<ChatData> ChatDatas { get; set; } = new();

        public IKernel SKernel => KernelFactory.SKernel;

        public AsyncRelayCommand SendCommand { get => _sendCommand ??= new AsyncRelayCommand(SendAsync); }

        private async Task SendAsync(CancellationToken cancellationToken)
        {
            AiLoading();
            var chatFunc = SKernel.CreateSemanticFunction(CodeSemanticFunctions.Chat);

            ChatDatas.Add(new ChatData()
            {
                Who = EWho.User,
                Content = Question,
            });

            var result = await SKernel.RunAsync(Question, chatFunc);

            if (result.ErrorOccurred)
            {
                await VS.MessageBox.ShowErrorAsync(result.LastErrorDescription);
                AiLoaded();
                return;
            }
            Question = "";
            ChatDatas.Add(new ChatData()
            {
                Content = result.ToString().Trim(),
                Who = EWho.Assistant
            });
            AiLoaded();
        }

        [RelayCommand]
        private void Clear()
        {
            var first = ChatDatas.First();
            ChatDatas.Clear();
            ChatDatas.Add(first);
        }
    }
}
