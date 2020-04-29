using CodeSharpenerCryptoAnalysis.AnalyzerModels;
using CodeSharpenerCryptoAnalysis.Common;
using CodeSharpenerCryptoAnalysis.Models;
using CodeSharpenerCryptoAnalzer.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeSharpenerCryptoAnalysis.CryslSectionsAnalyzers
{
    public class OrderSectionAnalyzer : IOrderSectionAnalyzer
    {
        private const string delimiter = ",";
        private static ServiceProvider serviceProvider { get; set; }

        public OrderSectionAnalyzer()
        {
            var services = new ServiceCollection();
            services.AddTransient<ICommonUtilities, CommonUtilities>();
            serviceProvider = services.BuildServiceProvider();
        }
        /// <summary>
        /// Checks and Reports if Order is Invalid
        /// </summary>
        /// <param name="validEvents"></param>
        /// <param name="containingMethod"></param>
        public bool IsValidOrder(Dictionary<string,Dictionary<string, List<MethodSignatureModel>>> currentEventOrderDict, List<string> eventOrderConstraint)
        {
            var currentEventsOrder = currentEventOrderDict.Select(x => x.Key).ToList();
            var commonUtilities = serviceProvider.GetService<ICommonUtilities>();
            Regex regex = commonUtilities.ListToRegex(eventOrderConstraint);
            bool isValid = regex.IsMatch(string.Join(delimiter, currentEventsOrder)) ? true : false;
            return isValid;
        }
    }
}
