using CCodeAI.Common;
using Microsoft.VisualStudio.OLE.Interop;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using static System.Net.Mime.MediaTypeNames;

namespace CCodeAI
{
    public partial class CCodeExplainWindowControl : UserControl
    {
        List<ChatData> ChatDatas = new List<ChatData>();


        public CCodeExplainWindowControl()
        {
            InitializeComponent();
            ChatDatas.Add(new ChatData() { Who = EWho.PlugIn, Content = "遇事不决，问AI。" });
            cChatList.ItemsSource = ChatDatas;
            RichTextBox r = new RichTextBox();

        }

        //代码解释
        public async Task CodeExplain(string code, string extension)
        {
            ChatData userChatData = new ChatData()
            {
                Who = EWho.User,
                Content = $"""
                {GetCodeType(extension)}代码
                {code}
                解释代码逻辑
                """
            };

            await AskAIAsync(userChatData);
        }

        //注释代码
        public async Task CodeNote(string code, string extension)
        {
            ChatData userChatData = new ChatData()
            {
                Who = EWho.User,
                Content = $"""
                {GetCodeType(extension)}代码
                {code}
                添加代码注释
                """
            };

            await AskAIAsync(userChatData);
        }

        //注释代码
        public async Task CodeOptimize(string code, string extension)
        {
            ChatData userChatData = new ChatData()
            {
                Who = EWho.User,
                Content = $"""
                {GetCodeType(extension)}代码
                {code}
                优化代码
                """
            };

            await AskAIAsync(userChatData);
        }

        public string GetCodeType(string extension)
        {
            return extension switch
            {
                ".cs" => "C#",
                ".js" => "JavaScript",
                ".ts" => "TypeScript",
                ".cpp" => "C++",
                _ => ""
            };
        }


        //代码加入提问框
        public async Task CodeAddAsk(string code)
        {
            cSendText.Text = code.Trim() + "\r\n";
        }

        private async void cAskBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cSendText.Text)) return;
            ChatData userChatData = new ChatData()
            {
                Who = EWho.User,
                Content = cSendText.Text
            };
            cSendText.Text = "";
            await AskAIAsync(userChatData);

        }

        public void RefreshChatData()
        {
            cChatList.ItemsSource = null;
            cChatList.ItemsSource = ChatDatas;
            cChatList.ScrollIntoView(ChatDatas.Last());
        }


        public void AiLoading()
        {
            cAskBtn.IsEnabled = false;
            cAskBtn.Content = "咨询中";
        }
        public void AiLoaded()
        {
            cAskBtn.IsEnabled = true;
            cAskBtn.Content = "发送";
        }

        private async Task AskAIAsync(ChatData userChatData, int length = 1)
        {
            AiLoading();

            try
            {
                StringBuilder prompt = new StringBuilder();
                ChatDatas.Add(userChatData);
                RefreshChatData();

                var count = length;
                var promptTokens = 0;
                for (int i = ChatDatas.Count - 1; i >= 0 && count > 0; i--)
                {
                    count--;
                    if (ChatDatas[i].Who == EWho.PlugIn) continue;
                    prompt.Append(ChatDatas[i].ToPrompt());
                    promptTokens += ChatDatas[i].Tokens;
                }
                prompt.Append("<|im_start|>assistant");

                GptRequest request = new GptRequest
                {
                    prompt = prompt.ToString(),
                    max_tokens = 1200,
                    temperature = 0.7,
                    frequency_penalty = 0,
                    presence_penalty = 0,
                    top_p = 0.95,
                    stop = new string[] { "<|im_end|>" }
                };

                using HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("api-key", AzureConfig.AppKey);

                string url = "https://ccode.openai.azure.com/openai/deployments/gpt-35-turbo/completions?api-version=2022-12-01";

                var httpResponse = await httpClient.PostAsJsonAsync(url, request);
                TextCompletion response = await httpResponse.Content.ReadFromJsonAsync<TextCompletion>();
                var chatData = new ChatData()
                {
                    Who = EWho.Assistant,
                    Content = response?.choices.FirstOrDefault()?.text?.Trim(new char[] { '\n' }),
                    Tokens = response.usage.completion_tokens,
                };

                userChatData.Tokens = response.usage.prompt_tokens - promptTokens;
                ChatDatas.Add(chatData);
            }
            catch (Exception ex)
            {
                ChatDatas.Add(new ChatData()
                {
                    Who = EWho.PlugIn,
                    Content = $"请求发生异常：{ex.Message}"
                }); 
            }
     
            RefreshChatData();
            AiLoaded();

         
        }

        private void cClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ChatDatas.Clear();
            ChatDatas.Add(new ChatData() { Who = EWho.PlugIn, Content = "遇事不决，问AI。" });
            RefreshChatData();
        }



        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RichTextBox_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var richText = ((RichTextBox)sender);
            string text = new TextRange(richText.Document.ContentStart, richText.Document.ContentEnd).Text;
            if (text.Length > 0)
            {
                ((RichTextBox)sender).AppendText("sfsfsfs");
            }
        }
    }

    public class ChatData
    {
        public EWho Who { get; set; }

        public string WhoText => Who.ToString();
        public string Content { get; set; }
        public int Tokens { get; set; }

        public string ToPrompt()
        {
            return $"<|im_start|>{Who.ToString().ToLower()}{Content}<|im_end|>";
        }


    }

    public enum EWho
    {
        PlugIn,
        User,
        Assistant
    }
}
