using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace CCodeAI.QuickActions;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class CodeOptimizationAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "CodeOptimization";

    private static readonly LocalizableString Title = new LocalizableResourceString(
        nameof(Resources.CodeOptimizationAnalyzerTitle),
        Resources.ResourceManager, 
        typeof(Resources));

    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(
        nameof(Resources.CodeOptimizationAnalyzerMessageFormat), 
        Resources.ResourceManager, 
        typeof(Resources));

    private static readonly LocalizableString Description = new LocalizableResourceString(
        nameof(Resources.CodeOptimizationAnalyzerDescription), 
        Resources.ResourceManager, 
        typeof(Resources));

    private const string Category = "CodeOptimization";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId, 
        Title, 
        MessageFormat, 
        Category, 
        DiagnosticSeverity.Info, 
        isEnabledByDefault: true, 
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics 
    { 
        get { return ImmutableArray.Create(Rule); } 
    }

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
        context.RegisterCodeBlockAction(AnalyzeCodeBlock);

        //context.RegisterOperationBlockAction(AnalyzeOperationBlock);
    }

    private void AnalyzeOperationBlock(OperationBlockAnalysisContext context)
    {
        var diagnostic = Diagnostic.Create(
            Rule,
            context.OperationBlocks.First().Syntax.GetLocation(),
            Resources.CodeOptimizationAnalyzerTitle);
    }

    private void AnalyzeCodeBlock(CodeBlockAnalysisContext context)
    {
        var diagnostic = Diagnostic.Create(
            Rule, 
            context.CodeBlock.GetLocation(), 
            Resources.CodeOptimizationAnalyzerTitle);

        context.ReportDiagnostic(diagnostic);
    }
}
