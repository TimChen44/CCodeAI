using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.SemanticKernel;

namespace CCodeAI.ViewModels;

internal partial class CodeGenWindowViewModel:ObservableObject
{
	public CodeGenWindowViewModel()
	{
		KernelFactory.Init();
	}

    public IKernel SKernel => KernelFactory.SKernel;
}
