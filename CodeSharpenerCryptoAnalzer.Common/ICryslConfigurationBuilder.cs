using CodeSharpenerCryptoAnalyzer.CryslBuilder.Models.CSharpModels;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace CodeSharpenerCryptoAnalyzer.CryslBuilder
{
    public interface ICryslConfigurationBuilder
    {
        CryslSettings GetCryslConfigurations(ImmutableArray<AdditionalText> AdditionalFiles);
    }
}
