///The login form first form to show starts Sales form when correct login is typed

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
            List<string> prodTestList = new List<string>();
            prodTestList.Add("Test");
            prodTestList.Add("Production");

            comboBoxProdOrTest.DataSource = prodTestList;
            comboBoxProdOrTest.SelectedIndex = 0;
            this.Text = "Forecast Login  " + Application.ProductVersion;
            
        }

        Dictionary<string, string> Users;
        private void buttonLogIN_Click(object sender, EventArgs e)
        {
            //Here is the four users and their login details
            Users = new Dictionary<string,string>();
            Users.Add("admin","admin222");
            Users.Add("kige", "kige111");
            Users.Add("sale", "sale321");
            Users.Add("suppl", "suppl123");
            string username;
            string password;
            //textBoxUserName.Text

            username = textBoxUserName.Text.ToLower();
            password = textBox1.Text.ToLower();

            ClassStaticVaribles.WritePermission writePermissons;
            if (username == "suppl")
            {
               
                writePermissons = ClassStaticVaribles.WritePermission.SupplWrite;
            }
            else if (username == "sale")
            {
                writePermissons = ClassStaticVaribles.WritePermission.SaleWrite;
            }
            else if (username == "admin")
            {
                writePermissons = ClassStaticVaribles.WritePermission.Write;
            }
            else 
            {
                writePermissons = ClassStaticVaribles.WritePermission.Read;
            }



            if (Users.ContainsKey(username) && Users[username].Equals(password))
            {
                labelMessageText.ForeColor = Color.Green;
                SetStatus("Welcome!! Starting up..");
                bool tempProd = true;
                if (comboBoxProdOrTest.SelectedIndex == 0)
                {
                    tempProd = false;
                }
                ClassStaticVaribles.SetProdOrTest(tempProd);

                Form1 salesForm = new Form1();

                salesForm.SetOnlyLook(writePermissons);
                salesForm.SetProdOrTestHeading(tempProd);


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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxProdOrTest_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void labelM3Version_Click(object sender, EventArgs e)
        {

        }
    }
}
