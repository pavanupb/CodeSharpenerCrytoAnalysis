using CryslData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSharpenerCryptoAnalysis.AnalyzerModels
{
    public class ConstraintsModel
    {
        public bool IsConstraintSatisfied { get; set; }
        public bool IsAdditionalConstraints { get; set; }
        public List<AdditionalConstraints> AdditionalConstraints { get; set; }
        public string SatisfiedConstraint { get; set; }
        public string NotSatisfiedParameter { get; set; }
        public ICollection<string> AcceptedParameterValues { get; set; }
    }
}
