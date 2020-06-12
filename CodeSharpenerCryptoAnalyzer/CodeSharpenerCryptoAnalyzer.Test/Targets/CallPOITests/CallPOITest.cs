using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets.AliasViaParameter
{
    public class CallPOITest
    {
        public void indirectAllocationSite3Address()
        {
            A a = new A();
            B load = a.b;
            string alias = load.c;
            //query for alias
        }

    }

    public class A
    {
        public B b { get; set; }

    }

    public class B
    {
        public string c { get; set; }
    }
}

