using CodeSharpenerCryptoAnalysis.AnalyzerModels;
using CodeSharpenerCryptoAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSharpenerCryptoAnalysis.CryslSectionsAnalyzers
{
    public interface IOrderSectionAnalyzer
    {
        bool IsValidOrder(List<KeyValuePair<string, string>> currentEventOrderDict, List<string> eventOrderConstraint);
    }
}
