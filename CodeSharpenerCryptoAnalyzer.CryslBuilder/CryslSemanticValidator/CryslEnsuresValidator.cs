using CryslData;
using CryslCSharpObjectBuilder.CryslSemanticValidator.EnsuresValidator;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryslCSharpObjectBuilder.CryslSemanticValidator
{
    public class CryslEnsuresValidator : AbstractValidator<EnsuresObject>
    {
        public CryslEnsuresValidator(ICollection<ObjectsDeclaration> objectsDeclarations, ICollection<Methods> eventMethods)
        {
            RuleForEach(x => x.EnsuresList)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .SetValidator(new EnsuresListValidator(objectsDeclarations))
                .WithMessage("Ensures list contains variables that have not been declared in the objects section");

            RuleForEach(x => x.AfterEventsList)
                .SetValidator(new EnsuresAfterListValidator(eventMethods))
                .When(x => x.AfterEventsList != null)
                .WithMessage("The AfterEvents contains variables that have not been defined in events section");

        }        
    }
}
