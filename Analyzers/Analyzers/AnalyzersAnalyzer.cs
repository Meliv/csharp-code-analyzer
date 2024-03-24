using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AnalyzersAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MST001";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            "Dependency Injection",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ConstructorDeclaration);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var constructorSyntax = (ConstructorDeclarationSyntax)context.Node;

            // Check if the constructor is part of a class declaration
            var classSyntax = constructorSyntax.Parent as ClassDeclarationSyntax;
            if (classSyntax == null)
                return;

            // Check if the constructor has parameters
            if (!constructorSyntax.ParameterList.Parameters.Any())
                return;

            // Check each constructor parameter
            foreach (var parameterSyntax in constructorSyntax.ParameterList.Parameters)
            {
                // Check if the parameter has a parent that is a ParameterListSyntax (this indicates a constructor parameter)
                if (parameterSyntax.Parent is ParameterListSyntax)
                {
                    if (HasForbiddenAttribute(context, parameterSyntax.Type))
                    {
                        // Report diagnostic
                        var diagnostic = Diagnostic.Create(
                            Rule,
                            parameterSyntax.GetLocation(),
                            parameterSyntax.Type.TryGetInferredMemberName());
                        
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }

        private bool HasForbiddenAttribute(SyntaxNodeAnalysisContext context, TypeSyntax typeSyntax)
        {
            var semanticModel = context.SemanticModel;

            // Get the type symbol for the type represented by the TypeSyntax
            var typeSymbol = semanticModel.GetTypeInfo(typeSyntax).Type;

            if (typeSymbol != null && typeSymbol is INamedTypeSymbol namedTypeSymbol)
            {
                // Get custom attributes applied to the type symbol
                var attributes = namedTypeSymbol.GetAttributes();

                // Check if the type has specific custom attributes
                if (attributes.Any(attr => attr.AttributeClass.Name == "TestOnly" || attr.AttributeClass.Name == "TestOnlyAttribute"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
