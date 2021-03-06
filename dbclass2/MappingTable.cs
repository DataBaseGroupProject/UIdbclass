﻿using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dbclass2
{
    public partial class MappingTable : Form
    {

        OracleConnection con;
        public MappingTable()
        {
            InitializeComponent();
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            MessageBox.Show(textBox1.Text + "Table Created");
        }

        private void AddButton_Click(object sender, EventArgs e)
        {

            string oradb = "Data Source=//taurus.ccec.unf.edu:1521/gporcl;User Id=esmart2;Password=esmart2A3;";

                      con = new OracleConnection(oradb);  // C#

                     con.Open();





                       OracleCommand cmd = new OracleCommand("select * from patient", con);

                       cmd.CommandType = CommandType.Text;
                       DataSet ds = new DataSet();
                        OracleDataAdapter da = new OracleDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                       dataGridView1.DataSource = ds.Tables[0];





           
            textBox1.Clear();
        }

        public void LoadGridData(DimensionalTableInfo info)
        {
            dataGridView1.DataSource = info;
        }

        //fetch content from previous form..ask karthika
        public DataTable GetResultsTable()
        {
            // Create the output table.
            DataTable d = new DataTable();

            // Loop through all process names.
            //for (int i = 0; i < this._dataArray.Count; i++)
            //{
            //    // The current process name.
            //    string name = this._names[i];

            //    // Add the program name to our columns.
            //    d.Columns.Add(name);

            //    // Add all of the memory numbers to an object list.
            //    List<object> objectNumbers = new List<object>();

            //    // Put every column's numbers in this List.
            //    foreach (double number in this._dataArray[i])
            //    {
            //        objectNumbers.Add((object)number);
            //    }

            //    // Keep adding rows until we have enough.
            //    while (d.Rows.Count < objectNumbers.Count)
            //    {
            //        d.Rows.Add();
            //    }

            //    // Add each item to the cells in the column.
            //    for (int a = 0; a < objectNumbers.Count; a++)
            //    {
            //        d.Rows[a][i] = objectNumbers[a];
            //    }
            //}
            return d;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {

            dataGridView1.DataSource = null;
            textBox1.Clear();

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string StrQuery;
            try
            {
                //using (SqlConnection conn = new SqlConnection(ConnString))
                //{
                //    using (SqlCommand comm = new SqlCommand())
                //    {
                //        comm.Connection = conn;
                //        conn.Open();
                //        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                //        {
                //            StrQuery = @"INSERT INTO tableName VALUES ("
                //                + dataGridView1.Rows[i].Cells["ColumnName"].Value + ", "
                //                + dataGridView1.Rows[i].Cells["ColumnName"].Value + ");";
                //            comm.CommandText = StrQuery;
                //            comm.ExecuteNonQuery();
                //        }
                //    }
                //}
            }
            catch
            {
                throw;
            }
        }


        //The code creates a DataTable and populates it with data. It is an entire Form and can be dropped into a Windows Forms application with a DataGridView in the designer.


    }
}
