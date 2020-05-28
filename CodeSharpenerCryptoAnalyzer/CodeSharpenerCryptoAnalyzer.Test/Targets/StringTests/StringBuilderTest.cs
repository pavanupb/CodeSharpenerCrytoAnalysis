using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets
{
    public class StringBuilderTest
    {
        public void stringBuilderTest()
        {
            StringBuilder b = new StringBuilder("Test");
            b.Append("ABC");
            string s = b.ToString();            
        }
    }
}
