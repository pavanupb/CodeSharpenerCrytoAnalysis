using CryslData;
using FluentValidation;
using System.Collections.Generic;

namespace CryslCSharpObjectBuilder.CryslSemanticValidator.EnsuresValidator
{
    public class EnsuresListValidator : AbstractValidator<string>
    {
        public EnsuresListValidator(ICollection<ObjectsDeclaration> objectDeclarations)
        {
            RuleFor(x => x)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must((x, y) => BeDeclaredInObjectsSection(x, objectDeclarations))
                .WithMessage("The Ensures Section contains {PropertyValue} that has not been declared in Objects");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ensuresName"></param>
        /// <param name="objectsDeclarations"></param>
        /// <returns></returns>
        private bool BeDeclaredInObjectsSection(string ensuresName, ICollection<ObjectsDeclaration> objectsDeclarations)
        {
            bool isEnsureVariableDeclared = false;
            foreach (var objects in objectsDeclarations)
            {
                if (ensuresName.Equals(objects.Var_name))
                {
                    isEnsureVariableDeclared = true;
                    break;
                }
            }

            bool isEnsuresDeclared = isEnsureVariableDeclared ? true : false;
            return isEnsuresDeclared;
        }
    }
}
