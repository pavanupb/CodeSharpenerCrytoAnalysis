using CodeSharpenerCryptoAnalysis.AnalyzerModels;
using CodeSharpenerCryptoAnalysis.Common;
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
        public bool IsValidOrder(ValidEvents validEvents, string containingMethod, List<KeyValuePair<string, string>> eventsOrderDict, List<string> eventOrderContraint)
        {
            if (validEvents.IsValidEvent)
            {
                foreach (var events in validEvents.ValidEventsDict)
                {
                    var analyzedEvents = eventsOrderDict.Select(x => x).Where(y => y.Key.Equals(validEvents.AggregatorName)).Select(x => x.Value);
                    if (analyzedEvents.Count() != 0)
                    {
                        if (analyzedEvents.FirstOrDefault().Equals(containingMethod))
                        {
                            var currentEventsOrder = eventsOrderDict.Select(x => x.Key).ToList();
                            var commonUtilities = serviceProvider.GetService<ICommonUtilities>();
                            Regex regex = commonUtilities.ListToRegex(eventOrderContraint);
                            bool valid = regex.IsMatch(string.Join(delimiter, currentEventsOrder)) ? true : false;
                            if (!valid)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        KeyValuePair<string, string> eventsToAdd = new KeyValuePair<string, string>(validEvents.AggregatorName, containingMethod);
                        eventsOrderDict.AddRange(Enumerable.Repeat(eventsToAdd, events.Value.Count));
                    }
                }
            }
            return true;
        }
    }
}
