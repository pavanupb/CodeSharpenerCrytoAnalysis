using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets
{
    public class InterproceduralTests
    {
        public void IdentityTest()
        {
            string alias1 = "This is a secret message";
            Identity(alias1);
            //query for mapped in Identity()
        }

        public void IdentityTest1()
        {
            string alias1 = "This is a secret message";
            string alias2 = alias1;
            Identity(alias1);
            OtherCall(alias2);
            //query for alias1
        }

        public void SummaryReuseTest1()
        {
            string alias1 = "This is a secret message";
            string alias2 = alias1;
            Identity(alias1);
            string alias3 = alias2;
            string alias4 = alias3;
            //query for alias4

        }

        public void branchWithCall()
        {
            string a1 = "This is a secret message";
            string a2 = "This is another secret message";
            object a = null;

            if(staticallyUnknown())
            {
                a = a1;
            }
            else
            {
                a = a2;
            }

            wrappedFoo(a);
            //query for a
        }

        public void interLoop()
        {
            String alias = "This is a secret message";
            string aliased2;
            string aliased = string.Empty;
            for(int i = 0; i < 20; i++)
            {
                Identity(alias);
                aliased = alias;
            }

            aliased2 = aliased;
            //query for aliased2 and mapped
        }

        public void doSummarize()
        {
            string a = "This is a secret message";
            string b = a;
            string c = b;
            string d = c;

            string e = d;
            string f = e;
            string g = a;

            if(staticallyUnknown())
            {
                g = f;
            }
            string h = g;
            //query for h

        }




        private static void Identity(string parameters)
        {
            string mapped = parameters;
        }

        private static void OtherCall(string parameters)
        {

        }

        private bool staticallyUnknown()
        {
            return true;
        }

        private void wrappedFoo(object param)
        {

        }
    }
}
