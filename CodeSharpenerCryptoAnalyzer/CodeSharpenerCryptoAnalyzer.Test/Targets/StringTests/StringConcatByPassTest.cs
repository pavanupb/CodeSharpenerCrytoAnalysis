using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets
{
    public class StringConcatByPassTest
    {
        public void stringConcatQueryByPass()
        {
            string taintedString = "This is a secret message";
            char[] taintedCharString = taintedString.ToCharArray();
            string t = new string(taintedCharString);            
        }
    }
}
