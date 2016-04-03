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


    public partial class Form1 : Form
    {

        
        private BindingSource bindingSource1 = new BindingSource();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private Button reloadButton = new Button();
        private Button submitButton = new Button();



        public Form1()
        {
            InitializeComponent();

            // Sets up the initial objects in the CheckedListBox.
            string[] myTables = { "Doctors", "Medications", "Patients", "Hospitals", "Nurses" };
            checkedListBox1.Items.AddRange(myTables);
            string[] myTables2 = { "DoctorID", "First Name", "Last Name", "OfficeAddress" };
            checkedListBox2.Items.AddRange(myTables2);







            // Changes the selection mode from double-click to single click.
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.CheckOnClick = true;

            InitializeMyControl();


            dataGridView1.Dock = DockStyle.Fill;

            reloadButton.Text = "reload";
            submitButton.Text = "submit";
            reloadButton.Click += new System.EventHandler(reloadButton_Click);
            submitButton.Click += new System.EventHandler(submitButton_Click);

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Top;
            panel.AutoSize = true;
            panel.Controls.AddRange(new Control[] { reloadButton, submitButton });

            this.Controls.AddRange(new Control[] { dataGridView1, panel });
            this.Load += new System.EventHandler(Form1_Load);
            this.Text = "DataGridView databinding and updating demo";




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

        private void Form1_Load(object sender, System.EventArgs e)
        {
            // Bind the DataGridView to the BindingSource
            // and load the data from the database.
            dataGridView1.DataSource = bindingSource1;
            GetData("select * from Customers");
        }



        private void reloadButton_Click(object sender, System.EventArgs e)
        {
            // Reload the data from the database.
            GetData(dataAdapter.SelectCommand.CommandText);
        }

        private void submitButton_Click(object sender, System.EventArgs e)
        {
            // Update the database with the user's changes.
            dataAdapter.Update((DataTable)bindingSource1.DataSource);
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

            try
            {
                DataAccess.Connect();
                List<string> results = DataAccess.GetTableName();
                foreach (var item in results)
                {
                    checkedListBox1.Items.Add(item);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("You are not connected!");
                
            }

        }

        private void GetData(string selectCommand)
        {
            try
            {
                // Specify a connection string. Replace the given value with a 
                // valid connection string for a Northwind SQL Server sample
                // database accessible to your system.
                String connectionString =
                    "Integrated Security=SSPI;Persist Security Info=False;" +
                    "Initial Catalog=Northwind;Data Source=localhost";

                // Create a new data adapter based on the specified query.
               SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCommand, connectionString);

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                bindingSource1.DataSource = table;

                // Resize the DataGridView columns to fit the newly loaded content.
                dataGridView1.AutoResizeColumns(
                    DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            }
            catch (SqlException)
            {
                MessageBox.Show("To run this example, replace the value of the " +
                    "connectionString variable with a connection string that is " +
                    "valid for your system.");
            }
        }

    





        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Fields have been transferred");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Table labeling has been modified");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
