using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(EventViolationRule, EventAggViolationRule, ConstraintAnalyzerViolationRule, OrderAnalyzerViolationRule, HardCodedCheckViolationRule); } }



        private static CryslJsonModel _cryslSpecificationModel;

        private static ServiceProvider _serviceProvider { get; set; }

        private static Dictionary<string, List<AddConstraints>> AdditionalConstraintsDict;

        private static List<KeyValuePair<string, string>> EventsOrderDict;

        private static List<KeyValuePair<string, string>> EventOrderContraint;

        private static List<ISymbol> TaintedVariables;

        //Events Related Dictionary
        //Key: Event_Var_Name
        //Value: List of MethodSignatureModel
        private static Dictionary<string, List<MethodSignatureModel>> ValidEventsDictionary;

        //Dictionary of all analyzed events
        private static Dictionary<string, string> EventsOrderDictionary;

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
            context.RegisterCodeBlockStartAction<SyntaxKind>(AnalyzeCodeBlockAction);
            context.RegisterSyntaxNodeAction(AnalyzeLocalDeclarationStatement, SyntaxKind.LocalDeclarationStatement);

            //All global assignements to analyzer goes below
            _cryslSpecificationModel = cryslCompilationModel;
            AdditionalConstraintsDict = new Dictionary<string, List<AddConstraints>>();
            EventsOrderDict = new List<KeyValuePair<string, string>>();
            ValidEventsDictionary = new Dictionary<string, List<MethodSignatureModel>>();
            EventsOrderDictionary = new Dictionary<string, string>();
            TaintedVariables = new List<ISymbol>();
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

        /// <summary>
        /// Report HardCoded Byte Array Values
        /// </summary>
        /// <param name="context"></param>
        private void AnalyzeLocalDeclarationStatement(SyntaxNodeAnalysisContext context)
        {
            var localDeclarationStatement = (LocalDeclarationStatementSyntax)context.Node;
            LocalDeclarationStatementVisitor localDeclarationStatementVisitor = new LocalDeclarationStatementVisitor();
            localDeclarationStatementVisitor.Visit(localDeclarationStatement);
            var isArrayInitializerPresent = localDeclarationStatementVisitor.GetResult();
            if(isArrayInitializerPresent.IsArrayInitializer)
            {
                var nodeSymbolInfo = context.SemanticModel.GetDeclaredSymbol(isArrayInitializerPresent.DeclaratorSyntax);
                var dataFlowAnalysisResult = context.SemanticModel.AnalyzeDataFlow(localDeclarationStatement);
                if(dataFlowAnalysisResult.ReadOutside.Contains(nodeSymbolInfo))
                {
                    var diagnsotics = Diagnostic.Create(HardCodedCheckViolationRule, localDeclarationStatement.GetLocation());
                    context.ReportDiagnostic(diagnsotics);
                }
            }
            /*var variableDeclarationNode = localDeclarationStatement.ChildNodes().OfType<VariableDeclarationSyntax>().FirstOrDefault().ChildNodes().OfType<VariableDeclaratorSyntax>().FirstOrDefault();
            var nodeSymbolInfo = context.SemanticModel.GetDeclaredSymbol(variableDeclarationNode) as ILocalSymbol;
            if(nodeSymbolInfo != null)
            {
                if(nodeSymbolInfo.Type.ToString().Equals("byte[]"))
                {
                    var dataFlowAnalysisResult = context.SemanticModel.AnalyzeDataFlow(localDeclarationStatement);
                    if(dataFlowAnalysisResult.ReadOutside.Contains(nodeSymbolInfo))
                    {
                        //Report Diagnsotics
                    }
                }
            }*/
            
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

                                    foreach (var parameter in validEvents.ValidMethods.Parameters)
                                    {
                                        var constraintsList = _cryslSpecificationModel.Constraints_Section.Constraints.Where(y => y.Object_Varname.ToString().Equals(parameter.Argument.ToString())).Select(x => x.Constraints_List);
                                        //Check only if constraint is present for the method arguments
                                        if (constraintsList.Count() != 0)
                                        {

                                            satisfiedConstraintsList = constraintsSectionAnalyzer.AnalyzeParameters(argumentsList, validEvents.ValidMethods.Parameters, _cryslSpecificationModel.Constraints_Section.Constraints);
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
                                                addConstraintsList = new List<AddConstraints>();
                                                AddConstraints additionalConstraints = new AddConstraints
                                                {
                                                    EventKey = validEvents.PropertyName,
                                                    EventVariableDeclarator = invokedMethod.AncestorsAndSelf().OfType<VariableDeclaratorSyntax>().First().Identifier.Text,
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
                                                    EventVariableDeclarator = invokedMethod.AncestorsAndSelf().OfType<VariableDeclaratorSyntax>().First().Identifier.Text,
                                                    ConstraintsModels = satisfiedConstraintsList
                                                };
                                                additionalConstraintsCheck.Add(additionalConstraints);
                                                AdditionalConstraintsDict[identifierSymbolInfo.ReturnType.ToString()] = additionalConstraintsCheck;

                                            }
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
                            //Analze only if type is of SPEC type
                            if (localInvocatorSymbolInfo.Type.ToString().Equals(_cryslSpecificationModel.Spec_Section.Class_Name))
                            {
                                var leftExprSymbolInfo = context.SemanticModel.GetSymbolInfo(simpleAssignExpr.Left).Symbol;
                                var rightExprSymbolInfo = context.SemanticModel.GetSymbolInfo(simpleAssignExpr.Right).Symbol;
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
                                            //Report Violation of Primary Constraints
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
                            }
                        }
                    }
                }
            }
        }

        private void AnalyzeCodeBlockAction(CodeBlockStartAnalysisContext<SyntaxKind> context)
        {
            //EventsOrderDictionary.Clear();
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
            //Clear the Events and Order Dictionary after Analyzing Each Method Block
            EventsOrderDictionary.Clear();
            ValidEventsDictionary.Clear();
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
