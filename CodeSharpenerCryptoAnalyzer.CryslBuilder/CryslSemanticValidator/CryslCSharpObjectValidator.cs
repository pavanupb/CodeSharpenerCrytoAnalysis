using CryslData;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using CryslCSharpObjectBuilder.CryslSemanticValidator;

namespace CryslCSharpObjectBuilder.CryslValidator
{
    class CryslCSharpObjectValidator:AbstractValidator<CryslJsonModel>
    {
        /// <summary>
        /// 
        /// </summary>        
        public CryslCSharpObjectValidator()
        {
            #region SPEC Section Validation
            RuleFor(x => x.Spec_Section.Crysl_Section)
                .Equal("SPEC")
                .WithMessage("The SPEC section contains invalid section name. Please fix in the Crysl file.");

            RuleFor(x => x.Spec_Section.Class_Name)
                .Must(IsValidNamespace)
                .WithMessage("The Namespace of the API is incorrect. Please fix in the Crysl file.");
            #endregion

            #region OBJECTS Section Validation
            RuleFor(x => x.Object_Section.Crysl_Section)
                .Equal("OBJECTS")
                .WithMessage("The OBJECTS sections contains invalid section name. Please fix in the Crysl file");

            RuleFor(x => x.Object_Section.Objects_Declaration)
                .Must(IsValidObjectDeclaration)
                .WithMessage("The declarations in OBJECTS section contains invalid type. Please fix in the Crysl file");
            #endregion

            #region EVENTS Section Validation
            RuleFor(x => x.Event_Section.Crysl_Section)
                .Equal("EVENTS")
                .WithMessage("The declarations in EVENTS section contains invalid type. Please fix in the Crysl file."); 

            RuleForEach(x => x.Event_Section.Methods)
                .SetValidator(model => new CryslCryptoSignatureValidator(model.Object_Section))
                .WithMessage("The arguments of the method in the EVENTS section has not been declared in the OBJECTS section");
            #endregion

            #region ORDER section Validation
            RuleFor(x => x.Order_Section.Crysl_Section)
                .Equal("ORDER")
                .WithMessage("The ORDER section contains invalid section name. Please fix in the Crysl file");

            RuleForEach(x => x.Order_Section.Event_Order)
                .SetValidator(model => new CryslEventOrderValidator(model.Event_Section.Methods))
                .WithMessage("The ORDER section contains invalid event. Please fix in the Crysl file");
            #endregion

            #region ENSURES section Validation
            RuleFor(x => x.Constraints_Section.Crysl_Section)
                .Equal("CONSTRAINTS")
                .WithMessage("The CONSTRAINTS section contains invalid section name. Please fix in the Crysl file");

            RuleForEach(x => x.Constraints_Section.Constraints)
                .SetValidator(model => new CryslConstraintsValidator(model.Object_Section.Objects_Declaration))
                .WithMessage("The CONSTRAINTS section contains variables that have not been declared");
            #endregion

            #region ENSURES section validation
            RuleFor(x => x.Constraints_Ensures.Crysl_Section)
                .Equal("ENSURES")
                .WithMessage("The ENSURES section contains invalid section name. Please fix in the crysl file");

            RuleForEach(x => x.Constraints_Ensures.EnsuresObject)
                .SetValidator(model => new CryslEnsuresValidator(model.Object_Section.Objects_Declaration, model.Event_Section.Methods))
                .WithMessage("The ENSURES section contains variables that have not been declared");
            #endregion


        }


        /// <summary>
        /// Check if namespace is correct.
        /// </summary>
        /// <param name="namespaceQualifier"></param>
        /// <returns>True if namespaceQualifier matches the regex.</returns>
        private bool IsValidNamespace(string namespaceQualifier)
        {
            bool isValidNamespace = Regex.IsMatch(namespaceQualifier, "(@?[a-z_A-Z]\\w+(?:\\.@?[a-z_A-Z]\\w+)*)$") ? true : false;            
            return isValidNamespace;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectDeclarations"></param>
        /// <returns></returns>
        private bool IsValidObjectDeclaration(ICollection<ObjectsDeclaration> objectDeclarations)
        {            
            foreach(var declaration in objectDeclarations)
            {
                //Check for a valid namespace or keywords
                if(!Regex.IsMatch(declaration.Object_type, "(@?[a-z_A-Z]\\w+(?:\\.@?[a-z_A-Z]\\w+)*)$") || !IsValidType(declaration.Object_type))
                {
                    return false;
                }                
            }
            return true;      
        }

        /// <summary>
        /// Checks whether declaration type is valid or not
        /// </summary>
        /// <param name="declarationType"></param>
        /// <returns>True if declaration type is a valid keyword</returns>
        private bool IsValidType(string declarationType)
        {
            CodeDomProvider keyWordIdentifier = CodeDomProvider.CreateProvider("C#");
            bool isValidType = keyWordIdentifier.IsValidIdentifier(declarationType) ? false : true;
            return isValidType;
        }
    }

    
}
