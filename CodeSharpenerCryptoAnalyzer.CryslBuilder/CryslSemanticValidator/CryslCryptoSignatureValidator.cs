using CryslData;
using CryslCSharpObjectBuilder.CryslSemanticValidator.EventsValidator;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryslCSharpObjectBuilder.CryslSemanticValidator
{
    public class CryslCryptoSignatureValidator : AbstractValidator<Methods>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectSection"></param>
        public CryslCryptoSignatureValidator(SectionObject objectSection)
        {            

            RuleForEach(x => x.Crypto_Signature)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .SetValidator(new EventsMethodSignatureValidator(objectSection))
                .WithMessage("The arguments of the method in the EVENTS section has not been declared in the OBJECTS section");

            RuleForEach(x => x.Aggregator.Aggregators)
                .Cascade(CascadeMode.StopOnFirstFailure)                
                .SetValidator(model => new EventAggregatorValidator(model.Crypto_Signature))
                .When(x => x.Aggregator != null)
                .WithMessage("The Aggregators have not been defined in the event section. Please fix in the crysl file");
        }        
    }
}
