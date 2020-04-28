using CryslData;
using FluentValidation;
using System.Collections.Generic;

namespace CryslCSharpObjectBuilder.CryslSemanticValidator.EnsuresValidator
{
    class EnsuresAfterListValidator : AbstractValidator<string>
    {
        public EnsuresAfterListValidator(ICollection<Methods> methods)
        {
            RuleFor(x => x)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must((x, y) => BeInAfterEventsDeclared(x, methods))
                .WithMessage("{PropertyValue} has not been defined in the Events section");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ensuresAfterEvent"></param>
        /// <param name="methods"></param>
        /// <returns></returns>
        private bool BeInAfterEventsDeclared(string ensuresAfterEvent, ICollection<Methods> methods)
        {
            bool isAfterEventDefined = false;
            foreach (var method in methods)
            {
                //If aggregator is null check in the events
                if (method.Aggregator is null)
                {
                    foreach (var cryptoSignature in method.Crypto_Signature)
                    {
                        if (cryptoSignature.Event_Var_Name.Equals(ensuresAfterEvent))
                        {
                            isAfterEventDefined = true;
                            break;
                        }
                    }
                }
                else
                {
                    if (method.Aggregator.Aggregator_Name.Equals(ensuresAfterEvent))
                    {
                        isAfterEventDefined = true;
                        break;
                    }
                }
            }

            bool isEventDefined = isAfterEventDefined ? true : false;
            return isEventDefined;
        }
    }
}
