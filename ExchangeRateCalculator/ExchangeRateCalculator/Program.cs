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
            // In the custructor a call is made to retrieve the excahnge rates
            // This is needed to check dynamically the supported currencies
            List<string> currencyList = calc.GetSupportedCurrencies().ToArray().ToList();

            if ((currencyList != null) && !currencyList.Any())
            {
                Console.WriteLine("Unable to get Excahnge Rates at the moment");
                return 3;
            }

            if (!args.Any())
            {
                PrintUsage();
                return 2;

            } 
            else if (args.Count() == 1)
            {
                // This program is expected to be called with "backupToDb" as
                // argument every day to store the currency to SQL DB
                if (string.Equals(args[0], "backupToDb", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Backed up current currency rate to DB");
                    String dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                    if (calc.RetrieveExchangeRate(dateNow))
                    {
                        SqlHandler.AddDataToDB(dateNow, calc.GetRetrievedExchangeRates());
                    }
                    else
                    {
                        Console.WriteLine("Unable to get Excahnge Rates at the moment");
                        return 3;
                    }

                }
                else
                {
                    PrintUsage();
                    return 2;
                }

            } 
            // Date is optional. If passed, exchange from that date is retrieved
            //                   Else, the latest is retrieved
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
                            Console.WriteLine("Invalid Date Exception!!" + e);
                            return 1;
                        }
                        secondCurrencyValue = calc.CalculateCurrency(firstCurrencyCode, secondCurrencyCode, firstCurrencyAmt,
                                dt.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        secondCurrencyValue = calc.CalculateCurrency(firstCurrencyCode, secondCurrencyCode, firstCurrencyAmt);
                    }

                    if (secondCurrencyValue < 0)
                    {
                        Console.WriteLine("Unable to get Excahnge Rates at the moment");
                        return 3;
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
    }
}