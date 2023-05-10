using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.VisualStudio.OLE.Interop;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCodeAI.Models;

public static class ChatListExtension
{
    public static ChatHistory GetChatHistory(
        this IEnumerable<ChatData> chatList,
        string prompt)
    {
        var chatHistory = new ChatHistory();
        chatHistory.AddMessage(ChatHistory.AuthorRoles.System, prompt);
        

        foreach (ChatData chat in chatList.Reverse().Take(7).Reverse()) //TODO:先控制在三次对话中，后续应该根据token控制会话长度
        {
            if (chat.Who == EWho.Welcome)
            {
                continue;
            }

            if (chat.Who == EWho.User)
            {
                chatHistory.AddMessage(ChatHistory.AuthorRoles.User, chat.Content);
            }
            else if(chat.Who == EWho.Assistant)
            {
                chatHistory.AddMessage(ChatHistory.AuthorRoles.Assistant, chat.Content);
            }
        }

        return chatHistory;
    }
}