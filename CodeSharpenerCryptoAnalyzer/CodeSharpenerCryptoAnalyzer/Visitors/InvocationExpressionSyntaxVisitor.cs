using CodeSharpenerCryptoAnalysis.Common;
using CodeSharpenerCryptoAnalysis.Models;
using CodeSharpenerCryptoAnalzer.Common;
using CryslData;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeSharpenerCryptoAnalysis.Visitors
{   
    public class InvocationExpressionSyntaxWalker : CSharpSyntaxWalker
    {
        private ServiceProvider serviceProvider { get; set; }
        private static CryslJsonModel _cryslSpecificationModel;
        private static SyntaxNodeAnalysisContext _context;
        private Dictionary<string, List<MethodSignatureModel>> methodSignatureDict = new Dictionary<string, List<MethodSignatureModel>>();
        private TextSpan nodeLocation;
        public InvocationExpressionSyntaxWalker(CryslJsonModel cryslModel, SyntaxNodeAnalysisContext analysisContext, TextSpan nodeSPan)
        {
            _cryslSpecificationModel = cryslModel;
            _context = analysisContext;

            var services = new ServiceCollection();
            services.AddTransient<ICommonUtilities, CommonUtilities>();         
            serviceProvider = services.BuildServiceProvider();
            nodeLocation = nodeSPan;
        }        

        /// <summary>
        /// Visit InvocationExpressions for a Given MethodDeclarationSyntax Node.
        /// </summary>
        /// <param name="invocationExpressionNode"></param>
        /// <returns></returns>
        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (!node.Span.Equals(nodeLocation))
            {
                var identifierNode = node.ChildNodes().OfType<IdentifierNameSyntax>();
                foreach (var identifier in identifierNode)
                {
                    var result = _cryslSpecificationModel.Event_Section.Methods.Select(x => x.Crypto_Signature
                     .Where(y => y.Method_Name.ToString().Equals(identifier.Identifier.Value.ToString())));
                    foreach (var methods in _cryslSpecificationModel.Event_Section.Methods)
                    {
                        // Check if method signature matches with the method signature defined in events section of the Crysl.
                        var cryptoMethods = methods.Crypto_Signature.Select(y => y).Where(x => x.Method_Name.ToString().Equals(identifier.Identifier.Value.ToString()));
                        if (cryptoMethods.Count() > 0)
                        {
                            if (_context.SemanticModel.GetSymbolInfo(identifier).Symbol.Kind.Equals(SymbolKind.Method))
                            {
                                var identifierSymbolInfo = (IMethodSymbol)_context.SemanticModel.GetSymbolInfo(identifier).Symbol;
                                // Check for valid event only if Identifier is of Spec type in Crysl.
                                if (identifierSymbolInfo.ReturnType.ToString().Equals(_cryslSpecificationModel.Spec_Section.Class_Name))
                                {
                                    List<MethodSignatureModel> methodSignatureModelsList = new List<MethodSignatureModel>();
                                    foreach (var method in cryptoMethods)
                                    {
                                        ICommonUtilities commonUtilities = serviceProvider.GetService<ICommonUtilities>();
                                        bool isValidEvent = commonUtilities.IsMethodInEvents(method, identifierSymbolInfo, _cryslSpecificationModel.Object_Section.Objects_Declaration);

                                        if (isValidEvent)
                                        {
                                            MethodSignatureModel methodSignatureModel = new MethodSignatureModel
                                            {
                                                MethodName = identifierSymbolInfo.Name,
                                                Parameters = method.Argument_types
                                            };
                                            methodSignatureModelsList.Add(methodSignatureModel);

                                            if (!methodSignatureDict.ContainsKey(method.Event_Var_Name))
                                            {
                                                methodSignatureDict.Add(method.Event_Var_Name, methodSignatureModelsList);
                                            }
                                            else
                                            {
                                                methodSignatureDict[method.Event_Var_Name] = methodSignatureModelsList;
                                            }
                                            /*methodSignatureList.Add(method.Event_Var_Name, new MethodSignatureModel
                                            {
                                                MethodName = identifierSymbolInfo.Name,
                                                Parameters = method.Argument_types                                            
                                            });*/
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Get the Method List
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<MethodSignatureModel>> GetMethodsList()
        {
            return methodSignatureDict;
        }
    }
}
