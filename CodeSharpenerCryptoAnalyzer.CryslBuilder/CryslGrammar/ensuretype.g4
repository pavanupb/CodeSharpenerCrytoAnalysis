grammar ensuretype;

/** grammar for ensuretype in crysl */

ensureslist: (ensure SEMICOLON)+;

ensure: VARNAME OSB VARNAME (COMMA VARNAME)* CSB (AFTER VARNAME (COMMA VARNAME)*)?;


NEWLINE:'\r'? '\n'; // return newlines to parser (is end-statement signal)
WS : [ \t\r\n]+ -> skip ; // toss out whitespace
