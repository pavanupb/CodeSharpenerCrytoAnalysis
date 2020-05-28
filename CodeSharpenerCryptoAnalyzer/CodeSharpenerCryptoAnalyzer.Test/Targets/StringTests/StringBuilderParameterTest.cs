using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets.StringTests
{
    public class StringBuilderParameterTest
    {
        private void StringBuilder()
        {
            string plainText = "This is a secret message";
            MyStringBuilder(plainText);
        }
        private void MyStringBuilder(string str)
        {
            string query = string.Empty;
            query = str;            
        }

    }
}
