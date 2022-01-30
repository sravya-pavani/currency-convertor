using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExchangeRateCalculator
{
    public class CalculateExchangeRate
    {
        public Dictionary<string, double> exchangeRates = new Dictionary<string, double>();
        public CalculateExchangeRate()
        {
            // We call this here to get the currency list supported
            RetrieveExchangeRate();
        }

        public void RetrieveExchangeRate()
        {           
            using (var client = new WebClient())
            {
                client.Headers.Add(SiteGlobal.ContentTypeHeader);
                client.Headers.Add(SiteGlobal.AcceptHeader);
                var res = client.DownloadString(SiteGlobal.ApiUrl);
                if (res != null)
                {
                    JObject results = JObject.Parse(res);
                    exchangeRates = JsonConvert.DeserializeObject<Dictionary<string, double>>(results[SiteGlobal.DataJson].ToString());
                    //Add Base Curerncy also to the dictionary
                    exchangeRates.Add(results[SiteGlobal.Base].ToString(), 1);
               }
            }           
        }

        public Dictionary<string, double>.KeyCollection GetSupportedCurrencies()
        {
                return exchangeRates.Keys;
        }

        public double CalculateCurrency(string firstCurrencyCode, string secondCurrencyCode, int firstCurrencyAmt)
        {
            RetrieveExchangeRate();
            double exchangeRateForInputCurr = exchangeRates[secondCurrencyCode] / exchangeRates[firstCurrencyCode];
            return firstCurrencyAmt * exchangeRateForInputCurr;
        }
    }
}
