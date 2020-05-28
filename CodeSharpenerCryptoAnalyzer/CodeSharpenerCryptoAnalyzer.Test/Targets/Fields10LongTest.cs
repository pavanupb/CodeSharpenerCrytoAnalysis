using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets
{
    public class Fields10LongTest
    {
        public void test()
        {
            TreeNode x = new TreeNode();
            string t = string.Empty;
            if (staticallyUnknown())
            {
                t = x.a;
            }
            if (staticallyUnknown())
            {
                t = x.b;
            }
            if (staticallyUnknown())
            {
                t = x.c;
            }
            if (staticallyUnknown())
            {
                t = x.d;
            }
            if (staticallyUnknown())
            {
                t = x.e;
            }
            if (staticallyUnknown())
            {
                t = x.f;
            }

            if (staticallyUnknown())
            {
                t = x.g;
            }
            if (staticallyUnknown())
            {
                t = x.h;
            }
            if (staticallyUnknown())
            {
                t = x.i;
            }
            if (staticallyUnknown())
            {
                t = x.j;
            }
            string h = t;
            //query for h(Should not be tainted)
        }

        public bool staticallyUnknown()
        {
            return true;
        }

        private class TreeNode
        {
            public string a { get; set; }
            public string b { get; set; }
            public string c { get; set; }
            public string d { get; set; }
            public string e { get; set; }
            public string f { get; set; }
            public string g { get; set; }
            public string h { get; set; }
            public string i { get; set; }
            public string j { get; set; }


        }
    }
}
