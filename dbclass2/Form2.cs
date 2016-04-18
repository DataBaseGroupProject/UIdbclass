using dbclass2.Objects;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
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

        Form1 frm;

        public Form2(Form1 fr)
        {
            InitializeComponent();

            frm = fr;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //This method will connect to the esmart2 database and will display the tables from esmart2 in a listbox as a facttable listbox and a dimension table listbox
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                DataAccess.Connect("Destination");
                List<string> res = DataAccess.GetFactTable();

                foreach (var itm in res)
                {
                    listBox1.Items.Add(itm);
                }



                List<string> dim = DataAccess.GetDimTable();
                foreach (var element in dim)
                {
                    listBox2.Items.Add(element);
                }

                MessageBox.Show("You are connected!");
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid User Name/Password");

            }

        }


        public void DisplayListBox()
        {
            try
            {

                DataAccess.Connect2();
                List<string> res = DataAccess.GetFactTable();

                foreach (var item in res)
                {
                    listBox1.Items.Add(item);
                }

                List<string> dim = DataAccess.GetDimTable();
                foreach (var item in dim)
                {
                    listBox2.Items.Add(item);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Invalid User Name/Password");

            }


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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}




