grammar CryslGrammar;

/** grammar for crysl rules */
import objecttype, eventtype, ordertype, constrainttype, ensuretype, commontype;

cryslsection: specsection objectssection eventssection orderssection constraintssection ensuressection;

specsection : SPECSECTIONNAME TYPE;
objectssection : OBJECTSSECTIONNAME objects;
eventssection : EVENTSSECTIONNAME eventlist;
orderssection : ORDERSSECTIONNAME orderlist;
constraintssection : CONSTRAINTSSECTIONNAME constraintslist;
ensuressection: ENSURESSECTIONNAME ensureslist;

SPECSECTIONNAME : 'SPEC';
OBJECTSSECTIONNAME : 'OBJECTS';
EVENTSSECTIONNAME : 'EVENTS';
ORDERSSECTIONNAME : 'ORDER';
CONSTRAINTSSECTIONNAME : 'CONSTRAINTS';
ENSURESSECTIONNAME : 'ENSURES';

NEWLINE:'\r'? '\n' ; // return newlines to parser (is end-statement signal)
WS : [ \t\r\n]+ -> skip ; // toss out whitespace