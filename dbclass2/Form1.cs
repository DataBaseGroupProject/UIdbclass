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
    using Objects;

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
            checkedListBox1.HorizontalScrollbar = true;

            checkedListBox2.CheckOnClick = true;
            checkedListBox2.HorizontalScrollbar = true;

            checkedListBox3.CheckOnClick = true;
            checkedListBox3.HorizontalScrollbar = true;            

            InitializeMyControl();
        }

        private void InitializeMyControl()
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                AccessInfo connect = new AccessInfo();

                connect.SourceUrl = textBox1.Text.Trim();
                connect.SourceUserName = textBox2.Text.Trim();
                connect.SourcePassword = textBox3.Text.Trim();

                connect.TargetUrl = textBox7.Text.Trim();
                connect.TargetUserName = textBox6.Text.Trim();
                connect.TargetPassword = textBox5.Text.Trim();

                DataAccess.LoginConnect();

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

        public void LoadForm()
        {
            this.Hide();
            frm.Show();

           frm.DisplayListBox();
        }

        List<string> list1 = new List<string>();
        List<string> list2 = new List<string>();
         private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DimensionalTableInfo tb = new DimensionalTableInfo();

                tb.TableName = textBox4.Text;

                tb.PrimaryKeys = new Dictionary<string, string>();                

                if (listBox1.Items.Count > 0)
                {
                    foreach (var item in listBox1.Items)
                    {
                       string s = item.ToString();
                        list1.Add(s);
                        //string[] keyInfo = item.ToString().Split(new string[] { "<-->" }, StringSplitOptions.None);
                        string[] keyInfo = s.Split(new string[] { "<-->" }, StringSplitOptions.None);
                        if (keyInfo.Count() > 1)
                            tb.PrimaryKeys.Add(keyInfo[0], keyInfo[1]);
                    }
                }
                else
                {
                    MessageBox.Show("Please Select at least One Primary Key.");
                }


                tb.Columns = new Dictionary<string, string>();

                if (listBox2.Items.Count > 0)
                {
                    foreach (var item in listBox2.Items)
                    {
                        string s = item.ToString();
                        list2.Add(s);
                        string[] columnInfo = s.Split(new string[] { "<-->" }, StringSplitOptions.None);
                        //string[] columnInfo = item.ToString().Split(new string[] { "<-->" }, StringSplitOptions.None);

                        if (columnInfo.Count() > 1)
                            tb.Columns.Add(columnInfo[0], columnInfo[1]);
                    }
                }
                else
                {
                    MessageBox.Show("Please Select at least One Column.");
                }

                FinalDimensionalTables.Add(tb);

               // DataAccess.InsertDimensionalData(tb.TableName, list1, list2);

                DataAccess.CreateDimenstionalTable(tb);

                MessageBox.Show("Table " + tb.TableName + " Created Successfully.");                

                /*for(int i = 0; i < listBox1.Items.Count; i++)
                {
                    list1.Add(li)
                }*/

                /*foreach (var item in listBox1.Items)
                {
                    list1.Add(item.ToString());
                }
                foreach (var item in listBox2.Items)
                {
                    list2.Add(item.ToString());
                }*/
               // DataAccess.InsertDimensionalData(tb.TableName,list1,list2);

                ClearExistingTableInfoList();
                ClearNewTableInfoList();
                textBox4.Text = string.Empty;
            }
            catch (Exception ex)
            {
                //Console.Out(ex.StackTrace);
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
            LoadForm();
            try
            {
                if (FinalDimensionalTables.Count > 1)
                {
                    FactTableInfo fact = new FactTableInfo();

                    fact.TableName = "Fact Table";

                    fact.PrimaryKeys = new Dictionary<string, string>();
                    fact.Columns = new Dictionary<string, string>();
                    fact.Relations = new Dictionary<string, string>();

                    fact.PrimaryKeys.Add("ID", "Int");

                    foreach (var table in FinalDimensionalTables)
                    {
                        foreach (var item in table.PrimaryKeys)
                        {
                            fact.Columns.Add(item.Key, item.Value);
                            fact.Relations.Add(table.TableName, item.Key);
                        }
                    }

                    int iRet = DataAccess.CreateFactTable(fact);

                    if (iRet == -1)
                        MessageBox.Show("Data Warehouse Created Successfully.");
                }
                else
                {
                    if (checkedListBox1.SelectedItems.Count > 0)
                    {
                        List<string> selectedTables = new List<string>();

                        foreach (var item in checkedListBox1.CheckedItems)
                        {
                            if (!selectedTables.Contains(item.ToString()))
                            {
                                selectedTables.Add(item.ToString());
                            }
                        }

                        int iRet = DataAccess.BuildDataWarhouse(selectedTables);

                        if (iRet == -1)
                        {
                            MessageBox.Show("Data Warehouse Created Successfully.");

                            iRet = DataAccess.LoadDataWarehouseDimensions(selectedTables);

                            //iRet += DataAccess.LoadDataWarehouseDimensions(selectedTables);

                            if (iRet == -1)
                            {
                                MessageBox.Show("Data Warehouse Data Loaded Successfully.");
                            }

                           // LoadForm();
                        }
                        else
                        {
                            MessageBox.Show("Please Select At least One Tables to Build the Data Warehouse.");
                        }
                    }
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
                    if (checkedListBox1.GetSelected(i) || checkedListBox1.GetItemChecked(i))
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

                MessageBox.Show("An error was encountered. Please try again later.");
            }
        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < checkedListBox2.Items.Count; i++)
                {
                    if (checkedListBox2.GetSelected(i) || checkedListBox2.GetItemChecked(i))
                    {
                        if (!listBox1.Items.Contains(checkedListBox2.Items[i]))
                        {
                            listBox1.Items.Add(checkedListBox2.Items[i]).ToString();
                            /*string s = "";
                            s = listBox1.Items.Add(checkedListBox2.Items[i]).ToString();
                            list1.Add(s);*/
                        }
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("An error was encountered. Please try again later.");
            }
        }
        
        private void checkedListBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < checkedListBox3.Items.Count; i++)
                {
                    if (checkedListBox3.GetSelected(i) || checkedListBox3.GetItemChecked(i))
                    {
                        if (!listBox2.Items.Contains(checkedListBox3.Items[i]))
                        {
                            listBox2.Items.Add(checkedListBox3.Items[i]).ToString();
                            /*string s = "";
                            s = listBox2.Items.Add(checkedListBox3.Items[i]).ToString();
                            list2.Add(s);*/
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error was encountered. Please try again later.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearExistingTableInfoList();
                ClearNewTableInfoList();
                ClearTableNamesList();
            }
            catch (Exception)
            {

                MessageBox.Show("An error was encountered. Please try again later.");
            }
        }        
    }
}

