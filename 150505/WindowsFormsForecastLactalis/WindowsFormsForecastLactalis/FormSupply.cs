using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using WindowsFormsForecastLactalis;

namespace WindowsFormsForecastLactalis
{
    public partial class FormSupply : Form
    {
        Form1 Form1Instance = new Form1();
        private List<string> Supplier = new List<string>();
        GetFromM3 m3_info = new GetFromM3();

        public FormSupply()
        {
            InitializeComponent();
            Supplier.Add("SUPPLIER 1");
            Supplier.Add("SUPPLIER 2");

            comboBoxSupplier.DataSource = Supplier;
            FillInfo();
        }

        private void FillInfo()
        {
            this.dataGridForecastInfo.DataSource = null;

            this.dataGridForecastInfo.Rows.Clear();
            bool even = true;
            if (comboBoxSupplier.SelectedItem.ToString().Contains("1"))
            {
                even = false;
            }

            foreach (PrognosInfo item in Form1.Products)
            {
                bool thisEven = true;
                if (item.ProductNumber % 2 > 0)
                {
                    thisEven = false;
                }

                if (thisEven == even)
                {
                    dataGridForecastInfo.Rows.Add(item.ProductNumber, item.ProductName, "RealiseretKampagn_LastYear", item.RealiseretKampagn_LastYear[1], item.RealiseretKampagn_LastYear[2], item.RealiseretKampagn_LastYear[3], item.RealiseretKampagn_LastYear[4], item.RealiseretKampagn_LastYear[5], item.RealiseretKampagn_LastYear[6], 0);
                    dataGridForecastInfo.Rows.Add("", "", "RealiseretSalgsbudget_LastYear", item.RealiseretSalgsbudget_LastYear[1], item.RealiseretSalgsbudget_LastYear[2], item.RealiseretSalgsbudget_LastYear[3], item.RealiseretSalgsbudget_LastYear[4], item.RealiseretSalgsbudget_LastYear[5], item.RealiseretSalgsbudget_LastYear[6]);

                    dataGridForecastInfo.Rows.Add("", "", "Realiserat_ThisYear", item.Realiserat_ThisYear[1], item.Realiserat_ThisYear[2], item.Realiserat_ThisYear[3], item.Realiserat_ThisYear[4], item.Realiserat_ThisYear[5], item.Realiserat_ThisYear[6]);
                    dataGridForecastInfo.Rows.Add("", "", "Kampagn_ThisYear", item.Kampagn_ThisYear[1], item.Kampagn_ThisYear[2], item.Kampagn_ThisYear[3], item.Kampagn_ThisYear[4], item.Kampagn_ThisYear[5], item.Kampagn_ThisYear[6]);
                    dataGridForecastInfo.Rows.Add("", "", "Salgsbudget_ThisYear", item.Salgsbudget_ThisYear[1], item.Salgsbudget_ThisYear[2], item.Salgsbudget_ThisYear[3], item.Salgsbudget_ThisYear[4], item.Salgsbudget_ThisYear[5], item.Salgsbudget_ThisYear[6]);
                    dataGridForecastInfo.Rows.Add("", "", "SalgsbudgetReguleret_ThisYear", item.SalgsbudgetReguleret_ThisYear[1], item.SalgsbudgetReguleret_ThisYear[2], item.SalgsbudgetReguleret_ThisYear[3], item.SalgsbudgetReguleret_ThisYear[4], item.SalgsbudgetReguleret_ThisYear[5], item.SalgsbudgetReguleret_ThisYear[6]);
                    dataGridForecastInfo.Rows.Add("", "", "Köpsbudget_ThisYear", item.Kopsbudget_ThisYear[1], item.Kopsbudget_ThisYear[2], item.Kopsbudget_ThisYear[3], item.Kopsbudget_ThisYear[4], item.Kopsbudget_ThisYear[5], item.Kopsbudget_ThisYear[6]);
                    dataGridForecastInfo.Rows.Add("", "", "Köpsorder_ThisYear", item.Kopsorder_ThisYear[1], item.Kopsorder_ThisYear[2], item.Kopsorder_ThisYear[3], item.Kopsorder_ThisYear[4], item.Kopsorder_ThisYear[5], item.Kopsorder_ThisYear[6]);
                }
            }
            //Thread.Sleep(2000);
            foreach (DataGridViewRow row in dataGridForecastInfo.Rows)
            {
                row.Height = 35;
                if (Convert.ToString(row.Cells[2].Value) == "RealiseretKampagn_LastYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Blue;


                }
                else if (Convert.ToString(row.Cells[2].Value) == "RealiseretSalgsbudget_LastYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Blue;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Kampagn_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Salgsbudget_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);


                }
                else if (Convert.ToString(row.Cells[2].Value) == "SalgsbudgetReguleret_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);


                }
                else if (Convert.ToString(row.Cells[2].Value) == "Köpsbudget_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);


                }
                else if (Convert.ToString(row.Cells[2].Value) == "Köpsorder_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Green;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);


                }
            }
            int colNBR = 0;
            foreach (DataGridViewColumn col in dataGridForecastInfo.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                if (colNBR < 2)
                {
                    col.DefaultCellStyle.ForeColor = Color.Black;
                    col.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
                    col.DefaultCellStyle.ForeColor = Color.Black;

                }
                colNBR++;
            }
        }

        private void dataGridForecastInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            Console.WriteLine("Value clicked... Column index: " + columnIndex + "  rowIndex: " + rowIndex);
            if (rowIndex % 8 == 4)
            {
                string temp = "Sales Person1: 8\nSales Person2: 10\nSales Person3: 10 \n";
                MessageBox.Show(temp);
            }
        }

        internal void SetForm1Instanse(Form1 form1)
        {
            Form1Instance = form1;
        }

        private void buttonSalesView_Click(object sender, EventArgs e)
        {
            Console.WriteLine("User press Sales View");
            this.Visible = false;
            Form1Instance.BringToFront();
            Form1Instance.Location = this.Location;
            Form1Instance.Show();
        }

        private void buttonGetProductsBySupplier_Click(object sender, EventArgs e)
        {
            FillInfo();
        }

        private void buttonTestSupplItems_Click(object sender, EventArgs e)
        {
            List<string> tempList = m3_info.GetListOfProductsBySupplier("3141");

            foreach(string item in tempList)
            {
                string tempName = m3_info.GetNameByItemNumber(item);
                Console.WriteLine(" Supplier produkt!! " + item + "  Name: " + tempName);
            }

        }
    }
}
