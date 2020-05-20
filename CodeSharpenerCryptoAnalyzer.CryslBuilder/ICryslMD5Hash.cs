using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSharpenerCryptoAnalyzer.CryslBuilder
{
    public interface ICryslMD5Hash
    {
        string GetHashCode(string cryslContents);

    }
}
