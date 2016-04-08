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

        public static List<string> GetNonKey(string tabname)
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
                string query = "SELECT column_name FROM all_tab_columns WHERE table_name =" + "'" + tabname + "'" + " and nullable = 'Y' ";
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

        public static List<string> GetPrimaryKey(string tabname)
        {

            List<string> result = new List<string>();

            //List<string> selectedtable = new List<string>();
            //selectedtable = tabnames;

            string selectedtable = tabname;
            try { 
            Connect();
            OracleCommand cmd = new OracleCommand();

            
                 cmd.Connection = con;
            string query = "SELECT column_name FROM all_tab_columns WHERE table_name =" + "'" + tabname + "'" +" and nullable = 'N' ";
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

    public static void Close()
        {
            con.Close();
            con.Dispose();
        }
    }
}
