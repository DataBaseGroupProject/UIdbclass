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
        public static List<DimensionalTableInfo> FinalDimensionalTables;
        public static List<FactTableInfo> FinalFactTables;
        Form2 frm;

        public Form1()
        {
            InitializeComponent();
            frm = new Form2(this);

            FinalDimensionalTables = new List<DimensionalTableInfo>();
            FinalFactTables = new List<FactTableInfo>();

            // StartPosition was set to FormStartPosition.Manual in the properties window.
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);

            checkedListBox1.CheckOnClick = true;
            checkedListBox1.CheckOnClick = true;

            InitializeMyControl();     
        }

        private void InitializeMyControl()
        {
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DimensionalTableInfo tb = new DimensionalTableInfo();

                tb.TableName = textBox4.Text;

                tb.PrimaryKeys = new Dictionary<string, string>();

                foreach (var item in listBox1.Items)
                {
                    string[] keyInfo = item.ToString().Split(new string[] { "<->" }, StringSplitOptions.None);

                    if (keyInfo.Count() > 1)
                        tb.PrimaryKeys.Add(keyInfo[0], keyInfo[1]);
                }

                tb.Columns = new Dictionary<string, string>();

                foreach (var item in listBox2.Items)
                {
                    string[] columnInfo = item.ToString().Split(new string[] { "<->" }, StringSplitOptions.None);

                    if (columnInfo.Count() > 1)
                        tb.Columns.Add(columnInfo[0], columnInfo[1]);
                }

                FinalDimensionalTables.Add(tb);

                DataAccess.CreateDimenstionalTable(tb);

                MessageBox.Show("Tables " + tb.TableName + " have been created Successfully.");

                ClearExistingTableInfoList();
                ClearNewTableInfoList();
                textBox4.Text = string.Empty;
            }
            catch (Exception)
            {

                MessageBox.Show("Failed to Create Table");
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

        private void ClearExistingTableInfoList()
        {
            if (checkedListBox2.Items.Count > 0)
            {
                for (int i = checkedListBox2.Items.Count - 1; i >= 0; i--)
                {
                    checkedListBox2.Items.RemoveAt(i);
                }
            }

            if (checkedListBox3.Items.Count > 0)
            {
                for (int i = checkedListBox3.Items.Count - 1; i >= 0; i--)
                {
                    checkedListBox3.Items.RemoveAt(i);
                }
            }
        }

        private void ClearNewTableInfoList()
        {
            if (listBox1.Items.Count > 0)
            {
                for (int i = listBox1.Items.Count - 1; i >= 0; i--)
                {
                    listBox1.Items.RemoveAt(i);
                }
            }

            if (listBox2.Items.Count > 0)
            {
                for (int i = listBox2.Items.Count - 1; i >= 0; i--)
                {
                    listBox2.Items.RemoveAt(i);
                }
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                if(FinalDimensionalTables.Count > 1)
                {
                    FactTableInfo fact = new FactTableInfo();

                    fact.TableName = "Fact Table";

                    fact.PrimaryKeys = new Dictionary<string, string>();

                    fact.PrimaryKeys.Add("ID", "Int");

                    foreach (var table in FinalDimensionalTables)
                    {
                        foreach (var item in table.PrimaryKeys)
                        {
                            fact.Columns = new Dictionary<string, string>();
                            fact.Columns.Add(item.Key, item.Value);

                            fact.Relations = new Dictionary<string, string>();
                            fact.Relations.Add(item.Key, item.Value);
                        }
                    }

                    DataAccess.CreateFactTable(fact);

                    MessageBox.Show("Table labeling has been changed");

                    this.Hide();
                    frm.Show();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to submit changes. Please try again later.");
            }
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
                }
            }
           
            foreach (string cols in columnlists)
            {
                if (!listBox1.Items.Contains(cols))
                {
                    listBox1.Items.Add(cols);
                }
            }
        }
        
        private void checkedListBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
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
                    }
                }

                foreach (string cols in columnlists)
                {
                    if (!listBox2.Items.Contains(cols))
                    {
                        listBox2.Items.Add(cols);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

