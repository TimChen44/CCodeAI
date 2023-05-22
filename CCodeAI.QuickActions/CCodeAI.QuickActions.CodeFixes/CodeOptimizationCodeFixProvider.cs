using CCodeAI.QuickActions.Skills;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Immutable;
using System.Composition;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CCodeAI.QuickActions;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CodeOptimizationCodeFixProvider)), Shared]
public class CodeOptimizationCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create<string>(CodeOptimizationAnalyzer.DiagnosticId); }
    }

    public sealed override FixAllProvider GetFixAllProvider()
    {
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
        return WellKnownFixAllProviders.BatchFixer;
    }

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        if(context.Span.Length > 0)
        {
            var action = CodeAction.Create(
                title: CodeFixResources.OptimizeCode,
                createChangedDocument: c => OptimizeCode(context.Document, diagnosticSpan, c),
                equivalenceKey: nameof(CodeFixResources.OptimizeCode)
            );

            context.RegisterCodeFix(
                action,
                diagnostic);
        }
        else
        {
            //Optimize Method or class
        }
    }

    private async Task<Document> OptimizeCode(
        Document document, 
        TextSpan diagnosticSpan, 
        CancellationToken cancellationToken)
    {
        var sourceText = await document.GetTextAsync();
        var selectedText = sourceText.GetSubText(diagnosticSpan);

        var resultCode = await KernelFactory.InvokeCodeFunctionAsync(
            CodeOptimizationSemanticFunction.CodeOptimize,
            selectedText.ToString(),
            cancellationToken);

        return document.WithText(sourceText.Replace(diagnosticSpan, resultCode));
    }
}
