using CodeSharpenerCryptoAnalysis.Models;
using CryslData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSharpenerCryptoAnalysis.AnalyzerModels
{
    public class ValidEvents
    {
        public MethodSignatureModel ValidMethods { get; set; }

        public bool IsValidEvent { get; set; }

        public string AggregatorName { get; set; }

        public bool IsProperty { get; set; }

        public string PropertyName { get; set; }
    }
}
