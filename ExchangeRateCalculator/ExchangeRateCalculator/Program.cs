using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExchangeRateCalculator
{
    class MainClass
    {
        static string firstCurrencyCode = "";
        static string secondCurrencyCode = "";
        static int firstCurrencyAmt = 0;
        static CalculateExchangeRate calc = new CalculateExchangeRate();


        public static int Main(string[] args)
        {
            List<string> currencyList = calc.GetSupportedCurrencies().ToArray().ToList();

            if (!args.Any())
            {
                PrintUsage();
                return 2;

            } 
            else if (args.Count() == 1)
            {
                if (string.Equals(args[0], "backupToDb", StringComparison.OrdinalIgnoreCase))
                {
                    // TODO: Implement logic to backup the currency data to DB
                    Console.Write("Backed up current dcurrency rate to DB");
                }
                else
                {
                    PrintUsage();
                    return 2;
                }

            } 
            else if (args.Count() == 3 || args.Count() == 4)
            {
                firstCurrencyCode = args[0].ToUpper();
                secondCurrencyCode = args[1].ToUpper();
                double secondCurrencyValue;
                if (currencyList.Contains(firstCurrencyCode) && 
                    currencyList.Contains(secondCurrencyCode) &&
                    int.TryParse(args[2], out firstCurrencyAmt)) {
                    if (args.Count() == 4 && args[3] != null)
                    {
                        DateTime dt;
                        try
                        {
                            dt = Convert.ToDateTime(args[3]);
                        } catch (FormatException e)
                        {
                            Console.WriteLine("Invalid Date !!");
                            return 1;
                        }
                        secondCurrencyValue = calc.CalculateCurrency(firstCurrencyCode, secondCurrencyCode, firstCurrencyAmt,
                                dt.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        secondCurrencyValue = calc.CalculateCurrency(firstCurrencyCode, secondCurrencyCode, firstCurrencyAmt);
                    }
                    Console.WriteLine(firstCurrencyAmt + " " + firstCurrencyCode + " = " + secondCurrencyValue + " " + secondCurrencyCode);
                }
                else
                {
                    Console.WriteLine("UnSupported Currency");
                    PrintUsage();
                    return 2;
                }
            }
            else
            {
                PrintUsage();
                return 2;
            }
            return 0;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Invalid Input !!");
            Console.WriteLine("Usage:");
            string programName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Console.WriteLine(programName + " <FROM_CURRENCYCODE> <TO_CURRENCYCODE> <AMOUNT> [<DATE>]");
            Console.WriteLine("OR");
            Console.WriteLine(programName + " backupToDb");
            Console.WriteLine("");
            Console.Write("Supported Currencies: ");
            List<string> currencyList = calc.GetSupportedCurrencies().ToArray().ToList();
            currencyList.ForEach(currCode => Console.Write(currCode.ToString() + " "));
        }

        public static string ValidateCurrencyCode(string codeName)
        {
            while (string.IsNullOrEmpty(codeName))
            {
                Console.WriteLine("Currency Code can't be empty! Input your Currency Code once more");
                codeName = Console.ReadLine();
            }
            while (!CheckCodeNames(codeName))
            {
                Console.WriteLine("Input your Currency Code in these values: ");
                calc.GetSupportedCurrencies().ToArray().ToList().ForEach(currCode => Console.Write(currCode.ToString() + " "));
                Console.WriteLine("> ");
                codeName = Console.ReadLine();
            }
            return codeName;
        }

        public static bool CheckCodeNames(string codeName)
        {
            bool isValid = false;
            if (SiteGlobal.ValidCodes.Contains(codeName.ToUpper()))
                isValid = true;
            else
                isValid = false;
            return isValid;
        }
    }
}