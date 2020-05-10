using CodeSharpenerCryptoAnalysis.Models;
using CryslData;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeSharpenerCryptoAnalzer.Common
{
    public class CommonUtilities : ICommonUtilities
    {
        private const string delimiter = ",";
        /// <summary>
        /// Check if the Method Signature Matches with the Method Signature of the Crysl
        /// </summary>
        /// <param name="cryptoMethod"></param>
        /// <param name="methodSymbol"></param>
        /// <returns></returns>
        public bool IsMethodInEvents(CryptoSignature cryptoMethod, IMethodSymbol methodSymbol, ICollection<ObjectsDeclaration> objectsDeclarations)
        {
            bool isMethodInEvents = false;
            if (methodSymbol != null)
            {
                if (cryptoMethod.Method_Name.Equals(methodSymbol.Name) && !cryptoMethod.Is_property)
                {
                    if (cryptoMethod.Argument_types.Count == methodSymbol.Parameters.Length)
                    {
                        bool isValidArgument = false;
                        if (cryptoMethod.Argument_types.Count > 0)
                        {
                            var argumentsArray = cryptoMethod.Argument_types.ToArray();
                            for (int i = 0; i < cryptoMethod.Argument_types.Count; i++)
                            {
                                isValidArgument = IsArgumentValid(argumentsArray[i], methodSymbol.Parameters[i], objectsDeclarations);
                                if (!isValidArgument)
                                {
                                    return false;
                                }
                            }
                            isMethodInEvents = true;
                        }
                        else
                        {
                            isMethodInEvents = true;
                        }
                    }
                }
            }
            return isMethodInEvents;
        }

        /// <summary>
        /// Gets the Invocator Type
        /// </summary>
        /// <param name="invocatorSymbol"></param>
        /// <returns></returns>
        public string GetInvocatorType(ISymbol invocatorSymbol)
        {
            if (invocatorSymbol != null)
            {
                SymbolKind symbolKind = invocatorSymbol.Kind;
                switch (symbolKind)
                {
                    case SymbolKind.NamedType:
                        var nameTypeSymbol = (INamedTypeSymbol)invocatorSymbol;
                        return nameTypeSymbol.ToString();

                    case SymbolKind.Local:
                        var localSymbol = (ILocalSymbol)invocatorSymbol;
                        return localSymbol.Type.ToString();

                    default: return invocatorSymbol.ToString();
                }
            }
            return string.Empty;

        }

        /// <summary>
        /// Check If Satisfies the Aggregator Condition
        /// </summary>
        /// <param name="methodSignatureModel"></param>
        /// <param name="aggregators"></param>
        /// <returns></returns>
        public bool CheckAggregator(Dictionary<string, List<MethodSignatureModel>> methodSignatureModel, ICollection<Aggregators> aggregators)
        {
            bool isPreviousEventPresent = false;
            string aggRegex = string.Empty;
            int aggregatorCounter = 0;
            foreach (var aggregator in aggregators)
            {
                var methodEvent = methodSignatureModel.Select(x => x.Key).Where(y => y.Equals(aggregator.Aggregator_Event_Varname)).FirstOrDefault();
                if (!String.IsNullOrEmpty(methodEvent))
                {
                    if (!(aggregatorCounter == 0))
                    {
                        if (!IsAggregatorCondition(isPreviousEventPresent, aggRegex, true))
                        {
                            return false;
                        }
                    }
                    isPreviousEventPresent = true;
                    aggRegex = aggregator.Aggregator_Regex;
                }
                else
                {
                    if (!(aggregatorCounter == 0))
                    {
                        if (!IsAggregatorCondition(isPreviousEventPresent, aggRegex, false))
                        {
                            return false;
                        }
                    }
                    isPreviousEventPresent = false;
                    aggRegex = aggregator.Aggregator_Regex;
                }
                aggregatorCounter++;
            }
            return true;
        }

        /// <summary>
        /// Builds Regex to Check the Order Sequence       
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public Regex ListToRegex(IEnumerable<string> list)
        {
            string pattern = "^" + string.Concat(list.Select((item, index) => ItemToRegex(item, index))) + "$";
            return new Regex(pattern);
        }

        /// <summary>
        /// Builds the Event Order List
        /// </summary>
        /// <param name="cryslModel"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetEventOrderList(CryslJsonModel cryslModel)
        {
            List<KeyValuePair<string, string>> eventOrderList = new List<KeyValuePair<string, string>>();
            foreach (var order in cryslModel.Order_Section.Event_Order)
            {
                string regex = string.IsNullOrEmpty(order.Regex) ? string.Empty : order.Regex;             

                string methodName = string.Empty;
                foreach(var methods in cryslModel.Event_Section.Methods)
                {
                    if(methods.Aggregator != null)
                    {
                       if(methods.Aggregator.Aggregator_Name.Equals(order.Aggregates))
                        {
                            methodName = methods.Crypto_Signature.FirstOrDefault().Method_Name;
                            break;
                        }
                    }
                    else if(methods.Crypto_Signature.FirstOrDefault().Event_Var_Name.Equals(order.Aggregates))
                    {
                        methodName = methods.Crypto_Signature.FirstOrDefault().Method_Name;
                        break;
                    }
                }
                eventOrderList.Add(new KeyValuePair<string, string>($"{order.Aggregates}{regex}", methodName));

            }
            return eventOrderList;
        }

        /// <summary>
        /// Builds a String of Valid Parameters.
        /// </summary>
        /// <param name="methods"></param>
        /// <returns>Valid Method Signatures that can be used for Diagnsotics Message</returns>
        public StringBuilder GetValidMethodSignatures(Methods methods)
        {
            StringBuilder methodSigBuilder = new StringBuilder();
            var lastItem = methods.Crypto_Signature.Last();
            foreach (var method in methods.Crypto_Signature)
            {
                methodSigBuilder.Append(method.Method_Name);
                if (method.Argument_types != null)
                {
                    if (method.Argument_types.Count() != 0)
                    {
                        methodSigBuilder.Append("(");
                        var methodParameters = string.Join(", ", method.Argument_types.Select(x => x.Argument));
                        methodSigBuilder.Append(methodParameters);
                        methodSigBuilder.Append(")");
                    }
                    else
                    {
                        methodSigBuilder.Append("( )");
                    }

                    if (!method.Equals(lastItem))
                    {
                        methodSigBuilder.Append(", ");
                    }
                }
            }
            return methodSigBuilder;
        }

        /// <summary>
        /// Returns a Regex For Order Sequence
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string ItemToRegex(string item, int index)
        {
            string value = item.TrimEnd('+', '*', '?');
            string suffix = item.Substring(value.Length);

            return $"(?:{(index <= 0 ? "" : delimiter)}{Regex.Escape(value)}){suffix}";
        }

        /// <summary>
        /// Checks if Aggregators Satisfies the Condition
        /// </summary>
        /// <param name="isPreviousPresent"></param>
        /// <param name="aggregatorRegex"></param>
        /// <param name="isCurrentPresent"></param>
        /// <returns></returns>
        private bool IsAggregatorCondition(bool isPreviousPresent, string aggregatorRegex, bool isCurrentPresent)
        {
            bool isConditionSatisfied;
            switch (aggregatorRegex)
            {
                case "|":
                    isConditionSatisfied = isPreviousPresent && isCurrentPresent ? false : true;
                    return isConditionSatisfied;

                case "&":
                    isConditionSatisfied = isPreviousPresent && isCurrentPresent ? true : false;
                    return isConditionSatisfied;

                default:
                    isConditionSatisfied = false;
                    break;
            }

            return isConditionSatisfied;

        }

        /// <summary>
        /// Check If Valid Argument
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="methodSymbol"></param>
        /// <returns></returns>
        private static bool IsArgumentValid(ArgumentTypes argument, IParameterSymbol methodSymbol, ICollection<ObjectsDeclaration> objectsDeclarations)
        {
            string objectType = GetTypeString(argument.Argument, objectsDeclarations);
            string methodSymbolType = string.Empty;
            if (methodSymbol.Type.ContainingNamespace != null)
            {
                methodSymbolType = $"{methodSymbol.Type.ContainingNamespace}.{methodSymbol.Type.Name.ToString()}";
            }
            //For Primitive Data Types
            else
            {
                if (methodSymbol.Type.Kind.Equals(SymbolKind.ArrayType))
                {
                    var arrayTypeSymbol = (IArrayTypeSymbol)methodSymbol.Type;
                    methodSymbolType = arrayTypeSymbol.ElementType.ToString();
                }
                else
                {
                    methodSymbolType = methodSymbol.Type.ToString();
                }
            }
            if (!string.IsNullOrEmpty(objectType))
            {
                if (methodSymbolType.Equals(objectType))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get the Type of a Variable Declared in Objects Section of Crysl
        /// </summary>
        /// <param name="variableDeclaration"></param>
        /// <returns></returns>
        private static string GetTypeString(string variableDeclaration, ICollection<ObjectsDeclaration> objectsDeclarations)
        {
            var objectType = objectsDeclarations.Select(x => x)
                .Where(y => y.Var_name.Equals(variableDeclaration)).FirstOrDefault();

            return objectType.Object_type;
        }        
    }
}
