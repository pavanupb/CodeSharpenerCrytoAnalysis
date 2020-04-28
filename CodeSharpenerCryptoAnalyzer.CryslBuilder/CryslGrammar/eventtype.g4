grammar eventtype;

/** grammar for eventtype in crysl */


eventlist : (events)+;
events : (event)+ aggregator	#WithAggregator
		 | sngevent				#WithoutAggregator
		 ;

aggregator : VARNAME COLON EQUALS VARNAME (OR VARNAME | AND VARNAME)* SEMICOLON;

event : VARNAME COLON VARNAME OP (VARNAME | UNSCORE) (COMMA VARNAME | COMMA UNSCORE)* CP SEMICOLON		#WithArguments
			| VARNAME COLON VARNAME OP CP SEMICOLON		#WithoutArguments
			| VARNAME COLON VARNAME EQUALS VARNAME SEMICOLON		#WithPropertiesOnly
			| VARNAME COLON VARNAME EQUALS VARNAME OP (VARNAME | UNSCORE) (COMMA VARNAME | COMMA UNSCORE)* CP SEMICOLON		#ObjectAssnWithArguments
			| VARNAME COLON VARNAME EQUALS VARNAME OP CP SEMICOLON		#ObjectAssnWithoutArguments
			;

sngevent : VARNAME COLON VARNAME EQUALS VARNAME SEMICOLON		#SngEventNoArguments
		   | VARNAME COLON VARNAME EQUALS VARNAME  OP CP SEMICOLON		#SngEventWithoutArguments
		   | VARNAME COLON VARNAME EQUALS VARNAME OP (VARNAME | UNSCORE) (COMMA VARNAME | COMMA UNSCORE)* CP SEMICOLON		#SngEventWithArgumentsUnscore		   
		   ;




		 