using CryslData;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryslCSharpObjectBuilder.CryslSemanticValidator
{
    public class CryslConstraintsValidator : AbstractValidator<Constraints>
    {
        public CryslConstraintsValidator(ICollection<ObjectsDeclaration> objectsDeclarations)
        {
            RuleFor(x => x.Object_Varname)
                .Must((x, y) => BeDeclaredInObjectsDeclaration(x.Object_Varname, objectsDeclarations))
                .WithMessage("The Constraints section contains {PropertyValue} that has not been declared in Objects");

            RuleFor(x => x.Additional_constraints.Object_Varname_Additional_Constraint)
                .Must((x, y) => BeDeclaredInObjectsDeclaration(x.Additional_constraints.Object_Varname_Additional_Constraint, objectsDeclarations))
                .When(x => x.Additional_constraints != null)
                .WithMessage("The Additional constraints contains {PropertyValue} that has not been declared in Objects");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectVarname"></param>
        /// <param name="objectsDeclarations"></param>
        /// <returns></returns>
        private bool BeDeclaredInObjectsDeclaration(string objectVarname, ICollection<ObjectsDeclaration> objectsDeclarations)
        {
            bool isObjectDeclared = false;
            foreach(var declarations in objectsDeclarations)
            {
                if(declarations.Var_name.Equals(objectVarname))
                {
                    isObjectDeclared = true;
                    break;
                }
            }
            bool isValidObjectVarname = isObjectDeclared ? true : false;
            return isValidObjectVarname;
        }
    }
}
