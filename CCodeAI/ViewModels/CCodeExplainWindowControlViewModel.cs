using CCodeAI.Models;
using CCodeAI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.SemanticKernel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CCodeAI.ViewModels
{
    public partial class CCodeExplainWindowControlViewModel : 
        ObservableObject
    {
        private AsyncRelayCommand _sendCommand;
        private string _question;
        private bool _isLoading = false;
        private CancellationTokenSource _coreSkillcancellationTokenSource;

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

        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading , value); }

        public void AiLoading()
        {
            IsLoading = true;
        }

        public void AiLoaded()
        {
            IsLoading = false;
        }

        private async Task SendAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(Question))
            {
                return;
            }

            AiLoading();
            try
            {
                var chatFunc = SKernel.CreateSemanticFunction(Resources.Resources.Chat);

                ChatDatas.Add(new ChatData()
                {
                    Who = EWho.User,
                    Content = Question,
                });

                var input = Question;
                Question = "";

                var result = await SKernel.RunAsync(input, chatFunc);

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
            catch (Exception ex)
            {
                await VS.MessageBox.ShowErrorAsync(ex.Message);
            }
            finally
            {
                AiLoaded();
            }
        }

        public async Task<string> CodeSkillAsync(
            string code,
            string extension,
            string semanticFuncation)
        {
            AiLoading();

            try
            {
                ChatDatas.Add(new ChatData()
                {
                    Content = Resources.Resources.InquiryAI,
                    Who = EWho.PlugIn
                });

                var explainFunc = SKernel.CreateSemanticFunction(semanticFuncation);

                var context = SKernel.CreateNewContext();
                context.Variables["extension"] = extension;

                _coreSkillcancellationTokenSource = new CancellationTokenSource();
                var result = await explainFunc.InvokeAsync(code, context, cancel: _coreSkillcancellationTokenSource.Token);

                if (result.ErrorOccurred)
                {
                    await VS.MessageBox.ShowErrorAsync(result.LastErrorDescription);
                    return null;
                }

                var content = result.ToString().Trim();

                ChatDatas.Add(new ChatData()
                {
                    Content = content,
                    Who = EWho.Assistant
                });

                return content;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                AiLoaded();                
                _coreSkillcancellationTokenSource?.Dispose();
                _coreSkillcancellationTokenSource = null;
            }
        }

        [RelayCommand]
        private void Clear()
        {
            var first = ChatDatas.First();
            ChatDatas.Clear();
            ChatDatas.Add(first);
        }

        [RelayCommand]
        private void OpenCodeGenWindow()
        {
            try
            {
                var window = new CodeGenWindow("c#", justCopy: true);
                if (window.ShowDialog() == true)
                {
                    Clipboard.SetDataObject(window.VM.Output);
                }
            }
            catch (Exception ex)
            {
                VS.MessageBox.ShowError(ex.Message);
            }

        }

        [RelayCommand]
        private void Cancel()
        {
            try
            {
                if (SendCommand.IsRunning)
                {
                    SendCommand.Cancel();
                }
                else
                {
                    _coreSkillcancellationTokenSource?.Cancel();
                }
            }
            catch (Exception ex)
            {
                VS.MessageBox.ShowError(ex.Message);
            }
            finally
            {
                AiLoaded();
            }
        }
    }
}
