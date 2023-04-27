using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.SemanticKernel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace CCodeAI.ViewModels
{
    public partial class CCodeExplainWindowControlViewModel : ObservableObject
    {
        private AsyncRelayCommand _sendCommand;
        private string _question;

        public CCodeExplainWindowControlViewModel()
        {
            ChatDatas.Add(new ChatData()
            {
                Who = EWho.PlugIn,
                Content = "遇事不决，问AI。"
            });

            SKernel = Kernel.Builder.Configure(c =>
            {
                c.AddOpenAITextCompletionService(
                    "ccode",
                    OpenAIConfig.Model,
                    OpenAIConfig.OpenAIKey
                    );
                //c.AddAzureTextCompletionService
                //(
                //    "ccode",
                //    "text-davinci-003",
                //    AzureConfig.Endpoint,
                //    AzureConfig.AppKey
                //);
            })
            .Build();
        }

        public string Question { get => _question; set => SetProperty(ref _question , value); }

        public ObservableCollection<ChatData> ChatDatas { get; set; } = new();

        public IKernel SKernel { get; set; }

        public AsyncRelayCommand SendCommand { get => _sendCommand ??= new AsyncRelayCommand(SendAsync); }

        private async Task SendAsync(CancellationToken cancellationToken)
        {
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
                return;
            }

            ChatDatas.Add(new ChatData()
            {
                Content = result.ToString().Trim(),
                Who = EWho.Assistant
            });
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
