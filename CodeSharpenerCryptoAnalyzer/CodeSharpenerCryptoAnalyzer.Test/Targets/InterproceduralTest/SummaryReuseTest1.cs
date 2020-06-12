using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets.InterproceduralTest
{
    public class SummaryReuseTest1
    {
        public void SummaryReuseTest()
        {
            IdentityFuncTest identityFuncTest = new IdentityFuncTest();
            string alias1 = "This is a secret message";
            string alias2 = alias1;
            identityFuncTest.Identity(alias1);
            string alias3 = alias2;
            string alias4 = alias3;        

        }        
    }
}
