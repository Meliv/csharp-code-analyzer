using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace CodeAnalyzer.Analyzers.Test.UnitTestNameAnalyzerTests
{
    public class MSTestTests
    {
        [Theory]
        [InlineData("InvalidMethodName")]
        [InlineData("_InvalidMethodName")]
        [InlineData("_InvalidMethodName_")]
        [InlineData("InvalidMethodName_")]
        [InlineData("Invalid_MethodName")]
        [InlineData("Invalid__MethodName")]
        [InlineData("Invalid___MethodName")]
        [InlineData("In_va_li_dM_et_ho_dN_ame")]
        public async Task MSTest_InvalidNames_DiagnosticRaised(string methodName)
        {
            string testCode = @$"
            using Microsoft.VisualStudio.TestTools.UnitTesting;

            namespace TestNamespace
            {{
                [TestClass]
                public class Test
                {{
                    [TestMethod]
                    public void {methodName}()
                    {{
                    }}
                }}
            }}";

            ImmutableArray<Diagnostic> diagnostics = await GetDiagnostics(testCode);

            Assert.Contains(diagnostics, diagnostic =>
                diagnostic.Id == "MLV002" &&
                diagnostic.GetMessage() == $"Test '{methodName}' has invalid naming convention. See description" &&
                diagnostic.Severity == DiagnosticSeverity.Warning);
        }

        [Fact]
        public async Task MSTest_ValidName_NoDiagnosticRaised()
        {
            string testCode = @$"
            using Microsoft.VisualStudio.TestTools.UnitTesting;

            namespace TestNamespace
            {{
                [TestClass]
                public class Test
                {{
                    [TestMethod]
                    public void Valid_Method_Name()
                    {{
                    }}
                }}
            }}";

            ImmutableArray<Diagnostic> diagnostics = await GetDiagnostics(testCode);

            Assert.Empty(diagnostics);
        }

        [Fact]
        public async Task MSTest_EmptyCodeBlock_NoDiagnostic()
        {
            string testCode = @"";

            ImmutableArray<Diagnostic> diagnostics = await GetDiagnostics(testCode);

            Assert.Empty(diagnostics);
        }

        private Task<ImmutableArray<Diagnostic>> GetDiagnostics(string testCode)
        {
            // As the code above is compiled from a string in sandbox and uses Xunit,
            // we need to instruct the code analysis assembly to manually
            // load the Xunit assembly or else it'll throw compilation errors
            CSharpCompilation compilation = CSharpCompilation.Create(default,
                new[] { CSharpSyntaxTree.ParseText(testCode) },
                new[]
                {
                MetadataReference.CreateFromFile(Assembly.Load("System.Private.CoreLib").Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(Assembly.Load("Microsoft.VisualStudio.TestPlatform.TestFramework").Location)
                },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            return compilation
                .WithAnalyzers(ImmutableArray.Create<DiagnosticAnalyzer>(new UnitTestNameAnalyzer()))
                .GetAllDiagnosticsAsync();
        }

    }
}
