using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsForecastLactalis
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            SetStatus("");
            
        }

        Dictionary<string, string> Users;
        private void buttonLogIN_Click(object sender, EventArgs e)
        {
            Users = new Dictionary<string,string>();
            Users.Add("admin","admin");
            string username;
            string password;
            //textBoxUserName.Text

            username = textBoxUserName.Text.ToLower();
            password = textBox1.Text.ToLower();

            if (Users.ContainsKey(username) && Users[username].Equals(password))
            {
                labelMessageText.ForeColor = Color.Green;
                SetStatus("Welcome!! Starting up..");
                Form1 salesForm = new Form1();
                salesForm.FormClosed += new FormClosedEventHandler(salesForm_FormClosed);
                salesForm.Show();
                // Close login form
                
                this.Hide();

            }
            else
            {
                SetStatus("Login Failed");
            }


        }

        private void salesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.WriteLine("Close login form");
            this.Close();
        }

        private void SetStatus(string status)
        {
            labelMessageText.Text = status;
            Application.DoEvents();
        }

        private void FormLogin_Shown(object sender, EventArgs e)
        {

        }
    }
}
