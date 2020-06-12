using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalyzer.Test.Targets.ByteArrayTest
{
    public class ByteArrayWithNewKeyword
    {
        public void ByteArrayInitializerWithNewKeyword()
        {
            byte[] key = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
            byte[] iv = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

            byte[] queryKey = key;
            byte[] queryIV = iv;

        }
    }
}
