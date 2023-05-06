namespace CCodeAI.Models;

public class WelcomeChatData:ChatData
{
    public WelcomeChatData()
    {
        Who = EWho.Welcome;
        Content = Resources.Resources.WhenDoubtAI;
    }
}
