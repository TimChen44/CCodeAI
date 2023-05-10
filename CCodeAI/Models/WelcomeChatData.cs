using CCodeAI.Extensions;
using CCodeAI.ViewModels;
using CommunityToolkit.Mvvm.Input;
using Microsoft.SemanticKernel.KernelExtensions;
using Microsoft.VisualStudio.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CCodeAI.Models;

public class WelcomeChatData:ChatData
{
    private AsyncRelayCommand<LocalSemanticFunctionModel> _executeCoreSkillCommand;

    public WelcomeChatData()
    {
    }

    public WelcomeChatData(CCodeExplainWindowControlViewModel cCodeExplainWindowControlViewModel)
    {
        Who = EWho.Welcome;
        Content = Resources.Resources.WhenDoubtAI;        

        Parent = cCodeExplainWindowControlViewModel;
        CoreSkill = Parent.SkillsProvider
            ?.GetSkills()
            ?.FirstOrDefault(p => p.Name == "CoreSkill");
        SemanticFunctions = CoreSkill?.SemanticFunctions;
    }

    public List<LocalSemanticFunctionModel> SemanticFunctions { get; set; }

    public CCodeExplainWindowControlViewModel Parent { get; }

    public SkillModel CoreSkill { get; }

    public AsyncRelayCommand<LocalSemanticFunctionModel> ExecuteCoreSkillCommand => _executeCoreSkillCommand ??= new AsyncRelayCommand<LocalSemanticFunctionModel>(ExcuteCodeSkillSKFunctionAsync);

    private async Task ExcuteCodeSkillSKFunctionAsync(
        LocalSemanticFunctionModel localSemanticFunctionModel)
    {
        if (localSemanticFunctionModel == null || CoreSkill == null)
        {
            return;
        }

        try
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            //get document view
            DocumentView docView = await VS.Documents.GetActiveDocumentViewAsync();
            if (docView == null) return;
            if (docView?.TextView == null) return; //not a text window

            //get selection
            var selection = docView?.TextView?.Selection;
            if (selection == null) return;

            SnapshotSpan selectedSpan = selection.StreamSelectionSpan.SnapshotSpan;
            string selectedText = selectedSpan.GetText();

            if (string.IsNullOrWhiteSpace(selectedText))
            {
                await VS.MessageBox.ShowWarningAsync("Please select some code first.");
                return;
            }

            var extension = Path.GetExtension(docView.FilePath);

            var codeType = CodeExtension.GetCodeType(extension);

            var coreSkill = KernelFactory.SKernel.ImportSemanticSkillFromDirectory(Parent.SkillsProvider.SkillsLocation, CoreSkill.Name);

            await Parent.CodeSkillAsync(selectedText, codeType, coreSkill[localSemanticFunctionModel.Name]);
        }
        catch (Exception ex)
        {
            await VS.MessageBox.ShowErrorAsync(ex.Message);
        }        
    }
}
