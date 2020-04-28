using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using CryslCSharpObjectBuilder.Models.CSharpModels;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Sharpen;
using Antlr4.Runtime.Atn;

namespace CryslCSharpObjectBuilder.LexersParsers
{
    public class VerboseErrorListener : BaseErrorListener
    {
        public ErrorMessageModel errorMessageModel = new ErrorMessageModel();
        List<SyntaxErrorModel> syntaxErrorModelList = new List<SyntaxErrorModel>();
        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            List<string> errorStack = ((Parser)recognizer).GetRuleInvocationStack().Reverse().ToList();
            SyntaxErrorModel syntaxErrorModel = new SyntaxErrorModel()
            {
                StackTrace = errorStack,
                Line = line,
                CharPositionInLine = charPositionInLine,
                Message = msg,
                OffendingSymbol = offendingSymbol
            };

            syntaxErrorModelList.Add(syntaxErrorModel);
            errorMessageModel.ErrorMessage = syntaxErrorModelList;           
            
        }
        
    }
}
