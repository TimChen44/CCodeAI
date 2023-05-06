using Microsoft.VisualStudio.OLE.Interop;
using System.Collections.Generic;
using System.Text;

namespace CCodeAI.Models;

public static class ChatListExtension
{
    public static string GetChatContext(this IEnumerable<ChatData> chatList)
    {
        var strBuilder = new StringBuilder();

        foreach (ChatData chat in chatList) 
        {
            if (chat.Who == EWho.Welcome)
                continue;

            strBuilder.Append(chat);
            strBuilder.Append('\n');
        }

        return strBuilder.ToString();
    }
}