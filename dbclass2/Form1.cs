using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dbclass2

{
    using System;
    using System.Drawing;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Data;
    using System.IO;
    using Oracle.DataAccess.Client;

    public partial class Form1 : Form
    {
        public static DataGridView dataGridView1 = new DataGridView();

        public static OracleConnection con;
        public Form1()
        {




            InitializeComponent();

            
            

        }







        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tables have been transferred");

        }

        private void button1_Click(object sender, EventArgs e)
        {
         
            
                DataAccess.Connect();

                List<string> results = DataAccess.GetTableName();

                foreach (var item in results)
                {
                    checkedListBox1.Items.Add(item);
                }
            
            
        }
        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tables have been transferred");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Table labeling has been changed");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

           
                DataAccess.Connect();

                List<string> results = DataAccess.GetTableName();

                foreach (var item in results)
                {
                    checkedListBox2.Items.Add(item);
                }
            

        }
    }
}