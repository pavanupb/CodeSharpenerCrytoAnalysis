using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using CodeSharpenerCryptoAnalysis.AnalyzerModels;
using CodeSharpenerCryptoAnalysis.CryslSectionsAnalyzers;
using CodeSharpenerCryptoAnalysis.Models;
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

        //Events Related Dictionary
        //Key: Event_Var_Name
        //Value: List of MethodSignatureModel
        private static Dictionary<string, List<MethodSignatureModel>> ValidEventsDictionary;

        //Dictionary of all analyzed events
        private static Dictionary<string, Dictionary<string, List<MethodSignatureModel>>> EventsOrderDictionary;

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
            context.RegisterCompilationStartAction(AnalyzeCompilationStartAction);
            

            //All global assignements to analyzer goes below
            _cryslSpecificationModel = cryslCompilationModel;
            AdditionalConstraintsDict = new Dictionary<string, AddConstraints>();
            EventsOrderDict = new List<KeyValuePair<string, string>>();
            ValidEventsDictionary = new Dictionary<string, List<MethodSignatureModel>>();
            EventsOrderDictionary = new Dictionary<string, Dictionary<string, List<MethodSignatureModel>>>();
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

                            if (validEvents.IsValidEvent)
                            {
                                //Check only if Aggregators are present
                                if (methods.Aggregator != null)
                                {
                                    //Add valid events to Events and Order Dictionary
                                    AddEventsToDictionary(methods.Aggregator.Aggregator_Name, validEvents);

                                    bool isAggConditionSatisfied = commonUtilities.CheckAggregator(ValidEventsDictionary, methods.Aggregator.Aggregators);
                                    if (!isAggConditionSatisfied)
                                    {
                                        //Report violation of aggregation condition
                                    }
                                }
                                //Add single event to dictionary if aggregator not present.
                                else
                                {
                                    AddEventsToDictionary(validEvents.PropertyName, validEvents);
                                }

                                //Iterate throigh parameters if present and check for constraints only if arguments are present
                                if (validEvents.ValidMethods.Parameters.Count > 0)
                                {
                                    //Check for valid constraints
                                    IConstraintsSectionAnalyzer constraintsSectionAnalyzer = _serviceProvider.GetService<IConstraintsSectionAnalyzer>();
                                    List<ConstraintsModel> satisfiedConstraintsList = new List<ConstraintsModel>();

                                    satisfiedConstraintsList = constraintsSectionAnalyzer.AnalyzeParameters(argumentsList, validEvents.ValidMethods.Parameters, _cryslSpecificationModel.Constraints_Section.Constraints);
                                    ReportConstraintsSection(context, satisfiedConstraintsList);

                                    AddConstraints additionalConstraintsCheck = new AddConstraints();
                                    AdditionalConstraintsDict.TryGetValue(identifierSymbolInfo.ReturnType.ToString(), out additionalConstraintsCheck);

                                    //Add only if the key is not present in the additional constraints dictionary
                                    if (additionalConstraintsCheck == null)
                                    {
                                        AddConstraints additionalConstraints = new AddConstraints
                                        {
                                            EventKey = validEvents.PropertyName,
                                            EventVariableDeclarator = invokedMethod.AncestorsAndSelf().OfType<VariableDeclaratorSyntax>().First().Identifier.Text,
                                            ConstraintsModels = satisfiedConstraintsList
                                        };
                                        AdditionalConstraintsDict.Add(identifierSymbolInfo.ReturnType.ToString(), additionalConstraints);
                                    }
                                }

                            }
                            else
                            {
                                //Report not a valid event
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

                                    //Add valid events to Events and Order Dictionary
                                    AddEventsToDictionary(validEvents.AggregatorName, validEvents);                                    

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
        /// Register CompilationEndAction
        /// </summary>
        /// <param name="context"></param>
        private static void AnalyzeCompilationStartAction(CompilationStartAnalysisContext context)
        {
            context.RegisterCompilationEndAction(AnalyzeEventOrderAction);

        }

        /// <summary>
        /// Check if the Event Order Constraint is Satified
        /// </summary>
        /// <param name="context"></param>
        private static void AnalyzeEventOrderAction(CompilationAnalysisContext context)
        {
            var orderCheckConstraint = _serviceProvider.GetService<IOrderSectionAnalyzer>();
            var isValidOrder = orderCheckConstraint.IsValidOrder(EventsOrderDictionary, EventOrderContraint);
            if(!isValidOrder)
            {
                //Report violation of order constraint
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

        /// <summary>
        /// Add Valid Events to Events Dictionary and Order Dictionary
        /// </summary>
        /// <param name="aggregatorName"></param>
        /// <param name="validEvents"></param>
        private static void AddEventsToDictionary(string aggregatorName, ValidEvents validEvents)
        {
            //Check only if Aggregators are present            
            //For instance it could be "Create" in Aes Crysl specification
            var aggregatorEvents = EventsOrderDictionary.Where(x => x.Key.ToString().Contains(aggregatorName)).Select(y => y).FirstOrDefault();

            //If the Event is not present
            if (aggregatorEvents.Value != null)
            {
                List<MethodSignatureModel> validMethodSignatures = new List<MethodSignatureModel>();                
                ValidEventsDictionary.TryGetValue(validEvents.PropertyName, out validMethodSignatures);

                //If ValidEventsDictionary contains the method but with different signature, update the dictionary
                if (validMethodSignatures != null)
                {
                    validMethodSignatures.Add(validEvents.ValidMethods);
                    ValidEventsDictionary[validEvents.PropertyName] = validMethodSignatures;

                }
                else
                {
                    List<MethodSignatureModel> methodSignatureList = new List<MethodSignatureModel>();
                    methodSignatureList.Add(validEvents.ValidMethods);                    
                    ValidEventsDictionary.Add(validEvents.PropertyName, methodSignatureList);
                }
                //Updating the value because the key is already present        
                EventsOrderDictionary[aggregatorName] = ValidEventsDictionary;


            }
            else
            {
                List<MethodSignatureModel> validMethodSignatures = new List<MethodSignatureModel>();
                ValidEventsDictionary.TryGetValue(validEvents.PropertyName, out validMethodSignatures);

                //If ValidEventsDictionary contains the method but with different signature, update the dictionary
                if (validMethodSignatures != null)
                {
                    validMethodSignatures.Add(validEvents.ValidMethods);
                    ValidEventsDictionary[validEvents.PropertyName] = validMethodSignatures;

                }
                else
                {
                    List<MethodSignatureModel> methodSignatureList = new List<MethodSignatureModel>();
                    methodSignatureList.Add(validEvents.ValidMethods);
                    ValidEventsDictionary.Add(validEvents.PropertyName, methodSignatureList);
                }
                EventsOrderDictionary.Add(aggregatorName, ValidEventsDictionary);
            }
        }
    }
}
