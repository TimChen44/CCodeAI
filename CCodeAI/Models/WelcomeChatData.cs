namespace CCodeAI;

public class WelcomeChatData:ChatData
{
    public WelcomeChatData()
    {
        Who = EWho.Welcome;
        Content = Resources.Resources.InquiryAI;
    }
}