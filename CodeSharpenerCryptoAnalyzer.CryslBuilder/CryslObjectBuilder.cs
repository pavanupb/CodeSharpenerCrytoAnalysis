using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CryslData;
using CryslCSharpObjectBuilder.CryslValidator;
using CryslCSharpObjectBuilder.LexersParsers;
using CryslCSharpObjectBuilder.Models.CSharpModels;
using CryslParser.Visitors;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CodeSharpenerCryptoAnalyzer.CryslBuilder
{
    public class CryslObjectBuilder : ICryslObjectBuilder
    {
        private ServiceProvider serviceProvider { get; set; }

        public CryslObjectBuilder()
        {
            var services = new ServiceCollection();
            services.AddTransient<ICryslGrammarVisitor<object>, CryslGrammarVisitor>();
            services.AddTransient<IValidator, CryslCSharpObjectValidator>();
            serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// 
        /// </summary>
        public CryslResult CryslToCSharpBuilder()
        {
            /*string[] cryslFilePath = { Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, @".//CryslParser//CryslObjectBuilder//CryslFiles//AES.crysl" };            
            string text = System.IO.File.ReadAllText(Path.Combine(cryslFilePath));*/
            string text = System.IO.File.ReadAllText("D:\\Master Thesis\\CodeSharpenerCrytoAnalysis\\CodeSharpenerCryptoAnalyzer.CryslBuilder\\CryslFiles\\AES.crysl");
            ICharStream stream = CharStreams.fromstring(text);
            CryslResult cryslResult = new CryslResult();
            CryslGrammarLexer lexer = new CryslGrammarLexer(stream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            CryslGrammarParser parser = new CryslGrammarParser(tokens);

            VerboseErrorListener errorListener = new VerboseErrorListener();
            parser.RemoveErrorListeners();
            parser.AddErrorListener(errorListener);

            parser.BuildParseTree = true;
            IParseTree tree = parser.cryslsection();
            if (errorListener.errorMessageModel.ErrorMessage == null)
            {
                ICryslGrammarVisitor<object> visitor = serviceProvider.GetService<ICryslGrammarVisitor<object>>();
                CryslJsonModel result = (CryslJsonModel)visitor.Visit(tree);
                cryslResult.CryslModel = result;

                IValidator objectValidator = serviceProvider.GetService<IValidator>();
                ValidationResult validationResult = objectValidator.Validate(result);
                if (!validationResult.IsValid)
                {
                    cryslResult.ValidationErrors = validationResult.Errors;
                    cryslResult.IsValid = validationResult.IsValid;
                }
                else
                {
                    cryslResult.IsValid = true;
                }
            }
            else
            {
                cryslResult.SyntaxErrors = errorListener.errorMessageModel;
                cryslResult.IsValid = false;
            }

            return cryslResult;
        }
    }
}

