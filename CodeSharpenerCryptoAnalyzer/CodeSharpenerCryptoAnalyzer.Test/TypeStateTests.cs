using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHelper;

namespace CodeSharpenerCryptoAnalyzer.Test
{
    [TestClass]
    public class UnitTest : DiagnosticVerifier
    {
        /// <summary>
        /// Test Method For String Concat Test
        /// </summary>
        [TestMethod]
        public void AliasViaParameterTest()
        {
            var path = "..//..//..//Targets//AliasViaParameter//AliasViaParametersTest.cs";
            var test = System.IO.File.ReadAllText(path);            

            var expectedTaint1 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 16, 13)
                                    }
            };

            var expectedTaint2 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 24, 13)
                                    }
            };

            VerifyCSharpDiagnostic(test, expectedTaint1, expectedTaint2);
        }

        /// <summary>
        /// Test Method For String Concat Test
        /// </summary>
        [TestMethod]
        public void CallPOITest()
        {
            var path = "..//..//..//Targets//CallPOITests//CallPOITest.cs";
            var test = System.IO.File.ReadAllText(path);        

            VerifyCSharpDiagnostic(test);
        }

        /// <summary>
        /// Test Method For String Concat Test
        /// </summary>
        [TestMethod]
        public void FieldlessTest()
        {
            var path = "..//..//..//Targets//FieldlessTest//FieldlessTest.cs";
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
                                        new DiagnosticResultLocation(path, 15, 13)
                                    }
            };

            var expectedTaint4 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 21, 13)
                                    }
            };

            var expectedTaint5 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 22, 13)
                                    }
            };

            var expectedTaint6 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 23, 13)
                                    }
            };

            var expectedTaint7 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 29, 13)
                                    }
            };

            var expectedTaint8 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 32, 17)
                                    }
            };

            var expectedTaint9 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 40, 13)
                                    }
            };

            var expectedTaint10 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 41, 13)
                                    }
            };

            var expectedTaint11 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 44, 17)
                                    }
            };

            var expectedTaint12 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 46, 13)
                                    }
            };


            VerifyCSharpDiagnostic(test, expectedTaint1, expectedTaint2, expectedTaint3, expectedTaint4, expectedTaint5, expectedTaint6, expectedTaint7, expectedTaint8, expectedTaint9, expectedTaint10, expectedTaint11, expectedTaint12);
        }

        /// <summary>
        /// Test Method For String Concat Test
        /// </summary>
        [TestMethod]
        public void FieldsTest()
        {
            var path = "..//..//..//Targets//FieldsTest//Fields10LongTest.cs";
            var test = System.IO.File.ReadAllText(path);

            VerifyCSharpDiagnostic(test);
        }

        /// <summary>
        /// Test Method For String Concat Test
        /// </summary>
        [TestMethod]
        public void FieldsBranchedTest()
        {
            var path = "..//..//..//Targets//FieldsBranchedTest//FieldsBranchedTest.cs";
            var test = System.IO.File.ReadAllText(path);

            var expectedTaint1 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 16, 17)
                                    }
            };

            var expectedTaint2 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 20, 17)
                                    }
            };

            var expectedTaint3 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 25, 17)
                                    }
            };

            var expectedTaint4 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 29, 17)
                                    }
            };

            var expectedTaint5 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 31, 13)
                                    }
            };

            var expectedTaint6 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation(path, 32, 13)
                                    }
            };            

            VerifyCSharpDiagnostic(test, expectedTaint1, expectedTaint2, expectedTaint3, expectedTaint4, expectedTaint5, expectedTaint6);
        }

        /// <summary>
        /// Test Method For String Concat Test
        /// </summary>
        [TestMethod]
        public void IdentityStringTest()
        {
            var path = "..//..//..//Targets//InterproceduralTest//IdentityStringTest.cs";
            var test = System.IO.File.ReadAllText(path);

            var expectedTaint1 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation("Test0.cs", 13, 13)
                                    }
            };

            var expectedTaint2 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation("Test0.cs", 14, 22)
                                    }
            };            

            VerifyCSharpDiagnostic(test, expectedTaint1, expectedTaint2);
        }

        /// <summary>
        /// Test Method For String Concat Test
        /// </summary>
        [TestMethod]
        public void IdentityStringTest1()
        {
            var path = "..//..//..//Targets//InterproceduralTest//IdentityStringTest1.cs";
            var test = System.IO.File.ReadAllText(path);

            var expectedTaint1 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation("Test0.cs", 13, 13)
                                    }
            };

            var expectedTaint2 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation("Test0.cs", 14, 13)
                                    }
            };

            var expectedTaint3 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation("Test0.cs", 15, 22)
                                    }
            };

            var expectedTaint4 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation("Test0.cs", 16, 23)
                                    }
            };            

            VerifyCSharpDiagnostic(test, expectedTaint1, expectedTaint2, expectedTaint3, expectedTaint4);
        }

        /// <summary>
        /// Test Method For String Concat Test
        /// </summary>
        [TestMethod]
        public void SummaryReuseTest1()
        {
            var path1 = "..//..//..//Targets//InterproceduralTest//SummaryReuseTest1.cs";
            var path2 = "..//..//..//Targets//InterproceduralTest//IdentityFuncTest.cs";

            var test1 = System.IO.File.ReadAllText(path1);
            var test2 = System.IO.File.ReadAllText(path2);

            var expectedTaint1 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation("Test0.cs", 13, 13)
                                    }
            };

            var expectedTaint2 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation("Test0.cs", 14, 13)
                                    }
            };

            var expectedTaint3 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation("Test0.cs", 15, 22)
                                    }
            };

            var expectedTaint4 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation("Test0.cs", 16, 13)
                                    }
            };

            var expectedTaint5 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation("Test0.cs", 17, 13)
                                    }
            };
            

            VerifyCSharpDiagnostic(new[] { test1, test2 }, expectedTaint1, expectedTaint2, expectedTaint3, expectedTaint4, expectedTaint5);
        }

        /// <summary>
        /// Test Method For String Concat Test
        /// </summary>
        [TestMethod]
        public void InterLoopTest()
        {
            var path = "..//..//..//Targets//InterproceduralTest//InterLoopTest.cs";
            var test = System.IO.File.ReadAllText(path);

            var expectedTaint1 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation("Test0.cs", 13, 13)
                                    }
            };

            var expectedTaint2 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation("Test0.cs", 18, 26)
                                    }
            };

            var expectedTaint3 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation("Test0.cs", 19, 17)
                                    }
            };

            var expectedTaint4 = new DiagnosticResult
            {
                Id = "HardCodedKey",
                Message = String.Format("Hard-Coded Key and IV value could lead to Security Vulnerability"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                                new[] {
                                        new DiagnosticResultLocation("Test0.cs", 22, 13)
                                    }
            };            

            VerifyCSharpDiagnostic(test, expectedTaint1, expectedTaint2, expectedTaint3, expectedTaint4);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CodeSharpenerCryptoAnalyzerAnalyzer();
        }

    }
}
