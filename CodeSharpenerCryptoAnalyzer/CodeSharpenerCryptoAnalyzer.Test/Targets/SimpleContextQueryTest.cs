using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets
{
    public class SimpleContextQueryTest
    {
        public void outerAllocation()
        {
            string alloc = "This is a secret message";
            methodOfQuery(alloc);
        }

        private void methodOfQuery(string alloc)
        {
            string alias = alloc;
            //query for alias;
        }


        public void outerAllocation2()
        {
            string alloc = "This is another secret message";          
            string same = alloc;
            methodOfQuery(alloc, same);
        }


        public void outerAllocation3()
        {
            string alloc = "This is a secret message";
            string same = string.Empty;
            methodOfQuery(alloc, same);
        }

        private void methodOfQuery(string alloc, string alias)
        {
            string query = alloc;
            //query for alloc
        }
    }
}
