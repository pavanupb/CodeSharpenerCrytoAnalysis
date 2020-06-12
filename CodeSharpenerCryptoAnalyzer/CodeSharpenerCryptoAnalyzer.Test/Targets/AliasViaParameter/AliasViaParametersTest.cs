using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets
{
    public class AliasViaParametersTest
    {
        public void aliasViaParameter()
        {
            A a = new A();
            A b = a;
            setAndLoadFieldOnAlias(a, b);
            b.Field = "This is a secret message";
            string query = a.Field;
            //query for query which is a.field

        }

        private static void setAndLoadFieldOnAlias(A a, A b)
        {
            b.Field = "This is a secret message";
        }


        private class A
        {
            public string Field { get; set; }
        }
    }
}
