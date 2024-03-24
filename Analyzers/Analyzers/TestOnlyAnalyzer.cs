using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TestOnlyAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MLV001";
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
            if (!(constructorSyntax.Parent is ClassDeclarationSyntax))
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
            // Get the type symbol for the type represented by the TypeSyntax
            ITypeSymbol typeSymbol = context.SemanticModel.GetTypeInfo(typeSyntax).Type;

            if (typeSymbol != null && typeSymbol is INamedTypeSymbol namedTypeSymbol)
            {
                string[] testOnlyAttributeNames = new[] { "TestOnly", "TestOnlyAttribute" };
                // Get custom attributes applied to the type symbol
                return namedTypeSymbol
                    .GetAttributes()
                    .Any(attr => testOnlyAttributeNames.Contains(attr.AttributeClass.Name));
            }

            return false;
        }
    }
}
