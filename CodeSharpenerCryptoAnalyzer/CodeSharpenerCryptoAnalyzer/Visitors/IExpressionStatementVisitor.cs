using CodeSharpenerCryptoAnalyzer.AnalyzerModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Visitors
{
    public interface IExpressionStatementVisitor
    {
        void VisitExpressionStatement(ExpressionStatementSyntax node);

        StringLiteralExpressionResult GetAssignmentExpressionResult();
    }
}
