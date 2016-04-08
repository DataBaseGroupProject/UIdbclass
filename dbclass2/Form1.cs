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
    using System;
    using System.Drawing;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Data;
    using System.IO;


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Sets up the initial objects in the CheckedListBox.
            /*string[] myTables = { "Doctors", "Medications", "Patients", "Hospitals", "Nurses" };
            checkedListBox1.Items.AddRange(myTables);
            string[] myTables2 = { "DoctorID", "First Name", "Last Name", "OfficeAddress" };
            checkedListBox2.Items.AddRange(myTables2);*/
            // Changes the selection mode from double-click to single click.
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.CheckOnClick = true;

            InitializeMyControl();
           
        }



        private void InitializeMyControl()
        {
            // Set to no text.
            textBox4.Text = "";
            // The password character is an asterisk.
            textBox4.PasswordChar = '*';
            // The control will allow no more than 14 characters.
            textBox4.MaxLength = 14;
            //comboBox1.Sorted = true;
            //comboBox. = SelectionMode.MultiExtended;
            checkedListBox1.Sorted = true;
            /*try
            {
                checkedListBox1.SelectionMode = SelectionMode.MultiSimple;
            }
            catch(ArgumentException ex)
            {
                throw ex;
            }*/
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

            foreach(var item in results)
            {
                checkedListBox1.Items.Add(item);
                comboBox1.Items.Add(item);
            }

            MessageBox.Show("You are connected!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Fields have been transferred");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Table labeling has been changed");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataAccess.Connect();
            //string seltab = comboBox1.SelectedItem.ToString();
            /*List<string> seltabs = new List<string>();
            List<string> results = DataAccess.GetColumns(seltabs);

            foreach (var item in results)
            {
                checkedListBox2.Items.Add(item);
                //comboBox1.Items.Add(item);
            }*/
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Hide();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataAccess.Connect();
            List<string> results = new List<string>();
            List<int> tabindex = new List<int>();

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    //string tab = (string)checkedListBox1.Items[i];
                    int tindex = checkedListBox1.Items.IndexOf(i);
                    tabindex.Add(tindex);
                    string tab = (string)checkedListBox1.Items[tindex];
                    results = DataAccess.GetColumns(tab);
                }
                /*if(checkedListBox1.GetItemChecked(i))
                {
                    string tab = (string)checkedListBox1.Items[i];
                    results = DataAccess.GetColumns(tab);
                }*/
            }
            List<string> columnlists = new List<string>();
            foreach (string item in results)
            {
                if (!columnlists.Contains(item))
                    columnlists.Add(item);
            }
            //columnlists.Clear();
            foreach(string cols in columnlists)
            {
                checkedListBox2.Items.Add(cols);
            }
        }
        //backup
        /*private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataAccess.Connect();
            List<string> results = new List<string>();

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    string tab = (string)checkedListBox1.Items[i];
                    results = DataAccess.GetColumns(tab);
                }
            }
            List<string> columnlists = new List<string>();
            foreach (string item in results)
            {
                if (!columnlists.Contains(item))
                    columnlists.Add(item);
            }
            //columnlists.Clear();
            foreach (string cols in columnlists)
            {
                checkedListBox2.Items.Add(cols);
            }
        }*/

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
