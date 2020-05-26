using CodeSharpenerCryptoAnalyzer.CryslBuilder.Models.CSharpModels;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace CodeSharpenerCryptoAnalyzer.CryslBuilder
{
    public class CryslConfigurationBuilder : ICryslConfigurationBuilder
    {
        public CryslSettings GetCryslConfigurations(ImmutableArray<AdditionalText> AdditionalFiles)
        {
            CryslSettings cryslSettings = new CryslSettings
            {
                CryslConfiguration = new CryslConfiguration
                {
                    CryslPath = string.Empty
                }
            };
            if (AdditionalFiles.Length > 0)
            {
                var configFiles = AdditionalFiles[0];
                var configJson = configFiles.GetText();
                try
                {
                    cryslSettings = JsonConvert.DeserializeObject<CryslSettings>(configJson.ToString());
                }
                catch (Exception ex)
                {

                }
            }

            return cryslSettings;
        }
    }
}
