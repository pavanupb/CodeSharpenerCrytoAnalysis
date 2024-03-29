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

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public partial class CryslGrammarLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		SPECSECTIONNAME=1, OBJECTSSECTIONNAME=2, EVENTSSECTIONNAME=3, ORDERSSECTIONNAME=4, 
		CONSTRAINTSSECTIONNAME=5, ENSURESSECTIONNAME=6, NEWLINE=7, WS=8, IMPLIES=9, 
		IN=10, OSB=11, CSB=12, AFTER=13, QTS=14, OFB=15, CFB=16, OR=17, AND=18, 
		EQUALS=19, COLON=20, OP=21, CP=22, UNSCORE=23, REGEX=24, COMMA=25, BOOL=26, 
		BYTE=27, SBYTE=28, CHAR=29, DECIMAL=30, DOUBLE=31, FLOAT=32, INT=33, UINT=34, 
		LONG=35, ULONG=36, SHORT=37, USHORT=38, VARNAME=39, ALPHA=40, DIGIT=41, 
		SEMICOLON=42, TYPE=43;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"SPECSECTIONNAME", "OBJECTSSECTIONNAME", "EVENTSSECTIONNAME", "ORDERSSECTIONNAME", 
		"CONSTRAINTSSECTIONNAME", "ENSURESSECTIONNAME", "NEWLINE", "WS", "IMPLIES", 
		"IN", "OSB", "CSB", "AFTER", "QTS", "OFB", "CFB", "OR", "AND", "EQUALS", 
		"COLON", "OP", "CP", "UNSCORE", "REGEX", "COMMA", "BOOL", "BYTE", "SBYTE", 
		"CHAR", "DECIMAL", "DOUBLE", "FLOAT", "INT", "UINT", "LONG", "ULONG", 
		"SHORT", "USHORT", "VARNAME", "ALPHA", "DIGIT", "SEMICOLON", "TYPE"
	};


	public CryslGrammarLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public CryslGrammarLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'SPEC'", "'OBJECTS'", "'EVENTS'", "'ORDER'", "'CONSTRAINTS'", "'ENSURES'", 
		null, null, "'=>'", "'in'", "'['", "']'", "'after'", "'\"'", "'{'", "'}'", 
		"'|'", "'&'", "'='", "':'", "'('", "')'", "'_'", null, "','", "'bool'", 
		"'byte'", "'sbyte'", "'char'", "'decimal'", "'double'", "'float'", "'int'", 
		"'uint'", "'long'", "'ulong'", "'short'", "'ushort'", null, null, null, 
		"';'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "SPECSECTIONNAME", "OBJECTSSECTIONNAME", "EVENTSSECTIONNAME", "ORDERSSECTIONNAME", 
		"CONSTRAINTSSECTIONNAME", "ENSURESSECTIONNAME", "NEWLINE", "WS", "IMPLIES", 
		"IN", "OSB", "CSB", "AFTER", "QTS", "OFB", "CFB", "OR", "AND", "EQUALS", 
		"COLON", "OP", "CP", "UNSCORE", "REGEX", "COMMA", "BOOL", "BYTE", "SBYTE", 
		"CHAR", "DECIMAL", "DOUBLE", "FLOAT", "INT", "UINT", "LONG", "ULONG", 
		"SHORT", "USHORT", "VARNAME", "ALPHA", "DIGIT", "SEMICOLON", "TYPE"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "CryslGrammar.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static CryslGrammarLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x2', '-', '\x119', '\b', '\x1', '\x4', '\x2', '\t', '\x2', 
		'\x4', '\x3', '\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', 
		'\x5', '\x4', '\x6', '\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', 
		'\t', '\b', '\x4', '\t', '\t', '\t', '\x4', '\n', '\t', '\n', '\x4', '\v', 
		'\t', '\v', '\x4', '\f', '\t', '\f', '\x4', '\r', '\t', '\r', '\x4', '\xE', 
		'\t', '\xE', '\x4', '\xF', '\t', '\xF', '\x4', '\x10', '\t', '\x10', '\x4', 
		'\x11', '\t', '\x11', '\x4', '\x12', '\t', '\x12', '\x4', '\x13', '\t', 
		'\x13', '\x4', '\x14', '\t', '\x14', '\x4', '\x15', '\t', '\x15', '\x4', 
		'\x16', '\t', '\x16', '\x4', '\x17', '\t', '\x17', '\x4', '\x18', '\t', 
		'\x18', '\x4', '\x19', '\t', '\x19', '\x4', '\x1A', '\t', '\x1A', '\x4', 
		'\x1B', '\t', '\x1B', '\x4', '\x1C', '\t', '\x1C', '\x4', '\x1D', '\t', 
		'\x1D', '\x4', '\x1E', '\t', '\x1E', '\x4', '\x1F', '\t', '\x1F', '\x4', 
		' ', '\t', ' ', '\x4', '!', '\t', '!', '\x4', '\"', '\t', '\"', '\x4', 
		'#', '\t', '#', '\x4', '$', '\t', '$', '\x4', '%', '\t', '%', '\x4', '&', 
		'\t', '&', '\x4', '\'', '\t', '\'', '\x4', '(', '\t', '(', '\x4', ')', 
		'\t', ')', '\x4', '*', '\t', '*', '\x4', '+', '\t', '+', '\x4', ',', '\t', 
		',', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x4', '\x3', '\x4', 
		'\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', 
		'\x3', '\x5', '\x3', '\x5', '\x3', '\x5', '\x3', '\x5', '\x3', '\x5', 
		'\x3', '\x5', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', 
		'\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', 
		'\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\a', '\x3', '\a', '\x3', 
		'\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', 
		'\x3', '\b', '\x5', '\b', '\x89', '\n', '\b', '\x3', '\b', '\x3', '\b', 
		'\x3', '\t', '\x6', '\t', '\x8E', '\n', '\t', '\r', '\t', '\xE', '\t', 
		'\x8F', '\x3', '\t', '\x3', '\t', '\x3', '\n', '\x3', '\n', '\x3', '\n', 
		'\x3', '\v', '\x3', '\v', '\x3', '\v', '\x3', '\f', '\x3', '\f', '\x3', 
		'\r', '\x3', '\r', '\x3', '\xE', '\x3', '\xE', '\x3', '\xE', '\x3', '\xE', 
		'\x3', '\xE', '\x3', '\xE', '\x3', '\xF', '\x3', '\xF', '\x3', '\x10', 
		'\x3', '\x10', '\x3', '\x11', '\x3', '\x11', '\x3', '\x12', '\x3', '\x12', 
		'\x3', '\x13', '\x3', '\x13', '\x3', '\x14', '\x3', '\x14', '\x3', '\x15', 
		'\x3', '\x15', '\x3', '\x16', '\x3', '\x16', '\x3', '\x17', '\x3', '\x17', 
		'\x3', '\x18', '\x3', '\x18', '\x3', '\x19', '\x3', '\x19', '\x3', '\x1A', 
		'\x3', '\x1A', '\x3', '\x1B', '\x3', '\x1B', '\x3', '\x1B', '\x3', '\x1B', 
		'\x3', '\x1B', '\x3', '\x1C', '\x3', '\x1C', '\x3', '\x1C', '\x3', '\x1C', 
		'\x3', '\x1C', '\x3', '\x1D', '\x3', '\x1D', '\x3', '\x1D', '\x3', '\x1D', 
		'\x3', '\x1D', '\x3', '\x1D', '\x3', '\x1E', '\x3', '\x1E', '\x3', '\x1E', 
		'\x3', '\x1E', '\x3', '\x1E', '\x3', '\x1F', '\x3', '\x1F', '\x3', '\x1F', 
		'\x3', '\x1F', '\x3', '\x1F', '\x3', '\x1F', '\x3', '\x1F', '\x3', '\x1F', 
		'\x3', ' ', '\x3', ' ', '\x3', ' ', '\x3', ' ', '\x3', ' ', '\x3', ' ', 
		'\x3', ' ', '\x3', '!', '\x3', '!', '\x3', '!', '\x3', '!', '\x3', '!', 
		'\x3', '!', '\x3', '\"', '\x3', '\"', '\x3', '\"', '\x3', '\"', '\x3', 
		'#', '\x3', '#', '\x3', '#', '\x3', '#', '\x3', '#', '\x3', '$', '\x3', 
		'$', '\x3', '$', '\x3', '$', '\x3', '$', '\x3', '%', '\x3', '%', '\x3', 
		'%', '\x3', '%', '\x3', '%', '\x3', '%', '\x3', '&', '\x3', '&', '\x3', 
		'&', '\x3', '&', '\x3', '&', '\x3', '&', '\x3', '\'', '\x3', '\'', '\x3', 
		'\'', '\x3', '\'', '\x3', '\'', '\x3', '\'', '\x3', '\'', '\x3', '(', 
		'\x3', '(', '\x3', '(', '\a', '(', '\x10A', '\n', '(', '\f', '(', '\xE', 
		'(', '\x10D', '\v', '(', '\x3', ')', '\x3', ')', '\x3', '*', '\x3', '*', 
		'\x3', '+', '\x3', '+', '\x3', ',', '\x6', ',', '\x116', '\n', ',', '\r', 
		',', '\xE', ',', '\x117', '\x2', '\x2', '-', '\x3', '\x3', '\x5', '\x4', 
		'\a', '\x5', '\t', '\x6', '\v', '\a', '\r', '\b', '\xF', '\t', '\x11', 
		'\n', '\x13', '\v', '\x15', '\f', '\x17', '\r', '\x19', '\xE', '\x1B', 
		'\xF', '\x1D', '\x10', '\x1F', '\x11', '!', '\x12', '#', '\x13', '%', 
		'\x14', '\'', '\x15', ')', '\x16', '+', '\x17', '-', '\x18', '/', '\x19', 
		'\x31', '\x1A', '\x33', '\x1B', '\x35', '\x1C', '\x37', '\x1D', '\x39', 
		'\x1E', ';', '\x1F', '=', ' ', '?', '!', '\x41', '\"', '\x43', '#', '\x45', 
		'$', 'G', '%', 'I', '&', 'K', '\'', 'M', '(', 'O', ')', 'Q', '*', 'S', 
		'+', 'U', ',', 'W', '-', '\x3', '\x2', '\a', '\x5', '\x2', '\v', '\f', 
		'\xF', '\xF', '\"', '\"', '\x4', '\x2', ',', '-', '\x41', '\x41', '\x5', 
		'\x2', '\x43', '\\', '\x61', '\x61', '\x63', '|', '\x3', '\x2', '\x32', 
		';', '\x5', '\x2', '\x30', '\x30', '\x43', '\\', '\x63', '|', '\x2', '\x11D', 
		'\x2', '\x3', '\x3', '\x2', '\x2', '\x2', '\x2', '\x5', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\a', '\x3', '\x2', '\x2', '\x2', '\x2', '\t', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\v', '\x3', '\x2', '\x2', '\x2', '\x2', '\r', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\xF', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x11', '\x3', '\x2', '\x2', '\x2', '\x2', '\x13', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\x15', '\x3', '\x2', '\x2', '\x2', '\x2', '\x17', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\x19', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x1B', '\x3', '\x2', '\x2', '\x2', '\x2', '\x1D', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\x1F', '\x3', '\x2', '\x2', '\x2', '\x2', '!', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '#', '\x3', '\x2', '\x2', '\x2', '\x2', '%', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\'', '\x3', '\x2', '\x2', '\x2', '\x2', 
		')', '\x3', '\x2', '\x2', '\x2', '\x2', '+', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '-', '\x3', '\x2', '\x2', '\x2', '\x2', '/', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x31', '\x3', '\x2', '\x2', '\x2', '\x2', '\x33', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x35', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x37', '\x3', '\x2', '\x2', '\x2', '\x2', '\x39', '\x3', '\x2', '\x2', 
		'\x2', '\x2', ';', '\x3', '\x2', '\x2', '\x2', '\x2', '=', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '?', '\x3', '\x2', '\x2', '\x2', '\x2', '\x41', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x43', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x45', '\x3', '\x2', '\x2', '\x2', '\x2', 'G', '\x3', '\x2', '\x2', '\x2', 
		'\x2', 'I', '\x3', '\x2', '\x2', '\x2', '\x2', 'K', '\x3', '\x2', '\x2', 
		'\x2', '\x2', 'M', '\x3', '\x2', '\x2', '\x2', '\x2', 'O', '\x3', '\x2', 
		'\x2', '\x2', '\x2', 'Q', '\x3', '\x2', '\x2', '\x2', '\x2', 'S', '\x3', 
		'\x2', '\x2', '\x2', '\x2', 'U', '\x3', '\x2', '\x2', '\x2', '\x2', 'W', 
		'\x3', '\x2', '\x2', '\x2', '\x3', 'Y', '\x3', '\x2', '\x2', '\x2', '\x5', 
		'^', '\x3', '\x2', '\x2', '\x2', '\a', '\x66', '\x3', '\x2', '\x2', '\x2', 
		'\t', 'm', '\x3', '\x2', '\x2', '\x2', '\v', 's', '\x3', '\x2', '\x2', 
		'\x2', '\r', '\x7F', '\x3', '\x2', '\x2', '\x2', '\xF', '\x88', '\x3', 
		'\x2', '\x2', '\x2', '\x11', '\x8D', '\x3', '\x2', '\x2', '\x2', '\x13', 
		'\x93', '\x3', '\x2', '\x2', '\x2', '\x15', '\x96', '\x3', '\x2', '\x2', 
		'\x2', '\x17', '\x99', '\x3', '\x2', '\x2', '\x2', '\x19', '\x9B', '\x3', 
		'\x2', '\x2', '\x2', '\x1B', '\x9D', '\x3', '\x2', '\x2', '\x2', '\x1D', 
		'\xA3', '\x3', '\x2', '\x2', '\x2', '\x1F', '\xA5', '\x3', '\x2', '\x2', 
		'\x2', '!', '\xA7', '\x3', '\x2', '\x2', '\x2', '#', '\xA9', '\x3', '\x2', 
		'\x2', '\x2', '%', '\xAB', '\x3', '\x2', '\x2', '\x2', '\'', '\xAD', '\x3', 
		'\x2', '\x2', '\x2', ')', '\xAF', '\x3', '\x2', '\x2', '\x2', '+', '\xB1', 
		'\x3', '\x2', '\x2', '\x2', '-', '\xB3', '\x3', '\x2', '\x2', '\x2', '/', 
		'\xB5', '\x3', '\x2', '\x2', '\x2', '\x31', '\xB7', '\x3', '\x2', '\x2', 
		'\x2', '\x33', '\xB9', '\x3', '\x2', '\x2', '\x2', '\x35', '\xBB', '\x3', 
		'\x2', '\x2', '\x2', '\x37', '\xC0', '\x3', '\x2', '\x2', '\x2', '\x39', 
		'\xC5', '\x3', '\x2', '\x2', '\x2', ';', '\xCB', '\x3', '\x2', '\x2', 
		'\x2', '=', '\xD0', '\x3', '\x2', '\x2', '\x2', '?', '\xD8', '\x3', '\x2', 
		'\x2', '\x2', '\x41', '\xDF', '\x3', '\x2', '\x2', '\x2', '\x43', '\xE5', 
		'\x3', '\x2', '\x2', '\x2', '\x45', '\xE9', '\x3', '\x2', '\x2', '\x2', 
		'G', '\xEE', '\x3', '\x2', '\x2', '\x2', 'I', '\xF3', '\x3', '\x2', '\x2', 
		'\x2', 'K', '\xF9', '\x3', '\x2', '\x2', '\x2', 'M', '\xFF', '\x3', '\x2', 
		'\x2', '\x2', 'O', '\x106', '\x3', '\x2', '\x2', '\x2', 'Q', '\x10E', 
		'\x3', '\x2', '\x2', '\x2', 'S', '\x110', '\x3', '\x2', '\x2', '\x2', 
		'U', '\x112', '\x3', '\x2', '\x2', '\x2', 'W', '\x115', '\x3', '\x2', 
		'\x2', '\x2', 'Y', 'Z', '\a', 'U', '\x2', '\x2', 'Z', '[', '\a', 'R', 
		'\x2', '\x2', '[', '\\', '\a', 'G', '\x2', '\x2', '\\', ']', '\a', '\x45', 
		'\x2', '\x2', ']', '\x4', '\x3', '\x2', '\x2', '\x2', '^', '_', '\a', 
		'Q', '\x2', '\x2', '_', '`', '\a', '\x44', '\x2', '\x2', '`', '\x61', 
		'\a', 'L', '\x2', '\x2', '\x61', '\x62', '\a', 'G', '\x2', '\x2', '\x62', 
		'\x63', '\a', '\x45', '\x2', '\x2', '\x63', '\x64', '\a', 'V', '\x2', 
		'\x2', '\x64', '\x65', '\a', 'U', '\x2', '\x2', '\x65', '\x6', '\x3', 
		'\x2', '\x2', '\x2', '\x66', 'g', '\a', 'G', '\x2', '\x2', 'g', 'h', '\a', 
		'X', '\x2', '\x2', 'h', 'i', '\a', 'G', '\x2', '\x2', 'i', 'j', '\a', 
		'P', '\x2', '\x2', 'j', 'k', '\a', 'V', '\x2', '\x2', 'k', 'l', '\a', 
		'U', '\x2', '\x2', 'l', '\b', '\x3', '\x2', '\x2', '\x2', 'm', 'n', '\a', 
		'Q', '\x2', '\x2', 'n', 'o', '\a', 'T', '\x2', '\x2', 'o', 'p', '\a', 
		'\x46', '\x2', '\x2', 'p', 'q', '\a', 'G', '\x2', '\x2', 'q', 'r', '\a', 
		'T', '\x2', '\x2', 'r', '\n', '\x3', '\x2', '\x2', '\x2', 's', 't', '\a', 
		'\x45', '\x2', '\x2', 't', 'u', '\a', 'Q', '\x2', '\x2', 'u', 'v', '\a', 
		'P', '\x2', '\x2', 'v', 'w', '\a', 'U', '\x2', '\x2', 'w', 'x', '\a', 
		'V', '\x2', '\x2', 'x', 'y', '\a', 'T', '\x2', '\x2', 'y', 'z', '\a', 
		'\x43', '\x2', '\x2', 'z', '{', '\a', 'K', '\x2', '\x2', '{', '|', '\a', 
		'P', '\x2', '\x2', '|', '}', '\a', 'V', '\x2', '\x2', '}', '~', '\a', 
		'U', '\x2', '\x2', '~', '\f', '\x3', '\x2', '\x2', '\x2', '\x7F', '\x80', 
		'\a', 'G', '\x2', '\x2', '\x80', '\x81', '\a', 'P', '\x2', '\x2', '\x81', 
		'\x82', '\a', 'U', '\x2', '\x2', '\x82', '\x83', '\a', 'W', '\x2', '\x2', 
		'\x83', '\x84', '\a', 'T', '\x2', '\x2', '\x84', '\x85', '\a', 'G', '\x2', 
		'\x2', '\x85', '\x86', '\a', 'U', '\x2', '\x2', '\x86', '\xE', '\x3', 
		'\x2', '\x2', '\x2', '\x87', '\x89', '\a', '\xF', '\x2', '\x2', '\x88', 
		'\x87', '\x3', '\x2', '\x2', '\x2', '\x88', '\x89', '\x3', '\x2', '\x2', 
		'\x2', '\x89', '\x8A', '\x3', '\x2', '\x2', '\x2', '\x8A', '\x8B', '\a', 
		'\f', '\x2', '\x2', '\x8B', '\x10', '\x3', '\x2', '\x2', '\x2', '\x8C', 
		'\x8E', '\t', '\x2', '\x2', '\x2', '\x8D', '\x8C', '\x3', '\x2', '\x2', 
		'\x2', '\x8E', '\x8F', '\x3', '\x2', '\x2', '\x2', '\x8F', '\x8D', '\x3', 
		'\x2', '\x2', '\x2', '\x8F', '\x90', '\x3', '\x2', '\x2', '\x2', '\x90', 
		'\x91', '\x3', '\x2', '\x2', '\x2', '\x91', '\x92', '\b', '\t', '\x2', 
		'\x2', '\x92', '\x12', '\x3', '\x2', '\x2', '\x2', '\x93', '\x94', '\a', 
		'?', '\x2', '\x2', '\x94', '\x95', '\a', '@', '\x2', '\x2', '\x95', '\x14', 
		'\x3', '\x2', '\x2', '\x2', '\x96', '\x97', '\a', 'k', '\x2', '\x2', '\x97', 
		'\x98', '\a', 'p', '\x2', '\x2', '\x98', '\x16', '\x3', '\x2', '\x2', 
		'\x2', '\x99', '\x9A', '\a', ']', '\x2', '\x2', '\x9A', '\x18', '\x3', 
		'\x2', '\x2', '\x2', '\x9B', '\x9C', '\a', '_', '\x2', '\x2', '\x9C', 
		'\x1A', '\x3', '\x2', '\x2', '\x2', '\x9D', '\x9E', '\a', '\x63', '\x2', 
		'\x2', '\x9E', '\x9F', '\a', 'h', '\x2', '\x2', '\x9F', '\xA0', '\a', 
		'v', '\x2', '\x2', '\xA0', '\xA1', '\a', 'g', '\x2', '\x2', '\xA1', '\xA2', 
		'\a', 't', '\x2', '\x2', '\xA2', '\x1C', '\x3', '\x2', '\x2', '\x2', '\xA3', 
		'\xA4', '\a', '$', '\x2', '\x2', '\xA4', '\x1E', '\x3', '\x2', '\x2', 
		'\x2', '\xA5', '\xA6', '\a', '}', '\x2', '\x2', '\xA6', ' ', '\x3', '\x2', 
		'\x2', '\x2', '\xA7', '\xA8', '\a', '\x7F', '\x2', '\x2', '\xA8', '\"', 
		'\x3', '\x2', '\x2', '\x2', '\xA9', '\xAA', '\a', '~', '\x2', '\x2', '\xAA', 
		'$', '\x3', '\x2', '\x2', '\x2', '\xAB', '\xAC', '\a', '(', '\x2', '\x2', 
		'\xAC', '&', '\x3', '\x2', '\x2', '\x2', '\xAD', '\xAE', '\a', '?', '\x2', 
		'\x2', '\xAE', '(', '\x3', '\x2', '\x2', '\x2', '\xAF', '\xB0', '\a', 
		'<', '\x2', '\x2', '\xB0', '*', '\x3', '\x2', '\x2', '\x2', '\xB1', '\xB2', 
		'\a', '*', '\x2', '\x2', '\xB2', ',', '\x3', '\x2', '\x2', '\x2', '\xB3', 
		'\xB4', '\a', '+', '\x2', '\x2', '\xB4', '.', '\x3', '\x2', '\x2', '\x2', 
		'\xB5', '\xB6', '\a', '\x61', '\x2', '\x2', '\xB6', '\x30', '\x3', '\x2', 
		'\x2', '\x2', '\xB7', '\xB8', '\t', '\x3', '\x2', '\x2', '\xB8', '\x32', 
		'\x3', '\x2', '\x2', '\x2', '\xB9', '\xBA', '\a', '.', '\x2', '\x2', '\xBA', 
		'\x34', '\x3', '\x2', '\x2', '\x2', '\xBB', '\xBC', '\a', '\x64', '\x2', 
		'\x2', '\xBC', '\xBD', '\a', 'q', '\x2', '\x2', '\xBD', '\xBE', '\a', 
		'q', '\x2', '\x2', '\xBE', '\xBF', '\a', 'n', '\x2', '\x2', '\xBF', '\x36', 
		'\x3', '\x2', '\x2', '\x2', '\xC0', '\xC1', '\a', '\x64', '\x2', '\x2', 
		'\xC1', '\xC2', '\a', '{', '\x2', '\x2', '\xC2', '\xC3', '\a', 'v', '\x2', 
		'\x2', '\xC3', '\xC4', '\a', 'g', '\x2', '\x2', '\xC4', '\x38', '\x3', 
		'\x2', '\x2', '\x2', '\xC5', '\xC6', '\a', 'u', '\x2', '\x2', '\xC6', 
		'\xC7', '\a', '\x64', '\x2', '\x2', '\xC7', '\xC8', '\a', '{', '\x2', 
		'\x2', '\xC8', '\xC9', '\a', 'v', '\x2', '\x2', '\xC9', '\xCA', '\a', 
		'g', '\x2', '\x2', '\xCA', ':', '\x3', '\x2', '\x2', '\x2', '\xCB', '\xCC', 
		'\a', '\x65', '\x2', '\x2', '\xCC', '\xCD', '\a', 'j', '\x2', '\x2', '\xCD', 
		'\xCE', '\a', '\x63', '\x2', '\x2', '\xCE', '\xCF', '\a', 't', '\x2', 
		'\x2', '\xCF', '<', '\x3', '\x2', '\x2', '\x2', '\xD0', '\xD1', '\a', 
		'\x66', '\x2', '\x2', '\xD1', '\xD2', '\a', 'g', '\x2', '\x2', '\xD2', 
		'\xD3', '\a', '\x65', '\x2', '\x2', '\xD3', '\xD4', '\a', 'k', '\x2', 
		'\x2', '\xD4', '\xD5', '\a', 'o', '\x2', '\x2', '\xD5', '\xD6', '\a', 
		'\x63', '\x2', '\x2', '\xD6', '\xD7', '\a', 'n', '\x2', '\x2', '\xD7', 
		'>', '\x3', '\x2', '\x2', '\x2', '\xD8', '\xD9', '\a', '\x66', '\x2', 
		'\x2', '\xD9', '\xDA', '\a', 'q', '\x2', '\x2', '\xDA', '\xDB', '\a', 
		'w', '\x2', '\x2', '\xDB', '\xDC', '\a', '\x64', '\x2', '\x2', '\xDC', 
		'\xDD', '\a', 'n', '\x2', '\x2', '\xDD', '\xDE', '\a', 'g', '\x2', '\x2', 
		'\xDE', '@', '\x3', '\x2', '\x2', '\x2', '\xDF', '\xE0', '\a', 'h', '\x2', 
		'\x2', '\xE0', '\xE1', '\a', 'n', '\x2', '\x2', '\xE1', '\xE2', '\a', 
		'q', '\x2', '\x2', '\xE2', '\xE3', '\a', '\x63', '\x2', '\x2', '\xE3', 
		'\xE4', '\a', 'v', '\x2', '\x2', '\xE4', '\x42', '\x3', '\x2', '\x2', 
		'\x2', '\xE5', '\xE6', '\a', 'k', '\x2', '\x2', '\xE6', '\xE7', '\a', 
		'p', '\x2', '\x2', '\xE7', '\xE8', '\a', 'v', '\x2', '\x2', '\xE8', '\x44', 
		'\x3', '\x2', '\x2', '\x2', '\xE9', '\xEA', '\a', 'w', '\x2', '\x2', '\xEA', 
		'\xEB', '\a', 'k', '\x2', '\x2', '\xEB', '\xEC', '\a', 'p', '\x2', '\x2', 
		'\xEC', '\xED', '\a', 'v', '\x2', '\x2', '\xED', '\x46', '\x3', '\x2', 
		'\x2', '\x2', '\xEE', '\xEF', '\a', 'n', '\x2', '\x2', '\xEF', '\xF0', 
		'\a', 'q', '\x2', '\x2', '\xF0', '\xF1', '\a', 'p', '\x2', '\x2', '\xF1', 
		'\xF2', '\a', 'i', '\x2', '\x2', '\xF2', 'H', '\x3', '\x2', '\x2', '\x2', 
		'\xF3', '\xF4', '\a', 'w', '\x2', '\x2', '\xF4', '\xF5', '\a', 'n', '\x2', 
		'\x2', '\xF5', '\xF6', '\a', 'q', '\x2', '\x2', '\xF6', '\xF7', '\a', 
		'p', '\x2', '\x2', '\xF7', '\xF8', '\a', 'i', '\x2', '\x2', '\xF8', 'J', 
		'\x3', '\x2', '\x2', '\x2', '\xF9', '\xFA', '\a', 'u', '\x2', '\x2', '\xFA', 
		'\xFB', '\a', 'j', '\x2', '\x2', '\xFB', '\xFC', '\a', 'q', '\x2', '\x2', 
		'\xFC', '\xFD', '\a', 't', '\x2', '\x2', '\xFD', '\xFE', '\a', 'v', '\x2', 
		'\x2', '\xFE', 'L', '\x3', '\x2', '\x2', '\x2', '\xFF', '\x100', '\a', 
		'w', '\x2', '\x2', '\x100', '\x101', '\a', 'u', '\x2', '\x2', '\x101', 
		'\x102', '\a', 'j', '\x2', '\x2', '\x102', '\x103', '\a', 'q', '\x2', 
		'\x2', '\x103', '\x104', '\a', 't', '\x2', '\x2', '\x104', '\x105', '\a', 
		'v', '\x2', '\x2', '\x105', 'N', '\x3', '\x2', '\x2', '\x2', '\x106', 
		'\x10B', '\x5', 'Q', ')', '\x2', '\x107', '\x10A', '\x5', 'Q', ')', '\x2', 
		'\x108', '\x10A', '\x5', 'S', '*', '\x2', '\x109', '\x107', '\x3', '\x2', 
		'\x2', '\x2', '\x109', '\x108', '\x3', '\x2', '\x2', '\x2', '\x10A', '\x10D', 
		'\x3', '\x2', '\x2', '\x2', '\x10B', '\x109', '\x3', '\x2', '\x2', '\x2', 
		'\x10B', '\x10C', '\x3', '\x2', '\x2', '\x2', '\x10C', 'P', '\x3', '\x2', 
		'\x2', '\x2', '\x10D', '\x10B', '\x3', '\x2', '\x2', '\x2', '\x10E', '\x10F', 
		'\t', '\x4', '\x2', '\x2', '\x10F', 'R', '\x3', '\x2', '\x2', '\x2', '\x110', 
		'\x111', '\t', '\x5', '\x2', '\x2', '\x111', 'T', '\x3', '\x2', '\x2', 
		'\x2', '\x112', '\x113', '\a', '=', '\x2', '\x2', '\x113', 'V', '\x3', 
		'\x2', '\x2', '\x2', '\x114', '\x116', '\t', '\x6', '\x2', '\x2', '\x115', 
		'\x114', '\x3', '\x2', '\x2', '\x2', '\x116', '\x117', '\x3', '\x2', '\x2', 
		'\x2', '\x117', '\x115', '\x3', '\x2', '\x2', '\x2', '\x117', '\x118', 
		'\x3', '\x2', '\x2', '\x2', '\x118', 'X', '\x3', '\x2', '\x2', '\x2', 
		'\b', '\x2', '\x88', '\x8F', '\x109', '\x10B', '\x117', '\x3', '\b', '\x2', 
		'\x2',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
