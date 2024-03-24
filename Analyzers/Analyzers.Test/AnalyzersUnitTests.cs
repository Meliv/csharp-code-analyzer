﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Analyzers.Test.CSharpCodeFixVerifier<
    Analyzers.TestOnlyAnalyzer,
    Analyzers.AnalyzersCodeFixProvider>;

namespace Analyzers.Test
{
    [TestClass]
    public class TestOnlyAnalyzerTests
    {
        [TestMethod]
        public async Task InjectedReferenceToProhibitedInterface_DiagnosticRaised()
        {
            string testCode = @"
            using System;
            
            namespace TestNamespace
            {
                public class Test
                {
                    private readonly IInterface _testInterface;

                    public Test(IInterface testInterface) => _testInterface = testInterface;
                }

                [TestOnly]
                public interface IInterface { }

                [AttributeUsage(AttributeTargets.Interface)]
                public class TestOnlyAttribute : Attribute { }
            }";

            DiagnosticResult expectedDiagnostic = new DiagnosticResult("MLV001", DiagnosticSeverity.Error)
                .WithSpan(10, 33, 10, 57)
                .WithArguments("IInterface");

            await VerifyCS.VerifyAnalyzerAsync(testCode, expectedDiagnostic);
        }

        [TestMethod]
        public async Task ClassWithConstructorButNoDependencies_NoDiagnostic()
        {
            string testCode = @"
            using System;
            namespace TestNamespace
            {
                public class Test
                {
                    public Test() { }
                }
            }";

            await VerifyCS.VerifyAnalyzerAsync(testCode);
        }

        [TestMethod]
        public async Task ClassWithConstructorAndDependencies_NoDiagnostic()
        {
            string testCode = @"
            using System;
            namespace TestNamespace
            {
                public class Test
                {
                    public Test(Dependency dep) { }
                }

                public class Dependency
                {
                }
            }";

            await VerifyCS.VerifyAnalyzerAsync(testCode);
        }

        [TestMethod]
        public async Task EmptyCodeBlock_NoDiagnostic()
        {
            string testCode = @"";

            await VerifyCS.VerifyAnalyzerAsync(testCode);
        }
    }
}
