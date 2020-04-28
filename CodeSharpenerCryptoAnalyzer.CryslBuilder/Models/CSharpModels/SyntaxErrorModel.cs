using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryslCSharpObjectBuilder.Models.CSharpModels
{
    public class SyntaxErrorModel
    {
        public List<string> StackTrace { get; set; }
        public int Line { get; set; }
        public int CharPositionInLine { get; set; }
        public IToken OffendingSymbol { get; set; }
        public string Message { get; set; }
    }

    public class ErrorMessageModel
    {
        public List<SyntaxErrorModel> ErrorMessage { get; set; }
    }
}
