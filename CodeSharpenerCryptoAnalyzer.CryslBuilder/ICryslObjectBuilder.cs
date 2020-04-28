using CryslCSharpObjectBuilder.Models.CSharpModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSharpenerCryptoAnalyzer.CryslBuilder
{
    public interface ICryslObjectBuilder
    {
        CryslResult CryslToCSharpBuilder();
    }
}
