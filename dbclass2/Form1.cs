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
        }


       



        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tables have been transferred");

            DimensionalTableInfo dt = new DimensionalTableInfo();

            DataAccess.CreateDimenstionalTable(dt);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataAccess.Connect();

            List<string> results = DataAccess.GetTableName();

            foreach(var item in results)
            {
                checkedListBox1.Items.Add(item);
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
