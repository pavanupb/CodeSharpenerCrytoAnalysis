using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets.InterproceduralTest
{
    public class IdentityStringTest
    {
        public void IdentityTest()
        {
            string alias1 = "This is a secret message";
            IdentityTest(alias1);
        }
        private static void IdentityTest(string parameters)
        {
            string mapped = parameters;
        }
    }
}
