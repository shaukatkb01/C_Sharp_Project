using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // Connection String
        string conString =
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDatabase;Integrated Security=True;";

        public Form1()
        {
            InitializeComponent();
        }

        // Form Load
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        // Load Data Function
        void LoadData()
        {
            SqlConnection con = new SqlConnection(conString);

            SqlDataAdapter da =
                new SqlDataAdapter("SELECT * FROM Students", con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView1.DataSource = dt;
        }

        // Save Button
        private void btnSave_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(conString);

            string query =
                "INSERT INTO Students (Name, Age) VALUES (@name, @age)";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@name", txtName.Text);
            cmd.Parameters.AddWithValue("@age", txtAge.Text);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Data Successfully Saved");

            LoadData();
        }
        private void txtName_TextChanged(Object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                string = "SELECT COUNT(*) FROM Students WHERE Name = @name";
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    lblUserExist.Text = "Name already exitst";
                    lblUserExist.ForeColor = Color.Red;
                }
                else
                {
                    lblUserExist.Text = "Available username";
                    lblUserExist.ForeColor = Color.Green;
                }
            }
        }
            
            }
    }

