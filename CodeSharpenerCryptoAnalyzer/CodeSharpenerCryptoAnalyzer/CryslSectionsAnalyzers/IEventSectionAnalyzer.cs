using CodeSharpenerCryptoAnalysis.AnalyzerModels;
using CryslData;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSharpenerCryptoAnalysis.CryslSectionsAnalyzers
{
    public interface IEventSectionAnalyzer
    {
        ValidEvents AnalyzeMemAccessExprSyntax(IdentifierNameSyntax identifier, IEnumerable<CryptoSignature> cryptoMethods, Methods methods, CryslJsonModel cryslData, SyntaxNodeAnalysisContext context, IMethodSymbol identifierSymbolInfo, TextSpan nodeSPan);
        ValidEvents AnalyzeAssignmentExprSyntax(IEnumerable<CryptoSignature> cryptoMethods, CryslJsonModel cryslModel, SyntaxNodeAnalysisContext context, ISymbol leftExprSymbol);
    }
}
