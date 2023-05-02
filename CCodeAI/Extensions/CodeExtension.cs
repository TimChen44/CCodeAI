namespace CCodeAI.Extensions;

public static class CodeExtension
{
    public static string GetCodeType(string extension)
    {
        return extension switch
        {
            ".cs" => "C#",
            ".js" => "JavaScript",
            ".ts" => "TypeScript",
            ".cpp" => "C++",
            ".vb" => "vb",
            _ => ""
        };
    }
}
