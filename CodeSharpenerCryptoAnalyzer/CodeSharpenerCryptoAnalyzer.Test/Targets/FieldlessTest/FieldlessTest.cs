using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets
{
    public class FieldlessTest
    {
        public void simpleAssignment1()
        {
            string taintedString1 = "This is a secret message";
            string alias1 = taintedString1;
            string query = alias1;
            //query for "query"
        }

        public void simpleAssignment2()
        {
            string alias1 = "This is a secret message";
            string alias2 = alias1;
            string alias3 = alias1;
            //query for alias3
        }

        public void branchWithOverwrite()
        {
            string alias2 = "This is a secret message";
            if(staticallyUnknown())
            {
                string alias1 = alias2;
                alias2 = string.Empty;
            }
            //query for alias2
        }

        public void branchWithOverwriteSwapped()
        {
            string alias2 = "This is a secret message";
            string alias1 = "This is another secret message";
            if(staticallyUnknown())
            {
                alias2 = alias1;
            }
            string query = alias2;
        }

        public void returnNullAllocation()
        {
            string alias2 = null;
            string query = alias2;
        }


        private bool staticallyUnknown()
        {
            return true;
        }
    }
}
