using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StokTrackingVer1._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string Username;
            string Password;

            Username = txtUsername.Text;
            Password = txtPassword.Text;

            if ((Username=="serdar") && (Password=="1234"))
            {
                Form1 form1 = new Form1();
                form1.Close();

                Form3 form3 = new Form3();
                form3.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Username or Password is Incorrect");        
            }
            
        }

        private void btnChefLogin_Click(object sender, EventArgs e)
        {
            string Username;
            string Password;

            Username = txtChefUser.Text;
            Password = txtChefPassword.Text;

            if ((Username == "merve") && (Password == "1234"))
            {
                Form1 form1 = new Form1();
                form1.Close();

                Form3 form3 = new Form3();
                form3.Show();
                form3.btnDeleteStock.Visible = false;
                this.Hide();
            }
            else
            {
                MessageBox.Show("Username or Password is Incorrect");
            }
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            string Username;
            string Password;

            Username = txtStaffUser.Text;
            Password = txtStaffPassword.Text;

            if ((Username == "eda") && (Password == "1234"))
            {
                Form1 form1 = new Form1();
                form1.Close();

                Form3 form3 = new Form3();
                form3.Show();
                form3.btnAddStock.Visible = false;
                form3.btnDeleteStock.Visible = false;
                form3.btnUpdateStock.Visible = false;
                this.Hide();
                
            }
            else
            {
                MessageBox.Show("Username or Password is Incorrect");
            }

            
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}


