using Microsoft.SemanticKernel;
using System.Threading;
using System.Threading.Tasks;

internal class KernelFactory
{
    public static IKernel SKernel { get; private set; }

    public static void Init()
    {
        if (SKernel != null)
        {
            return;
        }

        SKernel = Kernel.Builder.Configure(c =>
        {
            //c.AddOpenAITextCompletionService(
            //    "ccode",
            //    OpenAIConfig.Model,
            //    OpenAIConfig.OpenAIKey
            //    );
            c.AddAzureTextCompletionService
            (
                "ccode",
                "text-davinci-003",
                AzureConfig.Endpoint,
                AzureConfig.AppKey
            );

        })
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
