using CodeSharpenerCryptoAnalysis.AnalyzerModels;
using CryslData;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;


namespace CodeSharpenerCryptoAnalysis.CryslSectionsAnalyzers
{
    public class ConstraintsSectionAnalyzer : IConstraintsSectionAnalyzer
    {
        /// <summary>
        /// Check if Parameter Constraints are Satisfied
        /// </summary>
        /// <param name="argumentListSyntax"></param>
        /// <param name="cryslParameters"></param>
        /// <param name="constraintsList"></param>
        /// <returns></returns>
        public List<ConstraintsModel> AnalyzeParameters(IEnumerable<ArgumentListSyntax> argumentListSyntax, ICollection<ArgumentTypes> cryslParameters, ICollection<Constraints> constraintsList)
        {
            List<ConstraintsModel> constraintsModel = new List<ConstraintsModel>();
            foreach(var argument in argumentListSyntax)
            {
                var literalExpressionSyntax = argument.ChildNodes().OfType<ArgumentSyntax>().FirstOrDefault().ChildNodes().OfType<LiteralExpressionSyntax>();
                foreach(var parameter in cryslParameters)
                {
                    var parameterConstraints = constraintsList.Select(x => x).Where(y => y.Object_Varname.ToString().Equals(parameter.Argument));
                    constraintsModel.Add(IsValidLiteralExpression(literalExpressionSyntax, parameterConstraints));
                }                
            }

            return constraintsModel ;

        }

        /// <summary>
        /// Check if the Additional Constraints are Satisfied
        /// </summary>
        /// <param name="additionalConstraints"></param>
        /// <param name="localInvocatorSymbolInfo"></param>
        /// <param name="leftExprSymbolInfo"></param>
        /// <param name="rightExprSymbolInfo"></param>
        /// <param name="objectsDeclarations"></param>
        /// <returns></returns>
        public bool IsAdditionalConstraintSatisfied(AddConstraints additionalConstraints, ISymbol leftExprSymbolInfo, string rightExprValue, ICollection<ObjectsDeclaration> objectsDeclarations, ValidEvents validEvents)
        {
            bool isAdditionalConstraintSatisfied = true;
            foreach (var constraints in additionalConstraints.ConstraintsModels)
            {
                //Iterate through each additional constraints
                if (constraints.AdditionalConstraints != null)
                {
                    foreach (var constraint in constraints.AdditionalConstraints)
                    {
                        if (leftExprSymbolInfo.Kind.Equals(SymbolKind.Property))
                        {
                            IPropertySymbol leftExprPropertyInfo = (IPropertySymbol)leftExprSymbolInfo;
                            var objectTypes = objectsDeclarations.Select(x => x).Where(y => y.Object_type.ToString().Equals(leftExprPropertyInfo.Type.ToString()) && y.Var_name.ToString().Equals(validEvents.PropertyName));
                            // Iterate through all the matched object declarations
                            foreach (var objectType in objectTypes)
                            {
                                if (constraint.Object_Varname_Additional_Constraint.ToString().Equals(objectType.Var_name.ToString()))
                                {
                                    bool isAddConstraintSatisfied = false;
                                    foreach (var addConstraint in constraint.Additional_Constraints_List)
                                    {
                                        //The Right Expression Node Value Should be according to the value in the Constraint
                                        if (addConstraint.ToString().Equals(rightExprValue))
                                        {
                                            isAddConstraintSatisfied = true;
                                        }

                                    }
                                    if (!isAddConstraintSatisfied)
                                    {
                                        isAdditionalConstraintSatisfied = false;
                                        return isAdditionalConstraintSatisfied;
                                    }

                                }

                            }
                        }

                    }
                }

            }
            return isAdditionalConstraintSatisfied;

        }

        /// <summary>
        /// Check if Property Constraints are Satified
        /// </summary>
        /// <param name="cryslModel"></param>
        /// <param name="validEvents"></param>
        /// <param name="rightExprValue"></param>
        /// <returns></returns>
        public bool IsPropertyConstraintSatisfied(CryslJsonModel cryslModel, ValidEvents validEvents, string rightExprValue)
        {
            var constraintsList = cryslModel.Constraints_Section.Constraints.Select(x => x).Where(y => y.Object_Varname.ToString().Equals(validEvents.PropertyName)).Select(x => x.Constraints_List);
            if (constraintsList.Count() > 0)
            {
                bool isPrimaryConstraintSatisfied = false;
                foreach (var constraints in constraintsList)
                {
                    foreach (var constraint in constraints)
                    {
                        if (constraint.ToString().Equals(rightExprValue.ToString()))
                        {
                            isPrimaryConstraintSatisfied = true;
                        }
                    }
                    if (!isPrimaryConstraintSatisfied)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private ConstraintsModel IsValidLiteralExpression(IEnumerable<LiteralExpressionSyntax> literalExpressionSyntax, IEnumerable<Constraints> parameterConstraints)
        {
            ConstraintsModel constraintsModel = new ConstraintsModel();
            List<AdditionalConstraints> additionalConstraintsList = new List<AdditionalConstraints>();
            foreach(var literalExpression in literalExpressionSyntax)
            {
                var primaryConstraintSatisfied = parameterConstraints.Select(x => x).Where(y => (y.Additional_constraints == null) && y.Constraints_List.Contains(literalExpression.Token.Value.ToString()));
                if (primaryConstraintSatisfied.Count() != 0)
                {
                    constraintsModel.IsConstraintSatisfied = true;
                    constraintsModel.SatisfiedConstraint = literalExpression.Token.Value.ToString();
                    var literalConstraintsList = parameterConstraints.Select(x => x).Where(y => (y.Constraints_List.Contains(literalExpression.Token.Value.ToString())) && y.Additional_constraints != null);
                    if(literalConstraintsList.Count() != 0)
                    {
                        additionalConstraintsList = literalConstraintsList.Select(x => x.Additional_constraints).ToList();
                        constraintsModel.IsAdditionalConstraints = true;
                        constraintsModel.AdditionalConstraints = additionalConstraintsList;
                    }
                }
                else
                {
                    constraintsModel.IsConstraintSatisfied = false;
                    constraintsModel.NotSatisfiedParameter = literalExpression.Token.Value.ToString();
                    return constraintsModel;
                }                
            }
            return constraintsModel;
        }        
    }
}
