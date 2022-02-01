using System;
using System.Configuration;
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
            RetrieveExchangeRate();
        }

        public void RetrieveExchangeRate()
        {
            RetrieveExchangeRate("latest");
        }

        public void RetrieveExchangeRate(string exchangeRateDate)
        {           
            using (var client = new WebClient())
            {
                client.Headers.Add(SiteGlobal.ContentTypeHeader);
                client.Headers.Add(SiteGlobal.AcceptHeader);
                string uriPath = SiteGlobal.ApiUrl.Replace("{Date}", exchangeRateDate);

                var res = client.DownloadString(uriPath);
                if (res != null)
                {
                    JObject results = JObject.Parse(res);
                    exchangeRates = JsonConvert.DeserializeObject<Dictionary<string, double>>(results[SiteGlobal.DataJson].ToString());
                    //Add Base Currency also to the dictionary
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
            return CalculateCurrency(firstCurrencyCode, secondCurrencyCode, firstCurrencyAmt, "latest");
        }

        public double CalculateCurrency(string firstCurrencyCode, string secondCurrencyCode, int firstCurrencyAmt, string exchangeRateDate)
        {
            RetrieveExchangeRate(exchangeRateDate);
            double exchangeRateForInputCurr = exchangeRates[secondCurrencyCode] / exchangeRates[firstCurrencyCode];
            return firstCurrencyAmt * exchangeRateForInputCurr;
        }
    }
}
