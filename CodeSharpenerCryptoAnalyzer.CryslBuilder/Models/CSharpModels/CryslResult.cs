using CryslData;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryslCSharpObjectBuilder.Models.CSharpModels
{
    public class CryslResult
    {
        public CryslJsonModel CryslModel { get; set; }
        public ErrorMessageModel SyntaxErrors { get; set; }
        public bool IsValid { get; set; }
        public IList<ValidationFailure> ValidationErrors { get; set; }
        public string FilePath { get; set; }
    }
}
