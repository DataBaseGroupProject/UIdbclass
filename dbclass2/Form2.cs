using Oracle.DataAccess.Client;
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
    public partial class Form2 : Form
    {

       // DataSet sds;
        Form1 frm;
       

        public Form2(Form1 fr)
        {
            InitializeComponent();

            frm = fr;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {


                OracleConnection conn = new OracleConnection();

                 string constr = "Data Source=//taurus.ccec.unf.edu:1521/gporcl;User Id=esmart2;Password=esmart2A3;";
               // string constr = "Data Source =//localhost:1521/xe;User Id=system;Password=xoxoxo83;";
                conn.ConnectionString = constr;
                // OracleCommand cmd = new OracleCommand("SELECT tablespace_name, table_name From dba_tables", conn);
                OracleCommand cmd = new OracleCommand("SELECT * FROM dba_tables", conn);
                
                    conn.Open();

                cmd.CommandType = CommandType.Text;
                DataSet ds = new DataSet();
                OracleDataAdapter da = new OracleDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    dataGridView1.DataSource = ds.Tables[0].DefaultView;
                }






                //DataAccess.Connect(textBox1.Text, textBox2.Text, textBox3.Text);
                //DataAccess.Connect2();

                // List<string> results = DataAccess.GetTableName();


                // foreach (var item in results)
                // {


                //listBox1.Items.Add(item);
                // sds = new DataSet();
                // dataGridView1.DataSource = sds.Tables["item"];
                // dataGridView1.ReadOnly = true;
                // }

                MessageBox.Show("You are connected!");
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid User Name/Password");
               
            }




        }
    }
}
