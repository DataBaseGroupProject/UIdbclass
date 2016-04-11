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
            try
            {
                DimensionalTableInfo tb = new DimensionalTableInfo();

                tb.TableName = textBox4.Text;

                tb.PrimaryKeys = new Dictionary<string, string>();

                tb.PrimaryKeys.Add(listBox1.Items[0].ToString(), "varchar(50)");

                tb.Columns = new Dictionary<string, string>();

                tb.Columns.Add(listBox2.Items[0].ToString(), "varchar(50)");

                DataAccess.CreateDimenstionalTable(tb);

                MessageBox.Show("Tables " + tb.TableName + "have been created");
            }
            catch (Exception)
            {

                MessageBox.Show("Table creation error");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //DataAccess.Connect(textBox1.Text, textBox2.Text, textBox3.Text);
                DataAccess.Connect();

                List<string> results = DataAccess.GetTableName();

                foreach (var item in results)
                {
                    checkedListBox1.Items.Add(item);
                }

                MessageBox.Show("You are connected!");
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid User Name/Password");
                ClearTableNamesList();
            }
        }

        private void ClearTableNamesList()
        {
            if (checkedListBox1.Items.Count > 0)
            {
                for (int i = checkedListBox1.Items.Count - 1; i >= 0; i--)
                {
                    checkedListBox1.Items.RemoveAt(i);
                }
            }
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
            try
            {
                List<string> results = new List<string>();
                List<string> results2 = new List<string>();

                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        string tab = (string)checkedListBox1.Items[i];
                        results = DataAccess.GetPrimaryKey(tab);
                        results2 = DataAccess.GetNonKey(tab);
                    }
                }
  
                foreach (string cols in results)
                {
                    if (!checkedListBox2.Items.Contains(cols))
                    {
                        checkedListBox2.Items.Add(cols);
                    }
                }

                foreach (string cols in results2)
                {
                    if (!checkedListBox3.Items.Contains(cols))
                    {
                        checkedListBox3.Items.Add(cols);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


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
            columnlists.Clear();
            foreach (string cols in columnlists)
            {
                checkedListBox2.Items.Add(cols);
            }
        }*/

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataAccess.Connect();
            List<string> results = new List<string>();
            List<string> columnlists = new List<string>();

            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                if (checkedListBox2.GetItemChecked(i))
                {
                    string tab = (string)checkedListBox2.Items[i];
                    columnlists.Add(tab);
                    //results = DataAccess.GetColumns(tab);
                }
            }
            //List<string> columnlists = new List<string>();
            //foreach (string item in results)
            //{
            //    if (!columnlists.Contains(item))
            //        columnlists.Add(item);
            //}

            foreach (string cols in columnlists)
            {
                listBox1.Items.Add(cols);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

      

        private void checkedListBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            DataAccess.Connect();
            List<string> results = new List<string>();
            List<string> columnlists = new List<string>();

            for (int i = 0; i < checkedListBox3.Items.Count; i++)
            {
                if (checkedListBox3.GetItemChecked(i))
                {
                    string tab = (string)checkedListBox3.Items[i];
                    columnlists.Add(tab);
                    //results = DataAccess.GetColumns(tab);
                }
            }
            //List<string> columnlists = new List<string>();
            //foreach (string item in results)
            //{
            //    if (!columnlists.Contains(item))
            //        columnlists.Add(item);
            //}

            foreach (string cols in columnlists)
            {
                listBox2.Items.Add(cols);
            }

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }

