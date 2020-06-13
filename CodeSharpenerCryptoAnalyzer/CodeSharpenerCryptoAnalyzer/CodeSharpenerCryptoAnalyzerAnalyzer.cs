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
using CodeSharpenerCryptoAnalyzer.AnalyzerModels;
using CodeSharpenerCryptoAnalyzer.CryslBuilder;
using CodeSharpenerCryptoAnalyzer.CryslBuilder.Models.CSharpModels;
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
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.PointsToAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

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

        public const string HardCodedContextCheckDiagnosticId = "HardCodedContextKey";
        private static readonly LocalizableString HardCodedContextCheckTitle = new LocalizableResourceString(nameof(Resources.HardCodedContextTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString HardCodedContextCheckMessageFormat = new LocalizableResourceString(nameof(Resources.HardCodedContextMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString HardCodedContextCheckDescription = new LocalizableResourceString(nameof(Resources.HardCodedContextDescription), Resources.ResourceManager, typeof(Resources));
        private const string HardCodedContextCheckCategory = "Violation";
        private static DiagnosticDescriptor HardCodedContextCheckViolationRule = new DiagnosticDescriptor(HardCodedContextCheckDiagnosticId, HardCodedContextCheckTitle, HardCodedContextCheckMessageFormat, HardCodedContextCheckCategory, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: HardCodedContextCheckDescription);

        public const string DerivedTypeDiagnsoticId = "DerivedMethodInUse";
        private static readonly LocalizableString DerivedTypeTitle = new LocalizableResourceString(nameof(Resources.DerivedTypeTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DerivedTypeMessageFormat = new LocalizableResourceString(nameof(Resources.DerivedTypeMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DerivedTypeDescription = new LocalizableResourceString(nameof(Resources.DerivedTypeDescription), Resources.ResourceManager, typeof(Resources));
        private const string DerivedTypeCategory = "Warning";
        private static DiagnosticDescriptor DerivedTypeRule = new DiagnosticDescriptor(DerivedTypeDiagnsoticId, DerivedTypeTitle, DerivedTypeMessageFormat, DerivedTypeCategory, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: DerivedTypeDescription);
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(EventViolationRule, EventAggViolationRule, ConstraintAnalyzerViolationRule, OrderAnalyzerViolationRule, HardCodedCheckViolationRule, DerivedTypeRule, HardCodedContextCheckViolationRule); } }



        private static Dictionary<string, CryslJsonModel> _cryslSpecificationModel;
        private static ServiceProvider _serviceProvider { get; set; }

        private static Dictionary<string, List<AddConstraints>> AdditionalConstraintsDict;

        private static List<KeyValuePair<string, string>> EventOrderContraint;

        //Events Related Dictionary
        //Key: Event_Var_Name
        //Value: List of MethodSignatureModel
        private static Dictionary<string, Dictionary<string, List<MethodSignatureModel>>> ValidEventsDictionary;

        //Dictionary of all analyzed events
        private static Dictionary<string, Dictionary<string, List<KeyValuePair<string, string>>>> EventsOrderDictionary;

        private static List<KeyValuePair<ContextInformation, ISymbol>> TaintedValuesDictionary;

        private static Dictionary<string, List<KeyValuePair<ContextInformation, ISymbol>>> TaintedContextDictionary;

        private static Dictionary<string, List<CryslJsonModel>> ToAnalyzeCryslSection;

        private static List<CryslJsonModel> CryslSectionList;

        private bool IsCryslFilePresent;

        private bool IsTaintAnalysisOff;



        public CodeSharpenerCryptoAnalyzerAnalyzer()
        {
            var services = new ServiceCollection();
            services.AddTransient<ICommonUtilities, CommonUtilities>();
            services.AddTransient<IEventSectionAnalyzer, EventsSectionAnalyzer>();
            services.AddTransient<IConstraintsSectionAnalyzer, ConstraintsSectionAnalyzer>();
            services.AddTransient<IOrderSectionAnalyzer, OrderSectionAnalyzer>();
            services.AddTransient<ICryslConfigurationBuilder, CryslConfigurationBuilder>();
            services.AddSingleton<ICryslObjectBuilder, CryslObjectBuilder>();
            _serviceProvider = services.BuildServiceProvider();

            IsCryslFilePresent = false;
            IsTaintAnalysisOff = false;
        }

        public override void Initialize(AnalysisContext context)
        {
            //Register all the syntax nodes that needs to be analyzed
            context.RegisterCodeBlockStartAction<SyntaxKind>(AnalyzeCodeBlockAction);
            context.RegisterOperationBlockStartAction(AnalyzeUsingStartBlock);
            context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, SyntaxKind.ObjectCreationExpression);
            context.RegisterSyntaxNodeAction(AnalyzeMethodInvocationNode, SyntaxKind.InvocationExpression);
            context.RegisterSyntaxNodeAction(AnalyzeSimpleAssignmentExpression, SyntaxKind.SimpleAssignmentExpression);
            context.RegisterSyntaxNodeAction(AnalyzeLocalDeclarationStatement, SyntaxKind.LocalDeclarationStatement, SyntaxKind.FieldDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeMethodDeclarationNode, SyntaxKind.MethodDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeExpressionStatement, SyntaxKind.ExpressionStatement);
            context.RegisterCompilationStartAction(AnalyzeCompilationAction);


            //All global assignements to analyzer goes below           
            AdditionalConstraintsDict = new Dictionary<string, List<AddConstraints>>();
            ValidEventsDictionary = new Dictionary<string, Dictionary<string, List<MethodSignatureModel>>>();
            EventsOrderDictionary = new Dictionary<string, Dictionary<string, List<KeyValuePair<string, string>>>>();
            TaintedValuesDictionary = new List<KeyValuePair<ContextInformation, ISymbol>>();
            if (TaintedContextDictionary == null)
            {
                TaintedContextDictionary = new Dictionary<string, List<KeyValuePair<ContextInformation, ISymbol>>>();
            }
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
            List<KeyValuePair<ContextInformation, ISymbol>> taintedDictionary = new List<KeyValuePair<ContextInformation, ISymbol>>();
            TaintedContextDictionary.TryGetValue(context.ContainingSymbol.ToString(), out taintedDictionary);
            if (taintedDictionary != null)
            {
                lock (TaintedContextDictionary)
                {
                    TaintedContextDictionary.Remove(context.ContainingSymbol.ToString());
                }
            }
            ToAnalyzeCryslSection = new Dictionary<string, List<CryslJsonModel>>();
            CryslSectionList = new List<CryslJsonModel>();

            foreach (var taintedContextDictionary in TaintedContextDictionary)
            {
                foreach (var taintedValueDictionary in taintedContextDictionary.Value)
                {
                    if (taintedValueDictionary.Key.ContainingSymbolInfo.ToString().Equals(context.ContainingSymbol.ToString()))
                    {
                        TaintedValuesDictionary.Add(new KeyValuePair<ContextInformation, ISymbol>(taintedValueDictionary.Key, taintedValueDictionary.Value));
                    }
                }
            }
        }

        private void AnalyzeExpressionStatement(SyntaxNodeAnalysisContext context)
        {
            if (IsCryslFilePresent && !IsTaintAnalysisOff)
            {
                var expressionStatementNode = (ExpressionStatementSyntax)context.Node;
                ExpressionStatementVisitor expressionStatementVisitor = new ExpressionStatementVisitor();
                expressionStatementVisitor.VisitExpressionStatement(expressionStatementNode);
                var StringLiteralPresentResult = expressionStatementVisitor.GetAssignmentExpressionResult();

                if (StringLiteralPresentResult.IsStringLiteralInitializer && StringLiteralPresentResult.ExpressionSyntax.Left != null)
                {
                    var leftExpressionSymbolInfo = context.SemanticModel.GetSymbolInfo(StringLiteralPresentResult.ExpressionSyntax.Left).Symbol;

                    //In order to make object sensitive, identifer symbol info should be retrieved
                    var identifierLeftExprNodes = StringLiteralPresentResult.ExpressionSyntax.Left.ChildNodes().OfType<IdentifierNameSyntax>();
                    ISymbol identifierLeftExprSymbolInfo = null;
                    if (identifierLeftExprNodes.Count() > 0)
                    {
                        identifierLeftExprSymbolInfo = context.SemanticModel.GetSymbolInfo(identifierLeftExprNodes.FirstOrDefault()).Symbol;
                    }

                    var TaintedInformation = IsTaintedValueExists(leftExpressionSymbolInfo.ContainingSymbol, leftExpressionSymbolInfo, identifierLeftExprSymbolInfo);
                    if (!TaintedInformation.IsTainted)
                    {
                        ContextInformation contextInformation = new ContextInformation
                        {
                            ContainingSymbolInfo = leftExpressionSymbolInfo.ContainingSymbol,
                            ContainingObjectSymbolInfo = identifierLeftExprSymbolInfo
                        };
                        TaintedValuesDictionary.Add(new KeyValuePair<ContextInformation, ISymbol>(contextInformation, leftExpressionSymbolInfo));
                    }
                    var diagnostics = Diagnostic.Create(HardCodedCheckViolationRule, StringLiteralPresentResult.ExpressionSyntax.GetLocation());
                    context.ReportDiagnostic(diagnostics);
                }

                var simpleAssignmentExpression = expressionStatementNode.ChildNodes().OfType<AssignmentExpressionSyntax>();
                foreach (var assignmentExpression in simpleAssignmentExpression)
                {
                    if (assignmentExpression.IsKind(SyntaxKind.SimpleAssignmentExpression) && assignmentExpression.Left != null && assignmentExpression.Right != null)
                    {
                        var rightSymbolInfo = context.SemanticModel.GetSymbolInfo(assignmentExpression.Right);

                        //In order to make object sensitive, identifer symbol info should be retrieved
                        var identifierRightExprNodes = assignmentExpression.Right.ChildNodes().OfType<IdentifierNameSyntax>();
                        ISymbol identifierRightExprSymbolInfo = null;
                        if (identifierRightExprNodes.Count() > 0)
                        {
                            identifierRightExprSymbolInfo = context.SemanticModel.GetSymbolInfo(identifierRightExprNodes.FirstOrDefault()).Symbol;
                        }

                        if (rightSymbolInfo.Symbol != null)
                        {
                            ISymbol identifierLeftExprSymbolInfo = null;
                            var taintedRightSymbolInfo = IsTaintedValueExists(rightSymbolInfo.Symbol.ContainingSymbol, rightSymbolInfo.Symbol, identifierRightExprSymbolInfo);
                            if (taintedRightSymbolInfo.IsTainted)
                            {
                                var leftSymbolInfo = context.SemanticModel.GetSymbolInfo(assignmentExpression.Left);

                                //In order to make object sensitive, identifier symbol info should be retrieved
                                var identifierLeftExprNodes = assignmentExpression.Left.ChildNodes().OfType<IdentifierNameSyntax>();

                                if (identifierLeftExprNodes.Count() > 0)
                                {
                                    identifierLeftExprSymbolInfo = context.SemanticModel.GetSymbolInfo(identifierLeftExprNodes.FirstOrDefault()).Symbol;
                                }
                                if (leftSymbolInfo.Symbol != null)
                                {
                                    var taintedLeftSymbolInfo = IsTaintedValueExists(leftSymbolInfo.Symbol.ContainingSymbol, leftSymbolInfo.Symbol, identifierLeftExprSymbolInfo);
                                    if (!taintedLeftSymbolInfo.IsTainted)
                                    {
                                        lock (TaintedValuesDictionary)
                                        {
                                            ContextInformation contextInformation = new ContextInformation
                                            {
                                                ContainingSymbolInfo = leftSymbolInfo.Symbol.ContainingSymbol,
                                                ContainingObjectSymbolInfo = identifierLeftExprSymbolInfo
                                            };
                                            TaintedValuesDictionary.Add(new KeyValuePair<ContextInformation, ISymbol>(contextInformation, leftSymbolInfo.Symbol));
                                        }
                                    }
                                }
                                ReportDiagnostics(context, HardCodedContextCheckViolationRule, HardCodedCheckViolationRule, expressionStatementNode.GetLocation(), taintedRightSymbolInfo.TaintedContextInformation);
                                /*var diagnostics = (taintedRightSymbolInfo.TaintedContextInformation.CallerSymbolInfo != null) ? Diagnostic.Create(HardCodedContextCheckViolationRule, expressionStatementNode.GetLocation(), taintedRightSymbolInfo.TaintedContextInformation.CallerSymbolInfo.ToString()) : Diagnostic.Create(HardCodedCheckViolationRule, expressionStatementNode.GetLocation());
                                context.ReportDiagnostic(diagnostics);*/
                            }
                            else
                            {
                                var taintedRightInfo = IsTaintedValueExists(rightSymbolInfo.Symbol.ContainingSymbol, rightSymbolInfo.Symbol, identifierRightExprSymbolInfo);
                                if (!taintedRightInfo.IsTainted)
                                {
                                    var leftSymbolInfo = context.SemanticModel.GetSymbolInfo(assignmentExpression.Left);

                                    if (leftSymbolInfo.Symbol != null)
                                    {
                                        //In order to make object sensitive, identifier symbol info should be retrieved
                                        var identifierLeftExprNodes = assignmentExpression.Left.ChildNodes().OfType<IdentifierNameSyntax>();

                                        if (identifierLeftExprNodes.Count() > 0)
                                        {
                                            identifierLeftExprSymbolInfo = context.SemanticModel.GetSymbolInfo(identifierLeftExprNodes.FirstOrDefault()).Symbol;
                                        }
                                        SanitizeTaintValue(leftSymbolInfo.Symbol.ContainingSymbol, leftSymbolInfo.Symbol, identifierLeftExprSymbolInfo);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Report HardCoded Values
        /// </summary>
        /// <param name="context"></param>
        private void AnalyzeLocalDeclarationStatement(SyntaxNodeAnalysisContext context)
        {
            if (IsCryslFilePresent && !IsTaintAnalysisOff)
            {
                var localDeclarationStatement = context.Node;
                LocalDeclarationStatementVisitor localDeclarationStatementVisitor = new LocalDeclarationStatementVisitor();
                localDeclarationStatementVisitor.Visit(localDeclarationStatement);

                var isIdentifierNameNode = localDeclarationStatementVisitor.GetIdentifierNameSyntaxResult();
                if (isIdentifierNameNode.IsIdentifierPresent)
                {
                    var identifierSymbolInfo = context.SemanticModel.GetSymbolInfo(isIdentifierNameNode.IdentifierNameSyntaxNode).Symbol;
                    if (identifierSymbolInfo != null && identifierSymbolInfo.ContainingSymbol != null)
                    {
                        var taintedIdentifierSymbolInfo = IsTaintedValueExists(identifierSymbolInfo.ContainingSymbol, identifierSymbolInfo);
                        if (taintedIdentifierSymbolInfo.IsTainted)
                        {
                            var declaratorSymbolInfo = context.SemanticModel.GetDeclaredSymbol(isIdentifierNameNode.VariableDeclarator);
                            var taintedDeclaratorSymbolInfo = IsTaintedValueExists(declaratorSymbolInfo.ContainingSymbol, declaratorSymbolInfo);
                            if (!taintedDeclaratorSymbolInfo.IsTainted)
                            {
                                lock (TaintedValuesDictionary)
                                {
                                    ContextInformation contextInformation = new ContextInformation
                                    {
                                        ContainingSymbolInfo = declaratorSymbolInfo.ContainingSymbol
                                    };
                                    TaintedValuesDictionary.Add(new KeyValuePair<ContextInformation, ISymbol>(contextInformation, declaratorSymbolInfo));
                                }
                            }
                            ReportDiagnostics(context, HardCodedContextCheckViolationRule, HardCodedCheckViolationRule, localDeclarationStatement.GetLocation(), taintedIdentifierSymbolInfo.TaintedContextInformation);
                            /*var diagnostics = (taintedIdentifierSymbolInfo.TaintedContextInformation.CallerSymbolInfo != null) ? Diagnostic.Create(HardCodedContextCheckViolationRule, localDeclarationStatement.GetLocation(), taintedIdentifierSymbolInfo.TaintedContextInformation.CallerSymbolInfo.ToString()) : Diagnostic.Create(HardCodedCheckViolationRule, localDeclarationStatement.GetLocation());
                            context.ReportDiagnostic(diagnostics);*/
                        }
                    }
                }

                var isArrayInitializerPresent = localDeclarationStatementVisitor.GetByteArrayResult();
                if (isArrayInitializerPresent.IsArrayInitializer)
                {
                    //Adding to Tainted Dictionary for all Byte ArrayInitializer Types
                    var nodeSymbolInfo = context.SemanticModel.GetDeclaredSymbol(isArrayInitializerPresent.DeclaratorSyntax);
                    var taintedNodeSymbolInfo = IsTaintedValueExists(nodeSymbolInfo.ContainingSymbol, nodeSymbolInfo, null);
                    if (!taintedNodeSymbolInfo.IsTainted)
                    {
                        lock (TaintedValuesDictionary)
                        {
                            ContextInformation contextInformation = new ContextInformation
                            {
                                ContainingSymbolInfo = nodeSymbolInfo.ContainingSymbol
                            };
                            TaintedValuesDictionary.Add(new KeyValuePair<ContextInformation, ISymbol>(contextInformation, nodeSymbolInfo));
                        }
                    }
                    if (!localDeclarationStatement.Kind().Equals(SyntaxKind.FieldDeclaration))
                    {
                        var dataFlowAnalysisResult = context.SemanticModel.AnalyzeDataFlow(localDeclarationStatement);
                        if (dataFlowAnalysisResult.ReadOutside.Contains(nodeSymbolInfo))
                        {
                            var diagnsotics = Diagnostic.Create(HardCodedCheckViolationRule, localDeclarationStatement.GetLocation());
                            context.ReportDiagnostic(diagnsotics);
                        }
                    }
                }

                var isStringInitializerPresent = localDeclarationStatementVisitor.GetStringLiteralResult();
                if (isStringInitializerPresent.IsStringLiteralInitializer)
                {
                    //Adding to Tainted Dictionary for all Byte ArrayInitializer Types
                    var nodeSymbolInfo = context.SemanticModel.GetDeclaredSymbol(isStringInitializerPresent.DeclaratorSyntax);
                    var taintedNodeSymbolInfo = IsTaintedValueExists(nodeSymbolInfo.ContainingSymbol, nodeSymbolInfo);
                    if (!taintedNodeSymbolInfo.IsTainted)
                    {
                        lock (TaintedValuesDictionary)
                        {
                            ContextInformation contextInformation = new ContextInformation
                            {
                                ContainingSymbolInfo = nodeSymbolInfo.ContainingSymbol
                            };
                            TaintedValuesDictionary.Add(new KeyValuePair<ContextInformation, ISymbol>(contextInformation, nodeSymbolInfo));
                        }
                    }

                    if (!localDeclarationStatement.Kind().Equals(SyntaxKind.FieldDeclaration))
                    {
                        var dataFlowAnalysisResult = context.SemanticModel.AnalyzeDataFlow(localDeclarationStatement);
                        if (dataFlowAnalysisResult.ReadOutside.Contains(nodeSymbolInfo))
                        {
                            var diagnsotics = Diagnostic.Create(HardCodedCheckViolationRule, localDeclarationStatement.GetLocation());
                            context.ReportDiagnostic(diagnsotics);
                        }
                    }
                }
            }
        }

        private void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context)
        {
            var objectCreationNode = context.Node;
            var identifierNode = objectCreationNode.ChildNodes().OfType<IdentifierNameSyntax>();
            ICommonUtilities commonUtilities = _serviceProvider.GetService<ICommonUtilities>();
            var argumentsList = objectCreationNode.ChildNodes().OfType<ArgumentListSyntax>();

            //Check for tainted string arguments
            foreach (var argumentListSyntax in argumentsList)
            {
                var argumentSyntaxList = argumentListSyntax.Arguments;
                if (argumentSyntaxList != null)
                {
                    foreach (var arguments in argumentSyntaxList)
                    {
                        var identifierArgumentNode = arguments.ChildNodes().OfType<IdentifierNameSyntax>();
                        if (identifierArgumentNode.Count() != 0)
                        {
                            var identifierSymbolInfo = context.SemanticModel.GetSymbolInfo(identifierArgumentNode.FirstOrDefault()).Symbol;
                            if (identifierSymbolInfo != null)
                            {
                                var taintedIdentifierSymbolInfo = IsTaintedValueExists(identifierSymbolInfo.ContainingSymbol, identifierSymbolInfo);
                                if (taintedIdentifierSymbolInfo.IsTainted)
                                {
                                    var declaratorSyntaxNode = objectCreationNode.AncestorsAndSelf().OfType<VariableDeclaratorSyntax>();
                                    if (declaratorSyntaxNode.Count() != 0)
                                    {
                                        var declaratorSymbolInfo = context.SemanticModel.GetDeclaredSymbol(declaratorSyntaxNode.FirstOrDefault());
                                        var taintedDeclaratorSymbolInfo = IsTaintedValueExists(declaratorSymbolInfo.ContainingSymbol, declaratorSymbolInfo);
                                        if (!taintedDeclaratorSymbolInfo.IsTainted)
                                        {
                                            lock (TaintedValuesDictionary)
                                            {
                                                ContextInformation contextInformation = new ContextInformation
                                                {
                                                    ContainingSymbolInfo = declaratorSymbolInfo.ContainingSymbol
                                                };
                                                TaintedValuesDictionary.Add(new KeyValuePair<ContextInformation, ISymbol>(contextInformation, declaratorSymbolInfo));
                                            }
                                        }
                                    }
                                    ReportDiagnostics(context, HardCodedContextCheckViolationRule, HardCodedCheckViolationRule, arguments.GetLocation(), taintedIdentifierSymbolInfo.TaintedContextInformation);
                                    /*var diagnostics = (taintedIdentifierSymbolInfo.TaintedContextInformation.CallerSymbolInfo != null) ? Diagnostic.Create(HardCodedContextCheckViolationRule, arguments.GetLocation(), taintedIdentifierSymbolInfo.TaintedContextInformation.CallerSymbolInfo.ToString()) : Diagnostic.Create(HardCodedCheckViolationRule, arguments.GetLocation());
                                    context.ReportDiagnostic(diagnostics);*/
                                }
                            }
                        }
                    }
                }
            }

            if (identifierNode.Count() > 0)
            {
                var instantiatedMethodIdentifier = identifierNode.FirstOrDefault();
                string instantiatedMethodType = commonUtilities.GetInvocatorType(context.SemanticModel.GetSymbolInfo(instantiatedMethodIdentifier).Symbol);

                foreach (var cryslSpecificationModel in _cryslSpecificationModel.Values)
                {
                    if (instantiatedMethodType.Equals(cryslSpecificationModel.Spec_Section.Class_Name))
                    {
                        if (!ToAnalyzeCryslSection.ContainsKey(context.ContainingSymbol.ToString()))
                        {
                            CryslSectionList.Add(cryslSpecificationModel);
                            ToAnalyzeCryslSection.Add(context.ContainingSymbol.ToString(), CryslSectionList);
                        }
                        else if (!CryslSectionList.Contains(cryslSpecificationModel))
                        {
                            CryslSectionList.Add(cryslSpecificationModel);
                            ToAnalyzeCryslSection[context.ContainingSymbol.ToString()] = CryslSectionList;

                        }
                        foreach (var methods in cryslSpecificationModel.Event_Section.Methods)
                        {
                            // Check if method signature matches with the method signature defined in events section of the Crysl.
                            var cryptoMethods = methods.Crypto_Signature.Select(y => y).Where(x => x.Method_Name.ToString().Equals(instantiatedMethodIdentifier.Identifier.Value.ToString()));
                            if (cryptoMethods.Count() > 0)
                            {
                                IEventSectionAnalyzer analyzer = _serviceProvider.GetService<IEventSectionAnalyzer>();
                                var identifierSymbolInfo = (IMethodSymbol)context.SemanticModel.GetSymbolInfo(objectCreationNode).Symbol;

                                //Check for Valid Events
                                ValidEvents validEvents = analyzer.AnalyzeMemAccessExprSyntax(instantiatedMethodIdentifier, cryptoMethods, methods, cryslSpecificationModel, context, identifierSymbolInfo, objectCreationNode.Span, instantiatedMethodType);

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
                                                        var taintedArgumentSymbolInfo = IsTaintedValueExists(argumentSymbolInfo.Symbol.ContainingSymbol, argumentSymbolInfo.Symbol);
                                                        if (taintedArgumentSymbolInfo.IsTainted)
                                                        {
                                                            ReportDiagnostics(context, HardCodedContextCheckViolationRule, HardCodedCheckViolationRule, arguments.GetLocation(), taintedArgumentSymbolInfo.TaintedContextInformation);
                                                            /*var diagnsotics = (taintedArgumentSymbolInfo.TaintedContextInformation.CallerSymbolInfo != null) ? Diagnostic.Create(HardCodedContextCheckViolationRule, arguments.GetLocation(), taintedArgumentSymbolInfo.TaintedContextInformation.CallerSymbolInfo.ToString()) : Diagnostic.Create(HardCodedCheckViolationRule, arguments.GetLocation());
                                                            context.ReportDiagnostic(diagnsotics);*/
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
                                        AddEventsToDictionary(methods.Aggregator.Aggregator_Name, validEvents, cryptoMethods.FirstOrDefault().Method_Name, context.ContainingSymbol.ToString(), cryslSpecificationModel.Spec_Section.Class_Name);

                                        Dictionary<string, List<MethodSignatureModel>> validEventsDictionary = new Dictionary<string, List<MethodSignatureModel>>();
                                        ValidEventsDictionary.TryGetValue(cryslSpecificationModel.Spec_Section.Class_Name, out validEventsDictionary);

                                        bool isAggConditionSatisfied = commonUtilities.CheckAggregator(validEventsDictionary, methods.Aggregator.Aggregators);
                                        if (!isAggConditionSatisfied)
                                        {
                                            var diagnsotics = Diagnostic.Create(EventAggViolationRule, objectCreationNode.GetLocation(), cryslSpecificationModel.Spec_Section.Class_Name, validEvents.PropertyName);
                                            context.ReportDiagnostic(diagnsotics);
                                        }
                                    }
                                    //Add single event to dictionary if aggregator not present.
                                    else
                                    {
                                        AddEventsToDictionary(validEvents.PropertyName, validEvents, cryptoMethods.FirstOrDefault().Method_Name, context.ContainingSymbol.ToString(), cryslSpecificationModel.Spec_Section.Class_Name);
                                    }

                                    //Iterate throigh parameters if present and check for constraints only if arguments are present
                                    if (validEvents.ValidMethods.Parameters.Count > 0)
                                    {
                                        //Check for valid constraints
                                        IConstraintsSectionAnalyzer constraintsSectionAnalyzer = _serviceProvider.GetService<IConstraintsSectionAnalyzer>();
                                        List<ConstraintsModel> satisfiedConstraintsList = new List<ConstraintsModel>();

                                        satisfiedConstraintsList = constraintsSectionAnalyzer.AnalyzeParameters(argumentsList, validEvents.ValidMethods.Parameters, cryslSpecificationModel.Constraints_Section.Constraints, context);
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
                                            lock (addConstraintsList)
                                            {
                                                addConstraintsList.Add(additionalConstraints);
                                            }
                                            lock (AdditionalConstraintsDict)
                                            {
                                                AdditionalConstraintsDict.Add(identifierSymbolInfo.ReturnType.ToString(), addConstraintsList);
                                            }
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
                                            lock (additionalConstraintsCheck)
                                            {
                                                additionalConstraintsCheck.Add(additionalConstraints);
                                            }
                                            lock (AdditionalConstraintsDict)
                                            {
                                                AdditionalConstraintsDict[identifierSymbolInfo.ReturnType.ToString()] = additionalConstraintsCheck;
                                            }

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
                    var declaratorNode = identifierNode.FirstOrDefault().AncestorsAndSelf().OfType<VariableDeclaratorSyntax>();
                    ISymbol declaratorSymbolInfo = null;
                    if (declaratorNode.Count() != 0)
                    {
                        declaratorSymbolInfo = context.SemanticModel.GetDeclaredSymbol(declaratorNode.FirstOrDefault());
                    }

                    //Check for any tainted invocations
                    var invocatorIdentifierSymbolInfo = context.SemanticModel.GetSymbolInfo(identifierNode.FirstOrDefault()).Symbol;
                    if (invocatorIdentifierSymbolInfo != null)
                    {
                        var taintedInvocatorIdentifierSymbolInfo = IsTaintedValueExists(invocatorIdentifierSymbolInfo.ContainingSymbol, invocatorIdentifierSymbolInfo);
                        if (taintedInvocatorIdentifierSymbolInfo.IsTainted)
                        {
                            if (!taintedInvocatorIdentifierSymbolInfo.IsTainted)
                            {
                                lock (TaintedValuesDictionary)
                                {
                                    ContextInformation contextInformation = new ContextInformation
                                    {
                                        ContainingSymbolInfo = invocatorIdentifierSymbolInfo.ContainingSymbol
                                    };
                                    TaintedValuesDictionary.Add(new KeyValuePair<ContextInformation, ISymbol>(contextInformation, invocatorIdentifierSymbolInfo));
                                }
                            }
                            if (declaratorSymbolInfo != null)
                            {
                                var taintedDeclaratorSymbolInfo = IsTaintedValueExists(declaratorSymbolInfo.ContainingSymbol, declaratorSymbolInfo);
                                if (!taintedDeclaratorSymbolInfo.IsTainted)
                                {
                                    lock (TaintedValuesDictionary)
                                    {
                                        ContextInformation contextInformation = new ContextInformation
                                        {
                                            CallerSymbolInfo = context.ContainingSymbol
                                        };
                                        TaintedValuesDictionary.Add(new KeyValuePair<ContextInformation, ISymbol>(contextInformation, declaratorSymbolInfo));
                                    }
                                }
                            }
                            ReportDiagnostics(context, HardCodedContextCheckViolationRule, HardCodedCheckViolationRule, node.GetLocation(), taintedInvocatorIdentifierSymbolInfo.TaintedContextInformation);
                            /*var taintedDiagnsotics = (taintedInvocatorIdentifierSymbolInfo.TaintedContextInformation.CallerSymbolInfo != null) ? Diagnostic.Create(HardCodedContextCheckViolationRule, node.GetLocation(), taintedInvocatorIdentifierSymbolInfo.TaintedContextInformation.CallerSymbolInfo.ToString()) : Diagnostic.Create(HardCodedCheckViolationRule, node.GetLocation());
                            context.ReportDiagnostic(taintedDiagnsotics);*/
                        }
                    }

                    var invocatorIdentifier = identifierNode.FirstOrDefault();
                    var invokedMethod = identifierNode.LastOrDefault();
                    string invocatorType = commonUtilities.GetInvocatorType(context.SemanticModel.GetSymbolInfo(invocatorIdentifier).Symbol);
                    foreach (var cryslSpecificationModel in _cryslSpecificationModel.Values)
                    {
                        if (invocatorType.Equals(cryslSpecificationModel.Spec_Section.Class_Name))
                        {
                            if (!ToAnalyzeCryslSection.ContainsKey(context.ContainingSymbol.ToString()))
                            {
                                CryslSectionList.Add(cryslSpecificationModel);
                                ToAnalyzeCryslSection.Add(context.ContainingSymbol.ToString(), CryslSectionList);
                            }
                            else if (!CryslSectionList.Contains(cryslSpecificationModel))
                            {
                                CryslSectionList.Add(cryslSpecificationModel);
                                ToAnalyzeCryslSection[context.ContainingSymbol.ToString()] = CryslSectionList;
                            }

                            var result = cryslSpecificationModel.Event_Section.Methods.Select(x => x.Crypto_Signature
                             .Where(y => y.Method_Name.ToString().Equals(invokedMethod.Identifier.Value.ToString())));
                            foreach (var methods in cryslSpecificationModel.Event_Section.Methods)
                            {

                                // Check if method signature matches with the method signature defined in events section of the Crysl.
                                var cryptoMethods = methods.Crypto_Signature.Select(y => y).Where(x => x.Method_Name.ToString().Equals(invokedMethod.Identifier.Value.ToString()));
                                if (cryptoMethods.Count() > 0)
                                {
                                    IEventSectionAnalyzer analyzer = _serviceProvider.GetService<IEventSectionAnalyzer>();
                                    var identifierSymbolInfo = (IMethodSymbol)context.SemanticModel.GetSymbolInfo(invokedMethod).Symbol;

                                    //Check for Valid Events
                                    ValidEvents validEvents = analyzer.AnalyzeMemAccessExprSyntax(invokedMethod, cryptoMethods, methods, cryslSpecificationModel, context, identifierSymbolInfo, node.Span, invocatorType);

                                    if (validEvents.IsValidEvent)
                                    {
                                        //TODO: Check this implementation
                                        //Check if any argument value is tainted
                                        /*ArgumentsVisitor argumentsVisitor = new ArgumentsVisitor();
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
                                        }*/

                                        //Check only if Aggregators are present
                                        if (methods.Aggregator != null)
                                        {
                                            //Add valid events to Events and Order Dictionary
                                            AddEventsToDictionary(methods.Aggregator.Aggregator_Name, validEvents, cryptoMethods.FirstOrDefault().Method_Name, context.ContainingSymbol.ToString(), cryslSpecificationModel.Spec_Section.Class_Name);

                                            Dictionary<string, List<MethodSignatureModel>> validEventsDictionary = new Dictionary<string, List<MethodSignatureModel>>();
                                            ValidEventsDictionary.TryGetValue(cryslSpecificationModel.Spec_Section.Class_Name, out validEventsDictionary);

                                            bool isAggConditionSatisfied = commonUtilities.CheckAggregator(validEventsDictionary, methods.Aggregator.Aggregators);
                                            if (!isAggConditionSatisfied)
                                            {
                                                var diagnsotics = Diagnostic.Create(EventAggViolationRule, node.GetLocation(), cryslSpecificationModel.Spec_Section.Class_Name, validEvents.ValidMethods.MethodName);
                                                context.ReportDiagnostic(diagnsotics);
                                            }
                                        }
                                        //Add single event to dictionary if aggregator not present.
                                        else
                                        {
                                            AddEventsToDictionary(validEvents.PropertyName, validEvents, cryptoMethods.FirstOrDefault().Method_Name, context.ContainingSymbol.ToString(), cryslSpecificationModel.Spec_Section.Class_Name);
                                        }

                                        //Iterate throigh parameters if present and check for constraints only if arguments are present
                                        if (validEvents.ValidMethods.Parameters.Count > 0)
                                        {
                                            //Check for valid constraints
                                            IConstraintsSectionAnalyzer constraintsSectionAnalyzer = _serviceProvider.GetService<IConstraintsSectionAnalyzer>();
                                            List<ConstraintsModel> satisfiedConstraintsList = new List<ConstraintsModel>();

                                            satisfiedConstraintsList = constraintsSectionAnalyzer.AnalyzeParameters(argumentsList, validEvents.ValidMethods.Parameters, cryslSpecificationModel.Constraints_Section.Constraints, context);
                                            foreach (var constraints in satisfiedConstraintsList)
                                            {
                                                if (!constraints.IsConstraintSatisfied)
                                                {
                                                    var accepetedParameterValues = cryslSpecificationModel.Constraints_Section.Constraints.Where(x => x.Object_Varname.Equals(validEvents.ValidMethods.Parameters.FirstOrDefault().Argument) && x.Additional_constraints == null).Select(y => y.Constraints_List);
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
                                                    lock (addConstraintsList)
                                                    {
                                                        addConstraintsList.Add(additionalConstraints);
                                                    }
                                                    lock (AdditionalConstraintsDict)
                                                    {
                                                        AdditionalConstraintsDict.Add(identifierSymbolInfo.ReturnType.ToString(), addConstraintsList);
                                                    }
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
                                                    lock (additionalConstraintsCheck)
                                                    {
                                                        additionalConstraintsCheck.Add(additionalConstraints);
                                                    }
                                                    lock (AdditionalConstraintsDict)
                                                    {
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
                        else if (Type.GetType(invocatorType) != null)
                        {
                            if (Type.GetType(invocatorType).BaseType != null)
                            {
                                if (Type.GetType(invocatorType).BaseType.FullName.Equals(cryslSpecificationModel.Spec_Section.Class_Name) && !Type.GetType(invocatorType).FullName.Equals("System.Security.Cryptography.KeyedHashAlgorithm"))
                                {
                                    var diagnsotics = Diagnostic.Create(DerivedTypeRule, invocationExpressionNode.GetLocation(), invocatorType, cryslSpecificationModel.Spec_Section.Class_Name);
                                    context.ReportDiagnostic(diagnsotics);
                                }
                            }
                        }
                    }

                }
            }
            //Check for callee's tainted value
            var invExprSymbolInfo = context.SemanticModel.GetSymbolInfo(invocationExpressionNode).Symbol as IMethodSymbol;
            foreach (var arguments in argumentsList)
            {
                for (int i = 0; i < arguments.Arguments.Count; i++)
                {
                    var simpleMemAccessExpr = arguments.Arguments[i].ChildNodes().OfType<MemberAccessExpressionSyntax>();
                    //Callee's tainted value containing simple access expressions
                    if (simpleMemAccessExpr.Count() > 0)
                    {
                        foreach (var memAccessExpr in simpleMemAccessExpr)
                        {
                            var argumentSymbol = context.SemanticModel.GetSymbolInfo(memAccessExpr).Symbol;
                            if (argumentSymbol != null)
                            {
                                var taintedArgumentSymbolInfo = IsTaintedValueExists(argumentSymbol.ContainingSymbol, argumentSymbol);
                                if (taintedArgumentSymbolInfo.IsTainted)
                                {
                                    ReportDiagnostics(context, HardCodedContextCheckViolationRule, HardCodedCheckViolationRule, memAccessExpr.GetLocation(), taintedArgumentSymbolInfo.TaintedContextInformation);
                                    /*var diagnsotics = (taintedArgumentSymbolInfo.TaintedContextInformation.CallerSymbolInfo != null) ? Diagnostic.Create(HardCodedContextCheckViolationRule, memAccessExpr.GetLocation(), taintedArgumentSymbolInfo.TaintedContextInformation.CallerSymbolInfo.ToString()) : Diagnostic.Create(HardCodedCheckViolationRule, memAccessExpr.GetLocation());
                                    context.ReportDiagnostic(diagnsotics);*/
                                    if (invExprSymbolInfo != null)
                                    {
                                        var taintedInvExprSymbolInfo = IsTaintedValueExists(invExprSymbolInfo, invExprSymbolInfo.Parameters[i]);
                                        if (!taintedInvExprSymbolInfo.IsTainted)
                                        {
                                            lock (TaintedValuesDictionary)
                                            {
                                                ContextInformation contextInformation = new ContextInformation
                                                {
                                                    ContainingSymbolInfo = invExprSymbolInfo,
                                                    CallerSymbolInfo = context.ContainingSymbol
                                                };
                                                TaintedValuesDictionary.Add(new KeyValuePair<ContextInformation, ISymbol>(contextInformation, invExprSymbolInfo.Parameters[i]));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //callee's tainted value containing identifer symbols
                    else
                    {
                        var identifierNameSyntax = arguments.Arguments[i].ChildNodes().OfType<IdentifierNameSyntax>();
                        foreach (var identifierNameNode in identifierNameSyntax)
                        {
                            var identifierSymbolInfo = context.SemanticModel.GetSymbolInfo(identifierNameNode).Symbol;
                            if (identifierSymbolInfo != null)
                            {
                                var taintedIdentifierSymbolInfo = IsTaintedValueExists(identifierSymbolInfo.ContainingSymbol, identifierSymbolInfo);
                                if (taintedIdentifierSymbolInfo.IsTainted)
                                {
                                    var variableDeclaratorResult = GetVariableDeclarator(invocationExpressionNode, context);
                                    // Taint Variable Declarator only for methods containing inside "System" namespace and not for any user defined methods.
                                    if (invExprSymbolInfo != null)
                                    {
                                        if (variableDeclaratorResult.IsVariableDeclaratorSyntaxPresent && invExprSymbolInfo.Name.Equals("FromBase64String"))
                                        {
                                            var diagnsotics = Diagnostic.Create(HardCodedCheckViolationRule, variableDeclaratorResult.VariableDeclaratorSyntaxNode.GetLocation());
                                            context.ReportDiagnostic(diagnsotics);
                                            var taintedVariableDeclaratorResult = IsTaintedValueExists(variableDeclaratorResult.VariableDeclaratorSymbolInfo.ContainingSymbol, variableDeclaratorResult.VariableDeclaratorSymbolInfo);
                                            if (!taintedVariableDeclaratorResult.IsTainted)
                                            {
                                                lock (TaintedValuesDictionary)
                                                {
                                                    ContextInformation contextInformation = new ContextInformation
                                                    {
                                                        ContainingSymbolInfo = variableDeclaratorResult.VariableDeclaratorSymbolInfo.ContainingSymbol,
                                                    };
                                                    TaintedValuesDictionary.Add(new KeyValuePair<ContextInformation, ISymbol>(contextInformation, variableDeclaratorResult.VariableDeclaratorSymbolInfo));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ReportDiagnostics(context, HardCodedContextCheckViolationRule, HardCodedCheckViolationRule, identifierNameNode.GetLocation(), taintedIdentifierSymbolInfo.TaintedContextInformation);
                                            /*var diagnsotics = (taintedIdentifierSymbolInfo.TaintedContextInformation.CallerSymbolInfo != null) ? Diagnostic.Create(HardCodedContextCheckViolationRule, identifierNameNode.GetLocation(), taintedIdentifierSymbolInfo.TaintedContextInformation.CallerSymbolInfo.ToString()) : Diagnostic.Create(HardCodedCheckViolationRule, identifierNameNode.GetLocation());
                                            context.ReportDiagnostic(diagnsotics);*/
                                        }
                                    }
                                    //Condition to check callee's tainted symbol info.
                                    if (invExprSymbolInfo != null)
                                    {
                                        var taintedInvExprSymbolInfo = IsTaintedValueExists(invExprSymbolInfo, invExprSymbolInfo.Parameters[i]);
                                        if (!taintedInvExprSymbolInfo.IsTainted)
                                        {
                                            lock (TaintedValuesDictionary)
                                            {
                                                ContextInformation contextInformation = new ContextInformation
                                                {
                                                    ContainingSymbolInfo = invExprSymbolInfo,
                                                    CallerSymbolInfo = context.ContainingSymbol
                                                };
                                                TaintedValuesDictionary.Add(new KeyValuePair<ContextInformation, ISymbol>(contextInformation, invExprSymbolInfo.Parameters[i]));
                                            }
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
                            /*if (rightExprSymbolInfo != null && leftExprSymbolInfo != null)
                            {
                                if (IsTaintedValueExists(rightExprSymbolInfo.ContainingSymbol, rightExprSymbolInfo))
                                {
                                    if (!IsTaintedValueExists(leftExprSymbolInfo.ContainingSymbol, leftExprSymbolInfo))
                                    {
                                        lock (TaintedValuesDictionary)
                                        {
                                            TaintedValuesDictionary.Add(new KeyValuePair<ISymbol, ISymbol>(leftExprSymbolInfo.ContainingSymbol, leftExprSymbolInfo));
                                        }
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
                            }*/
                            //Analze only if type is of SPEC type
                            foreach (var cryslSpecificationModel in _cryslSpecificationModel.Values)
                            {
                                if (localInvocatorSymbolInfo.Type.ToString().Equals(cryslSpecificationModel.Spec_Section.Class_Name))
                                {
                                    if (!ToAnalyzeCryslSection.ContainsKey(context.ContainingSymbol.ToString()))
                                    {
                                        CryslSectionList.Add(cryslSpecificationModel);
                                        ToAnalyzeCryslSection.Add(context.ContainingSymbol.ToString(), CryslSectionList);
                                    }
                                    else if (!CryslSectionList.Contains(cryslSpecificationModel))
                                    {
                                        CryslSectionList.Add(cryslSpecificationModel);
                                        ToAnalyzeCryslSection[context.ContainingSymbol.ToString()] = CryslSectionList;
                                    }

                                    var assignExprDataFlow = context.SemanticModel.AnalyzeDataFlow(simpleAssignExpr);
                                    bool isValidEvent = false;
                                    foreach (var methods in cryslSpecificationModel.Event_Section.Methods)
                                    {
                                        if (leftExprSymbolInfo != null)
                                        {
                                            var cryptoMethods = methods.Crypto_Signature.Select(x => x).Where(y => y.Method_Name.ToString().Equals(leftExprSymbolInfo.Name.ToString()));
                                            if (cryptoMethods.Count() > 0)
                                            {
                                                //Set the flag to true as a valid event
                                                IEventSectionAnalyzer analyzer = _serviceProvider.GetService<IEventSectionAnalyzer>();
                                                var validEvents = analyzer.AnalyzeAssignmentExprSyntax(cryptoMethods, cryslSpecificationModel, context, leftExprSymbolInfo);
                                                isValidEvent = validEvents.IsValidEvent;

                                                //Add valid events to Events and Order Dictionary
                                                AddEventsToDictionary(validEvents.AggregatorName, validEvents, cryptoMethods.FirstOrDefault().Method_Name, context.ContainingSymbol.ToString(), cryslSpecificationModel.Spec_Section.Class_Name);

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
                                                bool isPrimaryConstraintSatisfied = constraintsSectionAnalyzer.IsPropertyConstraintSatisfied(cryslSpecificationModel, validEvents, rightExprValue);
                                                if (!isPrimaryConstraintSatisfied)
                                                {
                                                    var accepetedParameterValues = cryslSpecificationModel.Constraints_Section.Constraints.Where(x => x.Object_Varname.Equals(validEvents.PropertyName) && x.Additional_constraints == null).Select(y => y.Constraints_List);
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
                                                AdditionalConstraintsDict.TryGetValue(cryslSpecificationModel.Spec_Section.Class_Name, out additionalConstraintsList);

                                                //Check additionalConstraints for null because additionalConstraints is an out parameter, so if TryGet does not return anything, then additionalConstraints would be set to null
                                                if (additionalConstraintsList != null)
                                                {
                                                    foreach (var additionalConstraints in additionalConstraintsList)
                                                    {
                                                        if (additionalConstraints.EventVariableDeclarator.ToString().Equals(localInvocatorSymbolInfo.Name.ToString()))
                                                        {
                                                            constraintsSectionAnalyzer = _serviceProvider.GetService<IConstraintsSectionAnalyzer>();
                                                            bool isAdditionalConstraintSatisfied = constraintsSectionAnalyzer.IsAdditionalConstraintSatisfied(additionalConstraints, leftExprSymbolInfo, rightExprValue, cryslSpecificationModel.Object_Section.Objects_Declaration, validEvents);
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
                                    }
                                    if (!isValidEvent)
                                    {
                                        //Report Diagnostics as not a Valid Event
                                    }
                                    //Check if tainted variables are santized
                                    if (leftExprSymbolInfo != null)
                                    {
                                        var taintedLeftExprSymbolInfo = IsTaintedValueExists(leftExprSymbolInfo.ContainingSymbol, leftExprSymbolInfo);
                                        if (taintedLeftExprSymbolInfo.IsTainted)
                                        {
                                            if (rightExprSymbolInfo != null)
                                            {
                                                var taintedRightExprSymbolInfo = IsTaintedValueExists(rightExprSymbolInfo.ContainingSymbol, rightExprSymbolInfo);
                                                if (!taintedRightExprSymbolInfo.IsTainted)
                                                {
                                                    if (TaintedValuesDictionary.Count != 0)
                                                    {
                                                        lock (TaintedValuesDictionary)
                                                        {
                                                            ContextInformation contextInformation = new ContextInformation
                                                            {
                                                                ContainingSymbolInfo = leftExprSymbolInfo.ContainingSymbol
                                                            };
                                                            TaintedValuesDictionary.Remove(new KeyValuePair<ContextInformation, ISymbol>(contextInformation, leftExprSymbolInfo));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //In order to make object sensitive, identifier symbol info should be retrieved
                                                        var identifierLeftExprNodes = simpleAssignExpr.Left.ChildNodes().OfType<IdentifierNameSyntax>();
                                                        ISymbol identifierLeftExprSymbolInfo = null;
                                                        if (identifierLeftExprNodes.Count() > 0)
                                                        {
                                                            identifierLeftExprSymbolInfo = context.SemanticModel.GetSymbolInfo(identifierLeftExprNodes.FirstOrDefault()).Symbol;
                                                        }
                                                        SanitizeTaintValue(leftExprSymbolInfo.ContainingSymbol, leftExprSymbolInfo, identifierLeftExprSymbolInfo);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                else
                                {   //Check if tainted variables are santized
                                    if (leftExprSymbolInfo != null)
                                    {
                                        var taintedLeftExprSymbolInfo = IsTaintedValueExists(leftExprSymbolInfo.ContainingSymbol, leftExprSymbolInfo);
                                        if (taintedLeftExprSymbolInfo.IsTainted)
                                        {
                                            if (rightExprSymbolInfo != null)
                                            {
                                                var taintedRightExprSymbolInfo = IsTaintedValueExists(rightExprSymbolInfo.ContainingSymbol, rightExprSymbolInfo);
                                                if (!taintedRightExprSymbolInfo.IsTainted)
                                                {
                                                    if (TaintedValuesDictionary.Count != 0)
                                                    {
                                                        lock (TaintedValuesDictionary)
                                                        {
                                                            ContextInformation contextInformation = new ContextInformation
                                                            {
                                                                ContainingSymbolInfo = leftExprSymbolInfo.ContainingSymbol
                                                            };
                                                            TaintedValuesDictionary.Remove(new KeyValuePair<ContextInformation, ISymbol>(contextInformation, leftExprSymbolInfo));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //In order to make object sensitive, identifier symbol info should be retrieved
                                                        var identifierLeftExprNodes = simpleAssignExpr.Left.ChildNodes().OfType<IdentifierNameSyntax>();
                                                        ISymbol identifierLeftExprSymbolInfo = null;
                                                        if (identifierLeftExprNodes.Count() > 0)
                                                        {
                                                            identifierLeftExprSymbolInfo = context.SemanticModel.GetSymbolInfo(identifierLeftExprNodes.FirstOrDefault()).Symbol;
                                                        }
                                                        SanitizeTaintValue(leftExprSymbolInfo.ContainingSymbol, leftExprSymbolInfo, identifierLeftExprSymbolInfo);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //Check if right expression is sanitized
                            if (rightExprSymbolInfo != null && leftExprSymbolInfo != null)
                            {
                                var taintedRightExprSymbolInfo = IsTaintedValueExists(rightExprSymbolInfo.ContainingSymbol, rightExprSymbolInfo);
                                if (!taintedRightExprSymbolInfo.IsTainted)
                                {
                                    //Sanitize the left expression if the right expression value is not tainted
                                    var taintedLeftExprSymbolInfo = IsTaintedValueExists(leftExprSymbolInfo.ContainingSymbol, leftExprSymbolInfo);
                                    if (taintedLeftExprSymbolInfo.IsTainted)
                                    {
                                        //In order to make object sensitive, identifier symbol info should be retrieved
                                        var identifierLeftExprNodes = simpleAssignExpr.Left.ChildNodes().OfType<IdentifierNameSyntax>();
                                        ISymbol identifierLeftExprSymbolInfo = null;
                                        if (identifierLeftExprNodes.Count() > 0)
                                        {
                                            identifierLeftExprSymbolInfo = context.SemanticModel.GetSymbolInfo(identifierLeftExprNodes.FirstOrDefault()).Symbol;
                                        }
                                        SanitizeTaintValue(leftExprSymbolInfo.ContainingSymbol, leftExprSymbolInfo, identifierLeftExprSymbolInfo);
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
                    if (rightExprSymbolInfo != null && leftExprSymbolInfo != null)
                    {
                        var taintedRightExprSymbolInfo = IsTaintedValueExists(rightExprSymbolInfo.ContainingSymbol, rightExprSymbolInfo);
                        if (taintedRightExprSymbolInfo.IsTainted)
                        {
                            var taintedLeftExprSymbolInfo = IsTaintedValueExists(leftExprSymbolInfo.ContainingSymbol, leftExprSymbolInfo);
                            if (!taintedLeftExprSymbolInfo.IsTainted)
                            {
                                lock (TaintedValuesDictionary)
                                {
                                    ContextInformation contextInformation = new ContextInformation
                                    {
                                        ContainingSymbolInfo = leftExprSymbolInfo.ContainingSymbol
                                    };
                                    TaintedValuesDictionary.Add(new KeyValuePair<ContextInformation, ISymbol>(contextInformation, leftExprSymbolInfo));
                                }
                            }
                            ReportDiagnostics(context, HardCodedContextCheckViolationRule, HardCodedCheckViolationRule, simpleAssExpr.GetLocation(), taintedRightExprSymbolInfo.TaintedContextInformation);
                            /*var diagnsotics = (taintedRightExprSymbolInfo.TaintedContextInformation.CallerSymbolInfo != null) ? Diagnostic.Create(HardCodedContextCheckViolationRule, simpleAssExpr.GetLocation(), taintedRightExprSymbolInfo.TaintedContextInformation.CallerSymbolInfo.ToString()) : Diagnostic.Create(HardCodedCheckViolationRule, simpleAssExpr.GetLocation());
                            context.ReportDiagnostic(diagnsotics);*/
                        }
                    }

                    //Check if tainted variables are santized
                    if (leftExprSymbolInfo != null)
                    {
                        var taintedLeftExprSymbolInfo = IsTaintedValueExists(leftExprSymbolInfo.ContainingSymbol, leftExprSymbolInfo);
                        if (taintedLeftExprSymbolInfo.IsTainted)
                        {
                            if (rightExprSymbolInfo != null)
                            {
                                var taintedRightExprSymbolInfo = IsTaintedValueExists(rightExprSymbolInfo.ContainingSymbol, rightExprSymbolInfo);
                                if (!taintedRightExprSymbolInfo.IsTainted)
                                {
                                    //Sanitize the value in the local TaintedValueDictionary
                                    if (TaintedValuesDictionary.Count != 0)
                                    {
                                        lock (TaintedValuesDictionary)
                                        {
                                            ContextInformation contextInformation = new ContextInformation
                                            {
                                                ContainingSymbolInfo = leftExprSymbolInfo.ContainingSymbol
                                            };
                                            TaintedValuesDictionary.Remove(new KeyValuePair<ContextInformation, ISymbol>(contextInformation, leftExprSymbolInfo));
                                        }
                                    }
                                    //Sanitize the value in the global TaintedContextDictionary as well. 
                                    //Sanitizing a method parameter would sanitize the parameter in caller as well as callee
                                    //In order to make object sensitive, identifier symbol info should be retrieved
                                    var identifierLeftExprNodes = simpleAssignExpr.Left.ChildNodes().OfType<IdentifierNameSyntax>();
                                    ISymbol identifierLeftExprSymbolInfo = null;
                                    if (identifierLeftExprNodes.Count() > 0)
                                    {
                                        identifierLeftExprSymbolInfo = context.SemanticModel.GetSymbolInfo(identifierLeftExprNodes.FirstOrDefault()).Symbol;
                                    }
                                    SanitizeTaintValue(leftExprSymbolInfo.ContainingSymbol, leftExprSymbolInfo, identifierLeftExprSymbolInfo);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static TaintQueryInformation IsTaintedValueExists(ISymbol containingMethod, ISymbol nodeInfo, ISymbol containingObject = null)
        {
            TaintQueryInformation taintQueryResult = new TaintQueryInformation();
            try
            {
                if (containingMethod != null && nodeInfo != null && containingObject == null)
                {
                    TaintQueryInformation taintQueryInformation = new TaintQueryInformation
                    {
                        IsTainted = false,
                        TaintedContextInformation = new List<ContextInformation>()
                    };
                    lock (TaintedValuesDictionary)
                    {
                        //If tainted values are not present in ContextDictionary check in Current TaintedValuesDictionary
                        //TaintedValuesDictionary need not be checked for context, because it contains only current context values
                        foreach (var taintedValue in TaintedValuesDictionary)
                        {
                            if (taintedValue.Key.ContainingSymbolInfo != null)
                            {
                                bool taintedValuePresent = (taintedValue.Key.ContainingSymbolInfo.ToString().Equals(containingMethod.ToString()) && taintedValue.Value.Kind.Equals(nodeInfo.Kind) && taintedValue.Value.ToString().Equals(nodeInfo.ToString()) && taintedValue.Value.Name.ToString().Equals(nodeInfo.Name.ToString())) ? true : false;

                                if (taintedValuePresent)
                                {
                                    taintQueryInformation.IsTainted = true;
                                    taintQueryInformation.TaintedContextInformation.Add(taintedValue.Key);
                                    /*return new TaintQueryInformation
                                    {
                                        IsTainted = true,
                                        TaintedContextInformation = taintedValue.Key
                                    };*/
                                }
                            }
                        }
                    }
                    taintQueryResult = taintQueryInformation;
                }
                else if (containingMethod != null && nodeInfo != null && containingObject != null)
                {
                    TaintQueryInformation taintQueryInformation = new TaintQueryInformation
                    {
                        IsTainted = false,
                        TaintedContextInformation = new List<ContextInformation>()
                    };
                    lock (TaintedValuesDictionary)
                    {
                        //If tainted values are not present in ContextDictionary check in Current TaintedValuesDictionary
                        //TaintedValuesDictionary need not be checked for context, because it contains only current context values
                        foreach (var taintedValue in TaintedValuesDictionary)
                        {
                            if (taintedValue.Key.ContainingObjectSymbolInfo != null && taintedValue.Key.ContainingSymbolInfo != null)
                            {
                                bool taintedValuePresent = (taintedValue.Key.ContainingSymbolInfo.ToString().Equals(containingMethod.ToString()) && taintedValue.Key.ContainingObjectSymbolInfo.ToString().Equals(containingObject.ToString()) && taintedValue.Value.Kind.Equals(nodeInfo.Kind) && taintedValue.Value.ToString().Equals(nodeInfo.ToString()) && taintedValue.Value.Name.ToString().Equals(nodeInfo.Name.ToString())) ? true : false;

                                if (taintedValuePresent)
                                {
                                    taintQueryInformation.IsTainted = true;
                                    taintQueryInformation.TaintedContextInformation.Add(taintedValue.Key);

                                    /*return new TaintQueryInformation
                                    {
                                        IsTainted = true,
                                        TaintedContextInformation = taintedValue.Key
                                    };*/
                                }
                            }
                        }
                    }
                    taintQueryResult = taintQueryInformation;
                }
            }
            catch (Exception ex)
            {
                //Log the exception into a log file
            }
            return taintQueryResult;
        }

        private static void ReportDiagnostics(SyntaxNodeAnalysisContext context, DiagnosticDescriptor descriptionContextInfo, DiagnosticDescriptor descriptorInfo, Location location, List<ContextInformation> contextInformationList)
        {
            foreach (var contextInformation in contextInformationList)
            {
                if (contextInformation.CallerSymbolInfo != null)
                {
                    var diagnostic = Diagnostic.Create(descriptionContextInfo, location, contextInformation.CallerSymbolInfo.ToString());
                    context.ReportDiagnostic(diagnostic);
                }
                else
                {
                    var diagnostic = Diagnostic.Create(descriptorInfo, location);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        private static void SanitizeTaintValue(ISymbol containingMethod, ISymbol nodeInfo, ISymbol containingObject)
        {
            List<KeyValuePair<ContextInformation, ISymbol>> sanitizedValues = new List<KeyValuePair<ContextInformation, ISymbol>>();

            //TaintedValuesDictionary need not be checked for context, because it contains only current context values
            foreach (var taintedValue in TaintedValuesDictionary)
            {
                if (containingMethod != null && nodeInfo != null && containingObject == null)
                {
                    bool taintedValuePresent = (taintedValue.Value.Kind.Equals(nodeInfo.Kind) && taintedValue.Value.ToString().Equals(nodeInfo.ToString()) && taintedValue.Value.Name.ToString().Equals(nodeInfo.Name.ToString())) ? true : false;
                    if (taintedValuePresent)
                    {
                        lock (sanitizedValues)
                        {
                            if (!sanitizedValues.Contains(new KeyValuePair<ContextInformation, ISymbol>(taintedValue.Key, taintedValue.Value)))
                            {
                                sanitizedValues.Add(new KeyValuePair<ContextInformation, ISymbol>(taintedValue.Key, taintedValue.Value));
                            }
                        }
                    }
                }
                else if (containingMethod != null && nodeInfo != null && containingObject != null)
                {
                    if (taintedValue.Key.ContainingObjectSymbolInfo != null)
                    {
                        bool taintedValuePresent = (taintedValue.Key.ContainingObjectSymbolInfo.ToString().Equals(containingObject.ToString()) && taintedValue.Value.Kind.Equals(nodeInfo.Kind) && taintedValue.Value.ToString().Equals(nodeInfo.ToString()) && taintedValue.Value.Name.ToString().Equals(nodeInfo.Name.ToString())) ? true : false;
                        if (taintedValuePresent)
                        {
                            lock (sanitizedValues)
                            {
                                if (!sanitizedValues.Contains(new KeyValuePair<ContextInformation, ISymbol>(taintedValue.Key, taintedValue.Value)))
                                {
                                    sanitizedValues.Add(new KeyValuePair<ContextInformation, ISymbol>(taintedValue.Key, taintedValue.Value));
                                }
                            }
                        }
                    }
                }
            }

            //Remove all the sanitized variables
            foreach (var sanitizedVariables in sanitizedValues)
            {
                lock (TaintedValuesDictionary)
                {
                    TaintedValuesDictionary.Remove(sanitizedVariables);
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


        /*private void AnalyzeMethodAction(OperationAnalysisContext context)
        {
            WellKnownTypeProvider provider = WellKnownTypeProvider.GetOrCreate(context.Compilation);

            InterproceduralAnalysisConfiguration interproceduralAnalysisConfiguration = InterproceduralAnalysisConfiguration.Create(
                                                                    context.Options,
                                                                    SupportedDiagnostics,
                                                                    InterproceduralAnalysisKind.ContextSensitive,
                                                                    context.CancellationToken
                                                                    );

            var aliasAnalysisResult = CopyAnalysis.TryGetOrComputeResult(
                context.GetControlFlowGraph(),
                context.ContainingSymbol,
                context.Options,
                provider,
                interproceduralAnalysisConfiguration,
                interproceduralAnalysisPredicateOpt: null
                );

        }*/

        private void AnalyzeUsingStartBlock(OperationBlockStartAnalysisContext context)
        {
            context.RegisterOperationAction(AnalyzeUsingBlock, OperationKind.Using);
            //context.RegisterOperationAction(AnalyzeMethodAction, OperationKind.MethodBodyOperation);

        }

        private void AnalyzeCompilationAction(CompilationStartAnalysisContext context)
        {
            ICryslConfigurationBuilder cryslConfigurationBuilder = _serviceProvider.GetService<ICryslConfigurationBuilder>();
            CryslSettings cryslConfigurations = cryslConfigurationBuilder.GetCryslConfigurations(context.Options.AdditionalFiles);

            ICryslObjectBuilder cSharpObjectBuilder = _serviceProvider.GetService<ICryslObjectBuilder>();
            if (_cryslSpecificationModel == null)
            {
                _cryslSpecificationModel = new Dictionary<string, CryslJsonModel>();
            }

            //Check for different Crysl files
            if (Directory.Exists(cryslConfigurations.CryslConfiguration.CryslPath))
            {
                IsTaintAnalysisOff = cryslConfigurations.CryslConfiguration.IsTaintAnalysisOff;
                IsCryslFilePresent = true;
                string[] cryslFiles = Directory.GetFiles(cryslConfigurations.CryslConfiguration.CryslPath, "*.crysl");
                foreach (var cryslFile in cryslFiles)
                {
                    CryslResult cryslCompilationModel = cSharpObjectBuilder.CryslToCSharpBuilder(cryslFile);
                    if (cryslCompilationModel.IsValid && !_cryslSpecificationModel.Values.Contains(cryslCompilationModel.CryslModel))
                    {
                        if (_cryslSpecificationModel.ContainsKey(cryslFile))
                        {
                            lock (_cryslSpecificationModel)
                            {
                                _cryslSpecificationModel.Remove(cryslFile);
                                _cryslSpecificationModel.Add(cryslFile, cryslCompilationModel.CryslModel);
                            }
                        }
                        else
                        {
                            lock (_cryslSpecificationModel)
                            {
                                _cryslSpecificationModel.Add(cryslFile, cryslCompilationModel.CryslModel);
                            }
                        }
                    }
                }
            }
        }

        private void AnalyzeUsingBlock(OperationAnalysisContext context)
        {
            var currentEventsOrderDictionary = EventsOrderDictionary.ToImmutableSortedDictionary();

            foreach (var eventsOrderDictValues in currentEventsOrderDictionary)
            {
                Dictionary<string, List<KeyValuePair<string, string>>> orderDictValues = new Dictionary<string, List<KeyValuePair<string, string>>>();
                EventsOrderDictionary.TryGetValue(eventsOrderDictValues.Key, out orderDictValues);
                bool isEventPresent = false;
                bool isEventDictionaryPresent = true;

                List<KeyValuePair<string, string>> eventsOrderDictionary = new List<KeyValuePair<string, string>>();
                orderDictValues.TryGetValue(context.ContainingSymbol.ToString(), out eventsOrderDictionary);

                if (eventsOrderDictionary == null)
                {
                    eventsOrderDictionary = new List<KeyValuePair<string, string>>();
                }
                else
                {
                    isEventPresent = true;
                }

                var operationSymbol = context.Operation as Microsoft.CodeAnalysis.Operations.IUsingOperation;
                if (operationSymbol != null)
                {
                    foreach (var local in operationSymbol.Locals)
                    {
                        foreach (var cryslSpecificationModel in _cryslSpecificationModel.Values)
                        {
                            if (local.Type.ToString().Equals(cryslSpecificationModel.Spec_Section.Class_Name) && eventsOrderDictValues.Key.Equals(local.Type.ToString()))
                            {
                                foreach (var methods in cryslSpecificationModel.Event_Section.Methods)
                                {
                                    var cryptoMethods = methods.Crypto_Signature.Select(y => y).Where(x => x.Method_Name.ToString().Equals("Dispose"));
                                    if (cryptoMethods.Count() > 0)
                                    {

                                        MethodSignatureModel methodSignatureModel = new MethodSignatureModel
                                        {
                                            MethodName = "Dispose"
                                        };

                                        Dictionary<string, List<MethodSignatureModel>> validEventsDictionary = new Dictionary<string, List<MethodSignatureModel>>();
                                        ValidEventsDictionary.TryGetValue(cryslSpecificationModel.Spec_Section.Class_Name, out validEventsDictionary);

                                        if (validEventsDictionary == null)
                                        {
                                            validEventsDictionary = new Dictionary<string, List<MethodSignatureModel>>();
                                            isEventDictionaryPresent = false;
                                        }

                                        List<MethodSignatureModel> methodSignatureModels = new List<MethodSignatureModel>();
                                        validEventsDictionary.TryGetValue(cryptoMethods.FirstOrDefault().Event_Var_Name, out methodSignatureModels);
                                        if (methodSignatureModels != null)
                                        {
                                            methodSignatureModels.Add(methodSignatureModel);
                                            lock (ValidEventsDictionary)
                                            {
                                                validEventsDictionary[cryptoMethods.FirstOrDefault().Event_Var_Name] = methodSignatureModels;
                                            }
                                            if (methods.Aggregator == null)
                                            {
                                                lock (EventsOrderDictionary)
                                                {
                                                    eventsOrderDictionary.Add(new KeyValuePair<string, string>(cryptoMethods.FirstOrDefault().Event_Var_Name, cryptoMethods.FirstOrDefault().Method_Name));
                                                }
                                            }
                                            else
                                            {
                                                lock (EventsOrderDictionary)
                                                {
                                                    eventsOrderDictionary.Add(new KeyValuePair<string, string>(methods.Aggregator.Aggregator_Name, cryptoMethods.FirstOrDefault().Method_Name));
                                                }
                                            }

                                            if (isEventDictionaryPresent)
                                            {
                                                ValidEventsDictionary[cryslSpecificationModel.Spec_Section.Class_Name] = validEventsDictionary;
                                            }
                                            else
                                            {
                                                ValidEventsDictionary.Add(cryslSpecificationModel.Spec_Section.Class_Name, validEventsDictionary);
                                            }
                                        }
                                        else
                                        {
                                            List<MethodSignatureModel> methodSignatureModelList = new List<MethodSignatureModel>();
                                            methodSignatureModelList.Add(methodSignatureModel);
                                            lock (validEventsDictionary)
                                            {
                                                Dictionary<string, string> eventOrderDict = new Dictionary<string, string>();
                                                validEventsDictionary.Add(cryptoMethods.FirstOrDefault().Event_Var_Name, methodSignatureModelList);
                                            }
                                            lock (eventsOrderDictionary)
                                            {
                                                eventsOrderDictionary.Add(new KeyValuePair<string, string>(cryptoMethods.FirstOrDefault().Event_Var_Name, cryptoMethods.FirstOrDefault().Method_Name));
                                            }

                                            if (isEventDictionaryPresent)
                                            {
                                                ValidEventsDictionary[cryslSpecificationModel.Spec_Section.Class_Name] = validEventsDictionary;
                                            }
                                            else
                                            {
                                                ValidEventsDictionary.Add(cryslSpecificationModel.Spec_Section.Class_Name, validEventsDictionary);
                                            }
                                        }
                                    }
                                }
                            }

                            if (isEventPresent)
                            {
                                orderDictValues[context.ContainingSymbol.ToString()] = eventsOrderDictionary;
                                lock (EventsOrderDictionary)
                                {
                                    EventsOrderDictionary[eventsOrderDictValues.Key] = orderDictValues;
                                }

                            }
                            else
                            {
                                if (!orderDictValues.ContainsKey(context.ContainingSymbol.ToString()))
                                {
                                    orderDictValues.Add(context.ContainingSymbol.ToString(), eventsOrderDictionary);
                                    lock (EventsOrderDictionary)
                                    {
                                        EventsOrderDictionary[eventsOrderDictValues.Key] = orderDictValues;
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
            foreach (var eventsOrderDict in EventsOrderDictionary)
            {
                List<CryslJsonModel> cryslJsonModels = new List<CryslJsonModel>();
                ToAnalyzeCryslSection.TryGetValue(context.OwningSymbol.ToString(), out cryslJsonModels);
                if (cryslJsonModels != null)
                {
                    var cryslSpecificationModelList = cryslJsonModels.Select(x => x).Where(y => y.Spec_Section.Class_Name.Equals(eventsOrderDict.Key));
                    foreach (var cryslSpecificationModel in cryslSpecificationModelList)
                    {
                        var commonUtilities = _serviceProvider.GetService<ICommonUtilities>();
                        EventOrderContraint = commonUtilities.GetEventOrderList(cryslSpecificationModel);
                        if (EventsOrderDictionary.Count != 0)
                        {
                            List<KeyValuePair<string, string>> eventsOrderDictionary = new List<KeyValuePair<string, string>>();
                            eventsOrderDict.Value.TryGetValue(context.OwningSymbol.ToString(), out eventsOrderDictionary);
                            if (eventsOrderDictionary != null)
                            {
                                var orderCheckConstraint = _serviceProvider.GetService<IOrderSectionAnalyzer>();
                                var isValidOrder = orderCheckConstraint.IsValidOrder(eventsOrderDictionary, EventOrderContraint.Select(x => x.Key).ToList());
                                if (!isValidOrder)
                                {
                                    string validEventOrder = string.Join(", ", EventOrderContraint.Select(x => x.Value));
                                    var diagnsotics = Diagnostic.Create(OrderAnalyzerViolationRule, context.OwningSymbol.Locations[0], cryslSpecificationModel.Spec_Section.Class_Name, validEventOrder);
                                    context.ReportDiagnostic(diagnsotics);
                                }
                            }
                        }
                    }
                }
            }

            List<KeyValuePair<ContextInformation, ISymbol>> taintedDict = new List<KeyValuePair<ContextInformation, ISymbol>>();
            TaintedContextDictionary.TryGetValue(context.OwningSymbol.ToString(), out taintedDict);
            if (taintedDict == null)
            {
                lock (TaintedContextDictionary)
                {
                    TaintedContextDictionary.Add(context.OwningSymbol.ToString(), TaintedValuesDictionary.ToList());
                }
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
        private static void AddEventsToDictionary(string aggregatorName, ValidEvents validEvents, string methodName, string containingSymbol, string cryslSpecSection)
        {
            Dictionary<string, List<KeyValuePair<string, string>>> eventsOrderDict = new Dictionary<string, List<KeyValuePair<string, string>>>();
            EventsOrderDictionary.TryGetValue(cryslSpecSection, out eventsOrderDict);
            AddEventsAndOrder(aggregatorName, validEvents, methodName, containingSymbol, cryslSpecSection, eventsOrderDict);

        }

        private static VariableDeclaratorSyntaxResult GetVariableDeclarator(SyntaxNode syntaxNode, SyntaxNodeAnalysisContext context)
        {
            var variableDeclaratorSyntax = syntaxNode.AncestorsAndSelf().OfType<VariableDeclaratorSyntax>();
            VariableDeclaratorSyntaxResult variableDeclaratorSyntaxResult = new VariableDeclaratorSyntaxResult();
            if (variableDeclaratorSyntax.Count() > 0)
            {
                var variableDeclaratorSymbolInfo = context.SemanticModel.GetDeclaredSymbol(variableDeclaratorSyntax.FirstOrDefault());
                if (variableDeclaratorSymbolInfo.Kind.Equals(SymbolKind.Local))
                {
                    var variableDeclaratorLocalInfo = (ILocalSymbol)variableDeclaratorSymbolInfo;
                    if (variableDeclaratorLocalInfo.Type.ToString().Equals("byte[]"))
                    {
                        variableDeclaratorSyntaxResult.IsVariableDeclaratorSyntaxPresent = true;
                        variableDeclaratorSyntaxResult.VariableDeclaratorSyntaxNode = variableDeclaratorSyntax.FirstOrDefault();
                        variableDeclaratorSyntaxResult.VariableDeclaratorSymbolInfo = variableDeclaratorSymbolInfo;

                    }

                }

            }

            return variableDeclaratorSyntaxResult;
        }

        private static void AddEventsAndOrder(string aggregatorName, ValidEvents validEvents, string methodName, string containingSymbol, string cryslSpecSection, Dictionary<string, List<KeyValuePair<string, string>>> eventsOrderDict)
        {
            bool isCryslSpecPresent = true;
            bool isEventDictionaryPresent = true;
            if (eventsOrderDict == null)
            {
                isCryslSpecPresent = false;
                eventsOrderDict = new Dictionary<string, List<KeyValuePair<string, string>>>();
            }
            bool isEventPresent = false;
            List<KeyValuePair<string, string>> eventsOrderDictionary = new List<KeyValuePair<string, string>>();
            eventsOrderDict.TryGetValue(containingSymbol, out eventsOrderDictionary);

            if (eventsOrderDictionary == null)
            {
                eventsOrderDictionary = new List<KeyValuePair<string, string>>();
            }
            else
            {
                isEventPresent = true;
            }

            //Check only if Aggregators are present        
            //For instance it could be "Create" in Aes Crysl specification
            Dictionary<string, List<KeyValuePair<string, string>>> specEventDict = new Dictionary<string, List<KeyValuePair<string, string>>>();
            EventsOrderDictionary.TryGetValue(cryslSpecSection, out specEventDict);

            List<KeyValuePair<string, string>> contextEventsDict = new List<KeyValuePair<string, string>>();
            if (specEventDict != null)
            {
                specEventDict.TryGetValue(containingSymbol, out contextEventsDict);
            }

            KeyValuePair<string, string> aggregatorEvents = new KeyValuePair<string, string>();
            if (contextEventsDict != null)
            {
                aggregatorEvents = contextEventsDict.Where(x => x.Key.ToString().Contains(aggregatorName)).Select(y => y).FirstOrDefault();
            }

            //If the Event is not present
            if (!aggregatorEvents.Equals(default(KeyValuePair<string, string>)))
            {
                Dictionary<string, List<MethodSignatureModel>> validEventsDictionary = new Dictionary<string, List<MethodSignatureModel>>();
                ValidEventsDictionary.TryGetValue(cryslSpecSection, out validEventsDictionary);

                if (validEventsDictionary == null)
                {
                    validEventsDictionary = new Dictionary<string, List<MethodSignatureModel>>();
                    isEventDictionaryPresent = false;
                }

                List<MethodSignatureModel> validMethodSignatures = new List<MethodSignatureModel>();
                validEventsDictionary.TryGetValue(validEvents.PropertyName, out validMethodSignatures);

                //If ValidEventsDictionary contains the method but with different signature, update the dictionary
                if (validMethodSignatures != null)
                {
                    validMethodSignatures.Add(validEvents.ValidMethods);
                    lock (validEventsDictionary)
                    {
                        validEventsDictionary[validEvents.PropertyName] = validMethodSignatures;
                    }

                }
                else
                {
                    List<MethodSignatureModel> methodSignatureList = new List<MethodSignatureModel>();
                    methodSignatureList.Add(validEvents.ValidMethods);
                    lock (validEventsDictionary)
                    {
                        validEventsDictionary.Add(validEvents.PropertyName, methodSignatureList);
                    }
                }
                //Updating the value because the key is already present
                lock (EventsOrderDictionary)
                {
                    eventsOrderDictionary.Add(new KeyValuePair<string, string>(aggregatorName, methodName));
                }

                if (isEventDictionaryPresent)
                {
                    ValidEventsDictionary[cryslSpecSection] = validEventsDictionary;
                }
                else
                {
                    ValidEventsDictionary.Add(cryslSpecSection, validEventsDictionary);
                }

            }
            else
            {
                Dictionary<string, List<MethodSignatureModel>> validEventsDictionary = new Dictionary<string, List<MethodSignatureModel>>();
                ValidEventsDictionary.TryGetValue(cryslSpecSection, out validEventsDictionary);

                if (validEventsDictionary == null)
                {
                    validEventsDictionary = new Dictionary<string, List<MethodSignatureModel>>();
                    isEventDictionaryPresent = false;
                }

                List<MethodSignatureModel> validMethodSignatures = new List<MethodSignatureModel>();
                validEventsDictionary.TryGetValue(validEvents.PropertyName, out validMethodSignatures);

                //If ValidEventsDictionary contains the method but with different signature, update the dictionary
                if (validMethodSignatures != null)
                {
                    validMethodSignatures.Add(validEvents.ValidMethods);
                    lock (ValidEventsDictionary)
                    {
                        validEventsDictionary[validEvents.PropertyName] = validMethodSignatures;
                    }

                }
                else
                {
                    List<MethodSignatureModel> methodSignatureList = new List<MethodSignatureModel>();
                    methodSignatureList.Add(validEvents.ValidMethods);
                    lock (ValidEventsDictionary)
                    {
                        validEventsDictionary.Add(validEvents.PropertyName, methodSignatureList);
                    }
                }
                lock (EventsOrderDictionary)
                {
                    eventsOrderDictionary.Add(new KeyValuePair<string, string>(aggregatorName, methodName));
                }

                if (isEventDictionaryPresent)
                {
                    ValidEventsDictionary[cryslSpecSection] = validEventsDictionary;
                }
                else
                {
                    ValidEventsDictionary.Add(cryslSpecSection, validEventsDictionary);
                }
            }
            if (isEventPresent)
            {
                eventsOrderDict[containingSymbol] = eventsOrderDictionary;
                if (!isCryslSpecPresent)
                {
                    lock (EventsOrderDictionary)
                    {
                        EventsOrderDictionary.Add(cryslSpecSection, eventsOrderDict);
                    }
                }
                else
                {
                    lock (EventsOrderDictionary)
                    {
                        EventsOrderDictionary[cryslSpecSection] = eventsOrderDict;
                    }
                }
            }
            else
            {
                eventsOrderDict.Add(containingSymbol, eventsOrderDictionary);
                if (!isCryslSpecPresent)
                {
                    lock (EventsOrderDictionary)
                    {
                        EventsOrderDictionary.Add(cryslSpecSection, eventsOrderDict);
                    }
                }
                else
                {
                    lock (EventsOrderDictionary)
                    {
                        EventsOrderDictionary[cryslSpecSection] = eventsOrderDict;
                    }
                }
            }
        }
    }
}
