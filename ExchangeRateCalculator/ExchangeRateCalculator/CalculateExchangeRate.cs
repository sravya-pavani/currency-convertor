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
            RetrieveExchangeRate();
        }

        /// <summary>
        /// Retrived latest excahnge rate and store it in the "exchangeRates"
        /// </summary>
        public void RetrieveExchangeRate()
        {
            RetrieveExchangeRate("latest");
        }

        /// <summary>
        /// Retrived exchange rate from "exchangeRateDate" and store it in the "exchangeRates"
        /// The base currency is also treated just like other currency, but with exhcange rate 1
        /// </summary>
        /// <returns>If operation is successful</returns>
        public Boolean RetrieveExchangeRate(string exchangeRateDate)
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
                    return bool.Parse(results["success"].ToString());
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the supported currencies.
        /// </summary>
        /// <returns>The supported currencies.</returns>
        public Dictionary<string, double>.KeyCollection GetSupportedCurrencies()
        {
                return exchangeRates.Keys;
        }

        /// <summary>
        /// Gets the retrieved exchange rates.
        /// </summary>
        /// <returns>The retrieved exchange rates.</returns>
        public Dictionary<string, double> GetRetrievedExchangeRates()
        {
            return exchangeRates;
        }

        /// <summary>
        /// Calculates the currency.
        /// </summary>
        /// <returns>The converted currency</returns>
        /// <param name="firstCurrencyCode">First currency code.</param>
        /// <param name="secondCurrencyCode">Second currency code.</param>
        /// <param name="firstCurrencyAmt">First currency amt.</param>
        public double CalculateCurrency(string firstCurrencyCode, string secondCurrencyCode, int firstCurrencyAmt)
        {
            return CalculateCurrency(firstCurrencyCode, secondCurrencyCode, firstCurrencyAmt, "latest");
        }

        /// <summary>
        /// Calculates the currency.
        /// </summary>
        /// <returns>The converted currency on given date</returns>
        /// <param name="firstCurrencyCode">First currency code.</param>
        /// <param name="secondCurrencyCode">Second currency code.</param>
        /// <param name="firstCurrencyAmt">First currency amt.</param>
        /// <param name="exchangeRateDate">Exchange rate date.</param>
        public double CalculateCurrency(string firstCurrencyCode, string secondCurrencyCode, int firstCurrencyAmt, string exchangeRateDate)
        {
            if (RetrieveExchangeRate(exchangeRateDate))
            {
                double exchangeRateForInputCurr = exchangeRates[secondCurrencyCode] / exchangeRates[firstCurrencyCode];
                return firstCurrencyAmt * exchangeRateForInputCurr;
            }
            else
            {
                return -1;
            }
        }
    }
}
