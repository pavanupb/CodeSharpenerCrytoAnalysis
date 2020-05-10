using CodeSharpenerCryptoAnalysis.AnalyzerModels;
using CodeSharpenerCryptoAnalysis.Common;
using CodeSharpenerCryptoAnalysis.Models;
using CodeSharpenerCryptoAnalysis.Visitors;
using CodeSharpenerCryptoAnalzer.Common;
using CryslData;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeSharpenerCryptoAnalysis.CryslSectionsAnalyzers
{
    public class EventsSectionAnalyzer : IEventSectionAnalyzer
    {
        private static ServiceProvider serviceProvider { get; set; }
        public EventsSectionAnalyzer()
        {
            var services = new ServiceCollection();
            services.AddTransient<ICommonUtilities, CommonUtilities>();
            serviceProvider = services.BuildServiceProvider();
        }
        /// <summary>
        /// Analyze thr MemberAcessExpressionSyntax Nodes
        /// </summary>
        /// <param name="memberAccessExpressions"></param>
        /// <param name="cryslData"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public ValidEvents AnalyzeMemAccessExprSyntax(IdentifierNameSyntax identifier, IEnumerable<CryptoSignature> cryptoMethods, Methods methods, CryslJsonModel cryslData, SyntaxNodeAnalysisContext context, IMethodSymbol identifierSymbolInfo, TextSpan nodeSPan, string invocatorType)
        {
            ValidEvents validEvents = new ValidEvents();            
            // Check for valid event only if Identifier is of Spec type in Crysl.            
            if (invocatorType.Equals(cryslData.Spec_Section.Class_Name))
            {
                List<MethodSignatureModel> methodSignatureModelsList = new List<MethodSignatureModel>();
                //Iterate through different signatures of the same event and add the matched signature event
                foreach (var method in cryptoMethods)
                {
                    if(!String.IsNullOrEmpty(method.Object_variable))
                    {
                        var localDeclarationStatement = context.Node.Ancestors().OfType<LocalDeclarationStatementSyntax>();
                        if(localDeclarationStatement.Count() != 0)
                        {
                            var variableDeclarationNode = localDeclarationStatement.FirstOrDefault().ChildNodes().OfType<VariableDeclarationSyntax>().FirstOrDefault();
                            if (variableDeclarationNode.Variables.Count() > 0)
                            {
                                var declarationStmtSymInfo = context.SemanticModel.GetDeclaredSymbol(variableDeclarationNode.Variables.FirstOrDefault());
                                var declaratorType = declarationStmtSymInfo as ILocalSymbol;

                                var declaredObjectType = cryslData.Object_Section.Objects_Declaration.Select(x => x).Where(x => x.Var_name.Equals(method.Object_variable));
                                if(declaredObjectType.Count() != 0 && declaratorType != null)
                                {
                                    if(!declaredObjectType.First().Object_type.Equals(declaratorType.Type.ToString()))
                                    {
                                        validEvents.IsValidEvent = false;
                                        validEvents.IsProperty = false;
                                        return validEvents;
                                    }
                                }
                            }
                        }
                    }
                    ICommonUtilities commonUtilities = serviceProvider.GetService<ICommonUtilities>();
                    //Check if the Event is Valid
                    bool isValidEvent = commonUtilities.IsMethodInEvents(method, identifierSymbolInfo, cryslData.Object_Section.Objects_Declaration);
                    if (isValidEvent)
                    {
                        MethodSignatureModel currentValidEvent = new MethodSignatureModel
                        {
                            MethodName = identifierSymbolInfo.Name,
                            Parameters = method.Argument_types,
                        };
                        methodSignatureModelsList.Add(currentValidEvent);

                        validEvents.IsValidEvent = true;
                        validEvents.IsProperty = false;
                        validEvents.ValidMethods = currentValidEvent;
                        validEvents.PropertyName = method.Event_Var_Name; 

                        return validEvents;
                    }
                }
            }            
            validEvents.IsValidEvent = false;
            validEvents.IsProperty = false;
            return validEvents;
        }

        public ValidEvents AnalyzeAssignmentExprSyntax(IEnumerable<CryptoSignature> cryptoMethods, CryslJsonModel cryslModel, SyntaxNodeAnalysisContext context, ISymbol leftExprSymbol)
        {
            ValidEvents validEvents = new ValidEvents();            
            MethodSignatureModel methodSignatureModel = new MethodSignatureModel
            {
                MethodName = leftExprSymbol.Name.ToString()
            };
            
            //Iterate through all the satisfied crypto signatures
            foreach (var cryptoMethod in cryptoMethods)
            { 
                validEvents.IsValidEvent = true;
                validEvents.AggregatorName = cryptoMethod.Event_Var_Name;
                validEvents.ValidMethods = methodSignatureModel;
                if(cryptoMethod.Is_property)
                {
                    validEvents.IsProperty = true;
                    validEvents.PropertyName = cryptoMethod.Object_variable;
                }
            }

            return validEvents;
        }
    }
}
