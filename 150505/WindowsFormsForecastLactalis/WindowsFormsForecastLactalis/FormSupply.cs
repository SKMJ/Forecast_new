using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        public static Dictionary<int, PrognosInfoForSupply> SupplierProducts = new Dictionary<int, PrognosInfoForSupply>();
        Form1 form1Instance = new Form1();
        //private List<string> supplier = new List<string>();
        GetFromM3 m3Info = new GetFromM3();
        TextBoxForm infoboxSupply = new TextBoxForm();
        public Dictionary<int, List<int>> MotherSupplierWithChilds = new Dictionary<int, List<int>>();

        public Dictionary<string, string> OrderCodes = new Dictionary<string, string>();
        public Dictionary<int, string> AllProductsDict = new Dictionary<int, string>();
        int desiredStartLocationX;
        int desiredStartLocationY;
        int latestWeek;
        string latestProductNumber;
        Point latestMouseClick;
        int latestRow;
        string latestSelectedYear;
        int selectedYear;

        public FormSupply()
        {
            InitializeComponent();
            Console.WriteLine("Start FormSupply!");
            FixSupplierChoices();
            FixMotherChildList();

            SetupColumns();
            //FillInfo();
        }

        public FormSupply(Point pos)
            : this()
        {
            // here store the value for x & y into instance variables
            this.desiredStartLocationX = pos.X;
            this.desiredStartLocationY = pos.Y;

            Load += new EventHandler(Form2_Load);
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
            supplier.Add("SUPPLIER 2000");
            comboBoxSupplier.DataSource = supplier;
            comboBoxSupplier.DropDownStyle = ComboBoxStyle.DropDownList;

            List<int> yearList = new List<int>();
            yearList.Add(2015);
            yearList.Add(2016);
            yearList.Add(2017);

            comboBoxYear.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxYear.DataSource = new BindingSource(yearList, null);
        }

        public void SetAllProdDict(Dictionary<int, string> allProd)
        {
            foreach (KeyValuePair<int, string> item in allProd)
            {
                AllProductsDict.Add(item.Key, item.Value);
            }
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


        //NAme the columns in the grid
        public void SetupColumns()
        {
            if (latestSelectedYear != comboBoxYear.Text)
            {
                latestSelectedYear = comboBoxYear.Text;
                dataGridForecastInfo.ColumnCount = 56;
                dataGridForecastInfo.Columns[0].Name = "VareNr";
                dataGridForecastInfo.Columns[1].Name = "Beskrivelse";
                dataGridForecastInfo.Columns[2].Name = "Type";

                for (int i = 1; i < 54; i++)
                {
                    string temp = i + "." + comboBoxYear.Text;
                    dataGridForecastInfo.Columns[i + 2].Name = temp;
                }
            }
        }

        //Fill the UI info
        private void FillInfo()
        {
            SupplierProducts = new Dictionary<int, PrognosInfoForSupply>();
            this.dataGridForecastInfo.DataSource = null;

            this.dataGridForecastInfo.Rows.Clear();
            this.dataGridForecastInfo.AllowUserToAddRows = false;
            this.dataGridForecastInfo.AllowUserToDeleteRows = false;
            this.dataGridForecastInfo.AllowUserToOrderColumns = false;

            //bool even = true;
            if (comboBoxSupplier.SelectedItem.ToString().Equals("SUPPLIER 1") || comboBoxSupplier.SelectedItem.ToString().Equals("SUPPLIER 2"))
            {
                //This is just testcode to show something
                bool suppl1 = comboBoxSupplier.SelectedItem.ToString().Equals("SUPPLIER 1");
                CreateSupplyProducts(suppl1);
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
                        PrognosInfoForSupply product1 = new PrognosInfoForSupply(tempName, Convert.ToInt32(item), checkBoxLastYear.Checked);

                        product1.FillNumbers(selectedYear);
                        SupplierProducts.Add(Convert.ToInt32(item), product1);
                        //SupplierProductsFromM3.Add(product1);
                        Console.WriteLine(" Supplier produkt!! " + item + "  Name: " + tempName);
                    }
                }

            }
            else if (comboBoxSupplier.SelectedItem.ToString().Equals("SUPPLIER 2000"))
            {
                numericSupplyNBR.Value = 2000;
                List<string> tempList = m3Info.GetListOfProductsBySupplier("2000");

                foreach (string item in tempList)
                {
                    string tempName = m3Info.GetItemNameByItemNumber(item);
                    PrognosInfoForSupply product1 = new PrognosInfoForSupply(tempName, Convert.ToInt32(item), checkBoxLastYear.Checked);

                    product1.FillNumbers(selectedYear);
                    SupplierProducts.Add(Convert.ToInt32(item), product1);
                    //SupplierProductsFromM3.Add(product1);
                    Console.WriteLine(" Supplier produkt!! " + item + "  Name: " + tempName);
                }
            }

            PrepareGUI();
        }


        //Load GUI with info from SupplierProducts
        private void PrepareGUI()
        {
            Random randomNumber = new Random();

            foreach (PrognosInfoForSupply item in SupplierProducts.Values)
            {
                List<object> tempList;

                tempList = new List<object>();
                tempList.Add(item.ProductNumber.ToString());
                tempList.Add(item.ProductName);
                if (checkBoxLastYear.Checked)
                {
                    tempList.Add("RealiseretKampagn_LastYear");
                    for (int i = 1; i < 54; i++)
                    {
                        tempList.Add(item.RealiseretKampagn_LastYear[i]);
                    }
                }
                AddRowFromList(tempList);


                int number = randomNumber.Next(1, 1000);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("På lager: " + number);
                if (checkBoxLastYear.Checked)
                {
                    tempList.Add("RealiseretSalg_LastYear");
                    for (int i = 1; i < 54; i++)
                    {
                        tempList.Add(item.RealiseretSalg_LastYear[i]);
                    }                    
                }
                AddRowFromList(tempList);
                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("RealiseratSalg_ThisYear");
                for (int i = 1; i < 54; i++)
                {
                    tempList.Add(item.RealiseratSalg_ThisYear[i]);
                }
                AddRowFromList(tempList);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("Kampagn_ThisYear");
                for (int i = 1; i < 54; i++)
                {
                    tempList.Add(item.Kampagn_ThisYear[i]);
                }
                AddRowFromList(tempList);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("Salgsbudget_ThisYear");
                for (int i = 1; i < 54; i++)
                {
                    tempList.Add(item.Salgsbudget_ThisYear[i]);
                }
                AddRowFromList(tempList);


                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("Reguleret_Comment");
                for (int i = 1; i < 54; i++)
                {
                    tempList.Add("");
                }
                AddRowFromList(tempList);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("SalgsbudgetReguleret_TY");
                for (int i = 1; i < 54; i++)
                {
                    tempList.Add(item.SalgsbudgetReguleret_ThisYear[i]);
                }
                AddRowFromList(tempList);



                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("Salgsbudget_Summeret_ThisYear");
                for (int i = 1; i < 54; i++)
                {
                    tempList.Add(item.SalgsbudgetReguleret_ThisYear[i] + item.Salgsbudget_ThisYear[i]);
                }
                AddRowFromList(tempList);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("Köpsbudget_ThisYear");
                for (int i = 1; i < 54; i++)
                {
                    tempList.Add(item.Kopsbudget_ThisYear[i]);
                }
                AddRowFromList(tempList);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("Köpsorder_ThisYear");
                for (int i = 1; i < 54; i++)
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
                else if (Convert.ToString(row.Cells[2].Value) == "RealiseretSalg_LastYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Blue;
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "RealiseratSalg_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
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
                else if (Convert.ToString(row.Cells[2].Value) == "SalgsbudgetReguleret_TY")
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
                    col.DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);
                    col.DefaultCellStyle.ForeColor = Color.Black;
                }
                if (colNBR < 3)
                {
                    col.ReadOnly = false;
                }
                colNBR++;
            }
        }


        //Add testinfo Products
        private void CreateSupplyProducts(bool even)
        {
            SupplierProducts = new Dictionary<int, PrognosInfoForSupply>();
            if (even)
            {
                AddProductByNumber(2432);
                AddProductByNumber(1442);

            }
            else
            {
                AddProductByNumber(1443);
                AddProductByNumber(1447);
                AddProductByNumber(2239);
            }
        }


        private void AddProductByNumber(int number)
        {
            string prodName = "Unknown Name";
            if (AllProductsDict.ContainsKey(number))
            {
                prodName = AllProductsDict[number];
            }
            PrognosInfoForSupply product1 = new PrognosInfoForSupply(prodName, number, checkBoxLastYear.Checked);
            product1.FillNumbers(selectedYear);
            SupplierProducts.Add(number, product1);
        }


        //What to do when a field is clicked
        private void dataGridForecastInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void SetForm1Instanse(Form1 form1)
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
            dataGridForecastInfo.Visible = false;
            Application.DoEvents();
            dataGridForecastInfo.ClearSelection();
            SetupColumns();
            if (!infoboxSupply.Visible)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                FillInfo();
                stopwatch.Stop();
                double timeConnectSeconds = stopwatch.ElapsedMilliseconds / 1000.0;
                Console.WriteLine("Load all Customers Supply! Time (s): " + timeConnectSeconds);
            }
            else
            {
                MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before changing Supplier!");
            }
            dataGridForecastInfo.Visible = true;
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
                    PrognosInfoForSupply tempInfo = GetProductInfoByNumber(productNumber);
                    Console.WriteLine(" Product: " + temp2 + " Week: " + latestWeek);

                    //Create dummy value to show something before real numbers

                    string temp = "";
                    NavSQLSupplyInformation sqlConnection = new NavSQLSupplyInformation(selectedYear, productNumber);
                    temp = sqlConnection.GetSalesBudgetWeekInfo(latestWeek);

                    temp = "Salgsbudget" + " Product: " + temp2 + " Week: " + latestWeek + "\n\n" + temp;
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
                    PrognosInfoForSupply tempInfo = GetProductInfoByNumber(productNumber);
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
                    string temp = "\n\nCustomer1: " + part1 + "\nCustomer2: " + part2 + "\nCustomer3: " + part3 + "\nComment: ";// +tempInfo.Salgsbudget_Comment[latestWeek];
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

                    string temp = "";
                    string tempNewComment = GetValueFromGridAsString(rowIndex, columnIndex);

                    latestRow = rowIndex;
                    latestWeek = columnIndex - 2;
                    latestProductNumber = GetProductNumberFromRow(rowIndex);

                    temp = SupplierProducts[Convert.ToInt32(latestProductNumber)].SalgsbudgetReguleret_Comment[latestWeek];

                    infoboxSupply.SetInfoText(this, tempNewComment, "Reguleret Product: " + latestProductNumber + " Week: " + latestWeek);
                    infoboxSupply.SetOldInfo(temp);
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

        public PrognosInfoForSupply GetProductInfoByNumber(int productNbr)
        {
            if (SupplierProducts.ContainsKey(productNbr))
            {
                return SupplierProducts[productNbr];
            }
            else
            {
                return null;
            }
        }


        private string GetValueFromGridAsString(int row, int col)
        {
            string returnValue = "";

            if (row >= 0 && row < dataGridForecastInfo.RowCount && col >= 0 && col < dataGridForecastInfo.ColumnCount)
            {
                if (dataGridForecastInfo.Rows[row].Cells[col].Value != null)
                {
                    returnValue = dataGridForecastInfo.Rows[row].Cells[col].Value.ToString();
                }
            }
            else
            {
                Console.WriteLine("Supply, Value from grid out of bounds Row: " + row + " Col: " + col);
            }
            return returnValue;
        }



        private void dataGridForecastInfo_CellValidating_1(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            if (columnIndex == 0 & rowIndex == 0)
            {
                return;
            }

            if ((GetValueFromGridAsString(rowIndex, 2) == "Köpsbudget_ThisYear") && columnIndex > 2) // 1 should be your column index
            {
                int i;
                int productNumber = Convert.ToInt32(GetProductNumberFromRow(rowIndex));
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
                        int ammountToKop = Convert.ToInt32(e.FormattedValue) - SupplierProducts[productNumber].Kopsbudget_ThisYear[week];
                        SetKöpsbudget1102(week, productNumber.ToString(), Convert.ToInt32(e.FormattedValue));
                        //Todo write to database
                        NavSQLExecute conn = new NavSQLExecute();

                        //string startDato = "datum";
                        string comment = dataGridForecastInfo.Rows[rowIndex - 1].Cells[columnIndex].Value.ToString();

                        Dictionary<int, DateTime> startDate = new Dictionary<int, DateTime>();
                        string st = "12/29/2014";
                        startDate.Add(2015, DateTime.Parse(st));
                        st = "12/30/2013";
                        startDate.Add(2014, DateTime.Parse(st));
                        st = "01/04/2016";
                        startDate.Add(2016, DateTime.Parse(st));
                        st = "01/02/2017";
                        startDate.Add(2017, DateTime.Parse(st));

                        DateTime tempDate = DateTime.Parse(startDate[selectedYear].ToString());
                        DateTime answer = tempDate.AddDays((week - 1) * 7);
                        string format = "yyyy-MM-dd HH:MM:ss";    // modify the format depending upon input required in the column in database 

                        conn.InsertKöpsbudgetLine(productNumber.ToString(), answer.ToString(format), ammountToKop);
                        //conn.InsertBudgetLine(tempCustNumber, productNumber, answer.ToString(format), ammount);

                    }
                }
            }
            else if ((GetValueFromGridAsString(rowIndex, 2) == "SalgsbudgetReguleret_TY") && columnIndex > 2) // 1 should be your column index
            {
                int i;
                int productNumber = Convert.ToInt32(GetProductNumberFromRow(rowIndex));
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
                        if (Convert.ToInt32(e.FormattedValue) != SupplierProducts[productNumber].SalgsbudgetReguleret_ThisYear[week])
                        {
                            int ammount = Convert.ToInt32(e.FormattedValue) - SupplierProducts[productNumber].SalgsbudgetReguleret_ThisYear[week];

                            if (dataGridForecastInfo.Rows[rowIndex - 1].Cells[columnIndex].Value.ToString() != "")
                            {
                                SetRegulerat1102(week, productNumber.ToString(), Convert.ToInt32(e.FormattedValue));

                                int sBudget = Convert.ToInt32(dataGridForecastInfo.Rows[rowIndex - 2].Cells[columnIndex].Value);
                                int regSbudget = Convert.ToInt32(e.FormattedValue);
                                dataGridForecastInfo.Rows[rowIndex + 2].Cells[columnIndex].Value = sBudget + regSbudget;
                                //TODO: Write to database

                                //here a new budget_line post should be added to the database
                                
                                NavSQLExecute conn = new NavSQLExecute();
                               
                                //string startDato = "datum";
                                string comment = dataGridForecastInfo.Rows[rowIndex - 1].Cells[columnIndex].Value.ToString(); 

                                Dictionary<int, DateTime> startDate = new Dictionary<int, DateTime>();
                                string st = "12/29/2014";
                                startDate.Add(2015, DateTime.Parse(st));
                                st = "12/30/2013";
                                startDate.Add(2014, DateTime.Parse(st));
                                st = "01/04/2016";
                                startDate.Add(2016, DateTime.Parse(st));
                                st = "01/02/2017";
                                startDate.Add(2017, DateTime.Parse(st));

                                DateTime tempDate = DateTime.Parse(startDate[selectedYear].ToString());
                                DateTime answer = tempDate.AddDays((week - 1) * 7);
                                string format = "yyyy-MM-dd HH:MM:ss";    // modify the format depending upon input required in the column in database 

                                conn.InsertReguleretBudgetLine(productNumber.ToString(), answer.ToString(format), ammount, comment);
                                //conn.InsertBudgetLine(tempCustNumber, productNumber, answer.ToString(format), ammount);

                                dataGridForecastInfo.Rows[rowIndex - 1].Cells[columnIndex].Value = "";
                            

                            }
                            else
                            {
                                dataGridForecastInfo.Rows[rowIndex].Cells[columnIndex].Value = SupplierProducts[productNumber].SalgsbudgetReguleret_ThisYear[week];
                                MessageBox.Show("Please Write comment before changing value. Then type the value again and press enter to save.");

                            }

                            
                        }
                       

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
            int prodNBRint = Convert.ToInt32(productNumber);
            if (SupplierProducts.ContainsKey(prodNBRint))
            {
                SupplierProducts[prodNBRint].Salgsbudget_ThisYear[week] = Convert.ToInt32(value);
            }

        }

        public void SetRegulerat1102(int week, string productNumber, int value)
        {
            int prodNBRint = Convert.ToInt32(productNumber);
            if (SupplierProducts.ContainsKey(prodNBRint))
            {
                SupplierProducts[prodNBRint].SalgsbudgetReguleret_ThisYear[week] = Convert.ToInt32(value);

            }
        }


        public void SetProductRegComment(string comment)
        {
            int prodNBRint = Convert.ToInt32(latestProductNumber);

            dataGridForecastInfo.Rows[latestRow].Cells[latestWeek + 2].Value = comment;
            if (SupplierProducts.ContainsKey(prodNBRint))
            {
                SupplierProducts[prodNBRint].Salgsbudget_Comment[latestWeek] = comment;
            }
            //FillInfo();
        }


        private void buttonGetSupplierFromNBR_Click(object sender, EventArgs e)
        {
            dataGridForecastInfo.Visible = false;
            Application.DoEvents();
            dataGridForecastInfo.ClearSelection();
            SetupColumns();
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
            dataGridForecastInfo.Visible = true;

        }

        private void comboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedYear = (int)comboBoxYear.SelectedItem;
        }

        private void buttonGetProductByNumber_Click(object sender, EventArgs e)
        {
            dataGridForecastInfo.Visible = false;
            Application.DoEvents();  
            dataGridForecastInfo.ClearSelection();
            SetupColumns();

            SupplierProducts = new Dictionary<int, PrognosInfoForSupply>();
            this.dataGridForecastInfo.DataSource = null;

            this.dataGridForecastInfo.Rows.Clear();
            this.dataGridForecastInfo.AllowUserToAddRows = false;
            this.dataGridForecastInfo.AllowUserToDeleteRows = false;
            this.dataGridForecastInfo.AllowUserToOrderColumns = false;


            int prodNMBR = (int)numericUpDownPRoductNumber.Value;
            AddProductByNumber(prodNMBR);
            PrepareGUI();
            dataGridForecastInfo.Visible = true;
        }

        private void buttonCreateM3LactalisOrders_Click(object sender, EventArgs e)
        {
            CreateOrderForProduct(5, 23303);
        }

        private void CreateOrderForProduct(int week, int prodNumber)
        {
            if (SupplierProducts.Count > 0)
            {
                string codeOrder = SupplierProducts[0].Supplier + "XYZ" + SupplierProducts[0].WareHouse + "XYZ" + SupplierProducts[0].PrepLocation;
                string M3_order_code = "";
                if (OrderCodes.ContainsKey(codeOrder))
                {
                    M3_order_code = OrderCodes[codeOrder];
                }
                else
                {
                    M3_order_code = m3Info.GetNewOrderCode();
                    OrderCodes.Add(codeOrder, M3_order_code);
                }

                m3Info.CreateNewOrderProposal(M3_order_code, prodNumber.ToString(), 10, "2000", "20150821");
            }
        }

        private void checkBoxLastYear_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
