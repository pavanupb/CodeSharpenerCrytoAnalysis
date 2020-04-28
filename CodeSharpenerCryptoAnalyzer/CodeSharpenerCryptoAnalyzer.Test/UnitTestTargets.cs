using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSharpenerCryptoAnalysis.Test
{
    public class UnitTestTargets
    {
        public static void Test(string[] args)
        {
            int x = 0;
            int y = 1;
            int z = 2;

            add(ref x, ref y, ref z);
            Console.WriteLine(x);

        }
        public static int add(ref int a, ref int b, ref int c)
        {
            int addRes = a + b + c;
            return addRes;
        }
    }
}
