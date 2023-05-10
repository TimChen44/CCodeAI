using System.IO;
using System.Text.Json;
using static Microsoft.SemanticKernel.SemanticFunctions.PromptTemplateConfig;

namespace CCodeAI.Extensions;

internal static class LocalSemanticFunctionModelExtension
{
    public static CompletionConfig GetCompletionConfig(
        this LocalSemanticFunctionModel model)
    {
        var configPathName = Path.Combine(Path.GetDirectoryName(model.PathName), "config.json");

        if (!File.Exists(configPathName))
        {
            return null;
        }

        var text = File.ReadAllText(configPathName);

        return JsonSerializer.Deserialize<CompletionConfig>(text);
    }
}
