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
        public static List<PrognosInfo> SupplierProducts = new List<PrognosInfo>();
        Form1 form1Instance = new Form1();
        //private List<string> supplier = new List<string>();
        GetFromM3 m3Info = new GetFromM3();
        TextBoxForm infoboxSupply = new TextBoxForm();
        public Dictionary<int, List<int>> MotherSupplierWithChilds = new Dictionary<int, List<int>>();
        int desiredStartLocationX;
        int desiredStartLocationY;
        int latestWeek;
        string latestProductNumber;
        Point latestMouseClick;
        int latestRow;

        public FormSupply()
        {
            InitializeComponent();
            FixSupplierChoices();
            FixMotherChildList();

            SetupColumns();
            FillInfo();
        }

        private void FixMotherChildList()
        {
            List<int> temp = new List<int>();
            temp.Add(1102001);
            temp.Add(1102002);
            MotherSupplierWithChilds.Add(1102, temp);
        }

        private void FixSupplierChoices()
        {
            List<string> supplier = new List<string>();
            supplier.Add("SUPPLIER 1");
            supplier.Add("SUPPLIER 2");
            supplier.Add("SUPPLIER 1102");
            comboBoxSupplier.DataSource = supplier;
            comboBoxSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
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
            SupplierProducts = new List<PrognosInfo>();
            this.dataGridForecastInfo.DataSource = null;

            this.dataGridForecastInfo.Rows.Clear();
            this.dataGridForecastInfo.AllowUserToAddRows = false;
            this.dataGridForecastInfo.AllowUserToDeleteRows = false;
            this.dataGridForecastInfo.AllowUserToOrderColumns = false;

            bool even = true;
            if (comboBoxSupplier.SelectedItem.ToString().Equals("SUPPLIER 1") || comboBoxSupplier.SelectedItem.ToString().Equals("SUPPLIER 2"))
            {
                //This is just testcode to show something
                if (comboBoxSupplier.SelectedItem.ToString().Contains("1"))
                {
                    even = false;
                    numericSupplyNBR.Value = 1;
                }
                else
                {
                    numericSupplyNBR.Value = 2;
                }

                PrognosInfo tempPrognosInfo;
                foreach (PrognosInfo item in Form1.Products)
                {
                    bool thisEven = true;
                    if (item.ProductNumber % 2 > 0)
                    {
                        thisEven = false;
                    }

                    if (thisEven == even)
                    {
                        tempPrognosInfo = new PrognosInfo(item);
                        SupplierProducts.Add(tempPrognosInfo);

                    }
                }
            }
            else if (comboBoxSupplier.SelectedItem.ToString().Equals("SUPPLIER 1102"))
            {
                numericSupplyNBR.Value = 1102;
                //This will later load all numbers from databases
                foreach (int supplNBR in MotherSupplierWithChilds[1102])
                {
                    List<string> tempList = m3Info.GetListOfProductsBySupplier(supplNBR.ToString());

                    foreach (string item in tempList)
                    {
                        string tempName = m3Info.GetItemNameByItemNumber(item);
                        PrognosInfo product1 = new PrognosInfo(tempName, Convert.ToInt32(item), 999);
                        product1.FillNumbers();
                        SupplierProducts.Add(product1);
                        //SupplierProductsFromM3.Add(product1);
                        Console.WriteLine(" Supplier produkt!! " + item + "  Name: " + tempName);
                    }
                }

            }

            Random randomNumber = new Random();

            foreach (PrognosInfo item in SupplierProducts)
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


                int number = randomNumber.Next(1, 1000);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("På lager: " + number);
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
                tempList.Add("Reguleret_Comment");
                for (int i = 1; i < 53; i++)
                {
                    tempList.Add(item.SalgsbudgetReguleret_Comment[i]);
                }
                AddRowFromList(tempList);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("Salgsbudget_Summeret_ThisYear");
                for (int i = 1; i < 53; i++)
                {
                    tempList.Add(item.SalgsbudgetReguleret_ThisYear[i] + item.Salgsbudget_ThisYear[i]);
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
                else if (Convert.ToString(row.Cells[2].Value) == "Realiserat_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Blue;
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Kampagn_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Salgsbudget_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    //row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "SalgsbudgetReguleret_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
                    row.ReadOnly = false;
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
                    //row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Realiserat_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    //row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Salgsbudget_Summeret_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.MediumOrchid;
                    //row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular);
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value).Contains("Commen"))
                {
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

            if (!infoboxSupply.Visible)
            {
                Console.WriteLine("User press Sales View");
                this.Visible = false;
                form1Instance.BringToFront();
                form1Instance.Location = this.Location;
                form1Instance.Show();
            }
            else
            {
                MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before changing view!");
            }
        }

        private void buttonGetProductsBySupplier_Click(object sender, EventArgs e)
        {
            if (!infoboxSupply.Visible)
            {
                FillInfo();
            }
            else
            {
                MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before changing Supplier!");
            }
        }

        private void buttonTestSupplItems_Click(object sender, EventArgs e)
        {
            List<string> tempList = m3Info.GetListOfProductsBySupplier("1102001");
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
            latestMouseClick = System.Windows.Forms.Cursor.Position;

            //for the Sales info forecast show extra info
            if ((GetValueFromGridAsString(rowIndex, 2) == "Salgsbudget_ThisYear") && columnIndex > 2)
            {
                if (!infoboxSupply.Visible)
                {
                    string temp2 = GetProductNumberFromRow(rowIndex);
                    int productNumber = Convert.ToInt32(temp2);

                    int latestWeek = columnIndex - 2;
                    PrognosInfo tempInfo = GetProductInfoByNumber(productNumber);
                    Console.WriteLine(" Product: " + temp2 + " Week: " + latestWeek);

                    //Create dummy value to show something before real numbers
                    int all = Convert.ToInt32(GetValueFromGridAsString(rowIndex, columnIndex));
                    int part1 = all;
                    int part2 = 0;
                    int part3 = 0;
                    if (all >= 25)
                    {
                        part1 = all - 25;
                        part2 = 15;
                        part3 = 10;
                    }
                    string temp = "\n\nCustomer1: " + part1 + "\nCustomer2: " + part2 + "\nCustomer3: " + part3 + "\nComment: " + tempInfo.Salgsbudget_Comment[latestWeek];
                    temp = "Salgsbudget" + " Product: " + temp2 + " Week: " + latestWeek + temp;
                    MessageBox.Show(temp);
                }
                else
                {
                    MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window to see info!");
                }
            }
            else if ((GetValueFromGridAsString(rowIndex, 2) == "Realiserat_ThisYear") && columnIndex > 2)
            {
                if (!infoboxSupply.Visible)
                {
                    string temp2 = GetProductNumberFromRow(rowIndex);
                    int productNumber = Convert.ToInt32(temp2);
                    int latestWeek = columnIndex - 2;
                    PrognosInfo tempInfo = GetProductInfoByNumber(productNumber);
                    Console.WriteLine(" Product: " + temp2 + " Week: " + latestWeek);

                    //Create dummy value to show something before real numbers
                    int all = Convert.ToInt32(GetValueFromGridAsString(rowIndex, columnIndex));
                    int part1 = all;
                    int part2 = 0;
                    int part3 = 0;
                    if (all >= 25)
                    {
                        part1 = all - 25;
                        part2 = 15;
                        part3 = 10;
                    }
                    string temp = "\n\nCustomer1: " + part1 + "\nCustomer2: " + part2 + "\nCustomer3: " + part3 + "\nComment: " + tempInfo.Salgsbudget_Comment[latestWeek];
                    temp = "Realiserat" + " Product: " + temp2 + " Week: " + latestWeek + temp;
                    MessageBox.Show(temp);
                }
                else
                {
                    MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before open info!");
                }
            }
            else if (GetValueFromGridAsString(rowIndex, 2).Contains("Comment"))
            {
                if (!infoboxSupply.Visible)
                {
                    string temp = GetValueFromGridAsString(rowIndex, columnIndex);
                    //string temp2 = GetValueFromGridAsString(rowIndex - 6, 0);

                    latestRow = rowIndex;
                    latestWeek = columnIndex - 2;
                    latestProductNumber = GetProductNumberFromRow(rowIndex);
                    infoboxSupply.SetInfoText(this, temp, "Reguleret Product: " + latestProductNumber + " Week: " + latestWeek);
                    infoboxSupply.TopMost = true;

                    if (infoboxSupply.desiredStartLocationX == 0 && infoboxSupply.desiredStartLocationY == 0)
                    {
                        infoboxSupply.SetNextLocation(latestMouseClick);
                    }
                    else
                    {
                        infoboxSupply.Location = latestMouseClick;
                    }
                    infoboxSupply.Show();
                    infoboxSupply.FocusTextBox();
                }
                else
                {
                    MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before open a new one!");
                }
            }
        }

        public PrognosInfo GetProductInfoByNumber(int productNbr)
        {
            foreach (PrognosInfo item in SupplierProducts)
            {
                if (item.ProductNumber == productNbr)
                {
                    return item;
                }
            }
            return null;
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

            if ((GetValueFromGridAsString(rowIndex, 2) == "Köpsbudget_ThisYear") && columnIndex > 2) // 1 should be your column index
            {
                int i;
                string productNumber = GetProductNumberFromRow(rowIndex);
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

                        SetKöpsbudget1102(week, productNumber, Convert.ToInt32(e.FormattedValue));
                        //Todo write to database

                    }
                }
            }
            else if ((GetValueFromGridAsString(rowIndex, 2) == "SalgsbudgetReguleret_ThisYear") && columnIndex > 2) // 1 should be your column index
            {
                int i;
                string productNumber = GetProductNumberFromRow(rowIndex);
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
                    if (value < -1000000 || value > 1000000)
                    {
                        e.Cancel = true;
                        MessageBox.Show(new Form() { TopMost = true }, "Please enter a numeric value within range. Product: " + productNumber + " Week: " + week);
                    }
                    else
                    {
                        SetRegulerat1102(week, productNumber, Convert.ToInt32(e.FormattedValue));

                        int sBudget = Convert.ToInt32(dataGridForecastInfo.Rows[rowIndex - 1].Cells[columnIndex].Value);
                        int regSbudget = Convert.ToInt32(e.FormattedValue);
                        dataGridForecastInfo.Rows[rowIndex + 1].Cells[columnIndex].Value = sBudget + regSbudget;

                        //TODO: Write to database

                    }
                }
            }
        }

        private void FormSupply_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1Instance.Close();
        }

        private string GetProductNumberFromRow(int rowIndex)
        {
            string returnString = "";

            int row = rowIndex;
            while ((GetValueFromGridAsString(row, 0).Length < 1) && row >= 0)
            {
                row--;
            }
            returnString = GetValueFromGridAsString(row, 0);
            return returnString;
        }

        private void SetKöpsbudget1102(int week, string productNumber, int value)
        {
            foreach (PrognosInfo item in SupplierProducts)
            {
                if (item.ProductNumber.ToString() == productNumber)
                {
                    item.Salgsbudget_ThisYear[week] = Convert.ToInt32(value);
                }
            }
        }

        public void SetRegulerat1102(int week, string productNumber, int value)
        {
            foreach (PrognosInfo item in SupplierProducts)
            {
                if (item.ProductNumber.ToString() == productNumber)
                {
                    item.SalgsbudgetReguleret_ThisYear[week] = Convert.ToInt32(value);
                }
            }
        }

        internal void SetProductRegComment(string comment)
        {
            foreach (PrognosInfo item in SupplierProducts)
            {
                if (item.ProductNumber.ToString() == latestProductNumber)
                {
                    //item.SalgsbudgetReguleret_ThisYear[week] = Convert.ToInt32(value);
                    dataGridForecastInfo.Rows[latestRow].Cells[latestWeek + 2].Value = comment;
                    item.Salgsbudget_Comment[latestWeek] = comment;
                    //TODO: Write to database
                }
            }
            
            //FillInfo();
        }

        private void buttonGetSupplierFromNBR_Click(object sender, EventArgs e)
        {
            if (numericSupplyNBR.Value == 1102)
            {                
                comboBoxSupplier.SelectedItem = "SUPPLIER 1102";
                if (!infoboxSupply.Visible)
                {
                    FillInfo();
                }
                else
                {
                    MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before changing Supplier!");
                }
            }
            else
            {
                MessageBox.Show(new Form() { TopMost = true }, "No supplier with that number");
            }


        }
    }
}
