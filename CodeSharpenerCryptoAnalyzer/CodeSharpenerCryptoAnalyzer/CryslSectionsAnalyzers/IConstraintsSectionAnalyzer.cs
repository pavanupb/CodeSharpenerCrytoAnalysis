using CodeSharpenerCryptoAnalysis.AnalyzerModels;
using CryslData;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace CodeSharpenerCryptoAnalysis.CryslSectionsAnalyzers
{
    public interface IConstraintsSectionAnalyzer
    {
        List<ConstraintsModel> AnalyzeParameters(IEnumerable<ArgumentListSyntax> actualParameters, ICollection<ArgumentTypes> cryslParameters, ICollection<Constraints> constraintsList, SyntaxNodeAnalysisContext context);
        bool IsAdditionalConstraintSatisfied(AddConstraints additionalConstraints, ISymbol leftExprSymbolInfo, string rightExprValue, ICollection<ObjectsDeclaration> objectsDeclarations, ValidEvents validEvents);
        bool IsPropertyConstraintSatisfied(CryslJsonModel cryslModel, ValidEvents validEvents, string rightExprValue);
    }
}
