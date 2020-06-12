using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets.InterproceduralTest
{
    public class InterLoopTest
    {
        public void interLoop()
        {
            String alias = "This is a secret message";
            string aliased2;
            string aliased = string.Empty;
            for (int i = 0; i < 20; i++)
            {
                Identity(alias);
                aliased = alias;
            }

            aliased2 = aliased;            
        }

        private static void Identity(string parameters)
        {
            string mapped = parameters;
        }
    }
}
