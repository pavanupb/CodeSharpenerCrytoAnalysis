using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;
using CodeSharpenerCryptoAnalysis;
using System.IO;
using CodeSharpenerCryptoAnalyzer;

namespace CodeSharpenerCryptoAnalysis.Test
{
    [TestClass]
    public class UnitTest : CodeFixVerifier
    {
        /// <summary>
        /// Test Method For String Concat Test
        /// </summary>
        [TestMethod]
        public void StringConcatTest()
        {
            var path = "..//..//..//Targets//StringTests//StringConcatTest.cs";
            var test = System.IO.File.ReadAllText(path);

            var expectedTaint1 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 13, 13)
                                    }
            };

            var expectedTaint2 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 15, 17)
                                    }
            };

            VerifyCSharpDiagnostic(test, expectedTaint1, expectedTaint2);  
        }

        /// <summary>
        /// Test Method For String Concat Test
        /// </summary>
        [TestMethod]
        public void StringConcatByPassTest()
        {
            var path = "..//..//..//Targets//StringTests//StringConcatByPassTest.cs";
            var test = System.IO.File.ReadAllText(path);

            var expectedTaint1 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 13, 13)
                                    }
            };

            var expectedTaint2 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 14, 40)
                                    }
            };

            var expectedTaint3 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 15, 35)
                                    }
            };

            VerifyCSharpDiagnostic(test, expectedTaint1, expectedTaint2, expectedTaint3);
        }

        /// <summary>
        /// Test Method For String Concat Test
        /// </summary>
        [TestMethod]
        public void StringToCharArrayTest()
        {
            var path = "..//..//..//Targets//StringTests//StringToCharArrayTest.cs";
            var test = System.IO.File.ReadAllText(path);

            var expectedTaint1 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 13, 13)
                                    }
            };

            var expectedTaint2 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 14, 13)
                                    }
            };

            VerifyCSharpDiagnostic(test, expectedTaint1, expectedTaint2);
        }

        /// <summary>
        /// Test Method For String Concat Test
        /// </summary>
        [TestMethod]
        public void StringBuilderTest()
        {
            var path = "..//..//..//Targets//StringTests//StringBuilderTest.cs";
            var test = System.IO.File.ReadAllText(path);

            var expectedTaint1 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 13, 13)
                                    }
            };

            var expectedTaint2 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 14, 13)
                                    }
            };

            var expectedTaint3 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 15, 24)
                                    }
            };

            VerifyCSharpDiagnostic(test, expectedTaint1, expectedTaint2, expectedTaint3);
        }

        /// <summary>
        /// Test Method For String Concat Test
        /// </summary>
        [TestMethod]
        public void StringBuilderParameterTest()
        {
            var path = "..//..//..//Targets//StringTests//StringBuilderParameterTest.cs";
            var test = System.IO.File.ReadAllText(path);

            var expectedTaint1 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 13, 13)
                                    }
            };

            var expectedTaint2 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 14, 29)
                                    }
            };
            

            VerifyCSharpDiagnostic(test, expectedTaint1, expectedTaint2);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CodeSharpenerCryptoAnalyzerAnalyzer();
        }
        
    }
}
