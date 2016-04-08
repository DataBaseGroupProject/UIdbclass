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
                //string oradb = "Data Source=//localhost:1521/xe;User Id=system;Password=karthika86;";
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

                cmd.CommandText = "SELECT table_name FROM user_tables";

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

        public static List<string> GetColumns(string tabname)
        {
            List<string> result = new List<string>();

            //List<string> selectedtable = new List<string>();
            //selectedtable = tabnames;

            string selectedtable = tabname;

            //foreach (string tabname in selectedtable)
            //{
                try
                {
                    Connect();

                    OracleCommand cmd = new OracleCommand();

                    cmd.Connection = con;
                    string query = "SELECT column_name FROM all_tab_columns where table_name =" + "'" + tabname + "'";
                    cmd.CommandText = query;

                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(reader["column_name"].ToString());
                    }

                    Close();
                }
                catch (Exception)
                {
                    throw;
                }
            //}
            return result;  
        }
        /*public static List<string> RemoveColumns(string tabname)
        {
            List<string> result = new List<string>();

            //List<string> selectedtable = new List<string>();
            //selectedtable = tabnames;

            string selectedtable = tabname;

            //foreach (string tabname in selectedtable)
            //{
            try
            {
                Connect();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;
                string query = "SELECT column_name FROM all_tab_columns where table_name =" + "'" + tabname + "'";
                cmd.CommandText = query;

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //result.Add(reader["column_name"].ToString());
                    for(int i = 0; i < result.)
                }

                Close();
            }
            catch (Exception)
            {
                throw;
            }
            //}
            return result;
        }*/


        /// <summary>
        /// Check if Table Exist in DB
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>int</returns>
        public static int DoesTableExist(string tableName)
        {
            List<string> result = new List<string>();

            try
            {
                Connect();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                cmd.CommandText = "SELECT table_name FROM user_tables Where table_name = '" + tableName + "'";

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

            return result.Count;
        }

        /// <summary>
        /// Create Dimenstional Table 
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        public static int CreateDimenstionalTable(DimensionalTableInfo Table)
        {
            int result = 0;

            string pk = string.Empty;

            Table = new DimensionalTableInfo();

            Table.TableName = "DOC";

            Table.PrimaryKeys = new Dictionary<string, string>();

            Table.PrimaryKeys.Add("PID", "Int");

            Table.Columns = new Dictionary<string, string>();

            Table.Columns.Add("Col1", "Int");
            Table.Columns.Add("Col2", "Varchar(50)");

            try
            {
                if (DoesTableExist(Table.TableName) > 0)
                    return -99;

                Connect();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                StringBuilder sb = new StringBuilder();

                sb.AppendLine(("CREATE TABLE " + Table.TableName));

                sb.AppendLine((" ( "));

                foreach (var item in Table.PrimaryKeys)
                {
                    sb.AppendLine(item.Key + " " + item.Value + ",");

                    pk = pk + item.Key + ",";
                }

                foreach (var item in Table.Columns)
                {
                    sb.AppendLine(item.Key + " " + item.Value + ",");
                }

                pk = pk.TrimEnd(',');

                sb.AppendLine(" CONSTRAINT " + Table.TableName + "_PK PRIMARY KEY (" + pk + ") ");

                sb.AppendLine((")"));

                cmd.CommandText = sb.ToString();

                result = cmd.ExecuteNonQuery();

                Close();
            }
            catch (Exception ex)
            {
                throw ex;
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
