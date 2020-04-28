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
                foreach (var method in cryptoMethods)
                {
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
                        //Go to the Containing Method Declaration Node
                        var containingMethodDeclarationNode = identifier.FirstAncestorOrSelf<MethodDeclarationSyntax>();
                        var invExprSyntaxWalker = new InvocationExpressionSyntaxWalker(cryslData, context, nodeSPan);
                        //Walk through the current method to find all invocations of the given type
                        invExprSyntaxWalker.Visit(containingMethodDeclarationNode);
                        Dictionary<string, List<MethodSignatureModel>> validEventsDict = invExprSyntaxWalker.GetMethodsList();
                        if (!validEventsDict.ContainsKey(method.Event_Var_Name))
                        {
                            validEventsDict.Add(method.Event_Var_Name, methodSignatureModelsList);
                        }
                        //If there are two events of same type
                        else if(validEventsDict.ContainsKey(method.Event_Var_Name))
                        {
                            foreach(var methodSig in validEventsDict.Values)
                            {
                                methodSignatureModelsList.AddRange(methodSig);

                            }
                            validEventsDict[method.Event_Var_Name] = methodSignatureModelsList;
                        }
                        //Check if the Aggregator Condition Satisfies
                        if (methods.Aggregator != null)
                        {
                            bool isAggregatorCondition = commonUtilities.CheckAggregator(validEventsDict, methods.Aggregator.Aggregators);
                            if (isAggregatorCondition)
                            {
                                validEvents.IsValidEvent = true;
                                validEvents.ValidEventsDict = validEventsDict;
                                validEvents.AggregatorName = methods.Aggregator.Aggregator_Name;
                            }
                            else
                            {
                                validEvents.IsValidEvent = false;
                            }
                        }
                        else
                        {
                            validEvents.IsValidEvent = true;
                            validEvents.ValidEventsDict = validEventsDict;
                            validEvents.AggregatorName = method.Event_Var_Name;
                            
                        }

                        return validEvents;
                    }
                }
            }
            validEvents.IsValidEvent = false;
            return validEvents;
        }

        public ValidEvents AnalyzeAssignmentExprSyntax(IEnumerable<CryptoSignature> cryptoMethods, CryslJsonModel cryslModel, SyntaxNodeAnalysisContext context, ISymbol leftExprSymbol)
        {
            ValidEvents validEvents = new ValidEvents();            
            MethodSignatureModel methodSignatureModel = new MethodSignatureModel
            {
                MethodName = leftExprSymbol.Name.ToString()
            };
            List<MethodSignatureModel> methodSignatureModelsList = new List<MethodSignatureModel>();
            methodSignatureModelsList.Add(methodSignatureModel);

            //Iterate through all the satisfied crypto signatures
            foreach (var cryptoMethod in cryptoMethods)
            {
                Dictionary<string, List<MethodSignatureModel>> validEventsDict = new Dictionary<string, List<MethodSignatureModel>>();
                validEventsDict.Add(cryptoMethod.Event_Var_Name, methodSignatureModelsList);
                validEvents.IsValidEvent = true;
                validEvents.AggregatorName = cryptoMethod.Event_Var_Name;
                validEvents.ValidEventsDict = validEventsDict;
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
