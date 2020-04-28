using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using CodeSharpenerCryptoAnalysis.AnalyzerModels;
using CodeSharpenerCryptoAnalysis.CryslSectionsAnalyzers;
using CodeSharpenerCryptoAnalyzer.CryslBuilder;
using CodeSharpenerCryptoAnalzer.Common;
using CryslCSharpObjectBuilder.Models.CSharpModels;
using CryslData;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace CodeSharpenerCryptoAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CodeSharpenerCryptoAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CodeSharpenerCryptoAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        private static CryslJsonModel _cryslSpecificationModel;

        private static ServiceProvider _serviceProvider { get; set; }

        private static Dictionary<string, AddConstraints> AdditionalConstraintsDict;

        private static List<KeyValuePair<string, string>> EventsOrderDict;

        private static List<string> EventOrderContraint;

        public override void Initialize(AnalysisContext context)
        {
            var services = new ServiceCollection();
            services.AddTransient<ICommonUtilities, CommonUtilities>();
            services.AddTransient<IEventSectionAnalyzer, EventsSectionAnalyzer>();
            services.AddTransient<IConstraintsSectionAnalyzer, ConstraintsSectionAnalyzer>();
            services.AddTransient<IOrderSectionAnalyzer, OrderSectionAnalyzer>();
            services.AddTransient<ICryslObjectBuilder, CryslObjectBuilder>();
            _serviceProvider = services.BuildServiceProvider();

            ICryslObjectBuilder cSharpObjectBuilder = _serviceProvider.GetService<ICryslObjectBuilder>();
            CryslResult cryslCompilationModel = cSharpObjectBuilder.CryslToCSharpBuilder();

            if (cryslCompilationModel.IsValid)
            {
                InitializeContext(context, cryslCompilationModel.CryslModel);
            }
            else
            {
                return;
            }
        }

        public void InitializeContext(AnalysisContext context, CryslJsonModel cryslCompilationModel)
        {
            //Register all the syntax nodes that needs to be analyzed
            context.RegisterSyntaxNodeAction(AnalyzeMethodInvocationNode, SyntaxKind.InvocationExpression);
            context.RegisterSyntaxNodeAction(AnalyzeSimpleAssignmentExpression, SyntaxKind.SimpleAssignmentExpression);

            //All global assignements to analyzer goes below
            _cryslSpecificationModel = cryslCompilationModel;
            AdditionalConstraintsDict = new Dictionary<string, AddConstraints>();
            EventsOrderDict = new List<KeyValuePair<string, string>>();
            var commonUtilities = _serviceProvider.GetService<ICommonUtilities>();
            EventOrderContraint = commonUtilities.GetEventOrderList(_cryslSpecificationModel);

        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            // Find just those named type symbols with names containing lowercase letters.
            if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
            {
                // For all such symbols, produce a diagnostic.
                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }

        /// <summary>
        /// Analyze Method Invocation Nodes
        /// </summary>
        /// <param name="context"></param>
        private static void AnalyzeMethodInvocationNode(SyntaxNodeAnalysisContext context)
        {
            var diagnostics = context.Compilation.GetDiagnostics();
            var invocationExpressionNode = context.Node;
            var memAcessExprNode = invocationExpressionNode.ChildNodes().OfType<MemberAccessExpressionSyntax>();
            var argumentsList = invocationExpressionNode.ChildNodes().OfType<ArgumentListSyntax>();
            ICommonUtilities commonUtilities = _serviceProvider.GetService<ICommonUtilities>();

            foreach (var node in memAcessExprNode)
            {
                var identifierNode = node.ChildNodes().OfType<IdentifierNameSyntax>();

                if (identifierNode.Count() > 0)
                {
                    var invocatorIdentifier = identifierNode.FirstOrDefault();
                    var invokedMethod = identifierNode.LastOrDefault();
                    string invocatorType = commonUtilities.GetInvocatorType(context.SemanticModel.GetSymbolInfo(invocatorIdentifier).Symbol);
                    var result = _cryslSpecificationModel.Event_Section.Methods.Select(x => x.Crypto_Signature
                     .Where(y => y.Method_Name.ToString().Equals(invokedMethod.Identifier.Value.ToString())));
                    foreach (var methods in _cryslSpecificationModel.Event_Section.Methods)
                    {
                        // Check if method signature matches with the method signature defined in events section of the Crysl.
                        var cryptoMethods = methods.Crypto_Signature.Select(y => y).Where(x => x.Method_Name.ToString().Equals(invokedMethod.Identifier.Value.ToString()));
                        if (cryptoMethods.Count() > 0)
                        {
                            IEventSectionAnalyzer analyzer = _serviceProvider.GetService<IEventSectionAnalyzer>();
                            var identifierSymbolInfo = (IMethodSymbol)context.SemanticModel.GetSymbolInfo(invokedMethod).Symbol;
                            //Check for Valid Events
                            ValidEvents validEvents = analyzer.AnalyzeMemAccessExprSyntax(invokedMethod, cryptoMethods, methods, _cryslSpecificationModel, context, identifierSymbolInfo, node.Span, invocatorType);

                            //Check for Valid Order
                            IOrderSectionAnalyzer orderSectionAnalyzer = _serviceProvider.GetService<IOrderSectionAnalyzer>();
                            bool isOrderValid = orderSectionAnalyzer.IsValidOrder(validEvents, context.ContainingSymbol.ToString(), EventsOrderDict, EventOrderContraint);

                            //Report If Order Constraint is Violated
                            if (!isOrderValid)
                            {
                                //Report Diagnostics Order Violation
                            }

                            //If Event is Valid then Check Constraints
                            if (validEvents.IsValidEvent)
                            {
                                //Check for Valid Constraints
                                foreach (var parameter in validEvents.ValidEventsDict)
                                {
                                    IConstraintsSectionAnalyzer constraintsSectionAnalyzer = _serviceProvider.GetService<IConstraintsSectionAnalyzer>();
                                    List<ConstraintsModel> satisfiedConstraintsList = new List<ConstraintsModel>();
                                    foreach (var parameterValue in parameter.Value)
                                    {
                                        //Check for constraints only if arguments are present
                                        if (parameterValue.Parameters.Count != 0)
                                        {
                                            satisfiedConstraintsList = constraintsSectionAnalyzer.AnalyzeParameters(argumentsList, parameterValue.Parameters, _cryslSpecificationModel.Constraints_Section.Constraints);
                                            ReportConstraintsSection(context, satisfiedConstraintsList);
                                            AddConstraints additionalConstraints = new AddConstraints
                                            {
                                                EventKey = parameter.Key,
                                                EventVariableDeclarator = invokedMethod.AncestorsAndSelf().OfType<VariableDeclaratorSyntax>().First().Identifier.Text,
                                                ConstraintsModels = satisfiedConstraintsList
                                            };
                                            AdditionalConstraintsDict.Add(identifierSymbolInfo.ReturnType.ToString(), additionalConstraints);
                                        }
                                    }

                                }

                            }
                            else
                            {
                                //Report context to diagnostics as invalid event
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Analyze Assignment Expression Nodes
        /// </summary>
        /// <param name="context"></param>
        private static void AnalyzeSimpleAssignmentExpression(SyntaxNodeAnalysisContext context)
        {
            var simpleAssExpr = context.Node;
            if (simpleAssExpr.Kind().Equals(SyntaxKind.SimpleAssignmentExpression))
            {
                var simpleAssignExpr = (AssignmentExpressionSyntax)simpleAssExpr;

                //Check Only For MemberAcessExpressionSyntax Nodes
                if (simpleAssignExpr.ChildNodes().OfType<MemberAccessExpressionSyntax>().FirstOrDefault() != null)
                {
                    //Get the invocator
                    var invocator = simpleAssignExpr.ChildNodes().OfType<MemberAccessExpressionSyntax>().FirstOrDefault().ChildNodes().OfType<IdentifierNameSyntax>().FirstOrDefault();

                    var invocatorSymbolInfo = context.SemanticModel.GetSymbolInfo(invocator).Symbol;
                    if (invocatorSymbolInfo.Kind.Equals(SymbolKind.Local))
                    {
                        var localInvocatorSymbolInfo = (ILocalSymbol)invocatorSymbolInfo;
                        //Analze only if type is of SPEC type
                        if (localInvocatorSymbolInfo.Type.ToString().Equals(_cryslSpecificationModel.Spec_Section.Class_Name))
                        {
                            var leftExprSymbolInfo = context.SemanticModel.GetSymbolInfo(simpleAssignExpr.Left).Symbol;
                            var rightExprSymbolInfo = context.SemanticModel.GetSymbolInfo(simpleAssignExpr.Right).Symbol;
                            bool isValidEvent = false;
                            foreach (var methods in _cryslSpecificationModel.Event_Section.Methods)
                            {
                                var cryptoMethods = methods.Crypto_Signature.Select(x => x).Where(y => y.Method_Name.ToString().Equals(leftExprSymbolInfo.Name.ToString()));
                                if (cryptoMethods.Count() > 0)
                                {
                                    //Set the flag to true as a valid event
                                    IEventSectionAnalyzer analyzer = _serviceProvider.GetService<IEventSectionAnalyzer>();
                                    var validEvents = analyzer.AnalyzeAssignmentExprSyntax(cryptoMethods, _cryslSpecificationModel, context, leftExprSymbolInfo);
                                    isValidEvent = validEvents.IsValidEvent;

                                    //Check for Valid Order
                                    IOrderSectionAnalyzer orderSectionAnalyzer = _serviceProvider.GetService<IOrderSectionAnalyzer>();
                                    bool isOrderValid = orderSectionAnalyzer.IsValidOrder(validEvents, context.ContainingSymbol.ToString(), EventsOrderDict, EventOrderContraint);
                                    if (!isOrderValid)
                                    {
                                        //Report Diagnsotics as Violation of Order Constraint
                                    }

                                    string rightExprValue = string.Empty;
                                    if (simpleAssignExpr.Right.Kind().Equals(SyntaxKind.NumericLiteralExpression))
                                    {
                                        var literalExpressionSyntax = (LiteralExpressionSyntax)simpleAssignExpr.Right;
                                        rightExprValue = literalExpressionSyntax.Token.Value.ToString();

                                    }
                                    else if (rightExprSymbolInfo != null)
                                    {
                                        rightExprValue = rightExprSymbolInfo.Name.ToString();
                                    }
                                    //Check if primary constraints are satified if any
                                    IConstraintsSectionAnalyzer constraintsSectionAnalyzer = _serviceProvider.GetService<IConstraintsSectionAnalyzer>();
                                    bool isPrimaryConstraintSatisfied = constraintsSectionAnalyzer.IsPropertyConstraintSatisfied(_cryslSpecificationModel, validEvents, rightExprValue);
                                    if (!isPrimaryConstraintSatisfied)
                                    {
                                        //Report Violation of Primary Constraints
                                    }

                                    //Check if additional constraints are satisfied if any
                                    AddConstraints additionalConstraints = new AddConstraints();
                                    AdditionalConstraintsDict.TryGetValue(_cryslSpecificationModel.Spec_Section.Class_Name, out additionalConstraints);

                                    //Check additionalConstraints for null because additionalConstraints is an out parameter, so if TryGet does not return anything, then additionalConstraints would be set to null
                                    if (additionalConstraints != null)
                                    {
                                        if (additionalConstraints.EventVariableDeclarator.ToString().Equals(localInvocatorSymbolInfo.Name.ToString()))
                                        {
                                            constraintsSectionAnalyzer = _serviceProvider.GetService<IConstraintsSectionAnalyzer>();
                                            bool isAdditionalConstraintSatisfied = constraintsSectionAnalyzer.IsAdditionalConstraintSatisfied(additionalConstraints, leftExprSymbolInfo, rightExprValue, _cryslSpecificationModel.Object_Section.Objects_Declaration, validEvents);
                                            if (!isAdditionalConstraintSatisfied)
                                            {
                                                //Report Violation of Additional Constraints
                                            }
                                        }
                                    }

                                }
                            }
                            if (!isValidEvent)
                            {
                                //Report Diagnostics as not a Valid Event
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Report if Constraints are not satisfied
        /// </summary>
        /// <param name="context"></param>
        /// <param name="constraintsModelsList"></param>
        private static void ReportConstraintsSection(SyntaxNodeAnalysisContext context, List<ConstraintsModel> constraintsModelsList)
        {
            foreach (var constraints in constraintsModelsList)
            {
                if (!constraints.IsConstraintSatisfied)
                {
                    //Report Diagnostics as constraints not satisfied
                }
            }
        }
    }
}
