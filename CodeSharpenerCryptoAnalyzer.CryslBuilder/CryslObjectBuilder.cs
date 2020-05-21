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
using System.IO;
using System.Collections.Generic;

namespace CodeSharpenerCryptoAnalyzer.CryslBuilder
{
    public class CryslObjectBuilder : ICryslObjectBuilder
    {
        private ServiceProvider serviceProvider { get; set; }

        private Dictionary<string, string> cryslFiles = new Dictionary<string, string>();        

        public CryslObjectBuilder()
        {
            var services = new ServiceCollection();
            services.AddTransient<ICryslGrammarVisitor<object>, CryslGrammarVisitor>();
            services.AddTransient<IValidator, CryslCSharpObjectValidator>();
            services.AddTransient<ICryslMD5Hash, CryslMD5Hash>();
            serviceProvider = services.BuildServiceProvider();                  
        }

        /// <summary>
        /// 
        /// </summary>
        public CryslResult CryslToCSharpBuilder(string cryslPath)
        {
            CryslResult cryslResult = new CryslResult();
            /*string[] cryslFilePath = { Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, @".//CryslParser//CryslObjectBuilder//CryslFiles//AES.crysl" };            
            string text = System.IO.File.ReadAllText(Path.Combine(cryslFilePath));*/
            //string text = System.IO.File.ReadAllText("D:\\Master Thesis\\CodeSharpenerCrytoAnalysis\\CodeSharpenerCryptoAnalyzer.CryslBuilder\\CryslFiles\\SymmetricAlgorithms.crysl");
            string text = System.IO.File.ReadAllText(cryslPath);

            //Check if the crysl file has already been parsed
            if(!cryslFiles.ContainsKey(cryslPath))
            {
                lock (cryslFiles)
                {
                    ICryslMD5Hash cryslMD5Hash = serviceProvider.GetService<ICryslMD5Hash>();
                    string cryslContentHashCode = cryslMD5Hash.GetHashCode(text);
                    cryslFiles.Add(cryslPath, cryslContentHashCode);
                }   
            }
            //Check if the crysl file has been changed from the last known read
            else
            {
                ICryslMD5Hash cryslMD5Hash = serviceProvider.GetService<ICryslMD5Hash>();
                string cryslContentHashCode = cryslMD5Hash.GetHashCode(text);
                string currentCryslContentHashCode = cryslFiles[cryslPath];

                if(cryslContentHashCode.Equals(currentCryslContentHashCode))
                {
                    cryslResult.IsFileChanged = false;
                    return cryslResult;
                }
                else
                {
                    lock (cryslFiles)
                    {
                        cryslFiles.Remove(cryslPath);
                        cryslFiles.Add(cryslPath, cryslContentHashCode);
                    }
                }
            }
                             
            ICharStream stream = CharStreams.fromstring(text);            
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
                cryslResult.IsFileChanged = true;

                IValidator objectValidator = serviceProvider.GetService<IValidator>();
                ValidationResult validationResult = objectValidator.Validate(result);
                if (!validationResult.IsValid)
                {
                    cryslResult.ValidationErrors = validationResult.Errors;
                    cryslResult.IsValid = validationResult.IsValid;
                    cryslResult.FilePath = text;
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
                cryslResult.FilePath = text;
            }

            return cryslResult;
        }
    }
}

