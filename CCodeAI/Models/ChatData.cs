namespace CCodeAI.Models;

public class ChatData
{
    public EWho Who { get; set; }

    public string Content { get; set; }

    public int Tokens { get; set; }

    public string ToPrompt()
    {
        return $"<|im_start|>{Who.ToString().ToLower()}{Content}<|im_end|>";
    }

    public override string ToString() => $"{Who}:{Content}";

    public virtual string History { get
        {
            return Who switch
            {
                EWho.User => $"Me: {Content}",
                _ => $"AI: {Content}"
            };
        } 
    }
}
