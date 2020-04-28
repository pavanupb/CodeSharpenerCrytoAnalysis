using CodeSharpenerCryptoAnalysis.AnalyzerModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSharpenerCryptoAnalysis.CryslSectionsAnalyzers
{
    public interface IOrderSectionAnalyzer
    {
        bool IsValidOrder(ValidEvents validEvents, string containingMethod, List<KeyValuePair<string, string>> eventsOrderDict, List<string> eventOrderContraint);
    }
}
