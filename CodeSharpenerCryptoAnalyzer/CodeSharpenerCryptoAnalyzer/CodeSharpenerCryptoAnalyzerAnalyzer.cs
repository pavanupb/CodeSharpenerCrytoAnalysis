using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CodeSharpenerCryptoAnalysis.AnalyzerModels;
using CodeSharpenerCryptoAnalysis.CryslSectionsAnalyzers;
using CodeSharpenerCryptoAnalysis.Models;
using CodeSharpenerCryptoAnalyzer.CryslBuilder;
using CodeSharpenerCryptoAnalyzer.Visitors;
using CodeSharpenerCryptoAnalzer.Common;
using CryslCSharpObjectBuilder.Models.CSharpModels;
using CryslData;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.CopyAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;

namespace CodeSharpenerCryptoAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CodeSharpenerCryptoAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string EventDiagnosticId = "EventViolationRule";
        private static readonly LocalizableString EventTitle = new LocalizableResourceString(nameof(Resources.EventAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString EventMessageFormat = new LocalizableResourceString(nameof(Resources.EventAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString EventDescription = new LocalizableResourceString(nameof(Resources.EventAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string EventCategory = "Violation";
        private static DiagnosticDescriptor EventViolationRule = new DiagnosticDescriptor(EventDiagnosticId, EventTitle, EventMessageFormat, EventCategory, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: EventDescription);

        public const string EventAggDiagnosticId = "EventViolationRule";
        private static readonly LocalizableString EventAggTitle = new LocalizableResourceString(nameof(Resources.EventAggAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString EventAggMessageFormat = new LocalizableResourceString(nameof(Resources.EventAggAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString EventAggDescription = new LocalizableResourceString(nameof(Resources.EventAggAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string EventAggCategory = "Violation";
        private static DiagnosticDescriptor EventAggViolationRule = new DiagnosticDescriptor(EventAggDiagnosticId, EventAggTitle, EventAggMessageFormat, EventAggCategory, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: EventAggDescription);

        public const string ConstraintDiagnosticId = "ConstraintViolationRule";
        private static readonly LocalizableString ConstraintAnalyzerTitle = new LocalizableResourceString(nameof(Resources.ConstraintAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString ConstraintAnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.ConstraintAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString ConstraintAnalyzerDescription = new LocalizableResourceString(nameof(Resources.ConstraintAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string ConstraintAnalyzerCategory = "Violation";
        private static DiagnosticDescriptor ConstraintAnalyzerViolationRule = new DiagnosticDescriptor(ConstraintDiagnosticId, ConstraintAnalyzerTitle, ConstraintAnalyzerMessageFormat, ConstraintAnalyzerCategory, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: ConstraintAnalyzerDescription);

        public const string OrderDiagnosticId = "OrderViolationRule";
        private static readonly LocalizableString OrderAnalyzerTitle = new LocalizableResourceString(nameof(Resources.OrderAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString OrderAnalyzerMessageFormat = new LocalizableResourceString(nameof(Resources.OrderAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString OrderAnalyzerDescription = new LocalizableResourceString(nameof(Resources.OrderAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string OrderAnalyzerCategory = "Violation";
        private static DiagnosticDescriptor OrderAnalyzerViolationRule = new DiagnosticDescriptor(OrderDiagnosticId, OrderAnalyzerTitle, OrderAnalyzerMessageFormat, OrderAnalyzerCategory, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: OrderAnalyzerDescription);

        public const string HardCodedCheckDiagnosticId = "HardCodedKey";
        private static readonly LocalizableString HardCodedCheckTitle = new LocalizableResourceString(nameof(Resources.HardCodedKeysTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString HardCodedCheckMessageFormat = new LocalizableResourceString(nameof(Resources.HardCodedMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString HardCodedCheckDescription = new LocalizableResourceString(nameof(Resources.HardCodedDescription), Resources.ResourceManager, typeof(Resources));
        private const string HardCodedCheckCategory = "Violation";
        private static DiagnosticDescriptor HardCodedCheckViolationRule = new DiagnosticDescriptor(HardCodedCheckDiagnosticId, HardCodedCheckTitle, HardCodedCheckMessageFormat, HardCodedCheckCategory, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: HardCodedCheckDescription);

        public const string DerivedTypeDiagnsoticId = "DerivedMethodInUse";
        private static readonly LocalizableString DerivedTypeTitle = new LocalizableResourceString(nameof(Resources.DerivedTypeTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DerivedTypeMessageFormat = new LocalizableResourceString(nameof(Resources.DerivedTypeMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DerivedTypeDescription = new LocalizableResourceString(nameof(Resources.DerivedTypeDescription), Resources.ResourceManager, typeof(Resources));
        private const string DerivedTypeCategory = "Warning";
        private static DiagnosticDescriptor DerivedTypeRule = new DiagnosticDescriptor(DerivedTypeDiagnsoticId, DerivedTypeTitle, DerivedTypeMessageFormat, DerivedTypeCategory, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: DerivedTypeDescription);
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(EventViolationRule, EventAggViolationRule, ConstraintAnalyzerViolationRule, OrderAnalyzerViolationRule, HardCodedCheckViolationRule, DerivedTypeRule); } }



        private static CryslJsonModel _cryslSpecificationModel;
        private static ServiceProvider _serviceProvider { get; set; }

        private static Dictionary<string, List<AddConstraints>> AdditionalConstraintsDict;

        private static List<KeyValuePair<string, string>> EventOrderContraint;

        //Events Related Dictionary
        //Key: Event_Var_Name
        //Value: List of MethodSignatureModel
        private static Dictionary<string, List<MethodSignatureModel>> ValidEventsDictionary;

        //Dictionary of all analyzed events
        private static Dictionary<string, string> EventsOrderDictionary;

        private static List<KeyValuePair<ISymbol, ISymbol>> TaintedValuesDictionary;

        private static Dictionary<string, List<KeyValuePair<ISymbol, ISymbol>>> TaintedContextDictionary;

        public override void Initialize(AnalysisContext context)
        {

            var services = new ServiceCollection();
            services.AddTransient<ICommonUtilities, CommonUtilities>();
            services.AddTransient<IEventSectionAnalyzer, EventsSectionAnalyzer>();
            services.AddTransient<IConstraintsSectionAnalyzer, ConstraintsSectionAnalyzer>();
            services.AddTransient<IOrderSectionAnalyzer, OrderSectionAnalyzer>();
            services.AddTransient<ICryslObjectBuilder, CryslObjectBuilder>();
            _serviceProvider = services.BuildServiceProvider();

            //Check for different Crysl files

            var currentDirectory = Directory.GetCurrentDirectory();

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
            context.RegisterCodeBlockStartAction<SyntaxKind>(AnalyzeCodeBlockAction);
            context.RegisterOperationBlockStartAction(AnalyzeUsingStartBlock);
            context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, SyntaxKind.ObjectCreationExpression);
            context.RegisterSyntaxNodeAction(AnalyzeMethodInvocationNode, SyntaxKind.InvocationExpression);
            context.RegisterSyntaxNodeAction(AnalyzeSimpleAssignmentExpression, SyntaxKind.SimpleAssignmentExpression);
            context.RegisterSyntaxNodeAction(AnalyzeLocalDeclarationStatement, SyntaxKind.LocalDeclarationStatement);
            context.RegisterSyntaxNodeAction(AnalyzeMethodDeclarationNode, SyntaxKind.MethodDeclaration);


            //All global assignements to analyzer goes below
            _cryslSpecificationModel = cryslCompilationModel;
            AdditionalConstraintsDict = new Dictionary<string, List<AddConstraints>>();
            ValidEventsDictionary = new Dictionary<string, List<MethodSignatureModel>>();
            EventsOrderDictionary = new Dictionary<string, string>();
            TaintedValuesDictionary = new List<KeyValuePair<ISymbol, ISymbol>>();
            if (TaintedContextDictionary == null)
            {
                TaintedContextDictionary = new Dictionary<string, List<KeyValuePair<ISymbol, ISymbol>>>();
            }          

            var commonUtilities = _serviceProvider.GetService<ICommonUtilities>();
            EventOrderContraint = commonUtilities.GetEventOrderList(_cryslSpecificationModel);

        }

        /*private static void AnalyzeSymbol(SymbolAnalysisContext context)
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
        }*/

        private void AnalyzeMethodDeclarationNode(SyntaxNodeAnalysisContext context)
        {
            List<KeyValuePair<ISymbol, ISymbol>> taintedDictionary = new List<KeyValuePair<ISymbol, ISymbol>>();
            TaintedContextDictionary.TryGetValue(context.ContainingSymbol.ToString(), out taintedDictionary);
            if (taintedDictionary != null)
            {
                TaintedContextDictionary.Remove(context.ContainingSymbol.ToString());
            }

        }

        /// <summary>
        /// Report HardCoded Values
        /// </summary>
        /// <param name="context"></param>
        private void AnalyzeLocalDeclarationStatement(SyntaxNodeAnalysisContext context)
        {
            var localDeclarationStatement = (LocalDeclarationStatementSyntax)context.Node;
            LocalDeclarationStatementVisitor localDeclarationStatementVisitor = new LocalDeclarationStatementVisitor();
            localDeclarationStatementVisitor.Visit(localDeclarationStatement);

            var isIdentifierNameNode = localDeclarationStatementVisitor.GetIdentifierNameSyntaxResult();
            if (isIdentifierNameNode.IsIdentifierPresent)
            {
                var identifierSymbolInfo = context.SemanticModel.GetSymbolInfo(isIdentifierNameNode.IdentifierNameSyntaxNode).Symbol;
                if (identifierSymbolInfo != null && identifierSymbolInfo.ContainingSymbol != null)
                {
                    if (IsTaintedValueExists(identifierSymbolInfo.ContainingSymbol, identifierSymbolInfo))
                    {
                        var declaratorSymbolInfo = context.SemanticModel.GetDeclaredSymbol(isIdentifierNameNode.VariableDeclarator);
                        if (!IsTaintedValueExists(declaratorSymbolInfo.ContainingSymbol, declaratorSymbolInfo))
                        {
                            TaintedValuesDictionary.Add(new KeyValuePair<ISymbol, ISymbol>(declaratorSymbolInfo.ContainingSymbol, declaratorSymbolInfo));
                        }
                        var diagnostics = Diagnostic.Create(HardCodedCheckViolationRule, localDeclarationStatement.GetLocation());
                        context.ReportDiagnostic(diagnostics);
                    }
                }

            }

            var isArrayInitializerPresent = localDeclarationStatementVisitor.GetByteArrayResult();
            if (isArrayInitializerPresent.IsArrayInitializer)
            {
                //Adding to Tainted Dictionary for all Byte ArrayInitializer Types
                var nodeSymbolInfo = context.SemanticModel.GetDeclaredSymbol(isArrayInitializerPresent.DeclaratorSyntax);
                if (!IsTaintedValueExists(nodeSymbolInfo.ContainingSymbol, nodeSymbolInfo))
                {
                    TaintedValuesDictionary.Add(new KeyValuePair<ISymbol, ISymbol>(nodeSymbolInfo.ContainingSymbol, nodeSymbolInfo));
                }
                var dataFlowAnalysisResult = context.SemanticModel.AnalyzeDataFlow(localDeclarationStatement);
                if (dataFlowAnalysisResult.ReadOutside.Contains(nodeSymbolInfo))
                {
                    var diagnsotics = Diagnostic.Create(HardCodedCheckViolationRule, localDeclarationStatement.GetLocation());
                    context.ReportDiagnostic(diagnsotics);
                }
            }

            var isStringInitializerPresent = localDeclarationStatementVisitor.GetStringLiteralResult();
            if (isStringInitializerPresent.IsStringLiteralInitializer)
            {
                //Adding to Tainted Dictionary for all Byte ArrayInitializer Types
                var nodeSymbolInfo = context.SemanticModel.GetDeclaredSymbol(isStringInitializerPresent.DeclaratorSyntax);
                if (!IsTaintedValueExists(nodeSymbolInfo.ContainingSymbol, nodeSymbolInfo))
                {
                    TaintedValuesDictionary.Add(new KeyValuePair<ISymbol, ISymbol>(nodeSymbolInfo.ContainingSymbol, nodeSymbolInfo));
                }
                var dataFlowAnalysisResult = context.SemanticModel.AnalyzeDataFlow(localDeclarationStatement);
                if (dataFlowAnalysisResult.ReadOutside.Contains(nodeSymbolInfo))
                {
                    var diagnsotics = Diagnostic.Create(HardCodedCheckViolationRule, localDeclarationStatement.GetLocation());
                    context.ReportDiagnostic(diagnsotics);
                }
            }
        }

        private void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context)
        {
            var objectCreationNode = context.Node;
            var identifierNode = objectCreationNode.ChildNodes().OfType<IdentifierNameSyntax>();
            ICommonUtilities commonUtilities = _serviceProvider.GetService<ICommonUtilities>();
            var argumentsList = objectCreationNode.ChildNodes().OfType<ArgumentListSyntax>();

            if (identifierNode.Count() > 0)
            {
                var instantiatedMethodIdentifier = identifierNode.FirstOrDefault();
                string instantiatedMethodType = commonUtilities.GetInvocatorType(context.SemanticModel.GetSymbolInfo(instantiatedMethodIdentifier).Symbol);

                if (instantiatedMethodType.Equals(_cryslSpecificationModel.Spec_Section.Class_Name))
                {
                    foreach (var methods in _cryslSpecificationModel.Event_Section.Methods)
                    {

                        // Check if method signature matches with the method signature defined in events section of the Crysl.
                        var cryptoMethods = methods.Crypto_Signature.Select(y => y).Where(x => x.Method_Name.ToString().Equals(instantiatedMethodIdentifier.Identifier.Value.ToString()));
                        if (cryptoMethods.Count() > 0)
                        {
                            IEventSectionAnalyzer analyzer = _serviceProvider.GetService<IEventSectionAnalyzer>();
                            var identifierSymbolInfo = (IMethodSymbol)context.SemanticModel.GetSymbolInfo(objectCreationNode).Symbol;

                            //Check for Valid Events
                            ValidEvents validEvents = analyzer.AnalyzeMemAccessExprSyntax(instantiatedMethodIdentifier, cryptoMethods, methods, _cryslSpecificationModel, context, identifierSymbolInfo, objectCreationNode.Span, instantiatedMethodType);

                            if (validEvents.IsValidEvent)
                            {
                                //Check if any argument value is tainted                                
                                foreach (var argumentList in argumentsList)
                                {
                                    if (argumentList.Arguments != null)
                                    {
                                        foreach (var arguments in argumentList.Arguments)
                                        {
                                            ArgumentsVisitor argumentsVisitor = new ArgumentsVisitor();
                                            argumentsVisitor.Visit(arguments);
                                            var isIdentifierPresent = argumentsVisitor.GetResult();
                                            if (isIdentifierPresent.IsIdentifierNodePresent)
                                            {
                                                var argumentSymbolInfo = context.SemanticModel.GetSymbolInfo(isIdentifierPresent.IdentifierNameSyntax);
                                                if (argumentSymbolInfo.Symbol != null)
                                                {
                                                    if (IsTaintedValueExists(argumentSymbolInfo.Symbol.ContainingSymbol, argumentSymbolInfo.Symbol))
                                                    {
                                                        var diagnsotics = Diagnostic.Create(HardCodedCheckViolationRule, arguments.GetLocation());
                                                        context.ReportDiagnostic(diagnsotics);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                //Check only if Aggregators are present
                                if (methods.Aggregator != null)
                                {
                                    //Add valid events to Events and Order Dictionary
                                    AddEventsToDictionary(methods.Aggregator.Aggregator_Name, validEvents, cryptoMethods.FirstOrDefault().Method_Name);

                                    bool isAggConditionSatisfied = commonUtilities.CheckAggregator(ValidEventsDictionary, methods.Aggregator.Aggregators);
                                    if (!isAggConditionSatisfied)
                                    {
                                        var diagnsotics = Diagnostic.Create(EventAggViolationRule, objectCreationNode.GetLocation(), validEvents.PropertyName);
                                        context.ReportDiagnostic(diagnsotics);
                                    }
                                }
                                //Add single event to dictionary if aggregator not present.
                                else
                                {
                                    AddEventsToDictionary(validEvents.PropertyName, validEvents, cryptoMethods.FirstOrDefault().Method_Name);
                                }

                                //Iterate throigh parameters if present and check for constraints only if arguments are present
                                if (validEvents.ValidMethods.Parameters.Count > 0)
                                {
                                    //Check for valid constraints
                                    IConstraintsSectionAnalyzer constraintsSectionAnalyzer = _serviceProvider.GetService<IConstraintsSectionAnalyzer>();
                                    List<ConstraintsModel> satisfiedConstraintsList = new List<ConstraintsModel>();

                                    satisfiedConstraintsList = constraintsSectionAnalyzer.AnalyzeParameters(argumentsList, validEvents.ValidMethods.Parameters, _cryslSpecificationModel.Constraints_Section.Constraints, context);
                                    foreach (var constraints in satisfiedConstraintsList)
                                    {
                                        if (!constraints.IsConstraintSatisfied)
                                        {
                                            string validParameterValues = string.Empty;
                                            if (constraints.AcceptedParameterValues.Count() != 0)
                                            {
                                                validParameterValues = string.Join(", ", constraints.AcceptedParameterValues);
                                            }

                                            var diagnsotics = Diagnostic.Create(ConstraintAnalyzerViolationRule, objectCreationNode.GetLocation(), constraints.NotSatisfiedParameter, validParameterValues);
                                            context.ReportDiagnostic(diagnsotics);
                                        }
                                    }
                                    //ReportConstraintsSection(context, satisfiedConstraintsList);

                                    List<AddConstraints> additionalConstraintsCheck = new List<AddConstraints>();
                                    AdditionalConstraintsDict.TryGetValue(identifierSymbolInfo.ReturnType.ToString(), out additionalConstraintsCheck);

                                    List<AddConstraints> addConstraintsList;
                                    //Add only if the key is not present in the additional constraints dictionary
                                    if (additionalConstraintsCheck == null)
                                    {
                                        addConstraintsList = new List<AddConstraints>();
                                        AddConstraints additionalConstraints = new AddConstraints
                                        {
                                            EventKey = validEvents.PropertyName,
                                            EventVariableDeclarator = instantiatedMethodIdentifier.AncestorsAndSelf().OfType<VariableDeclaratorSyntax>().First().Identifier.Text,
                                            ConstraintsModels = satisfiedConstraintsList
                                        };
                                        addConstraintsList.Add(additionalConstraints);
                                        AdditionalConstraintsDict.Add(identifierSymbolInfo.ReturnType.ToString(), addConstraintsList);
                                    }
                                    //If key is present update the dictionary with the variable declarator
                                    else
                                    {
                                        AddConstraints additionalConstraints = new AddConstraints
                                        {
                                            EventKey = validEvents.PropertyName,
                                            EventVariableDeclarator = instantiatedMethodIdentifier.AncestorsAndSelf().OfType<VariableDeclaratorSyntax>().First().Identifier.Text,
                                            ConstraintsModels = satisfiedConstraintsList
                                        };
                                        additionalConstraintsCheck.Add(additionalConstraints);
                                        AdditionalConstraintsDict[identifierSymbolInfo.ReturnType.ToString()] = additionalConstraintsCheck;

                                    }
                                }
                            }
                            else
                            {
                                StringBuilder validEventsSig = commonUtilities.GetValidMethodSignatures(methods);
                                var diagnsotic = Diagnostic.Create(EventViolationRule, objectCreationNode.GetLocation(), instantiatedMethodIdentifier.Identifier.Text, validEventsSig.ToString());
                                context.ReportDiagnostic(diagnsotic);
                            }
                        }
                    }
                }
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
                    if (invocatorType.Equals(_cryslSpecificationModel.Spec_Section.Class_Name))
                    {
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
                                    //Check if any argument value is tainted
                                    ArgumentsVisitor argumentsVisitor = new ArgumentsVisitor();
                                    foreach (var arguments in argumentsList)
                                    {
                                        argumentsVisitor.Visit(arguments);
                                        var isIdentifierPresent = argumentsVisitor.GetResult();
                                        if (isIdentifierPresent.IsIdentifierNodePresent)
                                        {
                                            var argumentSymbolInfo = context.SemanticModel.GetSymbolInfo(isIdentifierPresent.IdentifierNameSyntax);
                                            if (IsTaintedValueExists(argumentSymbolInfo.Symbol.ContainingSymbol, argumentSymbolInfo.Symbol))
                                            {
                                                var diagnsotics = Diagnostic.Create(HardCodedCheckViolationRule, arguments.GetLocation());
                                                context.ReportDiagnostic(diagnsotics);
                                            }
                                        }
                                    }

                                    //Check only if Aggregators are present
                                    if (methods.Aggregator != null)
                                    {
                                        //Add valid events to Events and Order Dictionary
                                        AddEventsToDictionary(methods.Aggregator.Aggregator_Name, validEvents, cryptoMethods.FirstOrDefault().Method_Name);

                                        bool isAggConditionSatisfied = commonUtilities.CheckAggregator(ValidEventsDictionary, methods.Aggregator.Aggregators);
                                        if (!isAggConditionSatisfied)
                                        {
                                            var diagnsotics = Diagnostic.Create(EventAggViolationRule, node.GetLocation(), validEvents.PropertyName);
                                            context.ReportDiagnostic(diagnsotics);
                                        }
                                    }
                                    //Add single event to dictionary if aggregator not present.
                                    else
                                    {
                                        AddEventsToDictionary(validEvents.PropertyName, validEvents, cryptoMethods.FirstOrDefault().Method_Name);
                                    }

                                    //Iterate throigh parameters if present and check for constraints only if arguments are present
                                    if (validEvents.ValidMethods.Parameters.Count > 0)
                                    {
                                        //Check for valid constraints
                                        IConstraintsSectionAnalyzer constraintsSectionAnalyzer = _serviceProvider.GetService<IConstraintsSectionAnalyzer>();
                                        List<ConstraintsModel> satisfiedConstraintsList = new List<ConstraintsModel>();

                                        satisfiedConstraintsList = constraintsSectionAnalyzer.AnalyzeParameters(argumentsList, validEvents.ValidMethods.Parameters, _cryslSpecificationModel.Constraints_Section.Constraints, context);
                                        foreach (var constraints in satisfiedConstraintsList)
                                        {
                                            if (!constraints.IsConstraintSatisfied)
                                            {
                                                var accepetedParameterValues = _cryslSpecificationModel.Constraints_Section.Constraints.Where(x => x.Object_Varname.Equals(validEvents.ValidMethods.Parameters.FirstOrDefault().Argument) && x.Additional_constraints == null).Select(y => y.Constraints_List);
                                                string validParameterValues = string.Empty;
                                                if (accepetedParameterValues.Count() != 0)
                                                {
                                                    validParameterValues = string.Join(",", accepetedParameterValues.FirstOrDefault());
                                                }

                                                var diagnsotics = Diagnostic.Create(ConstraintAnalyzerViolationRule, node.GetLocation(), satisfiedConstraintsList.FirstOrDefault().NotSatisfiedParameter, validParameterValues);
                                                context.ReportDiagnostic(diagnsotics);
                                            }
                                        }
                                        //ReportConstraintsSection(context, satisfiedConstraintsList);

                                        List<AddConstraints> additionalConstraintsCheck = new List<AddConstraints>();
                                        AdditionalConstraintsDict.TryGetValue(identifierSymbolInfo.ReturnType.ToString(), out additionalConstraintsCheck);

                                        List<AddConstraints> addConstraintsList;
                                        //Add only if the key is not present in the additional constraints dictionary
                                        if (additionalConstraintsCheck == null)
                                        {
                                            if (invokedMethod.AncestorsAndSelf().OfType<VariableDeclaratorSyntax>().FirstOrDefault() != null)
                                            {
                                                addConstraintsList = new List<AddConstraints>();
                                                AddConstraints additionalConstraints = new AddConstraints
                                                {
                                                    EventKey = validEvents.PropertyName,
                                                    EventVariableDeclarator = invokedMethod.AncestorsAndSelf().OfType<VariableDeclaratorSyntax>().FirstOrDefault().Identifier.Text,
                                                    ConstraintsModels = satisfiedConstraintsList
                                                };
                                                addConstraintsList.Add(additionalConstraints);
                                                AdditionalConstraintsDict.Add(identifierSymbolInfo.ReturnType.ToString(), addConstraintsList);
                                            }
                                        }
                                        //If key is present update the dictionary with the variable declarator
                                        else
                                        {
                                            if (invokedMethod.AncestorsAndSelf().OfType<VariableDeclaratorSyntax>().FirstOrDefault() != null)
                                            {
                                                AddConstraints additionalConstraints = new AddConstraints
                                                {
                                                    EventKey = validEvents.PropertyName,
                                                    EventVariableDeclarator = invokedMethod.AncestorsAndSelf().OfType<VariableDeclaratorSyntax>().FirstOrDefault().Identifier.Text,
                                                    ConstraintsModels = satisfiedConstraintsList
                                                };
                                                additionalConstraintsCheck.Add(additionalConstraints);
                                                AdditionalConstraintsDict[identifierSymbolInfo.ReturnType.ToString()] = additionalConstraintsCheck;
                                            }

                                        }

                                    }
                                }
                                else
                                {
                                    StringBuilder validEventsSig = commonUtilities.GetValidMethodSignatures(methods);
                                    var diagnsotic = Diagnostic.Create(EventViolationRule, node.GetLocation(), invokedMethod.Identifier.Text, validEventsSig.ToString());
                                    context.ReportDiagnostic(diagnsotic);
                                }
                            }
                        }
                    }
                    else if (Type.GetType(invocatorType) != null)
                    {
                        if (Type.GetType(invocatorType).BaseType != null)
                        {
                            if (Type.GetType(invocatorType).BaseType.FullName.Equals(_cryslSpecificationModel.Spec_Section.Class_Name))
                            {
                                var diagnsotics = Diagnostic.Create(DerivedTypeRule, invocationExpressionNode.GetLocation(), invocatorType, _cryslSpecificationModel.Spec_Section.Class_Name);
                                context.ReportDiagnostic(diagnsotics);
                            }
                        }
                    }
                }
            }

            var invExprSymbolInfo = context.SemanticModel.GetSymbolInfo(invocationExpressionNode).Symbol as IMethodSymbol;
            foreach (var arguments in argumentsList)
            {
                for (int i = 0; i < arguments.Arguments.Count; i++)
                {
                    var simpleMemAccessExpr = arguments.Arguments[i].ChildNodes().OfType<MemberAccessExpressionSyntax>();
                    if (simpleMemAccessExpr.Count() > 0)
                    {
                        foreach (var memAccessExpr in simpleMemAccessExpr)
                        {
                            var argumentSymbol = context.SemanticModel.GetSymbolInfo(memAccessExpr).Symbol;
                            if (argumentSymbol != null)
                            {
                                if (IsTaintedValueExists(context.ContainingSymbol, argumentSymbol))
                                {
                                    var diagnsotics = Diagnostic.Create(HardCodedCheckViolationRule, memAccessExpr.GetLocation());
                                    context.ReportDiagnostic(diagnsotics);
                                    if (invExprSymbolInfo != null)
                                    {
                                        if (!IsTaintedValueExists(invExprSymbolInfo.ContainingSymbol, invExprSymbolInfo.Parameters[i]))
                                        {
                                            TaintedValuesDictionary.Add(new KeyValuePair<ISymbol, ISymbol>(invExprSymbolInfo, invExprSymbolInfo.Parameters[i]));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var identifierNameSyntax = arguments.Arguments[i].ChildNodes().OfType<IdentifierNameSyntax>();
                        foreach (var identifierNameNode in identifierNameSyntax)
                        {
                            var identifierSymbolInfo = context.SemanticModel.GetSymbolInfo(identifierNameNode).Symbol;
                            if (identifierSymbolInfo != null)
                            {
                                if (IsTaintedValueExists(context.ContainingSymbol, identifierSymbolInfo))
                                {
                                    var diagnsotics = Diagnostic.Create(HardCodedCheckViolationRule, identifierNameNode.GetLocation());
                                    context.ReportDiagnostic(diagnsotics);
                                    if (invExprSymbolInfo != null)
                                    {
                                        if (!IsTaintedValueExists(invExprSymbolInfo.ContainingSymbol, invExprSymbolInfo.Parameters[i]))
                                        {
                                            TaintedValuesDictionary.Add(new KeyValuePair<ISymbol, ISymbol>(invExprSymbolInfo, invExprSymbolInfo.Parameters[i]));
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
                    if (invocatorSymbolInfo != null)
                    {
                        if (invocatorSymbolInfo.Kind.Equals(SymbolKind.Local))
                        {
                            var localInvocatorSymbolInfo = (ILocalSymbol)invocatorSymbolInfo;

                            var leftExprSymbolInfo = context.SemanticModel.GetSymbolInfo(simpleAssignExpr.Left).Symbol;
                            var rightExprSymbolInfo = context.SemanticModel.GetSymbolInfo(simpleAssignExpr.Right).Symbol;

                            //Check for tainted values
                            if (rightExprSymbolInfo != null)
                            {
                                if (IsTaintedValueExists(context.ContainingSymbol, rightExprSymbolInfo))
                                {
                                    if (!IsTaintedValueExists(context.ContainingSymbol, leftExprSymbolInfo))
                                    {
                                        TaintedValuesDictionary.Add(new KeyValuePair<ISymbol, ISymbol>(context.ContainingSymbol, leftExprSymbolInfo));
                                        var diagnsotics = Diagnostic.Create(HardCodedCheckViolationRule, simpleAssExpr.GetLocation());
                                        context.ReportDiagnostic(diagnsotics);
                                    }
                                    //If already present in TaintedValueDictionary, just report the context
                                    else
                                    {
                                        var diagnsotics = Diagnostic.Create(HardCodedCheckViolationRule, simpleAssExpr.GetLocation());
                                        context.ReportDiagnostic(diagnsotics);
                                    }
                                }
                            }
                            //Analze only if type is of SPEC type
                            if (localInvocatorSymbolInfo.Type.ToString().Equals(_cryslSpecificationModel.Spec_Section.Class_Name))
                            {
                                var assignExprDataFlow = context.SemanticModel.AnalyzeDataFlow(simpleAssignExpr);
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
                                        AddEventsToDictionary(validEvents.AggregatorName, validEvents, cryptoMethods.FirstOrDefault().Method_Name);

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
                                            var accepetedParameterValues = _cryslSpecificationModel.Constraints_Section.Constraints.Where(x => x.Object_Varname.Equals(validEvents.PropertyName) && x.Additional_constraints == null).Select(y => y.Constraints_List);
                                            string validParameterValues = string.Empty;
                                            if (accepetedParameterValues.Count() != 0)
                                            {
                                                validParameterValues = string.Join(",", accepetedParameterValues.FirstOrDefault());
                                            }
                                            var diagnsotics = Diagnostic.Create(ConstraintAnalyzerViolationRule, simpleAssExpr.GetLocation(), rightExprValue, validParameterValues);
                                            context.ReportDiagnostic(diagnsotics);
                                        }

                                        //Check if additional constraints are satisfied if any
                                        List<AddConstraints> additionalConstraintsList = new List<AddConstraints>();
                                        AdditionalConstraintsDict.TryGetValue(_cryslSpecificationModel.Spec_Section.Class_Name, out additionalConstraintsList);

                                        //Check additionalConstraints for null because additionalConstraints is an out parameter, so if TryGet does not return anything, then additionalConstraints would be set to null
                                        if (additionalConstraintsList != null)
                                        {
                                            foreach (var additionalConstraints in additionalConstraintsList)
                                            {
                                                if (additionalConstraints.EventVariableDeclarator.ToString().Equals(localInvocatorSymbolInfo.Name.ToString()))
                                                {
                                                    constraintsSectionAnalyzer = _serviceProvider.GetService<IConstraintsSectionAnalyzer>();
                                                    bool isAdditionalConstraintSatisfied = constraintsSectionAnalyzer.IsAdditionalConstraintSatisfied(additionalConstraints, leftExprSymbolInfo, rightExprValue, _cryslSpecificationModel.Object_Section.Objects_Declaration, validEvents);
                                                    if (!isAdditionalConstraintSatisfied)
                                                    {
                                                        var validValues = additionalConstraints.ConstraintsModels.Select(x => x.AdditionalConstraints.Where(y => y.Object_Varname_Additional_Constraint.ToString().Equals(validEvents.PropertyName.ToString()))).FirstOrDefault().Select(x => x.Additional_Constraints_List);
                                                        string validParameterValues = string.Empty;
                                                        if (validValues.Count() != 0)
                                                        {
                                                            validParameterValues = string.Join(",", validValues.FirstOrDefault());
                                                        }
                                                        var diagnsotics = Diagnostic.Create(ConstraintAnalyzerViolationRule, simpleAssExpr.GetLocation(), rightExprValue, validParameterValues);
                                                        context.ReportDiagnostic(diagnsotics);
                                                    }
                                                }
                                            }

                                        }

                                    }
                                }
                                if (!isValidEvent)
                                {
                                    //Report Diagnostics as not a Valid Event
                                }
                                //Check if tainted variables are santized
                                if (IsTaintedValueExists(context.ContainingSymbol, leftExprSymbolInfo))
                                {
                                    if (rightExprSymbolInfo != null)
                                    {
                                        if (!IsTaintedValueExists(context.ContainingSymbol, rightExprSymbolInfo))
                                        {
                                            if (TaintedValuesDictionary.Count != 0)
                                            {
                                                TaintedValuesDictionary.Remove(new KeyValuePair<ISymbol, ISymbol>(context.ContainingSymbol, leftExprSymbolInfo));
                                            }
                                            else
                                            {
                                                SanitizeTaintValue(context.ContainingSymbol, leftExprSymbolInfo);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {   //Check if tainted variables are santized
                                if (IsTaintedValueExists(context.ContainingSymbol, leftExprSymbolInfo))
                                {
                                    if (rightExprSymbolInfo != null)
                                    {
                                        if (!IsTaintedValueExists(context.ContainingSymbol, rightExprSymbolInfo))
                                        {
                                            if (TaintedValuesDictionary.Count != 0)
                                            {
                                                TaintedValuesDictionary.Remove(new KeyValuePair<ISymbol, ISymbol>(context.ContainingSymbol, leftExprSymbolInfo));
                                            }
                                            else
                                            {
                                                SanitizeTaintValue(context.ContainingSymbol, leftExprSymbolInfo);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (simpleAssExpr.IsKind(SyntaxKind.SimpleAssignmentExpression))
                {
                    var simpleAssignmentExpr = (AssignmentExpressionSyntax)simpleAssExpr;
                    var leftExprSymbolInfo = context.SemanticModel.GetSymbolInfo(simpleAssignmentExpr.Left).Symbol;
                    var rightExprSymbolInfo = context.SemanticModel.GetSymbolInfo(simpleAssignmentExpr.Right).Symbol;

                    //Check for tainted values
                    if (rightExprSymbolInfo != null)
                    {
                        if (IsTaintedValueExists(context.ContainingSymbol, rightExprSymbolInfo))
                        {
                            if (!IsTaintedValueExists(context.ContainingSymbol, leftExprSymbolInfo))
                            {
                                TaintedValuesDictionary.Add(new KeyValuePair<ISymbol, ISymbol>(context.ContainingSymbol, leftExprSymbolInfo));
                                var diagnsotics = Diagnostic.Create(HardCodedCheckViolationRule, simpleAssExpr.GetLocation());
                                context.ReportDiagnostic(diagnsotics);
                            }
                        }
                    }

                    //Check if tainted variables are santized
                    if (IsTaintedValueExists(context.ContainingSymbol, leftExprSymbolInfo))
                    {
                        if (rightExprSymbolInfo != null)
                        {
                            if (!IsTaintedValueExists(context.ContainingSymbol, rightExprSymbolInfo))
                            {
                                //Sanitize the value in the local TaintedValueDictionary
                                if (TaintedValuesDictionary.Count != 0)
                                {
                                    TaintedValuesDictionary.Remove(new KeyValuePair<ISymbol, ISymbol>(context.ContainingSymbol, leftExprSymbolInfo));
                                }
                                //Sanitize the value in the global TaintedContextDictionary as well. 
                                //Sanitizing a method parameter would sanitize the parameter in caller as well as callee
                                SanitizeTaintValue(context.ContainingSymbol, leftExprSymbolInfo);
                            }
                        }
                    }

                }
            }

        }

        private static bool IsTaintedValueExists(ISymbol containingMethod, ISymbol nodeInfo)
        {
            try
            {
                List<KeyValuePair<ISymbol, ISymbol>> taintedDictionaryValues = new List<KeyValuePair<ISymbol, ISymbol>>();
                TaintedContextDictionary.TryGetValue(containingMethod.ToString(), out taintedDictionaryValues);
                if (taintedDictionaryValues != null)
                {
                    foreach (var taintedValue in taintedDictionaryValues)
                    {                        
                        bool taintedValuePresent = (taintedValue.Key.ToString().Equals(containingMethod.ToString()) && taintedValue.Value.Kind.Equals(nodeInfo.Kind) && taintedValue.Value.ToString().Equals(nodeInfo.ToString()) && taintedValue.Value.Name.ToString().Equals(nodeInfo.Name.ToString())) ? true : false;
                        
                        if (taintedValuePresent)
                        {
                            return true;
                        }
                    }
                }
                //If tainted values are not present in ContextDictionary check in Current TaintedValuesDictionary
                else
                {
                    foreach (var taintedValue in TaintedValuesDictionary)
                    {                        
                        bool taintedValuePresent = (taintedValue.Key.ToString().Equals(containingMethod.ToString()) && taintedValue.Value.Kind.Equals(nodeInfo.Kind) && taintedValue.Value.ToString().Equals(nodeInfo.ToString()) && taintedValue.Value.Name.ToString().Equals(nodeInfo.Name.ToString())) ? true : false;
                        
                        if (taintedValuePresent)
                        {
                            return true;
                        }
                    }

                    foreach (var taintedValueDictionary in TaintedContextDictionary)
                    {
                        foreach (var taintedValue in taintedValueDictionary.Value)
                        { 
                            bool taintedValuePresent = (taintedValue.Key.ToString().Equals(containingMethod.ToString()) && taintedValue.Value.Kind.Equals(nodeInfo.Kind) && taintedValue.Value.ToString().Equals(nodeInfo.ToString()) && taintedValue.Value.Name.ToString().Equals(nodeInfo.Name.ToString())) ? true : false;                            
                            if (taintedValuePresent)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Log the exception into a log file
            }
            return false;
        }

        private static void SanitizeTaintValue(ISymbol containingMethod, ISymbol nodeInfo)
        {
            List<KeyValuePair<ISymbol, ISymbol>> sanitizedValues = new List<KeyValuePair<ISymbol, ISymbol>>();
            foreach (var taintedValueDictionary in TaintedContextDictionary)
            {
                foreach (var taintedValue in taintedValueDictionary.Value)
                {
                    bool taintedValuePresent = (taintedValue.Key.ToString().Equals(containingMethod.ToString()) && taintedValue.Value.Kind.Equals(nodeInfo.Kind) && taintedValue.Value.ToString().Equals(nodeInfo.ToString()) && taintedValue.Value.Name.ToString().Equals(nodeInfo.Name.ToString())) ? true : false;
                    if (taintedValuePresent)
                    {
                        sanitizedValues.Add(new KeyValuePair<ISymbol, ISymbol>(taintedValue.Key, taintedValue.Value));                        
                    }
                }
                //Remove all the sanitized variables
                foreach (var sanitizedVariables in sanitizedValues)
                {
                    taintedValueDictionary.Value.Remove(sanitizedVariables);
                }
            }
        }

        private static bool IsMethodSignatureSame(IMethodSymbol taintedMethod, IMethodSymbol currentMethod)
        {
            if (taintedMethod.ContainingNamespace.Name.Equals(currentMethod.ContainingNamespace.Name) && taintedMethod.Name.Equals(currentMethod.Name) && taintedMethod.Parameters.Length.Equals(currentMethod.Parameters.Length))
            {
                for (int i = 0; i < currentMethod.Parameters.Length; i++)
                {
                    bool parameterMatch = false;
                    if (taintedMethod.Parameters[i].Name.Equals(currentMethod.Parameters[i].Name) && taintedMethod.Parameters[i].Type.Name.Equals(currentMethod.Parameters[i].Type.Name))
                    {
                        parameterMatch = true;
                    }
                    if (!parameterMatch)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }


        /*private void AnalyzeBlockAction(OperationBlockStartAnalysisContext context)
        {
            WellKnownTypeProvider provider = WellKnownTypeProvider.GetOrCreate(context.Compilation);

            InterproceduralAnalysisConfiguration interproceduralAnalysisConfiguration = InterproceduralAnalysisConfiguration.Create(
                                                                    context.Options,
                                                                    SupportedDiagnostics,
                                                                    InterproceduralAnalysisKind.ContextSensitive,
                                                                    context.CancellationToken
                                                                    );

            PointsToAnalysisResult pointsToAnalysisResult = PointsToAnalysis.TryGetOrComputeResult(
                context.GetControlFlowGraph(context.OperationBlocks.Last()),
                context.OwningSymbol,
                context.Options,
                provider,
                interproceduralAnalysisConfiguration,
                interproceduralAnalysisPredicateOpt: null
                ); 
            
            
        }*/

        private void AnalyzeUsingStartBlock(OperationBlockStartAnalysisContext context)
        {
            context.RegisterOperationAction(AnalyzeUsingBlock, OperationKind.Using);

        }

        private void AnalyzeUsingBlock(OperationAnalysisContext context)
        {
            var operationSymbol = context.Operation as Microsoft.CodeAnalysis.Operations.IUsingOperation;
            if (operationSymbol != null)
            {
                foreach (var local in operationSymbol.Locals)
                {
                    if (local.Type.ToString().Equals(_cryslSpecificationModel.Spec_Section.Class_Name))
                    {
                        foreach (var methods in _cryslSpecificationModel.Event_Section.Methods)
                        {
                            var cryptoMethods = methods.Crypto_Signature.Select(y => y).Where(x => x.Method_Name.ToString().Equals("Dispose"));
                            if (cryptoMethods.Count() > 0)
                            {

                                MethodSignatureModel methodSignatureModel = new MethodSignatureModel
                                {
                                    MethodName = "Dispose"
                                };

                                List<MethodSignatureModel> methodSignatureModels = new List<MethodSignatureModel>();
                                ValidEventsDictionary.TryGetValue(cryptoMethods.FirstOrDefault().Event_Var_Name, out methodSignatureModels);
                                if (methodSignatureModels != null)
                                {
                                    methodSignatureModels.Add(methodSignatureModel);
                                    ValidEventsDictionary[cryptoMethods.FirstOrDefault().Event_Var_Name] = methodSignatureModels;
                                    if (methods.Aggregator == null)
                                    {
                                        EventsOrderDictionary[cryptoMethods.FirstOrDefault().Event_Var_Name] = cryptoMethods.FirstOrDefault().Method_Name;
                                    }
                                    else
                                    {
                                        EventsOrderDictionary[methods.Aggregator.Aggregator_Name] = cryptoMethods.FirstOrDefault().Method_Name;
                                    }
                                }
                                else
                                {
                                    List<MethodSignatureModel> methodSignatureModelList = new List<MethodSignatureModel>();
                                    methodSignatureModelList.Add(methodSignatureModel);
                                    ValidEventsDictionary.Add(cryptoMethods.FirstOrDefault().Event_Var_Name, methodSignatureModelList);
                                    if (methods.Aggregator == null)
                                    {
                                        EventsOrderDictionary.Add(cryptoMethods.FirstOrDefault().Event_Var_Name, cryptoMethods.FirstOrDefault().Method_Name);
                                    }
                                    else
                                    {
                                        EventsOrderDictionary.Add(methods.Aggregator.Aggregator_Name, cryptoMethods.FirstOrDefault().Method_Name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AnalyzeCodeBlockAction(CodeBlockStartAnalysisContext<SyntaxKind> context)
        {
            context.RegisterCodeBlockEndAction(AnalyzeCodeBLockEndAction);
        }

        private void AnalyzeCodeBLockEndAction(CodeBlockAnalysisContext context)
        {

            if (EventsOrderDictionary.Count != 0)
            {
                var orderCheckConstraint = _serviceProvider.GetService<IOrderSectionAnalyzer>();
                var isValidOrder = orderCheckConstraint.IsValidOrder(EventsOrderDictionary, EventOrderContraint.Select(x => x.Key).ToList());
                if (!isValidOrder)
                {
                    string validEventOrder = string.Join(", ", EventOrderContraint.Select(x => x.Value));
                    var diagnsotics = Diagnostic.Create(OrderAnalyzerViolationRule, context.OwningSymbol.Locations[0], _cryslSpecificationModel.Spec_Section.Class_Name, validEventOrder);
                    context.ReportDiagnostic(diagnsotics);
                }
            }

            List<KeyValuePair<ISymbol, ISymbol>> taintedDict = new List<KeyValuePair<ISymbol, ISymbol>>();
            TaintedContextDictionary.TryGetValue(context.OwningSymbol.ToString(), out taintedDict);
            if (taintedDict == null)
            {
                TaintedContextDictionary.Add(context.OwningSymbol.ToString(), TaintedValuesDictionary.ToList());
            }

            //Clear the Events and Order Dictionary after Analyzing Each Method Block
            EventsOrderDictionary.Clear();
            ValidEventsDictionary.Clear();
            TaintedValuesDictionary.Clear();
        }

        /// <summary>
        /// Add Valid Events to Events Dictionary and Order Dictionary
        /// </summary>
        /// <param name="aggregatorName"></param>
        /// <param name="validEvents"></param>
        private static void AddEventsToDictionary(string aggregatorName, ValidEvents validEvents, string methodName)
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
                EventsOrderDictionary[aggregatorName] = methodName;


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
                EventsOrderDictionary.Add(aggregatorName, methodName);
            }
        }
    }
}
