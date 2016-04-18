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
        public static OracleConnection con2;

        public object CommandType { get; private set; }

        public static AccessInfo ConnectionInfo { get; set; }

        //public static string _DevAccessInfo = "Data Source=//localhost:1521/xe;User Id=system;Password=admin;";
        public static string _DevAccessInfo = "Data Source=//taurus.ccec.unf.edu:1521/gporcl;User Id=esmart1;Password=esmart1A3;";

        public static string DevAccessInfo { get { return _DevAccessInfo; }}

        public static int num = 0;

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



        //public static void Form2Connect(AccessInfo Access = null)
        //{
        //    try
        //    {
        //        string oradb = string.Empty;

        //        if (Access == null)
        //        {
        //            oradb = _Form2Access;
        //        }
        //        else
        //        {
        //            oradb = "Data Source=//" + Access.TargetUrl + ";User Id=" + Access.TargetUserName + ";Password=" + Access.TargetPassword + ";";
        //        }

        //        con = new OracleConnection(oradb);  // C#

        //        con.Open();

        //        ConnectionInfo = Access;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


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

        //will function as an alternative connection string for esmart2 but currently is setup for esmart1
        public static void Connect2()
        {
            try
            {
                //string oradb = "Data Source=//localhost:1521/xe;User Id=system;Password=admin;";
                string oradb = "Data Source=//taurus.ccec.unf.edu:1521/gporcl;User Id=esmart2;Password=esmart2A3;";

                con = new OracleConnection(oradb);  // C#

                con.Open();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public static void Connect2()
        //{
        //     string oradb2 = "Data Source=//taurus.ccec.unf.edu:1521/gporcl;User Id=esmart1;Password=esmart1A3; ";
        //    //string oradb2 = "Data Source=//localhost:1521/xe;User Id=system;Password=xoxoxo83;";
        //    con2 = new OracleConnection(oradb2);
        //    con2.Open();
        //}


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

        //queries for the dimension table to display the dimension table in form2
        public static List<string> GetDimTable()
        {
            List<string> result = new List<string>();

            try
            {
                Connect2();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                cmd.CommandText = "SELECT table_name FROM user_tables WHERE table_name NOT LIKE 'Fact%'";

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

        //queries for the fact table to display fact table in form2
        public static List<string> GetFactTable()
        {
            List<string> result = new List<string>();

            try
            {
                Connect2();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                cmd.CommandText = "SELECT table_name FROM user_tables WHERE table_name LIKE 'Fact%'";

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


                string query = (@"SELECT DISTINCT
                                        ucc.table_name
                                        , ucc.column_name
                                        , uc.constraint_type
                                        , AllColumns.nullable
                                        , AllColumns.data_length
                                        , AllColumns.data_type
      
                                    FROM user_cons_columns   ucc
                                        ,user_constraints    uc
                                        ,all_tab_columns AllColumns
      
                                    WHERE ucc.constraint_name = uc.constraint_name
                                    AND ucc.table_name      = uc.table_name
                                    AND AllColumns.column_name = ucc.column_name
                                    AND (uc.constraint_type  = 'P'  OR uc.constraint_type = 'U' OR uc.constraint_type = 'R')
                                    AND uc.table_name = " + "'" + tabname.ToUpper() + "'");

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

        public static List<ColumnInfo> GetAllColumnsObject(string tabname)
        {
            List<ColumnInfo> result = new List<ColumnInfo>();

            string selectedtable = tabname;

            try
            {
                Connect();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                string query = (@"SELECT DISTINCT AllColumns.column_name, AllColumns.data_type, AllColumns.nullable, AllColumns.data_length

                                  FROM all_tab_columns AllColumns
                                                                    
                                  WHERE AllColumns.table_name = " + "'" + tabname + "'");

                cmd.CommandText = query;

                OracleDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ColumnInfo obj = new ColumnInfo();

                    obj.Name = reader["column_name"].ToString();
                    obj.DataType = reader["data_type"].ToString();
                    obj.IsNull = reader["nullable"].ToString();
                    obj.DataLength = reader["data_length"].ToString();
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

                cmd.CommandText = "SELECT table_name FROM user_tables Where table_name = '" + tableName.ToUpper() + "'";

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
            //string result = "";

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


        public static int ManualCreateDimenstionalTable(string manseltab)
        {
            int result = 0;

            StringBuilder ren_manseltab = new StringBuilder();

            string incr = "";

            try
            {

                Connect();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                StringBuilder sb = new StringBuilder();

                incr = num.ToString();

                ren_manseltab.Append(manseltab);

                ren_manseltab.Append("_Man_Dim_");

                ren_manseltab.Append(incr);

                if (DoesTableExist(ren_manseltab.ToString()) > 0)
                    return -99;

                sb.AppendLine(("CREATE TABLE " + ren_manseltab + " AS "));

                sb.AppendLine(("("));

                sb.AppendLine(("SELECT * FROM " + manseltab));

                sb.AppendLine((")"));

                cmd.CommandText = sb.ToString();

                result = cmd.ExecuteNonQuery();

                num++;

                Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }


        /// <summary>
        /// Create and insert Dimensional Table 
        /// </summary>
        /// <param name="Table"></param>
        /// <returns>Int Update Count</returns>
        /*public static int InsertDimensionalData(string dimtable,List<string> pkslist,List<string> npkslist)
        {
            int result = 0;
            List<string> result1 = new List<string>();
            try
            {
               // if (DoesTableExist(dimtable) > 0)
                 //   return -99;

                Connect();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                StringBuilder sb = new StringBuilder();
                StringBuilder selectcolssb = new StringBuilder();
                StringBuilder selecttabssb = new StringBuilder();
                List<string> selectpkcollists = new List<string>();
                List<string> selectnpkcollists = new List<string>();
                List<string> seltables = new List<string>();

                foreach (var item in pkslist)
                {
                    string[] keyInfo = item.ToString().Split(new string[] { "<-->" }, StringSplitOptions.None);

                    selectcolssb.AppendFormat(keyInfo[2]);
                    selectcolssb.Append('.');
                    selectcolssb.AppendFormat(keyInfo[0]);

                    selectpkcollists.Add(selectcolssb.ToString());

                    if (!(seltables.Contains(keyInfo[2])))
                        seltables.Add(keyInfo[2]);
                }

                foreach (var item in npkslist)
                {
                    string[] keyInfo = item.ToString().Split(new string[] { "<-->" }, StringSplitOptions.None);

                    selectcolssb.AppendFormat(keyInfo[2]);
                    selectcolssb.Append('.');
                    selectcolssb.AppendFormat(keyInfo[0]);

                    selectnpkcollists.Add(selectcolssb.ToString());

                    if (!(seltables.Contains(keyInfo[2])))
                        seltables.Add(keyInfo[2]);
                }

                StringBuilder selpkcols = new StringBuilder();
                StringBuilder selnpkcols = new StringBuilder();
                int count;
                count = selectpkcollists.Count;
                for (int i = 0; i < count; i++)
                {
                    if(selectnpkcollists.Count != 0)
                    {
                        selpkcols.AppendFormat(selectpkcollists[i]);
                        selpkcols.Append(',');
                    }
                    else
                    {
                        selpkcols.AppendFormat(selectpkcollists[i]);
                    }                        
                }

                count = selectpkcollists.Count + selectnpkcollists.Count;
                //count = selectnpkcollists.Count;
                for (int i = selectnpkcollists.Count ; i < count; i++)
                {
                    if (i < (count))
                    {
                        if(i != count-1)
                        {
                            selnpkcols.AppendFormat(selectnpkcollists[i]);
                            selnpkcols.Append(',');
                        }
                        else
                        {
                            selnpkcols.AppendFormat(selectnpkcollists[i]);
                        }
                    }                    
                }

                int tabcnt = seltables.Count;
                string s = "";
                for (int i = 0; i < tabcnt; i++)
                {
                    if (i != (count - 1))
                    {
                        s = seltables[i];
                        selecttabssb.AppendFormat(seltables[i]);
                        selecttabssb.Append(' ');
                        selecttabssb.Append(s[0]);
                        selecttabssb.Append(',');
                    }
                    else
                    {
                        s = seltables[i];
                        selecttabssb.AppendFormat(seltables[i]);
                        selecttabssb.Append(' ');
                        selecttabssb.Append(s[0]);
                    }
                }
                string t1 = "";
                string t2 = "";
                StringBuilder joincond = new StringBuilder();

                if (tabcnt == 1)
                {
                    //sb.AppendLine("SELECT " + selpkcols.ToString() + selnpkcols.ToString());
                    sb.AppendLine("SELECT " + selpkcols.ToString());

                    sb.AppendLine(" FROM " + selecttabssb);
                }
                else
                {
                    for (int i = 0; i < tabcnt; i++)
                    {
                        if (i + 1 < tabcnt)
                        {
                            if (i + 1 == 1)
                            {
                                t1 = seltables[i];
                                t2 = seltables[i + 1];
                                joincond.Append(t1[0]);
                                joincond.AppendFormat(".P_PID = ");
                                joincond.Append(t2[0]);
                                joincond.AppendFormat(".P_PID");
                            }
                            else
                            {
                                joincond.AppendFormat(" AND ");
                                t1 = seltables[i];
                                t2 = seltables[i + 1];
                                joincond.Append(t1[0]);
                                joincond.AppendFormat(".P_PID = ");
                                joincond.Append(t2[0]);
                                joincond.AppendFormat(".P_PID");
                            }

                        }
                    }
                    //sb.AppendLine("SELECT " + selpkcols.ToString() + selnpkcols.ToString());
                    sb.AppendLine("SELECT " + selpkcols.ToString());
                    sb.AppendLine(" FROM " + selecttabssb);

                    sb.AppendLine(" WHERE (" + joincond.ToString() + ")");
                }

                cmd.CommandText = sb.ToString();

                // OracleDataReader reader = cmd.ExecuteReader();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        result1.Add(reader.GetName(i).ToString());
                    }
                    //result1.Add(reader["table_name"].ToString());
                }

                Close();
            }

            catch (Exception)
            {
                throw;
            }

            return result;
        }*/


        /// <summary>
        /// Create Fact Table 
        /// </summary>
        /// <param name="Table"></param>
        /// <returns>Int Update Count</returns>
        public static int CreateFactTable(FactTableInfo Table)
        {
            int result = 0;

            string pk = string.Empty;

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
            catch (Exception ex)
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

                        if (column.Count > 0)
                        {
                            d.PrimaryKeys = new Dictionary<string, string>();

                            foreach (var key in column)
                            {
                                if (!d.PrimaryKeys.ContainsKey(key.Name))
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
                                if (!d.Columns.ContainsKey(key.Name))
                                {
                                    if (key.DataType.ToLower() == "date")
                                        d.Columns.Add(key.Name, key.DataType);
                                    else
                                        d.Columns.Add(key.Name, key.DataType + "(" + key.DataLength + ")");
                                }
                            }
                        }

                        result += ManualCreateDimenstionalTable(table);
                    }

                    if (result != -99)
                    {
                        result = CreateFactTable(fact);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public static int LoadDataWarehouseDimensions(List<string> tables)
        {
            int result = 0;

            int index = 0;

            try
            {
                Connect();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                StringBuilder sb = new StringBuilder();

                if (tables.Count > 0)
                {
                    FactTableInfo fact = new FactTableInfo();

                    fact.PrimaryKeys = new Dictionary<string, string>();

                    fact.Columns = new Dictionary<string, string>();

                    fact.TableName = "Auto_Generated_Fact_Table";

                    foreach (var table in tables)
                    {
                        DimensionalTableInfo d = new DimensionalTableInfo();

                        d.TableName = table + "_Dimensional";

                        List<ColumnInfo> column = GetAllColumnsObject(table);

                        if (column.Count > 0)
                        {
                            sb.AppendLine("INSERT INTO " + d.TableName);

                            sb.AppendLine("(");

                            foreach (var item in column)
                            {
                                sb.AppendLine(item.Name + ", ");
                            }

                            index = sb.ToString().LastIndexOf(',');

                            if (index >= 0)
                                sb.Remove(index, 1);

                            sb.AppendLine(")");

                            sb.AppendLine("(");

                            sb.AppendLine("SELECT ");

                            foreach (var item in column)
                            {
                                sb.AppendLine(item.Name + ", ");
                            }

                            index = sb.ToString().LastIndexOf(',');

                            if (index >= 0)
                                sb.Remove(index, 1);

                            sb.AppendLine("FROM " + table);
                        }

                        sb.AppendLine(")");
                    }

                    cmd.CommandText = sb.ToString();

                    result += cmd.ExecuteNonQuery();

                    Close();

                    Connect();

                    cmd = new OracleCommand();

                    cmd.Connection = con;

                    sb = new StringBuilder();

                    int ID = new Random().Next(1, 1000000000);

                    foreach (var table in tables)
                    {
                        DimensionalTableInfo d = new DimensionalTableInfo();

                        sb.AppendLine("INSERT INTO " + fact.TableName);

                        List<ColumnInfo> column = GetAllColumnsObject("AUTO_GENERATED_FACT_TABLE");

                        if (column.Count > 0)
                        {
                            sb.AppendLine("(");

                            foreach (var item in column)
                            {
                                sb.AppendLine(item.Name + ", ");
                            }

                            index = sb.ToString().LastIndexOf(',');

                            if (index >= 0)
                                sb.Remove(index, 1);

                            sb.AppendLine(")");
                        }

                        sb.AppendLine("(");

                        column = GetPrimaryKeyObject(table);

                        if (column.Count > 0)
                        {
                            foreach (var key in column)
                            {
                                int i = column.IndexOf(key);

                                sb.AppendLine("SELECT ");

                                sb.AppendLine("ROWNUM AS ID, ");

                                sb.AppendLine(key.Name);

                                sb.AppendLine("FROM ");

                                sb.AppendLine(table);

                                sb.AppendLine("UNION");
                            }
                        }

                        index = sb.ToString().LastIndexOf("UNION");

                        if (index >= 0)
                            sb.Remove(index, 5);

                        sb.AppendLine(")");
                    }

                    cmd.CommandText = sb.ToString();

                    result += cmd.ExecuteNonQuery();

                    Close();
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

        public static int LoadDataWarhouse(List<string> tables)
        {
            int result = 0;

            try
            {
                Connect();

                OracleCommand cmd = new OracleCommand();

                cmd.Connection = con;

                StringBuilder sb = new StringBuilder();

                if (tables.Count > 0)
                {
                    FactTableInfo fact = new FactTableInfo();

                    fact.PrimaryKeys = new Dictionary<string, string>();

                    fact.Columns = new Dictionary<string, string>();

                    fact.Relations = new Dictionary<string, string>();

                    fact.TableName = "Auto_Generated_Fact_Table";

                    foreach (var table in tables)
                    {
                        DimensionalTableInfo d = new DimensionalTableInfo();

                        d.TableName = table + "_Dimensional";

                        List<ColumnInfo> column = GetAllColumnsObject(table);

                        if (column.Count > 0)
                        {
                            sb.AppendLine("INSERT INTO " + d.TableName);

                            sb.AppendLine("(");

                            sb.AppendLine("SELECT ");

                            foreach (var item in column)
                            {
                                sb.AppendLine(item.Name + ", ");
                            }

                            var index = sb.ToString().LastIndexOf(',');

                            if (index >= 0)
                                sb.Remove(index, 1);

                            sb.AppendLine("FROM " + table);
                        }

                        sb.AppendLine(")");
                    }

                    cmd.CommandText = sb.ToString();

                    result = cmd.ExecuteNonQuery();

                    Close();
                }
            }
            catch (Exception ex)
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

