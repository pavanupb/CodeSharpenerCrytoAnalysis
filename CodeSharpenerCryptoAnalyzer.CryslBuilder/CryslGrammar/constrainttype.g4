grammar constrainttype;

/** grammar for constraint type in crysl */


constraintslist : (constraints)+ ;
constraints : (constraint SEMICOLON)+;

constraint : VARNAME IN OFB (DIGIT)+ (COMMA (DIGIT)+)* CFB (IMPLIES constraint)?		#IntArguments
			 | VARNAME IN OFB QTS VARNAME QTS (COMMA QTS VARNAME QTS)* CFB (IMPLIES constraint)?		#StringArguments
			 ;


WS : [ \t\r\n]+ -> skip ; // toss out whitespace
 