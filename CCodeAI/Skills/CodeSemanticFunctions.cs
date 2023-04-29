internal static class CodeSemanticFunctions
{
    public const string CodeExplain =
        """
            ```{{$extension}}
            {{$input}}
            ```
            Explain Code
            """;

    public const string CodeNote =
        """
            ```{{$extension}}
            {{$input}}
            ```
            Add comments to the code
            """;

    public const string CodeOptimize =
        """
            ```{{$extension}}
            {{$input}}
            ```
            Optimize code
            """;

    public const string ContinuationCode =
        """
            ```{{$extension}}
            {{$input}}
            ```
            Continue writing the code
            """;

    public const string Chat =
        """
            The following is a conversation with an AI Coding assistant. The assistant is helpful, creative, clever, and very friendly.

            Ask:I have a question. Can you help? 
            Answer:Of course. I am your AI Copilot. Go on!
            Ask:{{$input}}
            Answer:
            """;
}
