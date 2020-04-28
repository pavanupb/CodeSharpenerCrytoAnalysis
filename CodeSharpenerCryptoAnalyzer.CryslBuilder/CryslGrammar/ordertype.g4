grammar ordertype;

/** grammar for ordertype in crysl */

orderlist : VARNAME REGEX? (COMMA VARNAME (REGEX)?)*;


