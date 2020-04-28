using CryslData;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryslCSharpObjectBuilder.CryslSemanticValidator
{
    public class CryslEventOrderValidator:AbstractValidator<EventOrder>
    {
        public CryslEventOrderValidator(ICollection<Methods> methods)
        {
            RuleFor(x => x.Aggregates)
                .Must((x, y) => IsAggregatesDefined(x.Aggregates, methods))
                .WithMessage("Order section contains {PropertyValue} that has not been defined in the events section");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregate"></param>
        /// <param name="methods"></param>
        /// <returns></returns>
        private bool IsAggregatesDefined(string aggregate, ICollection<Methods> methods)
        {
            bool isAggregateDefined = false;
            foreach (var method in methods)
            {
                //If aggregator is null check in the events
                if (method.Aggregator is null)
                {
                    foreach (var cryptoSignature in method.Crypto_Signature)
                    {
                        if (cryptoSignature.Event_Var_Name.Equals(aggregate))
                        {
                            isAggregateDefined = true;
                            break;
                        }
                    }                    
                }
                else
                {
                    if (method.Aggregator.Aggregator_Name.Equals(aggregate))
                    {
                        isAggregateDefined = true;
                        break;
                    }
                }                              
            }
            bool isValidOrderName = isAggregateDefined ? true : false;            
            return isValidOrderName;         
        }
    }
}
