using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets.InterproceduralTest
{
    public class IdentityStringTest1
    {
        public void IdentityTest1()
        {
            string alias1 = "This is a secret message";
            string alias2 = alias1;
            Identity(alias1);
            OtherCall(alias2);
            //query for alias1
        }

        private static void Identity(string parameters)
        {
            string mapped = parameters;
        }

        private static void OtherCall(string parameters)
        {

        }
    }
}
