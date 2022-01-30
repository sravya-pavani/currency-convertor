using System.Configuration;

namespace ExchangeRateCalculator
{
    public static class SiteGlobal
    {
        public static string[] ValidCodes { get;  set;}
        public static string BaseCurrencyCode { get; set; }
        public static string ValidCodesInString { get; set; }
        static public string ApiUrl { get; set; }
        static public string ContentTypeHeader { get; set; }
        static public string Base { get; set; }

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
        public static ExchangeRate rate = new ExchangeRate();

        static SiteGlobal()
        {

            ApiUrl = ConfigurationSettings.AppSettings["apiUrl"];
            ContentTypeHeader = ConfigurationSettings.AppSettings["contentTypeHeader"];
            AcceptHeader = ConfigurationSettings.AppSettings["acceptHeader"];
            DataJson = ConfigurationSettings.AppSettings["rates"];
            Base = ConfigurationSettings.AppSettings["base"];
            //  string conn = ConfigurationManager.ConnectionStrings["prod"].ConnectionString;
            ValidCodes = rate.validCodes;
            foreach (var item in ValidCodes)
                ValidCodesInString += "(" + item + ")/";
            ValidCodesInString = ValidCodesInString.Remove(ValidCodesInString.Length - 1, 1);
        }
    }
}
