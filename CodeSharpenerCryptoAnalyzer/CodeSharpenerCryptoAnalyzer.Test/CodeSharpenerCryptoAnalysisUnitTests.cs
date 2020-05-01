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
        /// Test Method For AESEncryption Without Arguments
        /// </summary>
        [TestMethod]
        public void AESEncryptionWithoutParameters()
        {
            var path = "..//..//..//Targets//AESEncryption.cs";
            var test = System.IO.File.ReadAllText(path); 

            VerifyCSharpDiagnostic(test);

            Console.WriteLine("Test");       
        }

        /*protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new CodeSharpenerCryptoAnalyzerCodeFixProvider();
        }*/

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CodeSharpenerCryptoAnalyzerAnalyzer();
        }
        
    }
}
