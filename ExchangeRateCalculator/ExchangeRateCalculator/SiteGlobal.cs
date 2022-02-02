using System;
using System.Configuration;

namespace ExchangeRateCalculator
{
    public static class SiteGlobal
    {
        /// <summary>
        /// Gets or sets the valid codes.
        /// </summary>
        /// <value>The valid codes.</value>
        static public string[] ValidCodes { get;  set;}

        /// <summary>
        /// Gets or sets the base currency code.
        /// </summary>
        /// <value>The base currency code.</value>
        static public string BaseCurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the API URL.
        /// </summary>
        /// <value>The API URL.</value>
        static public string ApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the content type header.
        /// </summary>
        /// <value>The content type header.</value>
        static public string ContentTypeHeader { get; set; }

        /// <summary>
        /// Gets or sets the base.
        /// </summary>
        /// <value>The base.</value>
        static public string Base { get; set; }

        /// <summary>
        /// Gets or sets the DBC onnection string.
        /// </summary>
        /// <value>The DBC onnection string.</value>
        static public string DBConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the accept header.
        /// </summary>
        /// <value>The accept header.</value>
        static public string AcceptHeader { get; set; }

        /// <summary>
        /// Gets or sets the data json.
        /// </summary>
        /// <value>The data json.</value>
        static public string DataJson { get; set; }

        static SiteGlobal()
        {

            ApiUrl = ConfigurationManager.AppSettings["apiUrl"];
            ContentTypeHeader = ConfigurationManager.AppSettings["contentTypeHeader"];
            AcceptHeader = ConfigurationManager.AppSettings["acceptHeader"];
            DataJson = ConfigurationManager.AppSettings["rates"];
            Base = ConfigurationManager.AppSettings["base"];
            DBConnectionString = ConfigurationManager.AppSettings["dbConnectionString"];
        }
    }
}
