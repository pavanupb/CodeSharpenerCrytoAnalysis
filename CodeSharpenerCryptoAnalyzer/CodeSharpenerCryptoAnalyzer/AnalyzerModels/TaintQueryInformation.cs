using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.AnalyzerModels
{
    public class TaintQueryInformation
    {
        public bool IsTainted { get; set; }
        public ContextInformation TaintedContextInformation { get; set; }
    }
}
