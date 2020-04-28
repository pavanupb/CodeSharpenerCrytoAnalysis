using CodeSharpenerCryptoAnalysis.Models;
using CryslData;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeSharpenerCryptoAnalzer.Common
{
    public interface ICommonUtilities
    {
        bool IsMethodInEvents(CryptoSignature cryptoMethod, IMethodSymbol methodSymbol, ICollection<ObjectsDeclaration> objectsDeclarations);

        bool CheckAggregator(Dictionary<string, List<MethodSignatureModel>> methodSignatureModels, ICollection<Aggregators> aggregators);

        List<string> GetEventOrderList(CryslJsonModel cryslModel);

        Regex ListToRegex(IEnumerable<string> list);
    }
}
