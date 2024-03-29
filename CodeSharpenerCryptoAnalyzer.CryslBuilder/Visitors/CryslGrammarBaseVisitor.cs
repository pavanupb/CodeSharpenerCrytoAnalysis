//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from CryslGrammar.g4 by ANTLR 4.8

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="ICryslGrammarVisitor{Result}"/>,
/// which can be extended to create a visitor which only needs to handle a subset
/// of the available methods.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public partial class CryslGrammarBaseVisitor<Result> : AbstractParseTreeVisitor<Result>, ICryslGrammarVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.cryslsection"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCryslsection([NotNull] CryslGrammarParser.CryslsectionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.specsection"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSpecsection([NotNull] CryslGrammarParser.SpecsectionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.objectssection"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitObjectssection([NotNull] CryslGrammarParser.ObjectssectionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.eventssection"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitEventssection([NotNull] CryslGrammarParser.EventssectionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.orderssection"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitOrderssection([NotNull] CryslGrammarParser.OrderssectionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.constraintssection"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitConstraintssection([NotNull] CryslGrammarParser.ConstraintssectionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.ensuressection"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitEnsuressection([NotNull] CryslGrammarParser.EnsuressectionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.objects"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitObjects([NotNull] CryslGrammarParser.ObjectsContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>IntValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitIntValue([NotNull] CryslGrammarParser.IntValueContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>ByteValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitByteValue([NotNull] CryslGrammarParser.ByteValueContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>SbyteValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSbyteValue([NotNull] CryslGrammarParser.SbyteValueContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>CharValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCharValue([NotNull] CryslGrammarParser.CharValueContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>DecimalValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitDecimalValue([NotNull] CryslGrammarParser.DecimalValueContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>DoubleValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitDoubleValue([NotNull] CryslGrammarParser.DoubleValueContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>FloatValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitFloatValue([NotNull] CryslGrammarParser.FloatValueContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>UintValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitUintValue([NotNull] CryslGrammarParser.UintValueContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>LongValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitLongValue([NotNull] CryslGrammarParser.LongValueContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>ShortValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitShortValue([NotNull] CryslGrammarParser.ShortValueContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>UshortValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitUshortValue([NotNull] CryslGrammarParser.UshortValueContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>BoolValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitBoolValue([NotNull] CryslGrammarParser.BoolValueContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>TypeValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitTypeValue([NotNull] CryslGrammarParser.TypeValueContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.eventlist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitEventlist([NotNull] CryslGrammarParser.EventlistContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>WithAggregator</c>
	/// labeled alternative in <see cref="CryslGrammarParser.events"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitWithAggregator([NotNull] CryslGrammarParser.WithAggregatorContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>WithoutAggregator</c>
	/// labeled alternative in <see cref="CryslGrammarParser.events"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitWithoutAggregator([NotNull] CryslGrammarParser.WithoutAggregatorContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.aggregator"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitAggregator([NotNull] CryslGrammarParser.AggregatorContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>SngEventNoArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.sngevent"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSngEventNoArguments([NotNull] CryslGrammarParser.SngEventNoArgumentsContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>SngEventWithoutArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.sngevent"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSngEventWithoutArguments([NotNull] CryslGrammarParser.SngEventWithoutArgumentsContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>SngEventWithArgumentsUnscore</c>
	/// labeled alternative in <see cref="CryslGrammarParser.sngevent"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitSngEventWithArgumentsUnscore([NotNull] CryslGrammarParser.SngEventWithArgumentsUnscoreContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>WithArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.event"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitWithArguments([NotNull] CryslGrammarParser.WithArgumentsContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>WithoutArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.event"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitWithoutArguments([NotNull] CryslGrammarParser.WithoutArgumentsContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>WithPropertiesOnly</c>
	/// labeled alternative in <see cref="CryslGrammarParser.event"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitWithPropertiesOnly([NotNull] CryslGrammarParser.WithPropertiesOnlyContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>ObjectAssnWithArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.event"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitObjectAssnWithArguments([NotNull] CryslGrammarParser.ObjectAssnWithArgumentsContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>ObjectAssnWithoutArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.event"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitObjectAssnWithoutArguments([NotNull] CryslGrammarParser.ObjectAssnWithoutArgumentsContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.orderlist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitOrderlist([NotNull] CryslGrammarParser.OrderlistContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.constraintslist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitConstraintslist([NotNull] CryslGrammarParser.ConstraintslistContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.constraints"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitConstraints([NotNull] CryslGrammarParser.ConstraintsContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>IntArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.constraint"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitIntArguments([NotNull] CryslGrammarParser.IntArgumentsContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>StringArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.constraint"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitStringArguments([NotNull] CryslGrammarParser.StringArgumentsContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.ensureslist"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitEnsureslist([NotNull] CryslGrammarParser.EnsureslistContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.ensure"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitEnsure([NotNull] CryslGrammarParser.EnsureContext context) { return VisitChildren(context); }
}
