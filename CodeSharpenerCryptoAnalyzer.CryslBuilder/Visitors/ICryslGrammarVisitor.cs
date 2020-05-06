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

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="CryslGrammarParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public interface ICryslGrammarVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.cryslsection"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCryslsection([NotNull] CryslGrammarParser.CryslsectionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.specsection"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSpecsection([NotNull] CryslGrammarParser.SpecsectionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.objectssection"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitObjectssection([NotNull] CryslGrammarParser.ObjectssectionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.eventssection"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEventssection([NotNull] CryslGrammarParser.EventssectionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.orderssection"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOrderssection([NotNull] CryslGrammarParser.OrderssectionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.constraintssection"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConstraintssection([NotNull] CryslGrammarParser.ConstraintssectionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.ensuressection"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEnsuressection([NotNull] CryslGrammarParser.EnsuressectionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.objects"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitObjects([NotNull] CryslGrammarParser.ObjectsContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>IntValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIntValue([NotNull] CryslGrammarParser.IntValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ByteValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitByteValue([NotNull] CryslGrammarParser.ByteValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>SbyteValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSbyteValue([NotNull] CryslGrammarParser.SbyteValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>CharValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCharValue([NotNull] CryslGrammarParser.CharValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>DecimalValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDecimalValue([NotNull] CryslGrammarParser.DecimalValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>DoubleValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDoubleValue([NotNull] CryslGrammarParser.DoubleValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>FloatValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFloatValue([NotNull] CryslGrammarParser.FloatValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>UintValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUintValue([NotNull] CryslGrammarParser.UintValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>LongValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLongValue([NotNull] CryslGrammarParser.LongValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ShortValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitShortValue([NotNull] CryslGrammarParser.ShortValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>UshortValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUshortValue([NotNull] CryslGrammarParser.UshortValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>BoolValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBoolValue([NotNull] CryslGrammarParser.BoolValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>TypeValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeValue([NotNull] CryslGrammarParser.TypeValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ByteArrayValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitByteArrayValue([NotNull] CryslGrammarParser.ByteArrayValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>SbyteArrayValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSbyteArrayValue([NotNull] CryslGrammarParser.SbyteArrayValueContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>CharArrayValue</c>
	/// labeled alternative in <see cref="CryslGrammarParser.objectlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCharArrayValue([NotNull] CryslGrammarParser.CharArrayValueContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.eventlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEventlist([NotNull] CryslGrammarParser.EventlistContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>WithAggregator</c>
	/// labeled alternative in <see cref="CryslGrammarParser.events"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWithAggregator([NotNull] CryslGrammarParser.WithAggregatorContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>WithoutAggregator</c>
	/// labeled alternative in <see cref="CryslGrammarParser.events"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWithoutAggregator([NotNull] CryslGrammarParser.WithoutAggregatorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.aggregator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAggregator([NotNull] CryslGrammarParser.AggregatorContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>SngEventNoArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.sngevent"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSngEventNoArguments([NotNull] CryslGrammarParser.SngEventNoArgumentsContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>SngEventWithoutArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.sngevent"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSngEventWithoutArguments([NotNull] CryslGrammarParser.SngEventWithoutArgumentsContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>SngEventWithArgumentsUnscore</c>
	/// labeled alternative in <see cref="CryslGrammarParser.sngevent"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSngEventWithArgumentsUnscore([NotNull] CryslGrammarParser.SngEventWithArgumentsUnscoreContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>SngEventMethodWithArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.sngevent"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSngEventMethodWithArguments([NotNull] CryslGrammarParser.SngEventMethodWithArgumentsContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>SngEventMethodWithoutArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.sngevent"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSngEventMethodWithoutArguments([NotNull] CryslGrammarParser.SngEventMethodWithoutArgumentsContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>WithArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.event"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWithArguments([NotNull] CryslGrammarParser.WithArgumentsContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>WithoutArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.event"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWithoutArguments([NotNull] CryslGrammarParser.WithoutArgumentsContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>WithPropertiesOnly</c>
	/// labeled alternative in <see cref="CryslGrammarParser.event"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWithPropertiesOnly([NotNull] CryslGrammarParser.WithPropertiesOnlyContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ObjectAssnWithArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.event"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitObjectAssnWithArguments([NotNull] CryslGrammarParser.ObjectAssnWithArgumentsContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ObjectAssnWithoutArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.event"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitObjectAssnWithoutArguments([NotNull] CryslGrammarParser.ObjectAssnWithoutArgumentsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.orderlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOrderlist([NotNull] CryslGrammarParser.OrderlistContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.constraintslist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConstraintslist([NotNull] CryslGrammarParser.ConstraintslistContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.constraints"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConstraints([NotNull] CryslGrammarParser.ConstraintsContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>IntArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.constraint"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIntArguments([NotNull] CryslGrammarParser.IntArgumentsContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>StringArguments</c>
	/// labeled alternative in <see cref="CryslGrammarParser.constraint"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStringArguments([NotNull] CryslGrammarParser.StringArgumentsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.ensureslist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEnsureslist([NotNull] CryslGrammarParser.EnsureslistContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CryslGrammarParser.ensure"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEnsure([NotNull] CryslGrammarParser.EnsureContext context);
}
