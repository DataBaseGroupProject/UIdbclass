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

        public static void CreateOracleConnection()
        {
        }


      //  DataSet sds;
        Form1 frm;
       // OracleConnection con;
        public Form2(Form1 fr)
        {
            InitializeComponent();

            frm = fr;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        //This method will connect to the esmart2 database and will display the tables from esmart2 in a listbox
        private void button1_Click(object sender, EventArgs e)
        {


            try
            {
                DataAccess.Connect2();

                List<string> res = DataAccess.GetFactTable();

                foreach (var itm in res)
                {
                    listBox1.Items.Add(itm);
                }



                List<string> dim = DataAccess.GetDimTable();
                foreach(var element in dim)
                {
                    listBox2.Items.Add(element);
                }

                MessageBox.Show("You are connected!");
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid User Name/Password");

            }

            //  try
            //        {
            //           string oradb = "Data Source=//taurus.ccec.unf.edu:1521/gporcl;User Id=esmart2;Password=esmart2A3;";

            //            con = new OracleConnection(oradb);  // C#

            //           con.Open();





            //            OracleCommand cmd = new OracleCommand("select * from patient", con);

            //            cmd.CommandType = CommandType.Text;
            //           DataSet ds = new DataSet();
            //            OracleDataAdapter da = new OracleDataAdapter();
            //            da.SelectCommand = cmd;
            //            da.Fill(ds);
            //           dataGridView1.DataSource = ds.Tables[0];

            //       }
            //       catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.Message);
            //       }
            //   }
            //}









            //try
            //{
            //    //DataAccess.Connect(textBox1.Text, textBox2.Text, textBox3.Text);
            //   // DataAccess.Connect();

            //    //List<string> results = DataAccess.GetTableName();

            //    //foreach (var item in results)
            //    {
            //         listBox1.Items.Add(item);
            //    }

            //    MessageBox.Show("You are connected!");
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Invalid User Name/Password");

            //}




        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
    



