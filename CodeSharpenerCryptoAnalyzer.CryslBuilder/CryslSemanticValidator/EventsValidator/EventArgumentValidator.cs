using CryslData;
using FluentValidation;

namespace CryslCSharpObjectBuilder.CryslSemanticValidator.EventsValidator
{
    public class EventArgumentValidator : AbstractValidator<ArgumentTypes>
    {
        public EventArgumentValidator(string eventVarName, SectionObject objects)
        {
            RuleFor(x => x.Argument)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must((x, y) => IsValidArguments(x.Argument, objects))
                .WithMessage($"The Event {eventVarName} contains argument " + "{PropertyValue} that has not been declared");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="sectionObject"></param>
        /// <returns></returns>
        private bool IsValidArguments(string argument, SectionObject sectionObject)
        {
            bool isObjectDeclared = false;
            foreach (var objectDeclarations in sectionObject.Objects_Declaration)
            {
                if (objectDeclarations.Var_name.Equals(argument) || argument.Equals("_"))
                {
                    isObjectDeclared = true;
                    break;
                }
            }
            
            bool isValidArgument = isObjectDeclared ? true : false;
            return isValidArgument;
        }
    }
}
