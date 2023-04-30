internal static class CodeSemanticFunctions
{
    public const string CodeExplain = """
            ```{{$extension}}
            {{$input}}
            ```
            解释代码：
            """;

    public const string CodeNote = """
            ```{{$extension}}
            {{$input}}
            ```
            给每一行代码添加注释：
            """;

    public const string CodeOptimize = """
            ```{{$extension}}
            {{$input}}
            ```
            优化代码：
            """;

    public const string ContinuationCode = """
            ```{{$extension}}
            {{$input}}
            ```
            根据注释续写代码，只需要返回新增的代码，不要返回其他内容：
            """;

    public const string Chat = """
            以下是与人工智能编码助理的对话。这位助理乐于助人，富有创造力，聪明，而且非常友好。

            问：我有一个问题。你能帮忙吗？
            答：当然，我是你的AI编码助理，请继续！
            问：{{$input}}
            答：
            """;
}
