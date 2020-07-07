using CodeSharpenerCryptoAnalyzer.AnalyzerModels;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Visitors
{
    public interface ILocalDeclarationStatementVisitor
    {
        void Visit(SyntaxNode node);
        IdentifierNameSyntaxResult GetIdentifierNameSyntaxResult();
        StringLiteralDeclarationResult GetStringLiteralResult();
        ByteArrayDeclarationResult GetByteArrayResult();
    }
}
