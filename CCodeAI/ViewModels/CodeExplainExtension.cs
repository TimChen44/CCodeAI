using Microsoft.SemanticKernel;
using System.Threading.Tasks;

namespace CCodeAI.ViewModels
{
    public static class CodeExplainExtension
    {
        public static async Task<string> CodeSkillAsync(
            this CCodeExplainWindowControlViewModel vm,
            string code,
            string extension,
            string semanticFuncation)
        {
            vm.AiLoading();
            vm.ChatDatas.Add(new ChatData()
            {
                Content = "询问AI中，请稍后...",
                Who = EWho.PlugIn
            });

            var explainFunc = vm.SKernel.CreateSemanticFunction(semanticFuncation);

            var context = vm.SKernel.CreateNewContext();
            context.Variables["extension"] = extension;

            var result = await explainFunc.InvokeAsync(code, context);

            if (result.ErrorOccurred)
            {
                await VS.MessageBox.ShowErrorAsync(result.LastErrorDescription);
                vm.AiLoaded();
                return null;
            }

            var content = result.ToString().Trim();

            vm.ChatDatas.Add(new ChatData()
            {
                Content = content,
                Who = EWho.Assistant
            });
            vm.AiLoaded();
            return content;
        }
    }
}
