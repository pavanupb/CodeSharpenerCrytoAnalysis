grammar objecttype;

/** grammar for objecttype in crysl */

objects :	(objectlist SEMICOLON)+;		
			
objectlist : INT VARNAME		#IntValue 
		  | BYTE VARNAME		#ByteValue
		  | SBYTE VARNAME		#SbyteValue  
		  | CHAR VARNAME 		#CharValue 
		  | DECIMAL VARNAME		#DecimalValue 
		  | DOUBLE VARNAME		#DoubleValue  
		  | FLOAT VARNAME		#FloatValue  
		  | UINT VARNAME		#UintValue  
		  | LONG VARNAME		#LongValue 
		  | SHORT VARNAME		#ShortValue  
		  | USHORT VARNAME		#UshortValue 
		  | BOOL VARNAME		#BoolValue 
		  | TYPE VARNAME		#TypeValue
		  ;



