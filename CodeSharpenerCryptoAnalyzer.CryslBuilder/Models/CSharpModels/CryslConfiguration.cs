using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSharpenerCryptoAnalyzer.CryslBuilder.Models.CSharpModels
{
    public partial class CryslSettings
    {
        [JsonProperty("crysl_configuration")]
        public CryslConfiguration CryslConfiguration { get; set; }
    }

    public partial class CryslConfiguration
    {
        [JsonProperty("crysl_path")]
        public string CryslPath { get; set; }
    }
}
