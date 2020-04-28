grammar commontype;

/** Grammar for common types */

IMPLIES : '=>' ;
IN : 'in' ;
OSB : '[';
CSB : ']';
AFTER : 'after';
QTS : '"' ;
OFB : '{' ;
CFB : '}';
OR : '|';
AND : '&';
EQUALS : '=';
COLON : ':';
OP : '(';
CP : ')';
UNSCORE : '_';
REGEX : ('*' | '+' | '?');
COMMA : ',';
BOOL : 'bool';
BYTE : 'byte';
SBYTE : 'sbyte';
CHAR : 'char';
DECIMAL : 'decimal';
DOUBLE : 'double';
FLOAT : 'float';
INT : 'int';
UINT : 'uint';
LONG : 'long';
ULONG : 'ulong';
SHORT : 'short';
USHORT : 'ushort';
VARNAME : ALPHA (ALPHA | DIGIT)*;
ALPHA : [a-zA-Z_];
DIGIT : [0-9];
SEMICOLON : ';';
TYPE : [a-zA-Z.]+;
