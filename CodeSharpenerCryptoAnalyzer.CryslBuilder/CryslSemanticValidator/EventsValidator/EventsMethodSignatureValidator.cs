using CryslData;
using FluentValidation;
using System.Collections.Generic;

namespace CryslCSharpObjectBuilder.CryslSemanticValidator.EventsValidator
{
    class EventsMethodSignatureValidator : AbstractValidator<CryptoSignature>
    {
        public EventsMethodSignatureValidator(SectionObject objects)
        {
            RuleForEach(x => x.Argument_types)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .SetValidator(model => new EventArgumentValidator(model.Event_Var_Name, objects))          
               .When(x => x.Argument_types != null)
               .WithMessage("The Event {PropertyValue} contains invalid arguments");

            RuleFor(x => x.Object_variable)
                .Must((x, y) => IsValidObjectVariable(x.Object_variable, objects))
                .When(x => x.Object_variable != null)
                .WithMessage((model) => $"The Event {model.Event_Var_Name} contains variable " + "{PropertyValue} that has not been declared");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectVariable"></param>
        /// <param name="sectionObject"></param>
        /// <returns></returns>
        private bool IsValidObjectVariable(string objectVariable, SectionObject sectionObject)
        {
            bool isObjectDeclared = false;
            foreach (var objectDeclarations in sectionObject.Objects_Declaration)
            {
                if (objectDeclarations.Var_name.Equals(objectVariable))
                {
                    isObjectDeclared = true;
                    break;
                }
            }            
            bool isValidObjectVariable = isObjectDeclared ? true : false;
            return isValidObjectVariable;            
        }
    }
}
