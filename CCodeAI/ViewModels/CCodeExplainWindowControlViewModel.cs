using CCodeAI.Extensions;
using CCodeAI.Models;
using CCodeAI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnvDTE;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.KernelExtensions;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SemanticFunctions;
using Microsoft.VisualStudio.Text.Editor;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
            var dir = Path.Combine(
                Path.GetDirectoryName(typeof(CCodeExplainWindowControlViewModel).Assembly.Location),
                "CCodeAISkills");

            SkillsProvider = new SkillsProvider(dir);
            ChatDatas.Add(new WelcomeChatData(this));
            ChatSkill = SkillsProvider.GetSkills().First(p => p.Name == "ChatSkill");
            SelectedChatSkill = ChatSkill.SemanticFunctions.First();

            KernelFactory.Init();
        }

        public string Question { get => _question; set => SetProperty(ref _question, value); }

        public ObservableCollection<ChatData> ChatDatas { get; set; } = new();

        public IKernel SKernel => KernelFactory.SKernel;

        public AsyncRelayCommand SendCommand { get => _sendCommand ??= new AsyncRelayCommand(SendAsync); }

        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading , value); }

        public SkillsProvider SkillsProvider { get; }

        public bool AddHistory { get; set; } = true;

        public SkillModel ChatSkill { get; }

        public LocalSemanticFunctionModel SelectedChatSkill;

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

            if (AzureConfig.AllowCalls() == false)
            {
                ChatDatas.Add(new ChatData()
                {
                    Content = AzureConfig.OverLimitMsg,
                    Who = EWho.PlugIn
                });
                return;
            }

            AiLoading();
            try
            {
                var input = Question;
                Question = "";

                ChatDatas.Add(new ChatData()
                {
                    Who = EWho.User,
                    Content = input,
                });

                var culture = System.Globalization.CultureInfo.CurrentCulture.EnglishName;
                var chatCompletion = SKernel.GetService<IChatCompletion>();

                var config = SelectedChatSkill.GetCompletionConfig();

                var result = await chatCompletion.GenerateMessageAsync(
                    ChatDatas.GetChatHistory(SelectedChatSkill.SemanticString.Replace("{{$culture}}",culture)),
                    new ChatRequestSettings()
                    {
                        Temperature = config.Temperature,
                        TopP = config.TopP,
                        PresencePenalty = config.PresencePenalty,
                        FrequencyPenalty = config.FrequencyPenalty,
                        StopSequences = config.StopSequences,
                        MaxTokens = config.MaxTokens,
                    },
                    cancellationToken);

                ChatDatas.Add(new ChatData()
                {
                    Content = result,
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
            string semanticFunction)
        {
            if (AzureConfig.AllowCalls() == false)
            {
                ChatDatas.Add(new ChatData()
                {
                    Content = AzureConfig.OverLimitMsg,
                    Who = EWho.PlugIn
                });
                return AzureConfig.OverLimitMsg;
            }

            AiLoading();

            try
            {
                var explainFunc = SKernel.CreateSemanticFunction(semanticFunction);

                var context = SKernel.CreateNewContext();
                context.Variables["extension"] = extension;
                context.Variables["culture"] = System.Globalization.CultureInfo.CurrentCulture.EnglishName;

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

        public async Task<string> CodeSkillAsync(
            string code,
            string extension,
            ISKFunction semanticFunction)
        {
            if (AzureConfig.AllowCalls() == false)
            {
                ChatDatas.Add(new ChatData()
                {
                    Content = AzureConfig.OverLimitMsg,
                    Who = EWho.PlugIn
                });
                return AzureConfig.OverLimitMsg;
            }

            AiLoading();

            try
            {
                var context = SKernel.CreateNewContext();
                context.Variables["extension"] = extension;
                context.Variables["culture"] = System.Globalization.CultureInfo.CurrentCulture.EnglishName;

                _coreSkillcancellationTokenSource = new CancellationTokenSource();
                var result = await semanticFunction.InvokeAsync(code, context, cancel: _coreSkillcancellationTokenSource.Token);

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

        [RelayCommand]
        private void RemoveChatData(ChatData chatData)
        {
            if (chatData != null)
            {
                ChatDatas.Remove(chatData);
            }
        }
    }
}
