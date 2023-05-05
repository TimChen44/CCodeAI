namespace CCodeAI;

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
