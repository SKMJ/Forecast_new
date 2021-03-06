﻿///This is the GUI File for the Sales view 
///This GUI runs all action for Sales to focus on
///Download numbers and save ForeCasts based on customers

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
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
        private string latestCustomerLoaded;
        private int latestWeek;
        Point latestMouseClick;
        int selectedYear;
        bool AssortmentFromM3 = true;
        ClassStaticVaribles.WritePermission OnlyLook;
        //Dictionary<string, string> custDictionary;

        private bool loadingNewProductsOngoing;

        public Dictionary<string, string> allProductsDict = new Dictionary<string, string>();

        public Dictionary<string, string> allProductsM3Dict = new Dictionary<string, string>();
        Dictionary<string, List<string>> allSuppliersM3 = new Dictionary<string, List<string>>();
        public Dictionary<string, string> newNumberDict = new Dictionary<string, string>();

        Dictionary<string, List<string>> allkedjaToCustM3 = new Dictionary<string, List<string>>();
        int latestCommentRow;
        string newCommentProdNBR = "";


        //Get_FromSimulatedM3 m3_info = new Get_FromSimulatedM3();
        public Form1()
        {
            var loginForm = new FormLogin();

            AssortmentFromM3 = true;
            InitializeComponent();
            Console.WriteLine("Start Form1!");


            loadingNewProductsOngoing = false;
            //test with Coop and test customer
            FixCustomerChoices();

            m3Info.TestM3Connection();
            SetupColumns();
            ClassStaticVaribles.SetNewNumberDict();
            allProductsM3Dict = m3Info.GetAllSkaevingeProductsDict();
            allSuppliersM3 = m3Info.GetSupplierWithItemsDict();

            LoadAllProductDict();


            dataGridForecastInfo.Width = this.Width - 50;
            dataGridForecastInfo.Height = this.Height - 250;
            ClassStaticVaribles.SetAllProductsNavDict(allProductsDict);
            ClassStaticVaribles.InitiateDate();
        }

        private void frm_sizeChanged(object sender, EventArgs e)
        {

        }


        //Populate the drop downlist wit customers
        private void FixCustomerChoices()
        {
            //Dictionary<string, string> cust = GetCustomers();
            ClassStaticVaribles.SetCustDictionary();
            //GetCustomersWitohutLoad();

            comboBoxAssortment.DropDownStyle = ComboBoxStyle.DropDownList;
            if (!AssortmentFromM3)
            {
                comboBoxAssortment.DataSource = new BindingSource(ClassStaticVaribles.AssortmentDictionaryNav, null);
                comboBoxAssortment.DisplayMember = "Key";
                comboBoxAssortment.ValueMember = "Key";
            }
            else
            {
                Dictionary<string, string> assormentList = m3Info.GetListOfAssortments();
                comboBoxAssortment.DataSource = new BindingSource(assormentList, null);
                //m3Info.GetDictOfKedjaToCustNBR();

                //foreach(KeyValuePair <string, List<string>> item in ClassStaticVaribles.Kedjor_toCUNO)
                //{
                //    Console.WriteLine("kedja: " + item.Key);
                //    foreach(string cunoitem in item.Value)
                //    {
                //        Console.WriteLine("kund: " + cunoitem);
                //    }
                //}


                comboBoxAssortment.DisplayMember = "Value";
                comboBoxAssortment.ValueMember = "Key";


            }


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

                foreach (DataGridViewColumn item in dataGridForecastInfo.Columns)
                {
                    item.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                }
            }
        }


        //Fill the grid with info from the Products
        public void FillSalesGUIInfo()
        {
            Dictionary<int, string> commentDict = new Dictionary<int, string>();
            this.dataGridForecastInfo.DataSource = null;

            this.dataGridForecastInfo.Rows.Clear();

            this.dataGridForecastInfo.AllowUserToAddRows = false;
            this.dataGridForecastInfo.AllowUserToDeleteRows = false;
            this.dataGridForecastInfo.AllowUserToOrderColumns = false;

            Dictionary<int, int> weekToLock = new Dictionary<int, int>();
            int weekProdNBR = 0;
            Products.Sort();
            foreach (PrognosInfoSales item in Products)
            {
                weekToLock.Add(weekProdNBR, item.WeekToLockFrom + 2); //+ 2 is offset from cell number to week number
                weekProdNBR++;
                List<object> tempList = new List<object>();
                if (ClassStaticVaribles.NewNumberDictNavKey.ContainsKey(item.ProductNumber.ToString()))
                {
                    tempList.Add(ClassStaticVaribles.NewNumberDictNavKey[item.ProductNumber.ToString()]);
                }
                else
                {
                    tempList.Add(item.ProductNumber.ToString());
                }
                tempList.Add(item.ProductName);


                tempList.Add("RealiseretSalg_LastYear");
                for (int i = 1; i < 54; i++)
                {
                    tempList.Add(item.RealiseretSalgs_LastYear[i]);
                }
                AddRowFromList(tempList);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("RealiseretKampagn_LastYear");
                for (int i = 1; i < 54; i++)
                {
                    tempList.Add(item.RealiseretKampagn_LastYear[i]);
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

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("Salgsbudget_Comment");
                for (int i = 1; i < 54; i++)
                {
                    tempList.Add("");
                }
                AddRowFromList(tempList);

                commentDict = item.Salgsbudget_Comment;
                DataGridViewRow tempRow = dataGridForecastInfo.Rows[dataGridForecastInfo.Rows.Count - 1];
                for (int i = 3; i < tempRow.Cells.Count; i++)
                {
                    string tempComment = commentDict[i - 2];
                    if (tempComment.Length > 1 &&
                        (!tempComment.EndsWith(" Comment:  ")))
                    {

                        tempRow.Cells[i].Style = new DataGridViewCellStyle { BackColor = Color.Yellow };
                    }
                }

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("Salgsbudget_ChangeHistory");
                for (int i = 1; i < 54; i++)
                {
                    tempList.Add(item.Salgsbudget_ChangeHistory[i]);
                }
                AddRowFromList(tempList);
                commentDict = item.Salgsbudget_Comment;
            }
            //Thread.Sleep(2000);

            weekProdNBR = 0;
            //After all is filled set colors and readonly properties
            foreach (DataGridViewRow row in dataGridForecastInfo.Rows)
            {
                row.Height = 20;
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
                    row.Cells[2].ReadOnly = true;
                    row.Cells[2].Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                    for (int i = 3; i < row.Cells.Count; i++)
                    {
                        if (weekToLock[weekProdNBR] < i && (OnlyLook == ClassStaticVaribles.WritePermission.SaleWrite || OnlyLook == ClassStaticVaribles.WritePermission.Write))
                        {

                            row.Cells[i].ReadOnly = false;
                            row.Cells[i].Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };


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

        public string GetNameFromLoadedProducts(string prodNBR)
        {
            string temp = "Name Unknown";
            //First try to see if you find the name in M3 on Warehouse
            //If not try with old Forecastdatabase, if not found Name Unknown
            if (ClassStaticVaribles.AllProductsM3Dict.ContainsKey(prodNBR))
            {
                temp = ClassStaticVaribles.AllProductsM3Dict[prodNBR];
            }
            else if (ClassStaticVaribles.AllProductsNavDict.ContainsKey(prodNBR))
            {
                temp = ClassStaticVaribles.AllProductsNavDict[prodNBR];
            }
            return temp;
        }

        //Add testinfo Products only for test purposes
        private void CreateProducts()
        {
            string tempCustNumber = ClassStaticVaribles.AssortmentDictionaryNav[comboBoxAssortment.Text];
            PrognosInfoSales product1 = new PrognosInfoSales(GetNameFromLoadedProducts("2432"), "2432", tempCustNumber);
            PrognosInfoSales product2 = new PrognosInfoSales(GetNameFromLoadedProducts("1442"), "1442", tempCustNumber);
            PrognosInfoSales product3 = new PrognosInfoSales(GetNameFromLoadedProducts("1238"), "1238", tempCustNumber);
            PrognosInfoSales product4 = new PrognosInfoSales(GetNameFromLoadedProducts("2442"), "2442", tempCustNumber);
            PrognosInfoSales product5 = new PrognosInfoSales(GetNameFromLoadedProducts("2735"), "2735", tempCustNumber);

            latestCustomerLoaded = tempCustNumber;
            SetStatus("Products loading 1/5");
            // MessageBox.Show("Place 1");
            product1.FillNumbers(selectedYear);
            // MessageBox.Show("Place 2");
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
                if (OnlyLook == ClassStaticVaribles.WritePermission.SupplWrite || OnlyLook == ClassStaticVaribles.WritePermission.Write)
                {
                    supplyViewInstance.SetOnlyLook(false);
                }
                else
                {
                    supplyViewInstance.SetOnlyLook(true);
                }

                this.Visible = false;
                supplyViewInstance.SetForm1Instanse(this);
                supplyViewInstance.BringToFront();
                supplyViewInstance.Show();
                //supplyViewInstance.SetAllProdDict(allProductsDict);
                //supplyViewInstance.SetAllProdM3Dict(allProductsM3Dict);
                //supplyViewInstance.SetAllSupplDict(allSuppliersM3);
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
            selectedYear = (int)comboBoxYear.SelectedItem;

            labelStatus.Visible = true;
            dataGridForecastInfo.Visible = false;
            SetStatus("Loading Products");

            loadingNewProductsOngoing = true;
            Stopwatch stopwatch = Stopwatch.StartNew();


            SetupColumns();

            if (!infoboxSales.Visible)
            {
                if (!AssortmentFromM3)//comboBoxAssortment.SelectedItem.ToString() != "COOP")
                {
                    Console.WriteLine("Hittepåkund vald");
                    Products = new List<PrognosInfoSales>();
                    CreateProducts();

                    FillSalesGUIInfo();
                }
                else
                {

                    //Console.WriteLine("");

                    // KeyValuePair<string, string> tempPair = (KeyValuePair<string, string>)comboBoxAssortment.SelectedItem;
                    string tempString = comboBoxAssortment.SelectedItem.ToString();
                    tempString = tempString.Substring(1);
                    string[] firstPart = tempString.Split(',');
                    tempString = firstPart[0];
                    Products = new List<PrognosInfoSales>();
                    List<string> productList = m3Info.GetListOfProductsNbrByAssortment(tempString);
                    latestCustomerLoaded = tempString;

                    if (productList != null)
                    {
                        int nbrItems = productList.Count;
                        int i = 0;
                        foreach (string item in productList)
                        {
                            i++;
                            string temp = GetNameFromLoadedProducts(item.ToString());
                            PrognosInfoSales product1 = new PrognosInfoSales(temp, item, tempString);
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
            dataGridForecastInfo.Rows[latestCommentRow].Cells[latestWeek + 2].Value = comment;
            //foreach (PrognosInfoSales item in Products)
            //{
            //    if (item.ProductNumber.ToString() == latestProductNumber)
            //    {
            //        item.Salgsbudget_Comment[latestWeek] = comment;
            //        //TODO: Write to database
            //    }
            //}
            //FillSalesGUIInfo();
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

                                    string tempCustNumber = ClassStaticVaribles.GetCustNavCodeFirst(latestCustomerLoaded); ;
                                    //string startDato = "datum";


                                    DateTime tempDate = DateTime.Parse(ClassStaticVaribles.StartDate[selectedYear].ToString());
                                    DateTime answer = tempDate.AddDays((week - 1) * 7);
                                    string format = "yyyy-MM-dd HH:mm:ss";    // modify the format depending upon input required in the column in database 
                                    string tempAssortment = comboBoxAssortment.Text;
                                    DateTime time = DateTime.Now;              // Use current time
                                    //string format = "yyyy-MM-dd HH:MM:ss";    // modify the format depending upon input required in the column in database 

                                    Console.WriteLine("TasteDato: " + time.ToString(format));
                                    string inputNow = time.ToString(format);
                                    string tempComment = GetValueFromGridAsString(rowIndex + 1, columnIndex);
                                    dataGridForecastInfo.Rows[rowIndex + 1].Cells[columnIndex].Value = "";
                                    if (tempComment.Length <= 1)
                                    {
                                        System.Threading.ThreadPool.QueueUserWorkItem(delegate
                                        {
                                            NavSQLExecute conn = new NavSQLExecute();
                                            conn.InsertBudgetLine(tempCustNumber, tempAssortment, productNumber, answer.ToString(format), ammount, inputNow, "");
                                        }, null);
                                    }
                                    else
                                    {
                                        newCommentProdNBR = productNumber;
                                        NavSQLExecute conn = new NavSQLExecute();
                                        conn.InsertBudgetLine(tempCustNumber, tempAssortment, productNumber, answer.ToString(format), ammount, inputNow, tempComment);
                                    }

                                    SetKöpsbudget(week, productNumber, Convert.ToInt32(e.FormattedValue));

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

            if (ClassStaticVaribles.NewNumberDictM3Key.ContainsKey(returnString))
            {
                returnString = ClassStaticVaribles.NewNumberDictM3Key[returnString];
            }
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
                        latestCommentRow = rowIndex;
                        string temp = GetValueFromGridAsString(rowIndex, columnIndex);

                        //string temp2 = GetValueFromGridAsString(rowIndex - 6, 0);


                        latestWeek = columnIndex - 2;
                        latestProductNumber = GetProductNumberFromRow(rowIndex);

                        foreach (PrognosInfoSales item in Products)
                        {
                            if (item.ProductNumber.ToString() == latestProductNumber)
                            {
                                temp = item.Salgsbudget_Comment[latestWeek];
                            }
                        }
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
                        //MessageBox.Show(GetProductNumberFromRow(rowIndex) + "\n\nKampagnen innefattar kund 1 och kund 2.");
                    }
                }
                else if (GetValueFromGridAsString(rowIndex, 2).Contains("History"))
                {
                    if (!infoboxSales.Visible)
                    {
                        string prodNBRTemp = GetProductNumberFromRow(rowIndex);
                        //int productNumber = Convert.ToInt32(prodNBRTemp);
                        string tempNewName = prodNBRTemp;
                        if (ClassStaticVaribles.NewNumberDictNavKey.ContainsKey(prodNBRTemp))
                        {
                            tempNewName = tempNewName + " New Number (" + ClassStaticVaribles.NewNumberDictNavKey[prodNBRTemp] + ")";
                        }

                        int latestWeek = columnIndex - 2;
                        //PrognosInfoForSupply tempInfo = GetProductInfoByNumber(productNumber);
                        //Console.WriteLine(" Product: " + temp2 + " Week: " + latestWeek);

                        //Create dummy value to show something before real numbers

                        string temp = "";

                        SQLCallsSalesCustomerInfo sqlConnection = new SQLCallsSalesCustomerInfo();
                        sqlConnection.SetYear(selectedYear);
                        //string tempCustNumber = ClassStaticVaribles.CustDictionary[comboBoxAssortment.Text];
                        temp = sqlConnection.GetSalesBudgetWeekInfo(latestWeek, prodNBRTemp, ClassStaticVaribles.GetCustNavCodes(latestCustomerLoaded));

                        temp = "Salgsbudget" + " Product: " + tempNewName + " Week: " + latestWeek + "\n\n" + temp;
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


            string prodNMBR = textBoxProdNBR.Text;
            if (ClassStaticVaribles.NewNumberDictM3Key.ContainsKey(prodNMBR))
            {
                prodNMBR = ClassStaticVaribles.NewNumberDictM3Key[prodNMBR];
            }

            string tempString = comboBoxAssortment.SelectedItem.ToString();
            tempString = tempString.Substring(1);
            string[] firstPart = tempString.Split(',');
            tempString = firstPart[0];

            string tempCustNumber = tempString;
            string temp = GetNameFromLoadedProducts(prodNMBR);


            PrognosInfoSales product1 = new PrognosInfoSales(temp, prodNMBR, tempCustNumber);
            latestCustomerLoaded = tempCustNumber;
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
            string query = @"select Varenr, Dato, Antal_Budget, Beskrivelse  from Kampagnelinier_Fordelt where Dato >= '2014/12/29' and Dato < '2016/01/05' order by varenr";
            Console.WriteLine("LoadAllProductsDict Query: ");
            tempTable = conn.QueryExWithTableReturn(query);
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
                    string prodNBR_ = row["Varenr"].ToString();
                    int nbr = Convert.ToInt32(prodNBR_);

                    string beskriv = row["Beskrivelse"].ToString();
                    if (!allProductsDict.ContainsKey(prodNBR_))
                    {
                        allProductsDict.Add(prodNBR_, beskriv);
                    }
                }
                Console.WriteLine(allProductsDict.ToString());
            }

        }

        private void labelStatus_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDownPRoductNumber_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBoxProdNBR_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridForecastInfo_ColumnDividerWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {

        }

        private void dataGridForecastInfo_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column.Index > 2)
            {
                int temp = e.Column.Width;
                for (int i = 3; i < dataGridForecastInfo.Columns.Count; i++)
                {
                    dataGridForecastInfo.Columns[i].Width = temp;
                }
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            dataGridForecastInfo.Width = this.Width - 50;
            dataGridForecastInfo.Height = this.Height - 250;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //var loginForm = new FormLogin();
            //loginForm.ShowDialog();


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("Close Sales form");
            //Application.Exit();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void dataGridForecastInfo_KeyUp(object sender, KeyEventArgs e)
        {
            if (newCommentProdNBR.Length > 0)
            {
                LoadSalesCommentAgain(newCommentProdNBR);
                newCommentProdNBR = "";
            }
        }


        private void LoadSalesCommentAgain(string newCommentProdNBR)
        {
            dataGridForecastInfo.Visible = false;
            labelStatus.Visible = true;
            SetStatus("Product Loading");
            loadingNewProductsOngoing = true;
            //SupplierProducts[prodNBR].UpdateReguleretInfo(selectedYear);

            foreach (PrognosInfoSales prognos in Products)
            {
                if (prognos.ProductNumber == newCommentProdNBR)
                {
                    prognos.UpdateSalesInfo(selectedYear);
                }
            }

            //throw new NotImplementedException();
            LoadingReady();
        }




        internal void SetOnlyLook(ClassStaticVaribles.WritePermission permission)
        {
            OnlyLook = permission;
        }

        internal void SetProdOrTestHeading(bool prod)
        {
            if(prod)
            {
                this.Text = "Forecast Sales Production Environment  "+ Application.ProductVersion;
            }
            else
            {
                this.Text = "Forecast Sales Test Environment  " + Application.ProductVersion; ;
                this.BackColor = Color.WhiteSmoke;
            }

        }
    }
}
