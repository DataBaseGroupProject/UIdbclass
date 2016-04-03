using Oracle.ManagedDataAccess.Client;
using System;
using Oracle.DataAccess.Types;
using System.Collections.Generic;
using System.Data;

namespace dbclass2
{
    class DataAccess
    {
        public static void CreateOracleConnection()
        {
        }

        public static OracleConnection con;

        public object CommandType { get; private set; }

        public static void Connect()
        {
            try
            {
                //string oradb = "Data Source=//localhost:1521/xe;User Id=system;Password=admin;";
                string oradb = "Data Source=//taurus.ccec.unf.edu:1521/gporcl;User Id=esmart1;Password=esmart1A3;";

                con = new OracleConnection(oradb);  // C#

                con.Open();
            }
            catch (Exception)
            {

                throw;
            }        
        }

        public static List<string> GetTableName()
        {
            List<string> result = new List<string>();

            try
            {
                Connect();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                cmd.CommandText = "SELECT table_name FROM user_tables ";

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(reader["table_name"].ToString());
                }

                Close();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static void Close()
        {
            con.Close();
            con.Dispose();
        }
    }
}
