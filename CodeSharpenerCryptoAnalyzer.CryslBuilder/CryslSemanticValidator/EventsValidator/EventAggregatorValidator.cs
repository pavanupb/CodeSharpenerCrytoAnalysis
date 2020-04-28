using CryslData;
using FluentValidation;
using System.Collections.Generic;

namespace CryslCSharpObjectBuilder.CryslSemanticValidator.EventsValidator
{
    public class EventAggregatorValidator : AbstractValidator<Aggregators>
    {
        public EventAggregatorValidator(ICollection<CryptoSignature> cryptoSignatures)
        {
            RuleFor(x => x.Aggregator_Event_Varname)
                .Must((x, y) => IsValidAggregators(x.Aggregator_Event_Varname, cryptoSignatures))
                .WithMessage("The aggregator {PropertyValue} has not been defined.");
        }

        private bool IsValidAggregators(string aggregatorName, ICollection<CryptoSignature> cryptoSignatures)
        {
            bool isEventVarNameDeclared = false;
            foreach (var cryptoSignature in cryptoSignatures)
            {
                if (aggregatorName.Equals(cryptoSignature.Event_Var_Name))
                {
                    isEventVarNameDeclared = true;
                    break;
                }
            }            
            bool isAggregatorDefined = isEventVarNameDeclared ? true : false;
            return isAggregatorDefined;
        }
    }
}
