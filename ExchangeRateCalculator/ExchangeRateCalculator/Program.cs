using System;

using System.Linq;

namespace ExchangeRateCalculator
{
    class MainClass
    {
        static string firstCurrencyCode = "";
        static string secondCurrencyCode = "";
        static int firstCurrencyAmt = 0;
        static CalculateExchangeRate calc = new CalculateExchangeRate();


        public static void Main(string[] args)
        {
            Console.Write("Supported Currencies: ");
            System.Collections.Generic.List<string> currencyList = calc.GetSupportedCurrencies().ToArray().ToList();
            currencyList.ForEach(currCode => Console.Write(currCode.ToString() + " "));
            if (!args.Any())
            {
                // No arguments provided, hence prompting for currencies
                ReadInput();
                double secondCurrencyValue = calc.CalculateCurrency(firstCurrencyCode, secondCurrencyCode, firstCurrencyAmt);
                Console.WriteLine(firstCurrencyAmt + firstCurrencyCode + " = " + secondCurrencyValue + secondCurrencyCode);
            } else if (args.Count() == 2)
            {
                if (string.Equals(args[1], "backupToDb", StringComparison.OrdinalIgnoreCase))
                {
                    // TODO: Implement logic to backup the currency data to DB
                    Console.Write("Backed up current dcurrency rate to DB");
                }
                else
                {
                    PrintUsage(args[0]);
                }

            } else if (args.Count() == 4)
            {
                firstCurrencyCode = args[1].ToUpper();
                secondCurrencyCode = args[2].ToUpper();
                if (!currencyList.Contains(firstCurrencyCode) || 
                    !currencyList.Contains(secondCurrencyCode) ||
                    int.TryParse(args[3], out firstCurrencyAmt)) {
                    double secondCurrencyValue = calc.CalculateCurrency(firstCurrencyCode, secondCurrencyCode, firstCurrencyAmt);
                    Console.WriteLine("convertedValue = " + secondCurrencyValue);
                }
                else
                {
                    PrintUsage(args[0]);
                }
            }
            else
            {
                PrintUsage(args[0]);
            }

        }

        private static void PrintUsage(string programName)
        {
            Console.WriteLine("Invalid Input !!");
            Console.WriteLine("Usage:");
            Console.WriteLine(programName + "<FROM_CURRENCYCODE> <TO_CURRENCYCODE> <AMOUNT>");
            Console.WriteLine("OR");
            Console.WriteLine(programName + "backupToDb");
        }

        /// <summary>
        /// Reads the input from the user. Example: EUR
        /// </summary>
        public static void ReadInput()
        {
            Console.WriteLine();
            Console.Write("Enter Currency Code to be converted FROM : ");
            firstCurrencyCode = Console.ReadLine();
            firstCurrencyCode = ValidateCurrencyCode(firstCurrencyCode).ToUpper();

            Console.Write("Enter Currency Code to be converted TO : ");
            secondCurrencyCode = Console.ReadLine();
            secondCurrencyCode = ValidateCurrencyCode(secondCurrencyCode).ToUpper();
            

            Console.Write("Enter First Currency in amount: ");
            var firstCurrencyAsString = Console.ReadLine();

            while (!int.TryParse(firstCurrencyAsString, out firstCurrencyAmt))
            {
                Console.WriteLine("This is not a number! Input your First Currency in amount once more");
                firstCurrencyAsString = Console.ReadLine();
            }
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