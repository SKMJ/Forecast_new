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
        Form1 form1Instance = new Form1();
        private List<string> supplier = new List<string>();
        GetFromM3 m3Info = new GetFromM3();
        int desiredStartLocationX;
        int desiredStartLocationY;

        public FormSupply()
        {
            InitializeComponent();
            supplier.Add("SUPPLIER 1");
            supplier.Add("SUPPLIER 2");

            comboBoxSupplier.DataSource = supplier;
            SetupColumns();
            FillInfo();
        }

        public FormSupply(Point pos)
            : this()
        {
            // here store the value for x & y into instance variables
            this.desiredStartLocationX = pos.X;
            this.desiredStartLocationY = pos.Y;

            Load += new EventHandler(Form2_Load);
        }

        private void Form2_Load(object sender, System.EventArgs e)
        {
            this.SetDesktopLocation(desiredStartLocationX, desiredStartLocationY);
        }


        //take an object list and fill the ui grid with a row with this info
        private void AddRowFromList(List<object> stringList)
        {
            object[] array = stringList.ToArray();
            dataGridForecastInfo.Rows.Add(array);

        }


        //NAme the columns in the info
        public void SetupColumns()
        {
            dataGridForecastInfo.ColumnCount = 55;
            dataGridForecastInfo.Columns[0].Name = "VareNr";
            dataGridForecastInfo.Columns[1].Name = "Beskrivelse";
            dataGridForecastInfo.Columns[2].Name = "Type";

            for (int i = 1; i < 53; i++)
            {
                string temp = i + ".2015";
                dataGridForecastInfo.Columns[i + 2].Name = temp;
            }
        }

        //Fill the UI info
        private void FillInfo()
        {
            this.dataGridForecastInfo.DataSource = null;

            this.dataGridForecastInfo.Rows.Clear();
            this.dataGridForecastInfo.AllowUserToAddRows = false;
            this.dataGridForecastInfo.AllowUserToDeleteRows = false;
            this.dataGridForecastInfo.AllowUserToOrderColumns = false;

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
                    List<object> tempList = new List<object>();
                    tempList.Add(item.ProductNumber.ToString());
                    tempList.Add(item.ProductName);
                    tempList.Add("RealiseretKampagn_LastYear");
                    for (int i = 1; i < 53; i++)
                    {
                        tempList.Add(item.RealiseretKampagn_LastYear[i]);
                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("RealiseretSalgsbudget_LastYear");
                    for (int i = 1; i < 53; i++)
                    {
                        tempList.Add(item.RealiseretSalgsbudget_LastYear[i]);
                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Realiserat_ThisYear");
                    for (int i = 1; i < 53; i++)
                    {
                        tempList.Add(item.Realiserat_ThisYear[i]);
                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Kampagn_ThisYear");
                    for (int i = 1; i < 53; i++)
                    {
                        tempList.Add(item.Kampagn_ThisYear[i]);
                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Salgsbudget_ThisYear");
                    for (int i = 1; i < 53; i++)
                    {
                        tempList.Add(item.Salgsbudget_ThisYear[i]);
                    }
                    AddRowFromList(tempList);


                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("SalgsbudgetReguleret_ThisYear");
                    for (int i = 1; i < 53; i++)
                    {
                        tempList.Add(item.SalgsbudgetReguleret_ThisYear[i]);
                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Köpsbudget_ThisYear");
                    for (int i = 1; i < 53; i++)
                    {
                        tempList.Add(item.Kopsbudget_ThisYear[i]);
                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Köpsorder_ThisYear");
                    for (int i = 1; i < 53; i++)
                    {
                        tempList.Add(item.Kopsorder_ThisYear[i]);
                    }
                    AddRowFromList(tempList);
                }
            }
            //Thread.Sleep(2000);
            foreach (DataGridViewRow row in dataGridForecastInfo.Rows)
            {
                row.Height = 35;
                if (Convert.ToString(row.Cells[2].Value) == "RealiseretKampagn_LastYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Blue;
                    row.ReadOnly = true;


                }
                else if (Convert.ToString(row.Cells[2].Value) == "RealiseretSalgsbudget_LastYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Blue;
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Kampagn_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Salgsbudget_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
                    row.ReadOnly = true;


                }
                else if (Convert.ToString(row.Cells[2].Value) == "SalgsbudgetReguleret_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
                    row.ReadOnly = true;


                }
                else if (Convert.ToString(row.Cells[2].Value) == "Köpsbudget_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
                    row.ReadOnly = false;


                }
                else if (Convert.ToString(row.Cells[2].Value) == "Köpsorder_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Green;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
                    row.ReadOnly = true;


                }
                else if (Convert.ToString(row.Cells[2].Value) == "Realiserat_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
                    row.ReadOnly = true;


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
                if (colNBR < 3)
                {
                    col.ReadOnly = false;
                }
                colNBR++;
            }
        }


        //What to do when a field is clicked
        private void dataGridForecastInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        internal void SetForm1Instanse(Form1 form1)
        {
            form1Instance = form1;
        }

        //Go back to sales view
        private void buttonSalesView_Click(object sender, EventArgs e)
        {
            Console.WriteLine("User press Sales View");
            this.Visible = false;
            form1Instance.BringToFront();
            form1Instance.Location = this.Location;
            form1Instance.Show();
        }

        private void buttonGetProductsBySupplier_Click(object sender, EventArgs e)
        {
            FillInfo();
        }

        private void buttonTestSupplItems_Click(object sender, EventArgs e)
        {
            List<string> tempList = m3Info.GetListOfProductsBySupplier("3141");

            foreach (string item in tempList)
            {
                string tempName = m3Info.GetItemNameByItemNumber(item);
                Console.WriteLine(" Supplier produkt!! " + item + "  Name: " + tempName);
            }

        }

        private void dataGridForecastInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;


            Console.WriteLine("Value clicked... Column index: " + columnIndex + "  rowIndex: " + rowIndex);

            //for the Sales info forecast show extra info
            if (rowIndex % 8 == 4 && columnIndex > 2)
            {

                string temp2 = GetValueFromGridAsString(rowIndex - 4, 0);
                int latestProductNumber = Convert.ToInt32(temp2);

                int latestWeek = columnIndex - 2;
                PrognosInfo tempInfo = form1Instance.GetProductInfoByNumber(latestProductNumber);
                Console.WriteLine(" Product: " + temp2 + " Week: " + latestWeek);

                string temp = "Sales Person1: 8 \nSales Person2: 10\nSales Person3: 10 \n" + "Comment: " + tempInfo.Salgsbudget_Comment[latestWeek];
                MessageBox.Show(temp);
            }
        }

        private string GetValueFromGridAsString(int row, int col)
        {
            string returnValue = "";

            if (row >= 0 && row < dataGridForecastInfo.RowCount && col >= 0 && col < dataGridForecastInfo.ColumnCount)
            {
                returnValue = dataGridForecastInfo.Rows[row].Cells[col].Value.ToString();
            }
            else
            {
                Console.WriteLine("Value from grid out of bounds Row: " + row + " Col: " + col);
            }
            return returnValue;
        }

       

        private void dataGridForecastInfo_CellValidating_1(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            if (rowIndex % 8 == 6 && columnIndex > 2) // 1 should be your column index
            {
                int i;
                string productNumber = GetValueFromGridAsString(rowIndex - 6, 0);
                int week = columnIndex - 2;
                if (!int.TryParse(Convert.ToString(e.FormattedValue), out i))
                {
                    e.Cancel = true;
                    MessageBox.Show(new Form() { TopMost = true }, "Please enter numeric value. Product: " + productNumber + " Week: " + week);
                }
                else
                {
                    // the input is numeric 
                    int value = Convert.ToInt32(e.FormattedValue);
                    if (value < 0 || value > 1000000)
                    {
                        e.Cancel = true;
                        MessageBox.Show(new Form() { TopMost = true }, "Please enter a positive possible value. Product: " + productNumber + " Week: " + week);
                    }
                    else
                    {
                        form1Instance.SetKöpsbudget(week, productNumber, value);
                        
                    }
                }
            }
        }

        private void FormSupply_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1Instance.Close();
        }
    }
}
