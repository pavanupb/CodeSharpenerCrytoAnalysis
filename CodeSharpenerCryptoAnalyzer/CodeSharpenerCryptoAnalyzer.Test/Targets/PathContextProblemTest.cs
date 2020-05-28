using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets
{
    public class PathContextProblemTest
    {
        public void start()
        {
            Inner i = new Inner();
            i.test1();
            i.test2();
        }
        public class Inner
        {
            public void callee(string a, string b)
            {
                string query = a;
                //query for a
            }

            public void test1()
            {
                string a1 = "This is a secret text";
                string b1 = a1;
                callee(a1, b1);
            }

            public void test2()
            {
                string a1 = "This is a secret text";
                string a2 = string.Empty;
                callee(a1, a2);
            }
        }

        public void start2()
        {
            Inner i = new Inner();
            i.test1();
            i.test2();
        }

        public class Inner2
        {
            public void callee(string a, string b)
            {
                string query = b;
                //query for b
            }

            public void test1()
            {
                string a1 = "This is a secret message";
                string b1 = a1;
                callee(a1, b1);
            }

            public void test2()
            {
                string a2 = string.Empty;
                string b2 = "This is another secret message";
                callee(a2, b2);
            }
        }
    }
}
