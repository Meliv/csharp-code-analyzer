using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeAnalyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UnitTestNameAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MLV002";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.MLV002Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.MLV002MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.MLV002Description), Resources.ResourceManager, typeof(Resources));

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            "Naming",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            MethodDeclarationSyntax methodDeclaration = (MethodDeclarationSyntax)context.Node;

            string methodName = methodDeclaration.Identifier.Text;
            
            if (IsTestMethod(methodDeclaration) && !Regex.IsMatch(methodName, @"^[^_]+_[^_]+_[^_]+$"))
            {
                var diagnostic = Diagnostic.Create(Rule, methodDeclaration.Identifier.GetLocation(), methodName);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private bool IsTestMethod(MethodDeclarationSyntax methodDeclaration)
        {
            string[] testAttributes = new[] { "Fact", "Theory", "TestMethod" };

            return methodDeclaration.AttributeLists
                .SelectMany(attributeList => attributeList.Attributes)
                .Any(attribute => testAttributes.Contains(attribute.Name.ToString()));
        }
    }
}
