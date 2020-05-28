using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets
{
    public class BasicStringTest
    {
        public void stringConcat()
        {
            string query = "a" + "b";
            if (staticallyUnknown())
                query += "c";
            //Query for query(should be tainted)
        }

        public void stringConcatQueryByPass()
        {
            string taintedString = "This is a secret message";
            char[] taintedCharString = taintedString.ToCharArray();
            string t = new string(taintedCharString);
            //Query for t(should be tainted)
        }        

        public void stringToCharArray()
        {
            char[] s = "password".ToCharArray();
            //Query for s(should be tainted)
        }

        public void stringBuilderTest()
        {
            StringBuilder b = new StringBuilder("Test");
            b.Append("ABC");
            string s = b.ToString();
            //Query for s(should be tainted)
        }        

        private void MyStringBuilder(string str)
        {
            string query = string.Empty;
            query = str;
            //Query for query(should be tainted)
        }

        private bool staticallyUnknown()
        {
            return true;
        }        
    }
}
