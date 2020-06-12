using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets
{
    public class FieldsBranchedTest
    {
        public void oneFieldsNoLoop()
        {
            Node x = new Node();            
            if (staticallyUnknown())
            {
                x.left = "This is secret message";
            }
            else if (staticallyUnknown())
            {
                x.right = x.left;
            }
            string t;
            if (staticallyUnknown())
            {
                t = x.left;
            }
            else
            {
                t = x.right;
            }
            string h = t;
            string query = h;
            //query for h(should be tainted)
        }

        public bool staticallyUnknown()
        {
            return true;
        }

    }

    public class Node
    {
        public string left { get; set; }
        public string right { get; set; } 

    }
}
