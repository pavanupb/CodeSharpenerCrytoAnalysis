using CryslData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CryslParser.Visitors
{
    public class CryslGrammarVisitor : CryslGrammarBaseVisitor<object>
    {
        CryslJsonModel cryslModel = new CryslJsonModel();

        public override object VisitCryslsection(CryslGrammarParser.CryslsectionContext context)
        {
            Visit(context.specsection());
            Visit(context.objectssection());
            Visit(context.eventssection());
            Visit(context.orderssection());
            Visit(context.constraintssection());
            Visit(context.ensuressection());

            return cryslModel;

        }


        #region SPEC SECTION
        public override object VisitSpecsection(CryslGrammarParser.SpecsectionContext context)
        {
            SectionSpec specSection = new SectionSpec();
            string sectionName = context.SPECSECTIONNAME().GetText();
            string specSectionValue = context.TYPE().GetText();

            specSection.Crysl_Section = sectionName;
            specSection.Class_Name = specSectionValue;

            cryslModel.Spec_Section = specSection;           
            
            return 0;
        }
        #endregion


        #region OBJECTS SECTION
        public override object VisitObjectssection(CryslGrammarParser.ObjectssectionContext context)
        {
            SectionObject objectSection = new SectionObject();
            string sectionName = context.OBJECTSSECTIONNAME().GetText();
            
            objectSection.Crysl_Section = sectionName;
            List<ObjectsDeclaration> objectsDeclarationList = (List<ObjectsDeclaration>)Visit(context.objects());            
            objectSection.Objects_Declaration = objectsDeclarationList;

            cryslModel.Object_Section = objectSection;
            return 0;
        }

        public override object VisitObjects(CryslGrammarParser.ObjectsContext context)
        {
            List<ObjectsDeclaration> objectDeclarationList = new List<ObjectsDeclaration>();
            foreach (var objects in context.objectlist())
            {
                ObjectsDeclaration declaration = (ObjectsDeclaration)Visit(objects);
                objectDeclarationList.Add(declaration);
            }
            return objectDeclarationList;
        }

        public override object VisitIntValue(CryslGrammarParser.IntValueContext context)
        {
            string intType = context.INT().GetText();
            string varName = context.VARNAME().GetText();

            ObjectsDeclaration objectDeclaration = new ObjectsDeclaration();
            objectDeclaration.Object_type = intType;
            objectDeclaration.Var_name = varName;
            
            return objectDeclaration;
        }

        public override object VisitByteValue(CryslGrammarParser.ByteValueContext context)
        {
            string byteType = context.BYTE().GetText();
            string varName = context.VARNAME().GetText();
            ObjectsDeclaration objectDeclaration = new ObjectsDeclaration();
            objectDeclaration.Object_type = byteType;
            objectDeclaration.Var_name = varName;

            return objectDeclaration;
        }

        public override object VisitSbyteValue(CryslGrammarParser.SbyteValueContext context)
        {
            string sbyteType = context.SBYTE().GetText();
            string varName = context.VARNAME().GetText();
            ObjectsDeclaration objectDeclaration = new ObjectsDeclaration();
            objectDeclaration.Object_type = sbyteType;
            objectDeclaration.Var_name = varName;

            return objectDeclaration;
        }

        public override object VisitCharValue(CryslGrammarParser.CharValueContext context)
        {
            string charType = context.CHAR().GetText();
            string varName = context.VARNAME().GetText();
            ObjectsDeclaration objectDeclaration = new ObjectsDeclaration();
            objectDeclaration.Object_type = charType;
            objectDeclaration.Var_name = varName;

            return objectDeclaration;
        }

        public override object VisitDecimalValue(CryslGrammarParser.DecimalValueContext context)
        {
            string decimalType = context.DECIMAL().GetText();
            string varName = context.VARNAME().GetText();
            ObjectsDeclaration objectDeclaration = new ObjectsDeclaration();
            objectDeclaration.Object_type = decimalType;
            objectDeclaration.Var_name = varName;

            return objectDeclaration;
        }

        public override object VisitDoubleValue(CryslGrammarParser.DoubleValueContext context)
        {
            string doubleType = context.DOUBLE().GetText();
            string varName = context.VARNAME().GetText();
            ObjectsDeclaration objectDeclaration = new ObjectsDeclaration();
            objectDeclaration.Object_type = doubleType;
            objectDeclaration.Var_name = varName;

            return objectDeclaration;
        }

        public override object VisitFloatValue(CryslGrammarParser.FloatValueContext context)
        {
            string floatType = context.FLOAT().GetText();
            string varName = context.VARNAME().GetText();
            ObjectsDeclaration objectDeclaration = new ObjectsDeclaration();
            objectDeclaration.Object_type = floatType;
            objectDeclaration.Var_name = varName;

            return objectDeclaration;
        }

        public override object VisitUintValue(CryslGrammarParser.UintValueContext context)
        {
            string uintType = context.UINT().GetText();
            string varName = context.VARNAME().GetText();
            ObjectsDeclaration objectDeclaration = new ObjectsDeclaration();
            objectDeclaration.Object_type = uintType;
            objectDeclaration.Var_name = varName;

            return objectDeclaration;
        }

        public override object VisitLongValue(CryslGrammarParser.LongValueContext context)
        {
            string longType = context.LONG().GetText();
            string varName = context.VARNAME().GetText();
            ObjectsDeclaration objectDeclaration = new ObjectsDeclaration();
            objectDeclaration.Object_type = longType;
            objectDeclaration.Var_name = varName;

            return objectDeclaration;
        }

        public override object VisitShortValue(CryslGrammarParser.ShortValueContext context)
        {
            string shortType = context.SHORT().GetText();
            string varName = context.VARNAME().GetText();
            ObjectsDeclaration objectDeclaration = new ObjectsDeclaration();
            objectDeclaration.Object_type = shortType;
            objectDeclaration.Var_name = varName;

            return objectDeclaration;
        }

        public override object VisitUshortValue(CryslGrammarParser.UshortValueContext context)
        {
            string ushortType = context.USHORT().GetText();
            string varName = context.VARNAME().GetText();
            ObjectsDeclaration objectDeclaration = new ObjectsDeclaration();
            objectDeclaration.Object_type = ushortType;
            objectDeclaration.Var_name = varName;

            return objectDeclaration;
        }

        public override object VisitBoolValue(CryslGrammarParser.BoolValueContext context)
        {
            string boolType = context.BOOL().GetText();
            string varName = context.VARNAME().GetText();
            ObjectsDeclaration objectDeclaration = new ObjectsDeclaration();
            objectDeclaration.Object_type = boolType;
            objectDeclaration.Var_name = varName;

            return objectDeclaration;
        }

        public override object VisitTypeValue(CryslGrammarParser.TypeValueContext context)
        {
            string typeValueType = context.TYPE().GetText();
            string varName = context.VARNAME().GetText();
            ObjectsDeclaration objectDeclaration = new ObjectsDeclaration();
            objectDeclaration.Object_type = typeValueType;
            objectDeclaration.Var_name = varName;

            return objectDeclaration;
        }
        #endregion

        #region EVENTS SECTION
        public override object VisitEventssection(CryslGrammarParser.EventssectionContext context)
        {
            SectionEvent eventSection = new SectionEvent();
            string sectionName = context.EVENTSSECTIONNAME().GetText();            
            eventSection.Crysl_Section = sectionName;

            List<Methods> methods = (List<Methods>)Visit(context.eventlist());            
            eventSection.Methods = methods;

            cryslModel.Event_Section = eventSection;
            return 0;
        }

        public override object VisitEventlist(CryslGrammarParser.EventlistContext context)
        {
            List<Methods> methodList = new List<Methods>();
            foreach (var events in context.events())
            {
                Methods method = (Methods)Visit(events);
                methodList.Add(method);
            }
            return methodList;
        }

        public override object VisitWithAggregator(CryslGrammarParser.WithAggregatorContext context)
        {
            List<CryptoSignature> cryptoSignatureList = new List<CryptoSignature>();
            Methods eventMethods = new Methods();
            
            foreach (var events in context.@event())
            {
                CryptoSignature cryptoSignature = (CryptoSignature)Visit(events);
                cryptoSignatureList.Add(cryptoSignature);
            }
            eventMethods.Crypto_Signature = cryptoSignatureList;
            Aggregator aggregator = (Aggregator)Visit(context.aggregator());
            eventMethods.Aggregator = aggregator;
            return eventMethods;
        }

        public override object VisitWithoutAggregator(CryslGrammarParser.WithoutAggregatorContext context)
        {
            List<CryptoSignature> cryptoSignatureList = new List<CryptoSignature>();
            Methods eventMethods = new Methods();
            
            CryptoSignature cryptoSignature = (CryptoSignature)Visit(context.sngevent());
            cryptoSignatureList.Add(cryptoSignature);
            
            eventMethods.Crypto_Signature = cryptoSignatureList;
            return eventMethods;
        }

        public override object VisitSngEventNoArguments(CryslGrammarParser.SngEventNoArgumentsContext context)
        {
            CryptoSignature cryptoSignature = new CryptoSignature();

            foreach (var varName in context.VARNAME())
            {
                //Check for crypto signature identifiers
                if (varName.Symbol.TokenIndex < context.COLON().Symbol.TokenIndex)
                {
                    cryptoSignature.Event_Var_Name = varName.GetText();
                }
                //Check for Object Assignment
                else if (varName.Symbol.TokenIndex > context.COLON().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.EQUALS().Symbol.TokenIndex)
                {
                    cryptoSignature.Object_variable = varName.GetText();
                }
                //Check for crypto signature methods
                else if (varName.Symbol.TokenIndex > context.EQUALS().Symbol.TokenIndex)
                {
                    cryptoSignature.Method_Name = varName.GetText();
                    cryptoSignature.Is_property = true;
                }
            }

            return cryptoSignature;
        }

        public override object VisitSngEventWithoutArguments(CryslGrammarParser.SngEventWithoutArgumentsContext context)
        {
            CryptoSignature cryptoSignature = new CryptoSignature();

            foreach (var varName in context.VARNAME())
            {
                //Check for crypto signature identifiers
                if (varName.Symbol.TokenIndex < context.COLON().Symbol.TokenIndex)
                {
                    cryptoSignature.Event_Var_Name = varName.GetText();
                }
                //Check for Object Assignment
                else if (varName.Symbol.TokenIndex > context.COLON().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.EQUALS().Symbol.TokenIndex)
                {
                    cryptoSignature.Object_variable = varName.GetText();
                }
                //Check for crypto signature methods
                else if (varName.Symbol.TokenIndex > context.EQUALS().Symbol.TokenIndex && (varName.Symbol.TokenIndex < context.OP().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.CP().Symbol.TokenIndex))
                {
                    cryptoSignature.Method_Name = varName.GetText();
                    cryptoSignature.Is_property = false;
                }
            }

            return cryptoSignature;
        }

        public override object VisitSngEventWithArgumentsUnscore(CryslGrammarParser.SngEventWithArgumentsUnscoreContext context)
        {
            Dictionary<int, string> argumentValues = new Dictionary<int, string>();
            CryptoSignature cryptoSignature = new CryptoSignature();
            //ArgumentTypes argumentTypes = new ArgumentTypes();
            List<ArgumentTypes> argumentsList = new List<ArgumentTypes>();

            foreach (var varName in context.VARNAME())
            {
                //Check for crypto signature identifiers
                if (varName.Symbol.TokenIndex < context.COLON().Symbol.TokenIndex)
                {
                    cryptoSignature.Event_Var_Name = varName.GetText();
                }

                //Check for Object Assignment
                else if (varName.Symbol.TokenIndex > context.COLON().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.EQUALS().Symbol.TokenIndex)
                {
                    cryptoSignature.Object_variable = varName.GetText();
                }
                //Check for crypto signature methods
                else if (varName.Symbol.TokenIndex > context.EQUALS().Symbol.TokenIndex && (varName.Symbol.TokenIndex < context.OP().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.CP().Symbol.TokenIndex))
                {
                    cryptoSignature.Method_Name = varName.GetText();
                    cryptoSignature.Is_property = false;
                }
                //Check for crypto signature argument values
                else if (varName.Symbol.TokenIndex > context.OP().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.CP().Symbol.TokenIndex)
                {
                    argumentValues.Add(varName.Symbol.TokenIndex, varName.GetText());
                }

            }
            //Check for optional arguments
            foreach (var opArguments in context.UNSCORE())
            {
                if (opArguments.Symbol.TokenIndex > context.OP().Symbol.TokenIndex && opArguments.Symbol.TokenIndex < context.CP().Symbol.TokenIndex)
                {
                    argumentValues.Add(opArguments.Symbol.TokenIndex, opArguments.GetText());
                }
            }
            var sortedArgumentsList = from arguments in argumentValues
                                      orderby arguments.Key ascending
                                      select arguments;

            foreach (KeyValuePair<int, string> sortedArguments in sortedArgumentsList)
            {
                ArgumentTypes argumentTypes = new ArgumentTypes();
                argumentTypes.Argument = sortedArguments.Value;
                argumentsList.Add(argumentTypes);
            }

            cryptoSignature.Argument_types = argumentsList;

            return cryptoSignature;
        }

        public override object VisitAggregator(CryslGrammarParser.AggregatorContext context)
        {            
            Aggregator aggregator = new Aggregator();
            List<Aggregators> aggregatorsList = new List<Aggregators>();            

            foreach (var varName in context.VARNAME())
            {
                Aggregators aggregators = new Aggregators();
                //check for aggregators name
                if (varName.Symbol.TokenIndex < context.COLON().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.EQUALS().Symbol.TokenIndex)
                {
                    aggregator.Aggregator_Name = varName.GetText();                    
                }

                //check for aggregators varname and regex between aggregators
                if (varName.Symbol.TokenIndex > context.COLON().Symbol.TokenIndex && varName.Symbol.TokenIndex > context.EQUALS().Symbol.TokenIndex)
                {                    
                    //aggregatorsDict.Add("aggregator_event_varname", varName.GetText());
                    aggregators.Aggregator_Event_Varname = varName.GetText();
                    
                    //Check for OR symbol
                    foreach (var aggOrSymbol in context.OR())
                    {
                        if (!String.IsNullOrEmpty(aggOrSymbol.GetText()))
                        {
                            int tokenDifference = aggOrSymbol.Symbol.TokenIndex - varName.Symbol.TokenIndex;
                            if (tokenDifference == 1)
                            { 
                                aggregators.Aggregator_Regex = aggOrSymbol.GetText();
                            }
                        }
                    }

                    //check for AND symbol
                    foreach (var aggAndSymbol in context.AND())
                    {
                        if (!String.IsNullOrEmpty(aggAndSymbol.GetText()))
                        {
                            int tokenDifference = aggAndSymbol.Symbol.TokenIndex - varName.Symbol.TokenIndex;
                            if (tokenDifference == 1)
                            {
                                //aggregatorsDict.Add("aggregator_regex", aggAndSymbol.GetText());
                                aggregators.Aggregator_Regex = aggAndSymbol.GetText();
                            }
                        }
                    }                   
                }
                if (!String.IsNullOrEmpty(aggregators.Aggregator_Event_Varname))
                {
                    aggregatorsList.Add(aggregators);
                }
            }
            aggregator.Aggregators = aggregatorsList;
            return aggregator;
        }

        public override object VisitWithArguments(CryslGrammarParser.WithArgumentsContext context)
        { 
            Dictionary<int, string> argumentValues = new Dictionary<int, string>();
            CryptoSignature cryptoSignature = new CryptoSignature();
            //ArgumentTypes argumentTypes = new ArgumentTypes();
            List<ArgumentTypes> argumentsList = new List<ArgumentTypes>();          

            foreach (var varName in context.VARNAME())
            {
                //Check for crypto signature identifiers
                if (varName.Symbol.TokenIndex < context.COLON().Symbol.TokenIndex)
                {
                    cryptoSignature.Event_Var_Name = varName.GetText();                    
                }
                //Check for crypto signature methods
                else if (varName.Symbol.TokenIndex > context.COLON().Symbol.TokenIndex && (varName.Symbol.TokenIndex < context.OP().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.CP().Symbol.TokenIndex))
                {
                    cryptoSignature.Method_Name = varName.GetText();
                    cryptoSignature.Is_property = false;                    
                }
                //Check for crypto signature argument values
                else if (varName.Symbol.TokenIndex > context.OP().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.CP().Symbol.TokenIndex)
                {              
                    argumentValues.Add(varName.Symbol.TokenIndex, varName.GetText());
                }

            }
            //Check for optional arguments
            foreach (var opArguments in context.UNSCORE())
            {
                if (opArguments.Symbol.TokenIndex > context.OP().Symbol.TokenIndex && opArguments.Symbol.TokenIndex < context.CP().Symbol.TokenIndex)
                { 
                    argumentValues.Add(opArguments.Symbol.TokenIndex, opArguments.GetText());
                }
            }
            var sortedArgumentsList = from arguments in argumentValues
                                orderby arguments.Key ascending
                                select arguments;

            foreach(KeyValuePair<int, string> sortedArguments in sortedArgumentsList)
            {
                ArgumentTypes argumentTypes = new ArgumentTypes();
                argumentTypes.Argument = sortedArguments.Value;
                argumentsList.Add(argumentTypes);
            }

            cryptoSignature.Argument_types = argumentsList;
            
            return cryptoSignature;
        }

        public override object VisitWithoutArguments(CryslGrammarParser.WithoutArgumentsContext context)
        {            
            CryptoSignature cryptoSignature = new CryptoSignature();

            foreach (var varName in context.VARNAME())
            {
                //Check for crypto signature identifiers
                if (varName.Symbol.TokenIndex < context.COLON().Symbol.TokenIndex)
                {
                    cryptoSignature.Event_Var_Name = varName.GetText();
                }
                //Check for crypto signature methods
                else if (varName.Symbol.TokenIndex > context.COLON().Symbol.TokenIndex && (varName.Symbol.TokenIndex < context.OP().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.CP().Symbol.TokenIndex))
                {
                    cryptoSignature.Method_Name = varName.GetText();
                    cryptoSignature.Is_property = false;
                    cryptoSignature.Argument_types = new Collection<ArgumentTypes>();
                }
            }
            
            return cryptoSignature;
        }

        public override object VisitWithPropertiesOnly(CryslGrammarParser.WithPropertiesOnlyContext context)
        {
            CryptoSignature cryptoSignature = new CryptoSignature();

            foreach (var varName in context.VARNAME())
            {
                //Check for crypto signature identifiers
                if (varName.Symbol.TokenIndex < context.COLON().Symbol.TokenIndex)
                {
                    cryptoSignature.Event_Var_Name = varName.GetText();
                }
                //Check for Object Assignment
                else if (varName.Symbol.TokenIndex > context.COLON().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.EQUALS().Symbol.TokenIndex)
                {
                    cryptoSignature.Object_variable = varName.GetText();
                }
                //Check for crypto signature methods
                else if (varName.Symbol.TokenIndex > context.EQUALS().Symbol.TokenIndex)
                {
                    cryptoSignature.Method_Name = varName.GetText();
                    cryptoSignature.Is_property = true;
                }
            }

            return cryptoSignature;
        }

        public override object VisitObjectAssnWithArguments(CryslGrammarParser.ObjectAssnWithArgumentsContext context)
        {
            Dictionary<int, string> argumentValues = new Dictionary<int, string>();
            CryptoSignature cryptoSignature = new CryptoSignature();
            //ArgumentTypes argumentTypes = new ArgumentTypes();
            List<ArgumentTypes> argumentsList = new List<ArgumentTypes>();

            foreach (var varName in context.VARNAME())
            {
                //Check for crypto signature identifiers
                if (varName.Symbol.TokenIndex < context.COLON().Symbol.TokenIndex)
                {
                    cryptoSignature.Event_Var_Name = varName.GetText();
                }

                //Check for Object Assignment
                else if(varName.Symbol.TokenIndex > context.COLON().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.EQUALS().Symbol.TokenIndex)
                {
                    cryptoSignature.Object_variable = varName.GetText();
                }
                //Check for crypto signature methods
                else if (varName.Symbol.TokenIndex > context.EQUALS().Symbol.TokenIndex && (varName.Symbol.TokenIndex < context.OP().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.CP().Symbol.TokenIndex))
                {
                    cryptoSignature.Method_Name = varName.GetText();
                    cryptoSignature.Is_property = false;
                }
                //Check for crypto signature argument values
                else if (varName.Symbol.TokenIndex > context.OP().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.CP().Symbol.TokenIndex)
                {
                    argumentValues.Add(varName.Symbol.TokenIndex, varName.GetText());
                }

            }
            //Check for optional arguments
            foreach (var opArguments in context.UNSCORE())
            {
                if (opArguments.Symbol.TokenIndex > context.OP().Symbol.TokenIndex && opArguments.Symbol.TokenIndex < context.CP().Symbol.TokenIndex)
                {
                    argumentValues.Add(opArguments.Symbol.TokenIndex, opArguments.GetText());
                }
            }
            var sortedArgumentsList = from arguments in argumentValues
                                      orderby arguments.Key ascending
                                      select arguments;

            foreach (KeyValuePair<int, string> sortedArguments in sortedArgumentsList)
            {
                ArgumentTypes argumentTypes = new ArgumentTypes();
                argumentTypes.Argument = sortedArguments.Value;
                argumentsList.Add(argumentTypes);
            }

            cryptoSignature.Argument_types = argumentsList;

            return cryptoSignature;
        }

        public override object VisitObjectAssnWithoutArguments(CryslGrammarParser.ObjectAssnWithoutArgumentsContext context)
        {
            CryptoSignature cryptoSignature = new CryptoSignature();

            foreach (var varName in context.VARNAME())
            {
                //Check for crypto signature identifiers
                if (varName.Symbol.TokenIndex < context.COLON().Symbol.TokenIndex)
                {
                    cryptoSignature.Event_Var_Name = varName.GetText();
                }
                //Check for Object Assignment
                else if (varName.Symbol.TokenIndex > context.COLON().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.EQUALS().Symbol.TokenIndex)
                {
                    cryptoSignature.Object_variable = varName.GetText();
                }
                //Check for crypto signature methods
                else if (varName.Symbol.TokenIndex > context.EQUALS().Symbol.TokenIndex && (varName.Symbol.TokenIndex < context.OP().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.CP().Symbol.TokenIndex))
                {
                    cryptoSignature.Method_Name = varName.GetText();
                    cryptoSignature.Is_property = false;
                }
            }

            return cryptoSignature;
        }

        #region TODO_TASKS
        //TODO: Crysl JSON schema needs to be changed to support the below condition
        /*public override object VisitWithPropertiesOnly(CryslGrammarParser.WithPropertiesOnlyContext context)
        {
            if (_cryslJsonModel.EventValueSig == null)
            {
                _cryslJsonModel.EventsValue.Add(
                    new Dictionary<object, object>
                    {
                        {"crypto_signature", _cryslJsonModel.EventSigValues = new List<Dictionary<object, object>>()}
                    });
            }
            foreach (var varName in context.VARNAME())
            {
                //Check for crypto signature identifiers
                if (varName.Symbol.TokenIndex < context.COLON().Symbol.TokenIndex)
                {
                    _cryslJsonModel.EventSigValues.Add(
                        new Dictionary<object, object>
                        {
                            {"event_var_name", varName.GetText() }
                        });
                }
                //Check for crypto signature methods
                else if (varName.Symbol.TokenIndex > context.COLON().Symbol.TokenIndex && )
                {
                    _cryslJsonModel.EventSigValues.Add(
                        new Dictionary<object, object>
                        {
                            {"method_name", varName.GetText() }

                        });
                }
            }
            return 0;
        }*/

        //TODO: Crysl JSON schema needs to be changed to support the below condition
        /*public override int VisitSngEventNoArguments(CryslGrammarParser.SngEventNoArgumentsContext context)
        {
            return base.VisitSngEventNoArguments(context);
        }*/

        //TODO: Crysl JSON schema needs to be changed to support the below condition
        /*public override int VisitSngEventWithoutArguments(CryslGrammarParser.SngEventWithoutArgumentsContext context)
        {
            return base.VisitSngEventWithoutArguments(context);
        }*/

        //TODO: Crysl JSON schema needs to be changed to support the below condition
        /*public override int VisitSngEventWithArgumentsUnscore(CryslGrammarParser.SngEventWithArgumentsUnscoreContext context)
        {
            return base.VisitSngEventWithArgumentsUnscore(context);
        }*/

        //TODO: Crysl JSON schema needs to be changed to support the below condition
        /*public override int VisitSngEventWithArgumentsVarname(CryslGrammarParser.SngEventWithArgumentsVarnameContext context)
        {
            return base.VisitSngEventWithArgumentsVarname(context);
        }*/
        #endregion
        #endregion

        #region ORDER SECTION
        public override object VisitOrderssection(CryslGrammarParser.OrderssectionContext context)
        {
            SectionOrder orderSection = new SectionOrder();
            string sectionName = context.ORDERSSECTIONNAME().GetText();
            orderSection.Crysl_Section = sectionName;

            List<EventOrder> eventOrder = (List<EventOrder>)Visit(context.orderlist());
            orderSection.Event_Order = eventOrder;

            cryslModel.Order_Section = orderSection;

            return 0;

        }

        public override object VisitOrderlist(CryslGrammarParser.OrderlistContext context)
        {
            List<EventOrder> eventOrderList = new List<EventOrder>();
            foreach(var aggregates in context.VARNAME())
            {
                EventOrder eventOrder = new EventOrder();
                eventOrder.Aggregates = aggregates.GetText();

                foreach(var regex in context.REGEX())
                {
                    int tokenDifference = regex.Symbol.TokenIndex - aggregates.Symbol.TokenIndex;
                    if(tokenDifference == 1)
                    {
                        eventOrder.Regex = regex.GetText();
                    }
                }
                eventOrderList.Add(eventOrder);
            }
            return eventOrderList;
        }
        #endregion

        #region CONSTRAINTS SECTION
        public override object VisitConstraintssection(CryslGrammarParser.ConstraintssectionContext context)
        {
            SectionConstraints constraintsSection = new SectionConstraints();
            string sectionName = context.CONSTRAINTSSECTIONNAME().GetText();
            constraintsSection.Crysl_Section = sectionName;

            List<Constraints> constraints = (List<Constraints>)Visit(context.constraintslist());
            constraintsSection.Constraints = constraints;

            cryslModel.Constraints_Section = constraintsSection;
            return 0;
        }

        public override object VisitConstraintslist(CryslGrammarParser.ConstraintslistContext context)
        {
            List<Constraints> constraintsList = new List<Constraints>();
            foreach(var constraints in context.constraints())
            {
                constraintsList = (List<Constraints>)Visit(constraints);
            }

            return constraintsList ;
        }

        public override object VisitConstraints(CryslGrammarParser.ConstraintsContext context)
        {
            List<Constraints> constraintsList = new List<Constraints>();
            foreach(var constraint in context.constraint())
            {
                Constraints constraints = (Constraints)Visit(constraint);
                constraintsList.Add(constraints);
            }
            return constraintsList;
        }

        public override object VisitIntArguments(CryslGrammarParser.IntArgumentsContext context)
        {           
            Constraints finalConstraints = new Constraints();

            if (context.IMPLIES() == null)
            {
                Constraints Constraints = BuildIntConstraintsObject(context);
                finalConstraints = Constraints;
            }

            if (context.IMPLIES() != null)
            {
                Constraints primaryConstraints = BuildIntConstraintsObject(context);

                Constraints additionalConstraints = (Constraints)Visit(context.constraint());

                AdditionalConstraints addConstraints = new AdditionalConstraints();
                addConstraints.Object_Varname_Additional_Constraint = additionalConstraints.Object_Varname;
                addConstraints.Additional_Constraints_List = additionalConstraints.Constraints_List;

                primaryConstraints.Additional_constraints = addConstraints;

                finalConstraints = primaryConstraints;
            }            
            return finalConstraints;
        }

        public override object VisitStringArguments(CryslGrammarParser.StringArgumentsContext context)
        {
            Constraints finalConstraints = new Constraints();            

            if(context.IMPLIES() == null)
            {
                Constraints Constraints = BuildStringConstraintsObject(context);
                finalConstraints = Constraints;
            }

            if(context.IMPLIES() != null)
            {
                Constraints primaryConstraints = BuildStringConstraintsObject(context);

                Constraints additionalConstraints = (Constraints)Visit(context.constraint());

                AdditionalConstraints addConstraints = new AdditionalConstraints();
                addConstraints.Object_Varname_Additional_Constraint = additionalConstraints.Object_Varname;
                addConstraints.Additional_Constraints_List = additionalConstraints.Constraints_List;

                primaryConstraints.Additional_constraints = addConstraints;

                finalConstraints = primaryConstraints;
            }
            return finalConstraints;
        }

        private Constraints BuildStringConstraintsObject(CryslGrammarParser.StringArgumentsContext context)
        {
            Constraints constraints = new Constraints();
            List<string> constraintsList = new List<string>();
            foreach (var varName in context.VARNAME())
            {
                //If Primary Constraint
                if (varName.Symbol.TokenIndex < context.IN().Symbol.TokenIndex)
                {
                    constraints.Object_Varname = varName.GetText();
                }

                //Primary Constraint List
                else if (varName.Symbol.TokenIndex > context.OFB().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.CFB().Symbol.TokenIndex)
                {
                    constraintsList.Add(varName.GetText());
                }
            }

            constraints.Constraints_List = constraintsList;
            return constraints;

        }        

        private Constraints BuildIntConstraintsObject(CryslGrammarParser.IntArgumentsContext context)
        {
            Constraints constraints = new Constraints();
            List<string> constraintsList = new List<string>();
            //If Primary Constraint
            if (context.VARNAME().Symbol.TokenIndex < context.IN().Symbol.TokenIndex)
            {
                constraints.Object_Varname = context.VARNAME().GetText();
            }

            StringBuilder constraintsBuilder = new StringBuilder();
            foreach (var digit in context.DIGIT())
            {
                //Primary Constraint List
                if (digit.Symbol.TokenIndex > context.OFB().Symbol.TokenIndex && digit.Symbol.TokenIndex < context.CFB().Symbol.TokenIndex)
                {               
                    constraintsBuilder.Append(digit.GetText());
                    
                    //Add comma between digits
                    foreach(var comma in context.COMMA())
                    {
                        int tokenDifference = comma.Symbol.TokenIndex - digit.Symbol.TokenIndex;
                        if(tokenDifference == 1)
                        {
                            constraintsBuilder.Append(comma.GetText());
                        }
                    }
                }
            }
            var constraintBuilderString = constraintsBuilder.ToString();
            var splitArray = constraintBuilderString.Split(',');
            constraintsList = constraintsBuilder.ToString().Split(',').ToList();

            constraints.Constraints_List = constraintsList;
            return constraints;

        }
        #endregion

        #region ENSURES SECTION
        public override object VisitEnsuressection(CryslGrammarParser.EnsuressectionContext context)
        {
            SectionEnsures ensureSection = new SectionEnsures();
            string sectionName = context.ENSURESSECTIONNAME().GetText();
            ensureSection.Crysl_Section = sectionName;

            List<EnsuresObject> ensuresList = (List<EnsuresObject>)Visit(context.ensureslist());

            ensureSection.EnsuresObject = ensuresList;
            cryslModel.Constraints_Ensures = ensureSection;

            return 0;
        }

        public override object VisitEnsureslist(CryslGrammarParser.EnsureslistContext context)
        {
            List<EnsuresObject> ensuresList = new List<EnsuresObject>();
            foreach(var ensure in context.ensure())
            {
                EnsuresObject ensuresObject = (EnsuresObject)Visit(ensure);
                ensuresList.Add(ensuresObject);
            }

            return ensuresList;
        }

        public override object VisitEnsure(CryslGrammarParser.EnsureContext context)
        {
            EnsuresObject ensuresObject = new EnsuresObject();
            List<string> ensuresList = new List<string>();
            List<string> afterEventsList = new List<string>();
            bool containsAfter = false;

            if(context.AFTER() != null)
            {                
                containsAfter = true;
            }
            foreach(var varName in context.VARNAME())
            {
                //Initialize the Function Name
                if(varName.Symbol.TokenIndex < context.OSB().Symbol.TokenIndex)
                {
                    ensuresObject.FuncName = varName.GetText();
                }

                //Compute the Ensures List
                else if(varName.Symbol.TokenIndex > context.OSB().Symbol.TokenIndex && varName.Symbol.TokenIndex < context.CSB().Symbol.TokenIndex)
                {
                    ensuresList.Add(varName.GetText());
                }
                
                //Check for After Events
                else if(containsAfter)
                {
                    if(varName.Symbol.TokenIndex > context.AFTER().Symbol.TokenIndex)
                    {
                        afterEventsList.Add(varName.GetText());
                    }
                }
            }

            ensuresObject.EnsuresList = ensuresList;
            ensuresObject.AfterEventsList = afterEventsList;

            return ensuresObject;
        }
        #endregion
    }
}
