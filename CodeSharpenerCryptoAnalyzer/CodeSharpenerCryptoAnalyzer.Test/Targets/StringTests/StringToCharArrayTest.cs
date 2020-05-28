using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets
{
    public class StringToCharArrayTest
    {
        public void stringToCharArray()
        {
            char[] s = "password".ToCharArray();
            var query = s;
        }
    }
}
