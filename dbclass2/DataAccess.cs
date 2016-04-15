using Oracle.ManagedDataAccess.Client;
using System;
using Oracle.DataAccess.Types;
using System.Collections.Generic;
using System.Data;
using System.Text;
using dbclass2.Objects;

namespace dbclass2
{
    class DataAccess
    {
        public static OracleConnection con;

        public object CommandType { get; private set; }

        public static AccessInfo ConnectionInfo { get; set; }

        public static string _DevAccessInfo = "Data Source=//localhost:1521/xe;User Id=system;Password=admin;";
        //public static string _DevAccessInfo = "Data Source=//taurus.ccec.unf.edu:1521/gporcl;User Id=esmart1;Password=esmart1A3;";

        public static string DevAccessInfo { get { return _DevAccessInfo; }}

        public static void LoginConnect(AccessInfo Access = null)
        {
            try
            {
                string oradb = string.Empty;

                if (Access == null)
                {
                    oradb = DevAccessInfo;
                }
                else
                {
                    oradb = "Data Source=//" + Access.SourceUrl + ";User Id=" + Access.SourceUserName + ";Password=" + Access.SourcePassword + ";";
                }

                con = new OracleConnection(oradb);  // C#

                con.Open();

                if (Access == null)
                {
                    oradb = DevAccessInfo;
                }
                else
                {
                    oradb = "Data Source=//" + Access.TargetUrl + ";User Id=" + Access.TargetUserName + ";Password=" + Access.TargetPassword + ";";
                }

                con = new OracleConnection(oradb);  // C#

                con.Open();

                ConnectionInfo = Access;
            }
            catch (Exception)
            {

                throw;
            }
        }

      
        public static void Connect(string ConnectionType = null)
        {
            try
            {
                string oradb = string.Empty;

                if (ConnectionType == "Source")
                    oradb = "Data Source=//" + ConnectionInfo.SourceUrl + ";User Id=" + ConnectionInfo.SourceUserName + ";Password=" + ConnectionInfo.SourcePassword + ";";
                else if (ConnectionType == "Destination")
                    oradb = "Data Source=//" + ConnectionInfo.TargetUrl + ";User Id=" + ConnectionInfo.TargetUserName + ";Password=" + ConnectionInfo.TargetPassword + ";";
                else
                    oradb = DevAccessInfo;

                con = new OracleConnection(oradb);  // C#

                con.Open();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public static void Connect2()
        {
            try
            {
                string oradb = "Data Source=//localhost:1521/xe;User Id=system;Password=admin;";
                //string oradb = "Data Source=//taurus.ccec.unf.edu:1521/gporcl;User Id=esmart1;Password=esmart1A3;";

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

        public static List<string> GetTableName2()
        {
            List<string> result = new List<string>();

            try
            {
                Connect2();

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




        public static List<string> GetNonKey(string selectedtable)
        {
            List<string> result = new List<string>();

            try
            {
                Connect();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                string query = (@"SELECT column_name, data_type, nullable, data_length
                                  FROM all_tab_cols
                                  WHERE  column_name Not In (SELECT cols.column_name
                                                             FROM all_constraints cons, all_cons_columns cols
                                                             WHERE cons.constraint_type = 'P'
                                                                   AND cons.constraint_name = cols.constraint_name
                                                                   AND cons.owner = cols.owner 
                                                                   AND cols.table_name = " + "'" + selectedtable + "')" +
                                          "AND table_name = " + "'" + selectedtable + "'");


                cmd.CommandText = query;

                OracleDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(reader["column_name"].ToString() + "<-->" + reader["data_type"].ToString() + "(" + reader["data_length"].ToString() + ")" + "<-->" + selectedtable);
                }

                Close();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static List<ColumnInfo> GetNonKeyObject(string selectedtable)
        {
            List<ColumnInfo> result = new List<ColumnInfo>();

            try
            {
                Connect();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                string query = (@"SELECT column_name, data_type, nullable, data_length
                                  FROM all_tab_cols
                                  WHERE  column_name Not In (SELECT cols.column_name
                                                             FROM all_constraints cons, all_cons_columns cols
                                                             WHERE cons.constraint_type = 'P'
                                                                   AND cons.constraint_name = cols.constraint_name
                                                                   AND cons.owner = cols.owner 
                                                                   AND cols.table_name = " + "'" + selectedtable + "')" +
                                          "AND table_name = " + "'" + selectedtable + "'");

                cmd.CommandText = query;

                OracleDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ColumnInfo obj = new ColumnInfo();

                    obj.Name = reader["column_name"].ToString();
                    obj.DataType = reader["data_type"].ToString();
                    obj.IsNull = reader["nullable"].ToString();
                    obj.DataLength = reader["data_length"].ToString();
                    obj.TransTable = selectedtable;

                    result.Add(obj);
                }

                Close();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static List<string> GetPrimaryKey(string tabname)
        {
            List<string> result = new List<string>();

            string selectedtable = tabname;

            try
            {
                Connect();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                string query = (@"SELECT DISTINCT AllColumns.column_name, AllColumns.data_type, AllColumns.data_length

                                  FROM all_tab_columns AllColumns
                                  JOIN all_cons_columns Cols ON AllColumns.column_name = cols.column_name
                                  JOIN all_constraints Cons ON cons.constraint_name = cols.constraint_name AND cons.owner = cols.owner
                                  
                                  WHERE (cons.constraint_type = 'P' OR Cons.constraint_type = 'U' OR AllColumns.nullable = 'N') 
                                        And AllColumns.table_name = " + "'" + tabname + "'");

                cmd.CommandText = query;

                OracleDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(reader["column_name"].ToString() + "<-->" + reader["data_type"].ToString() + "(" + reader["data_length"].ToString() + ")" + "<-->" + tabname);
                }

                Close();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static List<ColumnInfo> GetPrimaryKeyObject(string tabname)
        {
            List<ColumnInfo> result = new List<ColumnInfo>();

            string selectedtable = tabname;

            try
            {
                Connect();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                string query = (@"SELECT DISTINCT AllColumns.column_name, AllColumns.data_type, AllColumns.nullable, AllColumns.data_length, cons.constraint_type

                                  FROM all_tab_columns AllColumns
                                  JOIN all_cons_columns Cols ON AllColumns.column_name = cols.column_name
                                  JOIN all_constraints Cons ON cons.constraint_name = cols.constraint_name AND cons.owner = cols.owner
                                  
                                  WHERE (cons.constraint_type = 'P' OR Cons.constraint_type = 'U' OR AllColumns.nullable = 'N') 
                                        And AllColumns.table_name = " + "'" + tabname + "'");

                cmd.CommandText = query;

                OracleDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ColumnInfo obj = new ColumnInfo();

                    obj.Name = reader["column_name"].ToString();
                    obj.DataType = reader["data_type"].ToString();
                    obj.IsNull = reader["nullable"].ToString();
                    obj.DataLength = reader["data_length"].ToString();
                    obj.ConstraintType = reader["constraint_type"].ToString();
                    obj.TransTable = tabname;

                    result.Add(obj);
                }

                Close();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

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
        /// <returns>Int Update Count</returns>
        public static int CreateDimenstionalTable(DimensionalTableInfo Table)
        {
            int result = 0;

            string pk = string.Empty;

            //Table = new DimensionalTableInfo();

            //Table.TableName = "DOC";

            //Table.PrimaryKeys = new Dictionary<string, string>();

            //Table.PrimaryKeys.Add("PID", "Int");

            //Table.Columns = new Dictionary<string, string>();

            //Table.Columns.Add("Col1", "Int");
            //Table.Columns.Add("Col2", "Varchar(50)");

            try
            {
                if (DoesTableExist(Table.TableName) > 0)
                    return -99;

                Connect();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                StringBuilder sb = new StringBuilder();

                Table.TableName = Table.TableName.Replace(' ', '_');

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

        /// <summary>
        /// Create Fact Table 
        /// </summary>
        /// <param name="Table"></param>
        /// <returns>Int Update Count</returns>
        public static int CreateFactTable(FactTableInfo Table)
        {
            int result = 0;

            string pk = string.Empty;

            //Humam- Testing Code
            //---------------------
            //Table = new FactTableInfo();

            //Table.TableName = "ABC";

            //Table.PrimaryKeys = new Dictionary<string, string>();

            //Table.PrimaryKeys.Add("Dir", "Int");

            //Table.Columns = new Dictionary<string, string>();

            //Table.Columns.Add("Col3", "Int");
            //Table.Columns.Add("PID", "Int");

            //Table.Relations = new Dictionary<string, string>();

            //Table.Relations.Add("DOC", "PID");

            //---------------------

            try
            {
                if (DoesTableExist(Table.TableName) > 0)
                    return -99;

                Connect();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                StringBuilder sb = new StringBuilder();

                Table.TableName = Table.TableName.Replace(' ', '_');

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

                foreach (var item in Table.Relations)
                {
                    sb.AppendLine(" CONSTRAINT " + item.Key + "_FK FOREIGN KEY (" + item.Value + ") REFERENCES " + item.Key + "(" + item.Value + "),");
                }

                pk = pk.TrimEnd(',');

                sb.AppendLine(" CONSTRAINT " + Table.TableName + "_PK PRIMARY KEY (" + pk + ")");

                sb.AppendLine((")"));

                cmd.CommandText = sb.ToString();

                result = cmd.ExecuteNonQuery();

                Close();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static int BuildDataWarhouse(List<string> tables)
        {
            int result = 0;

            try
            {
                if(tables.Count > 0)
                {
                    FactTableInfo fact = new FactTableInfo();

                    fact.PrimaryKeys = new Dictionary<string, string>();
                    fact.Columns = new Dictionary<string, string>();
                    fact.Relations = new Dictionary<string, string>();

                    fact.TableName = "Auto_Generated_Fact_Table";

                    fact.PrimaryKeys.Add("ID", "int");

                    foreach (var table in tables)
                    {
                        DimensionalTableInfo d = new DimensionalTableInfo();

                        d.TableName = table + "_Dimensional";

                        List<ColumnInfo> column = GetPrimaryKeyObject(table);

                        column.RemoveAll(i => i.ConstraintType != "P");

                        if(column.Count > 0)
                        {
                            d.PrimaryKeys = new Dictionary<string, string>();

                            foreach(var key in column)
                            {
                                if(!d.PrimaryKeys.ContainsKey(key.Name))
                                    d.PrimaryKeys.Add(key.Name, key.DataType + "(" + key.DataLength + ")");

                                if (!fact.Columns.ContainsKey(key.Name))
                                    fact.Columns.Add(key.Name, key.DataType + "(" + key.DataLength + ")");

                                if (!fact.Relations.ContainsKey(table))
                                    fact.Relations.Add(table, key.Name);
                            }
                        }

                        column = GetNonKeyObject(table);

                        if (column.Count > 0)
                        {
                            d.Columns = new Dictionary<string, string>();

                            foreach (var key in column)
                            {
                                if(!d.Columns.ContainsKey(key.Name))
                                    d.Columns.Add(key.Name, key.DataType + "(" + key.DataLength + ")");
                            }
                        }

                        result += CreateDimenstionalTable(d);
                    }

                    if(result == -1)
                    {
                        result = CreateFactTable(fact);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return result;
        }

        /// <summary>
        ///  Load Data for Dimension Table
        /// </summary>
        /// <param name="Table"></param>
        /// <returns>Int Update Count</returns>

        public static int LoadDimensionTableData(FactTableInfo Table)
        {
            int result = 0;

            string pk = string.Empty;

            try
            {
                Connect();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                StringBuilder sb = new StringBuilder();

                foreach (var item in Table.Relations)
                {


                }

                cmd.CommandText = sb.ToString();

                result = cmd.ExecuteNonQuery();

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
