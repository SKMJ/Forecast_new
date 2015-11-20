///This is the GUI File for the Supply view 
///This GUI runs all action for Supply to focus on
///Shows all ForecastNumbers for all the suppliers
///The numbers is summed up from all the Customer Forecasts


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using WindowsFormsForecastLactalis;

namespace WindowsFormsForecastLactalis
{
    public partial class FormSupply : Form
    {
        List<string> dayOrderStrings = new List<string>();

        string newRegCommentProdNBR = "";

        public static Dictionary<string, PrognosInfoForSupply> SupplierProducts = new Dictionary<string, PrognosInfoForSupply>();
        Form1 form1Instance;// = new Form1();
        //private List<string> supplier = new List<string>();
        GetFromM3 m3Info = new GetFromM3();
        TextBoxForm infoboxSupply = new TextBoxForm();
        public Dictionary<int, List<int>> MotherSupplierWithChilds = new Dictionary<int, List<int>>();

        public Dictionary<string, string> OrderCodes = new Dictionary<string, string>();
        //public Dictionary<string, string> AllProductsDict_Supply = new Dictionary<string, string>();

        //public Dictionary<string, string> AllProductsM3Dict_Supply = new Dictionary<string, string>();
        //Dictionary<string, List<string>> AllSupplierM3Dict_Supply = new Dictionary<string, List<string>>();


        int desiredStartLocationX;
        int desiredStartLocationY;
        int latestWeek;
        string latestProductNumber;
        Point latestMouseClick;
        int latestRow;
        string latestSelectedYear;
        int selectedYear;
        bool OnlyLook = true;

        public FormSupply()
        {
            InitializeComponent();
            Console.WriteLine("Start FormSupply!");

            //FixMotherChildList();

            SetupColumns();
            FixSupplierChoices();
            dataGridForecastInfo.Width = this.Width - 50;
            dataGridForecastInfo.Height = this.Height - 250;
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

            foreach (KeyValuePair<string, List<string>> item in ClassStaticVaribles.AllSuppliersM3)//AllSupplierM3Dict_Supply)
            {
                if (ClassStaticVaribles.AllSuppliersNameDict.ContainsKey(item.Key))
                {
                    supplier.Add(item.Key + "  " + ClassStaticVaribles.AllSuppliersNameDict[item.Key] + "  " + ClassStaticVaribles.AllSuppliersM3[item.Key].Count);
                }
                else
                {
                    supplier.Add(item.Key + "  " + ClassStaticVaribles.AllSuppliersM3[item.Key].Count);
                }

            }


            supplier.Sort();


            comboBoxSupplier.DataSource = supplier;
            comboBoxSupplier.DropDownStyle = ComboBoxStyle.DropDownList;

            List<int> yearList = new List<int>();
            yearList.Add(2015);
            yearList.Add(2016);
            yearList.Add(2017);

            comboBoxYear.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxYear.DataSource = new BindingSource(yearList, null);
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
                    string temp = "";
                    if (comboBoxYear.Text.Length > 0)
                    {
                        temp = i + "." + comboBoxYear.Text;
                    }
                    else
                    {
                        temp = i + "." + 2015;
                    }
                    dataGridForecastInfo.Columns[i + 2].Name = temp;
                }

                foreach (DataGridViewColumn item in dataGridForecastInfo.Columns)
                {
                    item.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                }
            }
        }

        //Fill the UI info
        private void FillInfo()
        {
            SupplierProducts = new Dictionary<string, PrognosInfoForSupply>();
            this.dataGridForecastInfo.DataSource = null;

            this.dataGridForecastInfo.Rows.Clear();
            this.dataGridForecastInfo.AllowUserToAddRows = false;
            this.dataGridForecastInfo.AllowUserToDeleteRows = false;
            this.dataGridForecastInfo.AllowUserToOrderColumns = false;


            int n;
            string suppliernumberM3 = GetProdNBRFromSelectedItem(GetProdNBRFromSelectedItem(comboBoxSupplier.SelectedItem.ToString()));
            if (int.TryParse(suppliernumberM3, out n))
            {
                numericSupplyNBR.Value = Convert.ToDecimal(suppliernumberM3);
            }
            List<string> tempList = ClassStaticVaribles.AllSuppliersM3[suppliernumberM3];

            // List<int> tempList2 = GetListOfProductsWithSupplierLactaFrance();



            int numberOfProducts = tempList.Count;
            tempList.Sort();

            int i = 0;
            foreach (string item in tempList)
            {
                i++;

                //int tempInt = 0;
                //bool result = int.TryParse(item, out tempInt); //i now = 108
                //if (result)
                //{
                SetStatus("Products loading " + i.ToString() + "/" + numberOfProducts.ToString());
                //ClassStaticVaribles.AllProductsM3Dict
                string tempName = " ";
                if (ClassStaticVaribles.AllProductsM3Dict.ContainsKey(item))
                {

                    tempName = ClassStaticVaribles.AllProductsM3Dict[item];//m3Info.GetItemNameByItemNumber(item.ToString());
                }
                PrognosInfoForSupply product1 = new PrognosInfoForSupply(tempName, item, checkBoxLastYear.Checked);

                product1.FillNumbers(selectedYear);

                if (!SupplierProducts.ContainsKey(item))
                {
                    SupplierProducts.Add(item, product1);
                }

                //}
                //SupplierProductsFromM3.Add(product1);
                //Console.WriteLine(" Supplier produkt!! " + item + "  Name: " + tempName);
            }


            SetStatus("Products Preparing For User Interface. Will soon be ready to view.");
            PrepareGUI();
        }


        private string GetProdNBRFromSelectedItem(string p)
        {
            string[] temp = p.Split(' ');
            return temp[0];
        }


        //Load GUI with info from SupplierProducts
        private void PrepareGUI()
        {
            Random randomNumber = new Random();
            Dictionary<int, string> commentDict = new Dictionary<int, string>();

            var list = SupplierProducts.Keys.ToList();
            list.Sort();
            List<string> NewTempListWithM3Numbers = new List<string>();

            foreach (string item in list)
            {
                if (ClassStaticVaribles.NewNumberDictNavKey.ContainsKey(item))
                {
                    NewTempListWithM3Numbers.Add(ClassStaticVaribles.NewNumberDictNavKey[item]);
                }
                else
                {
                    NewTempListWithM3Numbers.Add(item);
                }
            }

            NewTempListWithM3Numbers.Sort();


            // Loop through keys.
            foreach (string key in NewTempListWithM3Numbers)
            {

                List<object> tempList;
                string tempKey = key;
                if (ClassStaticVaribles.NewNumberDictM3Key.ContainsKey(tempKey))
                {
                    tempKey = ClassStaticVaribles.NewNumberDictM3Key[tempKey];
                }

                PrognosInfoForSupply item = SupplierProducts[tempKey];

                tempList = new List<object>();
                if (ClassStaticVaribles.NewNumberDictNavKey.ContainsKey(item.ProductNumber.ToString()))
                {
                    tempList.Add(ClassStaticVaribles.NewNumberDictNavKey[item.ProductNumber.ToString()]);
                }
                else
                {
                    tempList.Add(item.ProductNumber.ToString());
                }
                tempList.Add(item.ProductName);
                if (checkBoxLastYear.Checked)
                {
                    tempList.Add("RealiseretSalg_LastYear");
                    for (int i = 1; i < 54; i++)
                    {
                        tempList.Add(item.RealiseretSalg_LastYear[i]);
                    }
                }
                AddRowFromList(tempList);


                int number = randomNumber.Next(1, 1000);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("På lager: ");
                if (checkBoxLastYear.Checked)
                {
                    tempList.Add("RealiseretKampagn_LastYear");
                    for (int i = 1; i < 54; i++)
                    {
                        tempList.Add(item.RealiseretKampagn_LastYear[i]);
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
                commentDict = item.SalgsbudgetReguleret_Comment;
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
                else if (Convert.ToString(row.Cells[2].Value) == "RealiseratSalg_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value).Length < 1)
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
                   
                    row.ReadOnly = OnlyLook;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Köpsbudget_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
                    row.ReadOnly = OnlyLook;
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
                    col.ReadOnly = true;
                }
                colNBR++;
            }
        }


        //Add testinfo Products
        private void CreateSupplyProducts(bool even)
        {
            SupplierProducts = new Dictionary<string, PrognosInfoForSupply>();
            if (even)
            {

                AddProductByNumber("2432");
                SetStatus("Loading products 1/2");
                AddProductByNumber("1442");
                SetStatus("Loading products 2/2");

            }
            else
            {
                SetStatus("Loading products 1/3");
                AddProductByNumber("1443");
                SetStatus("Loading products 2/3");
                AddProductByNumber("1447");
                SetStatus("Loading products 3/3");
                AddProductByNumber("2239");
            }
        }


        private void AddProductByNumber(string number)
        {
            string tempNumber = number.ToString();
            string prodName = "Unknown Name";
            //ClassStaticVaribles.AllProductsM3Dict
            if (ClassStaticVaribles.AllProductsNavDict.ContainsKey(tempNumber))
            {
                prodName = ClassStaticVaribles.AllProductsNavDict[tempNumber];
            }
            else if (ClassStaticVaribles.AllProductsM3Dict.ContainsKey(tempNumber))
            {
                prodName = ClassStaticVaribles.AllProductsM3Dict[tempNumber];
            }
            PrognosInfoForSupply product1 = new PrognosInfoForSupply(prodName, tempNumber, checkBoxLastYear.Checked);
            product1.FillNumbers(selectedYear);
            SupplierProducts.Add(tempNumber, product1);
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
            buttonGetProductByNumber.Enabled = false;
            buttonGetProductsBySupplier.Enabled = false;
            buttonGetSupplierFromNBR.Enabled = false;

            SetStatus("Load Products");
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
            LoadReadyStatus();
        }




        private void SetStatus(string status)
        {
            labelStatus.Text = status;
            labelStatus.Visible = true;
            dataGridForecastInfo.Visible = false;
            buttonGetProductByNumber.Enabled = false;
            buttonGetProductsBySupplier.Enabled = false;
            buttonGetSupplierFromNBR.Enabled = false;
            buttonCreateLactalisFile.Enabled = false;
            buttonSalesView.Enabled = false;
            comboBoxYear.Enabled = false;
            numericSupplyNBR.Enabled = false;
            //numericUpDownPRoductNumber.Enabled = false;
            textBoxProdNBR.Enabled = false;
            comboBoxSupplier.Enabled = false;


            labelStatus.Invalidate();
            labelStatus.Update();
            labelStatus.Refresh();
            Application.DoEvents();
        }

        private void LoadReadyStatus()
        {

            labelStatus.Visible = false;
            dataGridForecastInfo.Visible = true;
            buttonGetProductByNumber.Enabled = true;
            buttonGetProductsBySupplier.Enabled = true;
            buttonGetSupplierFromNBR.Enabled = true;
            buttonCreateLactalisFile.Enabled = true;
            buttonSalesView.Enabled = true;
            comboBoxYear.Enabled = true;
            numericSupplyNBR.Enabled = true;
            //numericUpDownPRoductNumber.Enabled = true;
            textBoxProdNBR.Enabled = true;
            comboBoxSupplier.Enabled = true;

            Application.DoEvents();
        }

        private void buttonTestSupplItems_Click(object sender, EventArgs e)
        {
            //List<string> tempList = m3Info.GetListOfProductsBySupplier("1102001");
            //foreach (string item in tempList)
            //{
            //    string tempName = m3Info.GetItemNameByItemNumber(item);
            //    Console.WriteLine(" Supplier produkt!! " + item + "  Name: " + tempName);
            //}
        }


        private void dataGridForecastInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            Console.WriteLine("Value clicked... Column index: " + columnIndex + "  rowIndex: " + rowIndex);
            latestMouseClick = System.Windows.Forms.Cursor.Position;

            //for the Sales info forecast show extra info
            if (GetValueFromGridAsString(rowIndex, columnIndex).Contains("lager") && columnIndex == 1)
            {
                if (!infoboxSupply.Visible)
                {
                    string temp2 = GetProductNumberFromRow(rowIndex);
                    string tempNewName = temp2;
                    PrognosInfoForSupply tempInfo = GetProductInfoByNumber(temp2);

                    Console.WriteLine(" Product: " + temp2 + " Week: " + latestWeek);



                    string temp = "";

                    //temp = tempInfo.StockDetails;
                    temp = tempInfo.GetStockInfo();
                    MessageBox.Show(temp, temp2 + " Stock Details: ");
                }
                else
                {
                    MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window to see info!");
                }
            }
            if ((GetValueFromGridAsString(rowIndex, 2) == "Salgsbudget_ThisYear") && columnIndex > 2)
            {
                if (!infoboxSupply.Visible)
                {
                    string temp2 = GetProductNumberFromRow(rowIndex);
                    string tempNewName = temp2;
                    if (ClassStaticVaribles.NewNumberDictNavKey.ContainsKey(temp2))
                    {
                        tempNewName = temp2 + " New Number (" + ClassStaticVaribles.NewNumberDictNavKey[temp2] + ")";
                    }
                    //int productNumber = Convert.ToInt32(temp2);

                    int latestWeek = columnIndex - 2;
                    PrognosInfoForSupply tempInfo = GetProductInfoByNumber(temp2);
                    Console.WriteLine(" Product: " + temp2 + " Week: " + latestWeek);

                    //Create dummy value to show something before real numbers

                    string temp = "";
                    NavSQLSupplyInformation sqlConnection = new NavSQLSupplyInformation(selectedYear, temp2);
                    temp = sqlConnection.GetSalesBudgetWeekInfo(latestWeek);

                    temp = "Salgsbudget" + " Product: " + tempNewName + " Week: " + latestWeek + "\n\n" + temp;
                    MessageBox.Show(temp);
                }
                else
                {
                    MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window to see info!");
                }
            }
            else if ((GetValueFromGridAsString(rowIndex, 2) == "Kampagn_ThisYear") && columnIndex > 2)
            {
                if (!infoboxSupply.Visible)
                {
                    string tempProdNBR = GetProductNumberFromRow(rowIndex);
                    string tempNewName = tempProdNBR;
                    if (ClassStaticVaribles.NewNumberDictNavKey.ContainsKey(tempProdNBR))
                    {
                        tempNewName = tempProdNBR + " New Number (" + ClassStaticVaribles.NewNumberDictNavKey[tempProdNBR] + ")";
                    }
                    //int productNumber = Convert.ToInt32(temp2);

                    int latestWeek = columnIndex - 2;
                    PrognosInfoForSupply tempInfo = GetProductInfoByNumber(tempProdNBR);
                    Console.WriteLine(" Product: " + tempProdNBR + " Week: " + latestWeek);

                    //Create dummy value to show something before real numbers

                    string temp = "";
                    GetFromM3 m3Connection = new GetFromM3();
                    temp = m3Connection.GetKampagnWeekInfo(latestWeek, tempProdNBR, selectedYear);

                    temp = "Kampagn " + " Product: " + tempNewName + " Week: " + latestWeek + "\n\n" + temp;
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
                    string tempProdNBR = GetProductNumberFromRow(rowIndex);
                    int productNumber = Convert.ToInt32(tempProdNBR);
                    int latestWeek = columnIndex - 2;
                    PrognosInfoForSupply tempInfo = GetProductInfoByNumber(tempProdNBR);
                    Console.WriteLine(" Product: " + tempProdNBR + " Week: " + latestWeek);

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
                    temp = "Realiserat" + " Product: " + tempProdNBR + " Week: " + latestWeek + temp;
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
                    string tempNewName = latestProductNumber;
                    if (ClassStaticVaribles.NewNumberDictNavKey.ContainsKey(latestProductNumber))
                    {
                        tempNewName = tempNewName + " New Number (" + ClassStaticVaribles.NewNumberDictNavKey[latestProductNumber] + ")";
                    }
                    if (latestWeek > 0)
                    {
                        temp = SupplierProducts[latestProductNumber].SalgsbudgetReguleret_Comment[latestWeek];

                        infoboxSupply.SetInfoText(this, tempNewComment, "Reguleret Product: " + tempNewName + " Week: " + latestWeek);
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
                }
                else
                {
                    MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before open a new one!");
                }
            }
        }

        public PrognosInfoForSupply GetProductInfoByNumber(string productNbr)
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

            if ((columnIndex == 0 && rowIndex == 0) || SupplierProducts.Count < 1)
            {
                Console.WriteLine("Validation while loading data skipped");
                return;
            }

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
                        int ammountToKop = Convert.ToInt32(e.FormattedValue) - SupplierProducts[productNumber].Kopsbudget_ThisYear[week];
                        SetKöpsbudget1102(week, productNumber.ToString(), Convert.ToInt32(e.FormattedValue));


                        //string startDato = "datum";
                        string comment = dataGridForecastInfo.Rows[rowIndex - 1].Cells[columnIndex].Value.ToString();

                        DateTime answer = ClassStaticVaribles.StartDate[selectedYear].AddDays((week - 1) * 7);
                        string format = "yyyy-MM-dd HH:mm:ss";    // modify the format depending upon input required in the column in database 


                        System.Threading.ThreadPool.QueueUserWorkItem(delegate
                        {
                            //Todo write to database

                            NavSQLExecute conn = new NavSQLExecute();
                            conn.InsertKöpsbudgetLine(productNumber.ToString(), answer.ToString(format), ammountToKop);

                        }, null);

                        //conn.InsertBudgetLine(tempCustNumber, productNumber, answer.ToString(format), ammount);

                    }
                }
            }
            else if ((GetValueFromGridAsString(rowIndex, 2) == "SalgsbudgetReguleret_TY") && columnIndex > 2) // 1 should be your column index
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
                        if (Convert.ToInt32(e.FormattedValue) != SupplierProducts[productNumber].SalgsbudgetReguleret_ThisYear[week])
                        {
                            int ammount = Convert.ToInt32(e.FormattedValue) - SupplierProducts[productNumber].SalgsbudgetReguleret_ThisYear[week];

                            if (dataGridForecastInfo.Rows[rowIndex - 1].Cells[columnIndex].Value.ToString() != "")
                            {
                                SetRegulerat1102(week, productNumber.ToString(), Convert.ToInt32(e.FormattedValue));

                                int sBudget = Convert.ToInt32(dataGridForecastInfo.Rows[rowIndex - 2].Cells[columnIndex].Value);
                                int regSbudget = Convert.ToInt32(e.FormattedValue);
                                if ((GetValueFromGridAsString(rowIndex + 1, 2).Contains("Salgsbudget_Summeret")))
                                {
                                    dataGridForecastInfo.Rows[rowIndex + 1].Cells[columnIndex].Value = sBudget + regSbudget;
                                }
                                //TODO: Write to database

                                //here a new budget_line post should be added to the database



                                //string startDato = "datum";
                                string comment = dataGridForecastInfo.Rows[rowIndex - 1].Cells[columnIndex].Value.ToString();



                                DateTime tempDate = DateTime.Parse(ClassStaticVaribles.StartDate[selectedYear].ToString());
                                DateTime answer = tempDate.AddDays((week - 1) * 7);
                                string format = "yyyy-MM-dd HH:mm:ss";    // modify the format depending upon input required in the column in database 

                                NavSQLExecute conn = new NavSQLExecute();
                                conn.InsertReguleretBudgetLine(productNumber.ToString(), answer.ToString(format), ammount, comment);
                                

                                //conn.InsertBudgetLine(tempCustNumber, productNumber, answer.ToString(format), ammount);

                                dataGridForecastInfo.Rows[rowIndex - 1].Cells[columnIndex].Value = "";
                                dataGridForecastInfo.Rows[rowIndex - 1].Cells[columnIndex].Style = new DataGridViewCellStyle { BackColor = Color.Yellow };

                            }
                            else
                            {
                                dataGridForecastInfo.Rows[rowIndex].Cells[columnIndex].Value = SupplierProducts[productNumber].SalgsbudgetReguleret_ThisYear[week];
                                MessageBox.Show("Please Write comment before changing value. Then type the value again and press enter to save.");

                            }
                            newRegCommentProdNBR = productNumber;

                        }


                    }
                }
            }
        }

        private void LoadReguleretAgain(string prodNBR)
        {
            dataGridForecastInfo.Visible = false;
            SetStatus("Loading Products");
            //Wait for the writing before loading comments again 
            SupplierProducts[prodNBR].UpdateReguleretInfo(selectedYear);

            dataGridForecastInfo.Visible = true;
            LoadReadyStatus();
        }

        private void FormSupply_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.WriteLine("Close supply form");
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

            if (ClassStaticVaribles.NewNumberDictM3Key.ContainsKey(returnString))
            {
                returnString = ClassStaticVaribles.NewNumberDictM3Key[returnString];
            }
            return returnString;
        }

        private void SetKöpsbudget1102(int week, string productNumber, int value)
        {
            //int prodNBRint = productNumber;
            if (SupplierProducts.ContainsKey(productNumber))
            {
                SupplierProducts[productNumber].Salgsbudget_ThisYear[week] = Convert.ToInt32(value);
            }

        }

        public void SetRegulerat1102(int week, string productNumber, int value)
        {

            if (SupplierProducts.ContainsKey(productNumber))
            {
                SupplierProducts[productNumber].SalgsbudgetReguleret_ThisYear[week] = Convert.ToInt32(value);

            }
        }


        public void SetProductRegComment(string comment)
        {
            //int prodNBRint = Convert.ToInt32(latestProductNumber);

            dataGridForecastInfo.Rows[latestRow].Cells[latestWeek + 2].Value = comment;
            //if (SupplierProducts.ContainsKey(latestProductNumber))
            //{
            //    SupplierProducts[latestProductNumber].Salgsbudget_Comment[latestWeek] = comment;
            //}
            //FillInfo();
        }


        private void buttonGetSupplierFromNBR_Click(object sender, EventArgs e)
        {
            //dataGridForecastInfo.Visible = false;
            //SetStatus("Loading products");
            //dataGridForecastInfo.ClearSelection();
            //SetupColumns();


            if (ClassStaticVaribles.AllSuppliersM3.ContainsKey(numericSupplyNBR.Value.ToString()))
            {
                comboBoxSupplier.SelectedItem = numericSupplyNBR.Value.ToString() + "  " + ClassStaticVaribles.AllSuppliersNameDict[numericSupplyNBR.Value.ToString()];
                dataGridForecastInfo.Visible = false;
                buttonGetProductByNumber.Enabled = false;
                buttonGetProductsBySupplier.Enabled = false;
                buttonGetSupplierFromNBR.Enabled = false;

                SetStatus("Load Products");
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
                LoadReadyStatus();
            }
            else
            {
                MessageBox.Show(new Form() { TopMost = true }, "No supplier with that number");
            }
            //dataGridForecastInfo.Visible = true;
            //LoadReadyStatus();

        }

        private void comboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedYear = (int)comboBoxYear.SelectedItem;
        }

        private void buttonGetProductByNumber_Click(object sender, EventArgs e)
        {
            dataGridForecastInfo.Visible = false;
            SetStatus("Loading Products");
            dataGridForecastInfo.ClearSelection();
            SetupColumns();

            SupplierProducts = new Dictionary<string, PrognosInfoForSupply>();
            this.dataGridForecastInfo.DataSource = null;

            this.dataGridForecastInfo.Rows.Clear();
            this.dataGridForecastInfo.AllowUserToAddRows = false;
            this.dataGridForecastInfo.AllowUserToDeleteRows = false;
            this.dataGridForecastInfo.AllowUserToOrderColumns = false;


            //int prodNMBR = (int)numericUpDownPRoductNumber.Value;
            string prodNMBR = textBoxProdNBR.Text;
            if (ClassStaticVaribles.NewNumberDictM3Key.ContainsKey(prodNMBR))
            {
                prodNMBR = ClassStaticVaribles.NewNumberDictM3Key[prodNMBR];
            }
            AddProductByNumber(prodNMBR);
            PrepareGUI();
            dataGridForecastInfo.Visible = true;
            LoadReadyStatus();
        }

        private void buttonCreateM3LactalisOrders_Click(object sender, EventArgs e)
        {
            CreateOrderForProduct(5, 23303);
        }

        private void CreateOrderForProduct(int week, int prodNumber)
        {
            if (SupplierProducts.Count > 0)
            {
                string codeOrder = SupplierProducts["0"].Supplier + "XYZ" + SupplierProducts["0"].WareHouse + "XYZ" + SupplierProducts["0"].PrepLocation;
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


        private void buttonCreateFile_Click(object sender, EventArgs e)
        {

        }

        private void CreateLinesForOneProduct(Dictionary<int, int> kopesBudgetFormNextWeek_TY, int[] weekPartPercentage, int lactalisProdNBR, int lactaNBRPerKolli)
        {
            int dayFromNow = 0;


            for (int i = 1; i < kopesBudgetFormNextWeek_TY.Count; i++)
            {
                int numberPerWeek = kopesBudgetFormNextWeek_TY[i] / lactaNBRPerKolli;
                int weekDay = 1;
                int thisWeekday = 1;
                while (thisWeekday != 0 && weekDay < 8)
                {
                    dayFromNow++;
                    DateTime today = DateTime.Now;
                    DateTime answer = today.AddDays(dayFromNow);

                    //Console.WriteLine("day nmbr: " + (int)answer.DayOfWeek + " is: {0:dddd}", answer);
                    thisWeekday = (int)answer.DayOfWeek;

                    // this code is (monday = 1, teuseday = 2,.... sunday=7
                    // answer from day of week code is (sunday=7 = 0, monday = 1, teuseday = 2,.... 
                    if (thisWeekday > 0) //if not sunday
                    {
                        weekDay = thisWeekday;
                    }
                    else
                    {
                        weekDay = 7; //sunday
                    }
                    double part = Convert.ToDouble(weekPartPercentage[weekDay]) / 100.0;

                    double numberThisDate = part * Convert.ToDouble(numberPerWeek);

                    if (numberPerWeek > 0)
                    {
                        Console.WriteLine("LActanbr: " + lactalisProdNBR + " number per week: " + numberPerWeek);
                    }


                    //weekDay++;
                    dayOrderStrings.Add(CreateLine(lactalisProdNBR, answer, Convert.ToInt32(numberThisDate)));
                }

            }

            //dayOrderStrings.Add(CreateLine(123, DateTime.Now, 525));
            //dayOrderStrings.Add(CreateLine(666666, DateTime.Now, 0));


        }


        private string CreateLine(int numberLact, DateTime date, int numberToBuy)
        {
            string temp = "";

            string tempNBR = numberLact.ToString();


            while (tempNBR.Length < 6)
            {
                tempNBR = " " + tempNBR;
            }
            temp = "\"" + tempNBR + "\"";
            temp = temp + ",\"" + "I2DEL" + "\",";
            temp = temp + "\"" + "\",";
            temp = temp + date.Year + ",";


            if (date.Month < 10)
            {
                temp = temp + "0";
            }
            temp = temp + date.Month + ",";
            if (date.Day < 10)
            {
                temp = temp + "0";
            }

            temp = temp + date.Day + ",";
            temp = temp + numberToBuy;

            //Console.WriteLine(temp);

            return temp;


        }

        private void checkBoxLastYear_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttonCreateLactalisFile_Click(object sender, EventArgs e)
        {
            SetStatus("Products to Forecast France Creating File");
            Stopwatch stopwatch = Stopwatch.StartNew();
            dayOrderStrings = new List<string>();

            List<string> productsToSendKopbudget = new List<string>();

            GetFromM3 m3 = new GetFromM3();

            //productsToSendKopbudget = GetListOfProductsWithSupplierLactaFrance();
            Dictionary<string, string> productsInSkaevinge = m3.GetAllSkaevingeProductsDict();
            Dictionary<string, string> productsInFile = new Dictionary<string,string>();

            foreach (KeyValuePair<string, string> item in productsInSkaevinge)
            {
                Dictionary<string, string> info = m3.GetItemInfoByItemNumber(item.Key);

                if (info != null && info.Count > 0 && Convert.ToInt32(info["INLActaFranceFile"]) > 0)
                {
                    productsInFile.Add(item.Key, item.Value);
                }
            }

            int i = 0;
            int numberOfProducts = productsToSendKopbudget.Count;

            foreach (KeyValuePair<string,string> item in productsInFile)
            {
                i++;
                SetStatus("Products to Forecast France Creating File Info for product " + i.ToString() + "/" + numberOfProducts.ToString());
                FileToSendInfoForPoduct currentProdFileInfo = new FileToSendInfoForPoduct(item.Key, 0);
                currentProdFileInfo.FillNumbers();

                int[] weekPartPercentage = currentProdFileInfo.weekPartPercentage;
                Dictionary<int, int> kopesBudgetTY = currentProdFileInfo.Kopsbudget_ThisYear;// sqlSupplyCalls.GetKopesBudget_TY();

                int lactalisProdNBR = currentProdFileInfo.LactalisFranceProductNumber;

                CreateLinesForOneProduct(kopesBudgetTY, weekPartPercentage, lactalisProdNBR, currentProdFileInfo.Antal_pr_kolli);
            }

            stopwatch.Stop();


            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fullName = System.IO.Path.Combine(desktopPath, "testOutput.txt");

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullName))
            {
                foreach (string line in dayOrderStrings)
                {
                    file.WriteLine(line);
                }
            }

            double timeConnectSeconds = stopwatch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Create France File! Time (s): " + timeConnectSeconds);
            SetStatus("File Created");
            System.Threading.Thread.Sleep(2000);
            LoadReadyStatus();

        }

        private List<string> GetListOfProductsWithSupplierLactaFrance()
        {
            List<string> returnList = new List<string>();

            // Todo write this query Here

            string queryText = @"select ForeCast_Startdato, Nummer,Leverandørnr,Leverandørs_varenr, lactalis_varenr, Beskrivelse, Antal_Forecast_uger, Forecast_fordelingsC_Mandag as FC_man, ";
            queryText = queryText + "Forecast_fordelingsC_Tirsdag as FC_tis, Forecast_fordelingsC_Onsdag as FC_ons, Forecast_fordelingsC_Torsdag as FC_tor, Forecast_fordelingsC_Fredag as FC_fre, ";

            queryText = queryText + "Forecast_fordelingsC_Lørdag as FC_lor, Forecast_fordelingsC_Søndag as FC_son   , Spærret, Undertryk_i_Købsbudget ";
            queryText = queryText + "from vare ";
            queryText = queryText + "where (Leverandørnr = '1102' or  Leverandørnr = '1100' or  Leverandørnr = '4016' ) and lactalis_varenr > 1 and Undertryk_i_Købsbudget='false'  and Spærret ='false' and ";
            queryText = queryText + "Antal_Forecast_uger > 0 and ForeCast_Startdato >= '1980-04-01' and Nummer NOT LIKE '%T%' and Nummer NOT LIKE '%U%' ";
            queryText = queryText + "order by lactalis_varenr ";

            NavSQLExecute conn = new NavSQLExecute();

            Console.WriteLine("LoadList of LActaFrance  Query: ");
            DataTable tempTable = conn.QueryExWithTableReturn(queryText);


            DataRow[] currentRows = tempTable.Select(null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
            {
                Console.WriteLine("No Kopsorder Line Found 3");
            }
            else
            {
                //loop trough all rows and write in tabs
                foreach (DataRow row in currentRows)
                {
                    string prodNBR = row["Nummer"].ToString();
                    int NBR = Convert.ToInt32(prodNBR);

                    returnList.Add(prodNBR);
                }
            }


            conn.Close();
            return returnList;
        }



        private void numericUpDownPRoductNumber_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FormSupply_SizeChanged(object sender, EventArgs e)
        {

            dataGridForecastInfo.Width = this.Width - 50;
            dataGridForecastInfo.Height = this.Height - 250;

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

        private void dataGridForecastInfo_KeyUp(object sender, KeyEventArgs e)
        {
            if (newRegCommentProdNBR.Length > 0)
            {

                LoadReguleretAgain(newRegCommentProdNBR);
                newRegCommentProdNBR = "";
            }
        }

        internal void SetOnlyLook(bool look)
        {
            OnlyLook = look;
        }
    }
}
