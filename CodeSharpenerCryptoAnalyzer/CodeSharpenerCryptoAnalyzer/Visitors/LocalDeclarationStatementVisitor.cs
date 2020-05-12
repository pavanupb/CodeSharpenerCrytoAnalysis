using CodeSharpenerCryptoAnalyzer.AnalyzerModels;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Visitors
{
    public class LocalDeclarationStatementVisitor : CSharpSyntaxWalker
    {
        private bool IsArrayInitializerPresent;
        private bool IsStringLiteralExpressionPresent;
        private VariableDeclaratorSyntax variableDeclarator;

        public LocalDeclarationStatementVisitor()
        {
            IsArrayInitializerPresent = false;
            IsStringLiteralExpressionPresent = false;
        }

        public override void VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            variableDeclarator = node;
            base.VisitVariableDeclarator(node);
        }          

        public override void VisitInitializerExpression(InitializerExpressionSyntax node)
        {
            if(node.Kind().Equals(SyntaxKind.ArrayInitializerExpression))
            {
                IsArrayInitializerPresent = true;
            }
            
        }        

        public override void VisitEqualsValueClause(EqualsValueClauseSyntax node)
        {
            var stringHardCodedValue = node.ChildNodes().OfType<LiteralExpressionSyntax>();
            if(stringHardCodedValue.Count() != 0)
            {
                if (stringHardCodedValue.First().Kind().Equals(SyntaxKind.StringLiteralExpression))
                {
                    IsStringLiteralExpressionPresent = true;
                }
            }
        }

        public ByteArrayDeclarationResult GetByteArrayResult()
        {
            ByteArrayDeclarationResult byteArrayDeclarationResult = new ByteArrayDeclarationResult
            {
                DeclaratorSyntax = variableDeclarator,
                IsArrayInitializer = IsArrayInitializerPresent

            };
            return byteArrayDeclarationResult;
        }

        public StringLiteralDeclarationResult GetStringLiteralResult()
        {
            StringLiteralDeclarationResult stringLiteralDeclarationResult = new StringLiteralDeclarationResult
            {
                DeclaratorSyntax = variableDeclarator,
                IsStringLiteralInitializer = IsStringLiteralExpressionPresent
            };
            return stringLiteralDeclarationResult;
        }
    }
}
