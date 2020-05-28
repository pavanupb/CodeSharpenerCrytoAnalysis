using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets
{
    public class StringConcatTest
    {
        public void stringConcat()
        {
            string query = "a" + "b";
            if (staticallyUnknown())
                query += "c";
            //Query for query(should be tainted)
        }

        public bool staticallyUnknown()
        {
            return true;
        }
    }
}
