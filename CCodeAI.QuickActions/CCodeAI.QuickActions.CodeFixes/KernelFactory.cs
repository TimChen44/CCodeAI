using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Memory;
using System.Threading;
using System.Threading.Tasks;

public class KernelFactory
{
    public static bool UseAzureOpenAI { get; } = true;

    public static IKernel SKernel { get; private set; }

    public static void Init()
    {
        if (SKernel != null)
        {
            return;
        }

        SKernel = Kernel.Builder.Configure(c =>
        {
            if (UseAzureOpenAI)
            {
                c.AddAzureTextCompletionService
                (
                    "ccode",
                    "text-davinci-003",
                    AzureConfig.Endpoint,
                    AzureConfig.AppKey
                );
                c.AddAzureTextEmbeddingGenerationService
                (
                    "ada", 
                    "text-embedding-ada-002",
                    AzureConfig.Endpoint,
                    AzureConfig.AppKey
                );
            }
            else
            {
                //c.AddOpenAITextCompletionService(
                //    "ccode",
                //    OpenAIConfig.Model,
                //    OpenAIConfig.OpenAIKey
                //    );
            }
        })
        .WithMemoryStorage(new VolatileMemoryStore())
        .Build();
    }

    public static async Task<string> InvokeCodeFuncationAsync(
        string semanticFuncation,
        string code,
        CancellationToken? cancellationToken = null,
        string extension = "csharp")
    {
        Init();

        var explainFunc = SKernel.CreateSemanticFunction(semanticFuncation);

        var context = SKernel.CreateNewContext();
        context.Variables["extension"] = extension;

        var result = await explainFunc.InvokeAsync(code, context,cancel:cancellationToken);

        if (result.ErrorOccurred)
        {
            throw result.LastException;
        }

        return result.ToString().Trim();
    }
}
