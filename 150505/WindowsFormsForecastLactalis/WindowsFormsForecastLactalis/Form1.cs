using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsForecastLactalis
{
    public partial class Form1 : Form
    {
        private static List<PrognosInfoSales> Products = new List<PrognosInfoSales>();
        public FormSupply supplyViewInstance;
        private List<string> assortments = new List<string>();
        private string latestComboBoxText = "";
        GetFromM3 m3Info = new GetFromM3();
        TextBoxForm infoboxSales = new TextBoxForm();
        private string latestProductNumber;
        private int latestWeek;
        Point latestMouseClick;
        int selectedYear;
        Dictionary<string, string> custDictionary;

        private bool loadingNewProductsOngoing;

        public Dictionary<int, string> allProductsDict = new Dictionary<int, string>();


        //Get_FromSimulatedM3 m3_info = new Get_FromSimulatedM3();
        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("Start Form1!");
            loadingNewProductsOngoing = false;
            //test with Coop and test customer
            FixCustomerChoices();

            //m3Info.TestM3Connection();
            SetupColumns();

            LoadAllProductDict();

        }


        //Populate the drop downlist wit customers
        private void FixCustomerChoices()
        {
            //Dictionary<string, string> cust = GetCustomers();
            custDictionary = GetCustomersWitohutLoad();

            comboBoxAssortment.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBoxAssortment.DataSource = new BindingSource(custDictionary, null);
            comboBoxAssortment.DisplayMember = "Key";
            comboBoxAssortment.ValueMember = "Key";

            List<int> yearList = new List<int>();
            yearList.Add(2015);
            yearList.Add(2016);
            yearList.Add(2017);

            comboBoxYear.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxYear.DataSource = new BindingSource(yearList, null);
        }


        private void dataGridForecastInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        //This takes a list of strings and add a row column by column
        private void AddRowFromList(List<object> stringList)
        {
            object[] array = stringList.ToArray();
            dataGridForecastInfo.Rows.Add(array);

        }


        //Name the columns in the grid
        public void SetupColumns()
        {
            if (comboBoxYear.Text != latestComboBoxText)
            {
                latestComboBoxText = comboBoxYear.Text;
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


        //Fill the grid with info from the Products
        public void FillSalesGUIInfo()
        {
            this.dataGridForecastInfo.DataSource = null;

            this.dataGridForecastInfo.Rows.Clear();

            this.dataGridForecastInfo.AllowUserToAddRows = false;
            this.dataGridForecastInfo.AllowUserToDeleteRows = false;
            this.dataGridForecastInfo.AllowUserToOrderColumns = false;

            Dictionary<int,int> weekToLock = new Dictionary<int,int>();
            int weekProdNBR =0;
            Products.Sort();
            foreach (PrognosInfoSales item in Products)
            {
                weekToLock.Add(weekProdNBR, item.WeekToLockFrom + 2); //+ 2 is offset from cell number to week number
                weekProdNBR++;
                List<object> tempList = new List<object>();
                tempList.Add(item.ProductNumber.ToString());
                tempList.Add(item.ProductName);
                tempList.Add("RealiseretKampagn_LastYear");
                for (int i = 1; i < 54; i++)
                {
                    tempList.Add(item.RealiseretKampagn_LastYear[i]);
                }
                AddRowFromList(tempList);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("RealiseretSalg_LastYear");
                for (int i = 1; i < 54; i++)
                {
                    tempList.Add(item.RealiseretSalgs_LastYear[i]);
                }
                AddRowFromList(tempList);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("Salgsbudget_LastYear");
                for (int i = 1; i < 54; i++)
                {
                    tempList.Add(item.Salgsbudget_LastYear[i]);
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

                //tempList = new List<object>();
                //tempList.Add("");
                //tempList.Add("");
                //tempList.Add("Salgsbudget_Comment");
                //for (int i = 1; i < 54; i++)
                //{
                //    tempList.Add(item.Salgsbudget_Comment[i]);
                //}
                //AddRowFromList(tempList);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("Salgsbudget_ChangeHistory");
                for (int i = 1; i < 54; i++)
                {
                    tempList.Add(item.Salgsbudget_ChangeHistory[i]);
                }
                AddRowFromList(tempList);
            }
            //Thread.Sleep(2000);
           
                weekProdNBR = 0;
            //After all is filled set colors and reaqdonly properties
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
                else if (Convert.ToString(row.Cells[2].Value) == "Salgsbudget_LastYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Blue;
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Kampagn_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Salgsbudget_ChangeHistory")
                {
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                    row.ReadOnly = true;
                }
                    
                else if (Convert.ToString(row.Cells[2].Value) == "Salgsbudget_ThisYear")
                {
                    
                    row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if(weekToLock[weekProdNBR] < i)
                        {
                            row.Cells[i].ReadOnly = false;
                            row.Cells[i].Style = new DataGridViewCellStyle { ForeColor = Color.Violet };
                        }
                        else
                        {
                            row.Cells[i].ReadOnly = true;
                            row.Cells[i].Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                        }
                    }
                    weekProdNBR++;
                }
                else if (Convert.ToString(row.Cells[2].Value).Contains("Commen"))
                {
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "RealiseratSalg_ThisYear")
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
                    col.ReadOnly = true;
                }
                colNBR++;
            }
        }


        //Add testinfo Products
        private void CreateProducts()
        {
            string tempCustNumber = custDictionary[comboBoxAssortment.Text];
            PrognosInfoSales product1 = new PrognosInfoSales(allProductsDict[2432], 2432, tempCustNumber);
            PrognosInfoSales product2 = new PrognosInfoSales(allProductsDict[1442], 1442, tempCustNumber);
            PrognosInfoSales product3 = new PrognosInfoSales(allProductsDict[1238], 1238, tempCustNumber);
            PrognosInfoSales product4 = new PrognosInfoSales(allProductsDict[2442], 2442, tempCustNumber);
            PrognosInfoSales product5 = new PrognosInfoSales(allProductsDict[2735], 2735, tempCustNumber);

            SetStatus("Products loading 1/5");
            product1.FillNumbers(selectedYear);
            SetStatus("Products loading 2/5");
            product2.FillNumbers(selectedYear);
            SetStatus("Products loading 3/5");
            product3.FillNumbers(selectedYear);
            SetStatus("Products loading 4/5");
            product4.FillNumbers(selectedYear);
            SetStatus("Products loading 5/5");
            product5.FillNumbers(selectedYear);


            Products.Add(product1);
            Products.Add(product2);
            Products.Add(product3);
            Products.Add(product4);
            Products.Add(product5);
            labelStatus.Visible = false;
        }


        private void SetStatus(string status)
        {
            labelStatus.Text = status;
            buttonSupplyView.Enabled = false;
            comboBoxYear.Enabled = false;
            labelStatus.Invalidate();
            labelStatus.Update();
            labelStatus.Refresh();
            Application.DoEvents();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        //Change to supply view
        private void buttonSupplyView_Click(object sender, EventArgs e)
        {
            Console.WriteLine("User press Supply View");
            if (!infoboxSales.Visible)
            {

                Point tempLocation = this.Location;
                supplyViewInstance = new FormSupply(tempLocation);
                supplyViewInstance.Location = this.Location;

                this.Visible = false;
                supplyViewInstance.SetForm1Instanse(this);
                supplyViewInstance.BringToFront();
                supplyViewInstance.Show();
                supplyViewInstance.SetAllProdDict(allProductsDict);
            }
            else
            {
                MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before changing view!");
            }
        }


        private void comboBoxAssortment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        //Load products by customer
        private void button1_Click(object sender, EventArgs e)
        {
            selectedYear = (int) comboBoxYear.SelectedItem;
            
            labelStatus.Visible = true;
            dataGridForecastInfo.Visible = false;
            SetStatus("Loading Products");

            loadingNewProductsOngoing = true;
            Stopwatch stopwatch = Stopwatch.StartNew();

            SetupColumns();

            if (!infoboxSales.Visible)
            {
                if (comboBoxAssortment.SelectedItem.ToString() != "COOP")
                {
                    Console.WriteLine("Hittepåkund vald");
                    Products = new List<PrognosInfoSales>();
                    CreateProducts();

                    FillSalesGUIInfo();
                }
                else
                {

                    Console.WriteLine("COOP vald");
                    Products = new List<PrognosInfoSales>();
                    List<int> productList = m3Info.GetListOfProductsNbrByAssortment("COOP");
                    int nbrItems = productList.Count;
                    if (productList != null)
                    {
                        int i = 0;
                        foreach (int item in productList)
                        {
                            i++;
                            string temp = m3Info.GetItemNameByItemNumber(item.ToString());
                            PrognosInfoSales product1 = new PrognosInfoSales(temp, item, "COOP");
                            product1.FillNumbers(selectedYear);
                            Products.Add(product1);
                            SetStatus("Products Loading " + i + "/" + nbrItems);
                        }
                        FillSalesGUIInfo();
                    }
                    else
                    {
                        MessageBox.Show("M3 communication fail");
                    }
                }
            }
            else
            {
                MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before loading new info!");
            }

            stopwatch.Stop();
            double timeConnectSeconds = stopwatch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Load all Customers Sales! Time (s): " + timeConnectSeconds);
            LoadingReady();
        }


        //Set comment from outside this form (textbox)
        public void SetProductComment(string comment)
        {
            foreach (PrognosInfoSales item in Products)
            {
                if (item.ProductNumber.ToString() == latestProductNumber)
                {
                    item.Salgsbudget_Comment[latestWeek] = comment;
                    //TODO: Write to database
                }
            }
            FillSalesGUIInfo();
        }


        private void buttonSupplyView_MouseClick(object sender, MouseEventArgs e)
        {

        }


        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
        }


        private void dataGridForecastInfo_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
        }


        private void dataGridForecastInfo_MouseClick(object sender, MouseEventArgs e)
        {
            latestMouseClick = System.Windows.Forms.Cursor.Position;

        }


        private void dataGridForecastInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            //Take care of History value and fill value when value changed
            //if (rowIndex >= 0 && GetValueFromGridAsString(rowIndex, 2) == "Salgsbudget_ThisYear" && columnIndex > 2)//(rowIndex % 7 == 5 && columnIndex > 2) // 1 should be your column index
            //{
            //    int i;
            //    string productNumber = GetProductNumberFromRow(rowIndex);//GetValueFromGridAsString(rowIndex - 5, 0);
            //    int week = columnIndex - 2;
            //    if (int.TryParse(GetValueFromGridAsString(rowIndex, columnIndex), out i))
            //    {
            //        foreach (PrognosInfoSales item in Products)
            //        {
            //            if (item.ProductNumber.ToString() == productNumber)
            //            {
            //                //Check so value really is changed to new value
            //                if (GetValueFromGridAsString(rowIndex, columnIndex) != GetLastNumber(item.Salgsbudget_ChangeHistory[week]))
            //                {
            //                    string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            //                    item.Salgsbudget_ChangeHistory[week] = item.Salgsbudget_ChangeHistory[week] + "  Changed by " + userName + " Date: " + DateTime.Now.ToShortDateString() + " To Value: " + GetValueFromGridAsString(rowIndex, columnIndex);
            //                }
            //                //TODO: Write to database
            //            }
            //        }
            //    }
            //    FillSalesGUIInfo();
            //}
        }


        //Velidate that input is valid value
        private void dataGridForecastInfo_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            //Do not validate while new products is loading
            if (!loadingNewProductsOngoing && GetValueFromGridAsString(rowIndex, 2) == "Salgsbudget_ThisYear" && columnIndex > 2)//(rowIndex % 7 == 5 && columnIndex > 2) // 1 should be your column index
            {
                int i;
                string productNumber = GetProductNumberFromRow(rowIndex);//GetValueFromGridAsString(rowIndex - 5, 0);
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
                        foreach (PrognosInfoSales item in Products)
                        {
                            if (item.ProductNumber.ToString() == productNumber)
                            {
                              //here a new budget_line post should be added to the database
                                int ammount = Convert.ToInt32(e.FormattedValue) - item.Salgsbudget_ThisYear[week];
                                if (ammount != 0)
                                {
                                   
                                    string tempCustNumber = custDictionary[comboBoxAssortment.Text];
                                    //string startDato = "datum";

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
                                    string tempAssortment = comboBoxAssortment.Text;
                                    System.Threading.ThreadPool.QueueUserWorkItem(delegate
                                    {
                                        NavSQLExecute conn = new NavSQLExecute();
                                        conn.InsertBudgetLine(tempCustNumber, tempAssortment, productNumber, answer.ToString(format), ammount);
                                    }, null);
                                    
                                }
                            }
                        }
                    }
                }
            }
        }

        //This function returns last number in a string
        private string GetLastNumber(string testString)
        {
            var matches = Regex.Matches(testString, @"\d+");
            if (matches.Count < 1)
            {
                return "No Number";
            }
            else
            {
                var lastMatch = matches[matches.Count - 1];
                var number = Int32.Parse(lastMatch.Value);
                //return testString.Remove(lastMatch.Index, lastMatch.Length).Insert(lastMatch.Index, number.ToString());
                return number.ToString();
            }
        }

        //returns the prooduct that the row is connected to
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


        //Handles when user click in the grid. Different actions for different Cells. 
        private void dataGridForecastInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string tempSender = sender.ToString();
            string TempE = e.ToString();
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            selectedYear = (int)comboBoxYear.SelectedItem;
            

            latestMouseClick = System.Windows.Forms.Cursor.Position;
            Console.WriteLine("Value clicked... Column index: " + columnIndex + "  rowIndex: " + rowIndex);
            if (columnIndex > 2)
            {
                if (GetValueFromGridAsString(rowIndex, 2).Contains("Comment"))
                {
                    if (!infoboxSales.Visible)
                    {
                        string temp = GetValueFromGridAsString(rowIndex, columnIndex);
                        //string temp2 = GetValueFromGridAsString(rowIndex - 6, 0);


                        latestWeek = columnIndex - 2;
                        latestProductNumber = GetProductNumberFromRow(rowIndex);
                        infoboxSales.SetInfoText(this, "", " Product: " + latestProductNumber + " Week: " + latestWeek);
                        infoboxSales.SetOldInfo(temp);
                        infoboxSales.TopMost = true;

                        //First time it is showed needs special handling
                        if (infoboxSales.desiredStartLocationX == 0 && infoboxSales.desiredStartLocationY == 0)
                        {
                            infoboxSales.SetNextLocation(latestMouseClick);
                        }
                        else
                        {
                            infoboxSales.Location = latestMouseClick;
                        }
                        infoboxSales.Show();
                        infoboxSales.SaveButtonVisible(true);
                        infoboxSales.FocusTextBox();
                    }
                    else
                    {
                        MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before open a new one!");
                    }
                }
                else if (GetValueFromGridAsString(rowIndex, 2) == "Kampagn_ThisYear")
                {
                    if (!infoboxSales.Visible)
                    {
                        MessageBox.Show(GetProductNumberFromRow(rowIndex) + "\n\nKampagnen innefattar kund 1 och kund 2.");
                    }
                }
                else if (GetValueFromGridAsString(rowIndex, 2).Contains("History"))
                {
                    if (!infoboxSales.Visible)
                    {
                        string temp2 = GetProductNumberFromRow(rowIndex);
                        int productNumber = Convert.ToInt32(temp2);

                        int latestWeek = columnIndex - 2;
                        //PrognosInfoForSupply tempInfo = GetProductInfoByNumber(productNumber);
                        //Console.WriteLine(" Product: " + temp2 + " Week: " + latestWeek);

                        //Create dummy value to show something before real numbers

                        string temp = "";

                        SQLCallsSalesCustomerInfo sqlConnection = new SQLCallsSalesCustomerInfo();
                        sqlConnection.SetYear(selectedYear);
                        string tempCustNumber = custDictionary[comboBoxAssortment.Text];
                        temp = sqlConnection.GetSalesBudgetWeekInfo(latestWeek, productNumber, tempCustNumber);

                        temp = "Salgsbudget" + " Product: " + temp2 + " Week: " + latestWeek + "\n\n" + temp;
                        MessageBox.Show(temp);
                    }
                    else
                    {
                        MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window to see info!");
                    }
                }
            }
        }

        //Return the value of a grid cell as string
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
                Console.WriteLine("Sales, Value from grid out of bounds Row: " + row + " Col: " + col);
            }
            return returnValue;
        }



        public void SetKöpsbudget(int week, string productNumber, int value)
        {
            foreach (PrognosInfoSales item in Products)
            {
                if (item.ProductNumber.ToString() == productNumber)
                {
                    item.Salgsbudget_ThisYear[week] = Convert.ToInt32(value);
                }
            }
        }

        private Dictionary<string, string> GetCustomers()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            SQLCallsSalesCustomerInfo newSQL = new SQLCallsSalesCustomerInfo();
            Dictionary<string, string> allCustomers = newSQL.GetAllCustomers();

            stopwatch.Stop();
            double timeConnectSeconds = stopwatch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Load all Customers Sales! Time (s): " + timeConnectSeconds);
            Console.WriteLine("Number of Customers: " + allCustomers.Count);

            foreach (KeyValuePair<string, string> kvp in allCustomers)
            {
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }

            return allCustomers;
        }


        private Dictionary<string, string> GetCustomersWitohutLoad()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Dictionary<string, string> allCustomers = new Dictionary<string, string>();
            allCustomers.Add("0", " ");
            allCustomers.Add("904", "914-KristianstadsOstföräd.EUR");
            allCustomers.Add("981", "ArlaNorge");
            allCustomers.Add("980", "ASKOLactalisNorge");
            allCustomers.Add("975", "AxFood");
            allCustomers.Add("986", "AxfooddirektebutiksLev.");
            allCustomers.Add("976", "Bergendahls");
            allCustomers.Add("60", "BESTWESTERN");
            allCustomers.Add("56", "CHEVALBLANCKANTINER");
            allCustomers.Add("67", "CHOISEHOTELS");
            allCustomers.Add("977", "CityGross");
            allCustomers.Add("52", "COOPCENTRALLAGER");
            allCustomers.Add("984", "Coopdirekteleverancer");
            allCustomers.Add("974", "COOPSverige");
            allCustomers.Add("41", "COORKANTINER");
            allCustomers.Add("47", "DANSKCATER");
            allCustomers.Add("68", "DANSKKROFERIE");
            allCustomers.Add("63", "DANSKEKONF.CENT.BOOK.SERVICE");
            allCustomers.Add("90", "Div.Finland");
            allCustomers.Add("51", "DIVERSE");
            allCustomers.Add("15", "DSCENTRAL-LAGER");
            allCustomers.Add("989", "Gekås");
            allCustomers.Add("26", "GENNEMFAKTVIASUPERGROS");
            allCustomers.Add("58", "grossist");
            allCustomers.Add("19", "HKI(tidlS-engros)");
            allCustomers.Add("65", "HOTELLER");
            allCustomers.Add("45", "HØRKRAM");
            allCustomers.Add("973", "ICA");
            allCustomers.Add("983", "ICAbutikspakketCentraltLev.");
            allCustomers.Add("982", "ICANorge");
            allCustomers.Add("71", "INCOCaterKBHgennemfak");
            allCustomers.Add("73", "INCOgennemfakMeyer");
            allCustomers.Add("92", "INEXFinland");
            allCustomers.Add("6", "Intervare");
            allCustomers.Add("IRMA", "IRMA");
            allCustomers.Add("101", "Island");
            allCustomers.Add("44", "KANTINER");
            allCustomers.Add("96", "KESKO");
            allCustomers.Add("43", "KURSUSEJD.");
            allCustomers.Add("34", "METRO");
            allCustomers.Add("4", "NETTO");
            allCustomers.Add("969", "NettoSverigeAB");
            allCustomers.Add("42", "OSTEHANDEL");
            allCustomers.Add("32", "OstehandlereSverige");
            allCustomers.Add("103", "Polen");
            allCustomers.Add("28", "REITAN");
            allCustomers.Add("988", "RemaNorge");
            allCustomers.Add("40", "RESTAURANTER");
            allCustomers.Add("979", "SalgsbilskunderiSverige");
            allCustomers.Add("10", "SALLING");
            allCustomers.Add("69", "SASRADDISON");
            allCustomers.Add("76", "Sodexo");
            allCustomers.Add("23", "SUPERGROSCENTRALLAGER");
            allCustomers.Add("967", "SvenskeKunde");
            allCustomers.Add("102", "Tyskland");
            allCustomers.Add("27", "ØVRIGEDANSKCATER-GENNEMFAK");
            allCustomers.Add("94", "Øvrigeexport(grønland)");
            allCustomers.Add("93", "ØvrigeFinland");

            Dictionary<string, string> allCustomersSwitched = new Dictionary<string, string>();
            foreach (KeyValuePair<string,string> item in allCustomers)
            {
                allCustomersSwitched.Add(item.Value, item.Key);
            }


            return allCustomersSwitched;
        }

        private void comboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedYear = (int)comboBoxYear.SelectedItem;
        }

        private void buttonGetProductByNumber_Click(object sender, EventArgs e)
        {
            dataGridForecastInfo.Visible = false;
            labelStatus.Visible = true;
            SetStatus("Product Loading"); 
            loadingNewProductsOngoing = true;
            SetupColumns();
            Products = new List<PrognosInfoSales>();
            this.dataGridForecastInfo.DataSource = null;

            this.dataGridForecastInfo.Rows.Clear();
            this.dataGridForecastInfo.AllowUserToAddRows = false;
            this.dataGridForecastInfo.AllowUserToDeleteRows = false;
            this.dataGridForecastInfo.AllowUserToOrderColumns = false;


            int prodNMBR = (int)numericUpDownPRoductNumber.Value;
            string tempCustNumber = custDictionary[comboBoxAssortment.Text];
            string temp = "Name Unknown";
            if(allProductsDict.ContainsKey(prodNMBR))
            {
                temp = allProductsDict[prodNMBR];
            }

            PrognosInfoSales product1 = new PrognosInfoSales(temp, prodNMBR, tempCustNumber);
            product1.FillNumbers(selectedYear);
            Products.Add(product1);
            FillSalesGUIInfo();
            LoadingReady();
        }

        private void LoadingReady()
        {
            loadingNewProductsOngoing = false;
            dataGridForecastInfo.Visible = true;
            labelStatus.Visible = false;
            buttonSupplyView.Enabled = true;
            comboBoxYear.Enabled = true;
        }

        public void LoadAllProductDict()
        {
            NavSQLExecute conn = new NavSQLExecute();
            DataTable tempTable = new DataTable();
            string queryString = @"select Varenr, Dato, Antal_Budget, Beskrivelse  from Kampagnelinier_Fordelt where Dato >= '2014/12/29' and Dato < '2016/01/05' order by varenr";
            Console.WriteLine("LoadAllProductsDict Query: ");
            tempTable = conn.QueryExWithTableReturn(queryString);
            conn.Close();
            DataRow[] currentRows = tempTable.Select(null, null, DataViewRowState.CurrentRows);
            currentRows = tempTable.Select(null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
            {
                Console.WriteLine("No Kopsorder Line Found");
            }
            else
            {
                //loop trough all rows and write in tabs
                foreach (DataRow row in currentRows)
                {
                    string prodNBR__ = row["Varenr"].ToString();
                    int nbr = Convert.ToInt32(prodNBR__);

                    string beskriv = row["Beskrivelse"].ToString();
                    if (!allProductsDict.ContainsKey(nbr))
                    {
                        allProductsDict.Add(nbr, beskriv);
                    }
                }
                Console.WriteLine(allProductsDict.ToString());
            }

        }

        private void labelStatus_Click(object sender, EventArgs e)
        {

        }



    }
}
