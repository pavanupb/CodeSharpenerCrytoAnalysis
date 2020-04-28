using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSharpenerCryptoAnalysis.AnalyzerModels
{
    public class AddConstraints
    {
        public string EventKey { get; set; }
        public string EventVariableDeclarator { get; set; }
        public List<ConstraintsModel> ConstraintsModels { get; set; }
    }
}
