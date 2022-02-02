using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ExchangeRateCalculator
{
    /// <summary>
    /// Assumption is that the DB and Table is created.
    /// 
    /// Table Name: EXCHANGE_HISTORY
    /// 
    /// The table is designed as follows:
    /// CREATE TABLE EXCHANGE_HISTORY(EXCHANGE_DATE NVARCHAR(50) NOT NULL, CURRENCY_CODE VARCHAR(10) NOT NULL, EXCHANGE_RATE DECIMAL(10, 7), PRIMARY KEY (EXCHANGE_DATE, CURRENCY_CODE))
    /// 
    /// The Table is designed to have a composite Key of Date and Currency Code
    /// 
    /// </summary>

    public class SqlHandler
    {
        readonly static string sqlCmdInsert = "INSERT INTO EXCHANGE_HISTORY(EXCHANGE_DATE,CURRENCY_CODE,EXCHANGE_RATE) VALUES(@param1,@param2,@param3)";

        public SqlHandler()
        {
        }

        /// <summary>
        /// Adds the data to SQL db.
        /// </summary>
        /// <param name="date">Date on which the rates are retrieved</param>
        /// <param name="rates">Currency Exchange Rates. The base currency can always be 
        /// identified as the entry having exchange rate 1</param>
        public static void AddDataToDB(string date, Dictionary<string, double> rates)
        {
            using (SqlConnection conn = new SqlConnection(SiteGlobal.DBConnectionString))
            {
                //TODO: Handle exception here, in case connection is not available
                conn.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = conn;
                    command.CommandType = CommandType.Text;

                    using (SqlCommand cmd = new SqlCommand(sqlCmdInsert, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@param1", SqlDbType.NVarChar, 50));
                        cmd.Parameters.Add(new SqlParameter("@param2", SqlDbType.VarChar, 10));
                        cmd.Parameters.Add(new SqlParameter("@param3", SqlDbType.Decimal));
                        foreach (KeyValuePair<string, double> item in rates)
                        {
                            cmd.Parameters[0].Value = date;
                            cmd.Parameters[1].Value = item.Key;
                            cmd.Parameters[2].Value = item.Value;
                            cmd.CommandType = CommandType.Text;
                            //TODO: Handle exception here, in case the data with Key already exists
                            cmd.ExecuteNonQuery();
                        }

                    }
                }

                conn.Close();
            }
        }
    }
}
