///This is the GUI File for the Supply view 
///This GUI runs all action for Supply to focus on
///Shows all ForecastNumbers for all the suppliers
///The numbers is summed up from all the Customer Forecasts


using System;
using System.Collections;
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

namespace WindowsFormsForecastLactalis
{
    public partial class FormSupply : Form
    {
        List<string> dayOrderStrings = new List<string>();
        private string latestComboBoxText = "";
        string newRegCommentProdNBR = "";

        public static List<PrognosInfoForSupply> Products = new List<PrognosInfoForSupply>();
        FormSales formSalesInstance;
        GetFromM3 m3Info = new GetFromM3();
        TextBoxForm infoboxSupply = new TextBoxForm();
        public Dictionary<int, List<int>> MotherSupplierWithChilds = new Dictionary<int, List<int>>();

        public Dictionary<string, string> OrderCodes = new Dictionary<string, string>();

        int desiredStartLocationX;
        int desiredStartLocationY;
        int latestClickedWeek;
        int latestClickedYear;
        string latestProductNumber;
        Point latestMouseClick;
        int latestRow;
        string latestSelectedYear;
        int selectedYear;
        bool OnlyLook = true;
        int forecastNbrWeeks;

        public FormSupply()
        {
            InitializeComponent();
            Console.WriteLine("Start FormSupply!");
            forecastNbrWeeks = 20;

            FixSupplierChoices();

            dataGridForecastInfo.Width = this.Width - 50;
            dataGridForecastInfo.Height = this.Height - 250;


            labelCalculator.Location = new System.Drawing.Point(10, this.Height - 70);
            textBoxCalculator.Location = new System.Drawing.Point(125, this.Height - 70);

            if (StaticVariables.Production)
            {
                this.Text = "Forecast Supply Production Environment  " + Application.ProductVersion;

            }
            else
            {

                this.Text = "Forecast Supply Test Environment  " + Application.ProductVersion;
                this.BackColor = Color.WhiteSmoke;
            }

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

            foreach (KeyValuePair<string, List<string>> item in StaticVariables.AllSuppliersM3)
            {
                if (StaticVariables.AllSuppliersNameDict.ContainsKey(item.Key))
                {
                    supplier.Add(item.Key + "  " + StaticVariables.AllSuppliersNameDict[item.Key] + "  " + StaticVariables.AllSuppliersM3[item.Key].Count);
                }
                else
                {
                    supplier.Add(item.Key + "  " + StaticVariables.AllSuppliersM3[item.Key].Count);
                }

            }
            supplier.Sort();

            comboBoxSupplier.DataSource = supplier;
            comboBoxSupplier.DropDownStyle = ComboBoxStyle.DropDownList;

            List<string> yearList = new List<string>();
            yearList.Add("Now");
            yearList.Add("2015");
            yearList.Add("2016");
            yearList.Add("2017");

            comboBoxYear.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxYear.DataSource = new BindingSource(yearList, null);
            comboBoxYear.SelectedIndex = comboBoxYear.Items.IndexOf("Now");
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
            if (comboBoxYear.Text != latestComboBoxText)
            {



                if (selectedYear == 1000)
                {
                    latestComboBoxText = comboBoxYear.Text;
                    dataGridForecastInfo.ColumnCount = 44;
                    dataGridForecastInfo.Columns[0].Name = "VareNr";
                    dataGridForecastInfo.Columns[1].Name = "Beskrivelse";
                    dataGridForecastInfo.Columns[2].Name = "Type";

                    int currentWeek = StaticVariables.GetForecastWeeknumberForDate(DateTime.Now);
                    if (StaticVariables.TestWeek > 0)
                    {
                        currentWeek = StaticVariables.TestWeek;
                    }

                    if (currentWeek > 26)
                    {
                        int weekNumber = currentWeek - forecastNbrWeeks;
                        for (int i = 1; i < (forecastNbrWeeks * 2 + 2); i++)
                        {
                            string temp;
                            if (weekNumber > 52)
                            {
                                int yearNext = DateTime.Now.Year + 1;
                                temp = (weekNumber - 52) + "." + yearNext.ToString();
                            }
                            else
                            {
                                temp = weekNumber + "." + DateTime.Now.Year.ToString();
                            }
                            if (weekNumber == currentWeek)
                            {
                                temp = temp + " Now";
                            }
                            weekNumber++;
                            dataGridForecastInfo.Columns[i + 2].Name = temp;
                        }

                        for (int i = (forecastNbrWeeks * 2 + 2); i <= dataGridForecastInfo.Columns.Count - 3; i++)
                        {
                            string temp = "XXXX";
                            weekNumber++;
                            dataGridForecastInfo.Columns[i + 2].Name = temp;
                        }
                    }
                    else
                    {
                        int weekNumber = currentWeek - forecastNbrWeeks;
                        for (int i = 1; i < (forecastNbrWeeks * 2 + 2); i++)
                        {
                            string temp;
                            if (weekNumber < 1)
                            {
                                int yearLast = DateTime.Now.Year - 1;
                                temp = (weekNumber + 52) + "." + yearLast.ToString();
                            }
                            else
                            {
                                temp = weekNumber + "." + DateTime.Now.Year.ToString();
                            }

                            if (weekNumber == currentWeek)
                            {
                                temp = temp + " Now";
                            }
                            weekNumber++;
                            dataGridForecastInfo.Columns[i + 2].Name = temp;
                        }

                        for (int i = (forecastNbrWeeks * 2 + 2); i <= dataGridForecastInfo.Columns.Count - 3; i++)
                        {
                            string temp = "XXXX";
                            weekNumber++;
                            dataGridForecastInfo.Columns[i + 2].Name = temp;
                        }
                    }
                    //selectedYear = 2016;
                }
                //selected year not Now
                else
                {
                    latestComboBoxText = comboBoxYear.Text;
                    dataGridForecastInfo.ColumnCount = 56;
                    dataGridForecastInfo.Columns[0].Name = "VareNr";
                    dataGridForecastInfo.Columns[1].Name = "Beskrivelse";
                    dataGridForecastInfo.Columns[2].Name = "Type";
                    for (int i = 1; i < 54; i++)
                    {
                        int currentWeek = StaticVariables.GetForecastWeeknumberForDate(DateTime.Now);

                        string temp = i + "." + comboBoxYear.Text;
                        if (i == currentWeek && selectedYear == DateTime.Now.Year)
                        {
                            temp = temp + " Now";
                        }
                        dataGridForecastInfo.Columns[i + 2].Name = temp;
                    }
                }

                foreach (DataGridViewColumn item in dataGridForecastInfo.Columns)
                {
                    item.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                }


                //Scroll to this week
                FocusNowColumn();
            }
        }





        Comparer _comparer = new Comparer(System.Globalization.CultureInfo.CurrentCulture);
        public int Compare(PrognosInfoForSupply x, PrognosInfoForSupply y)
        {
            if (x.Status < 90 && y.Status == 90) return -1;
            if (x.Status == 90 && y.Status < 90) return 1;
            string numxs = string.Concat(x.ProductNumber.TakeWhile(c => char.IsDigit(c)).ToArray());
            string numys = string.Concat(y.ProductNumber.TakeWhile(c => char.IsDigit(c)).ToArray());

            int xnum;
            int ynum;
            if (!int.TryParse(numxs, out xnum) || !int.TryParse(numys, out ynum))
            {
                return _comparer.Compare(x, y);
            }
            int compareNums = xnum.CompareTo(ynum);
            if (compareNums != 0)
            {
                return compareNums;
            }
            return _comparer.Compare(x, y);
        }

        //Fill the UI info
        private void FillInfo()
        {
            Products = new List<PrognosInfoForSupply>();
            //Clear purchase order lines as new will be fetched from M3
            StaticVariables.PurchaseOrderLinesM3.Clear();
            StaticVariables.ExpectedPurchaseOrderLinesM3.Clear();
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
            List<string> tempList = StaticVariables.AllSuppliersM3[suppliernumberM3];
            int numberOfProducts = tempList.Count;

            //buttonGetProductsBySupplier.Focus();
            checkBoxLastYear.Focus();
            int i = 0;
            foreach (string item in tempList)
            {
                if (StaticVariables.AbortLoad)
                {
                    StaticVariables.AbortLoad = false;
                    break;
                }
                i++;

                SetStatus("Products loading " + i.ToString() + "/" + numberOfProducts.ToString());

                string tempName = " ";
                if (StaticVariables.AllProductsM3Dict.ContainsKey(item))
                {

                    tempName = StaticVariables.AllProductsM3Dict[item];//m3Info.GetItemNameByItemNumber(item.ToString());
                }
                int status = 0;
                if (StaticVariables.dictItemStatus.ContainsKey(item))
                {
                    status = StaticVariables.dictItemStatus[item];
                }
                PrognosInfoForSupply product1 = new PrognosInfoForSupply(tempName, item, status);
                product1.SetWhatToLoad(checkBoxLastYear.Checked, checkBoxKampgn.Checked);

                product1.FillNumbers(selectedYear);
                Products.Add(product1);
                if (i == 1 || i == 10 || i == 3)
                {
                    ShowFirstTen();
                }
            }
            Products.Sort(Compare);
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
            Dictionary<int, int> weekToLock = new Dictionary<int, int>();
            this.dataGridForecastInfo.DataSource = null;
            

            this.dataGridForecastInfo.Rows.Clear();
            int weekProdNBR = 0;


            // Loop through keys.
            foreach (PrognosInfoForSupply item in Products)
            {
                List<object> tempList;
                
                //Load salesdata per week
                var salesThisYear = item.SalesRowsThisYear.GroupBy(r => new
                {
                    r.ItemNumber,
                    r.Week
                }).Select(r => new
                {
                    Item = r.Key.ItemNumber,
                    Week = r.Key.Week,
                    Sum = r.Sum(x => x.Quantity)
                });

                if (selectedYear == 1000)
                {
                    int currentWeek = StaticVariables.GetForecastWeeknumberForDate(DateTime.Now);
                    //currentWeek = 40;
                    int weekNumberStart = 0;
                    if (StaticVariables.TestWeek > 0)
                    {
                        currentWeek = StaticVariables.TestWeek;
                    }


                    weekNumberStart = currentWeek - forecastNbrWeeks;
                    for (int i = 1; i < (forecastNbrWeeks * 2 + 2); i++)
                    {
                        if (weekNumberStart < 1)
                        {
                            weekNumberStart = (weekNumberStart + 52);
                        }
                    }

                    weekToLock.Add(weekProdNBR, item.WeekToLockFrom + 3 - weekNumberStart); //+ 3 is offset from cell number to week number
                    weekProdNBR++;

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Vecka");
                    int k = 0;
                    for (int i = weekNumberStart; i <= (weekNumberStart + (2 * forecastNbrWeeks + 1)); i++)
                    {
                        if (i > 52)
                        {
                            k = i - 52;
                        }
                        else
                        {
                            k = i;
                        }
                        tempList.Add(k);

                        //tempList.Add(item.RealiseretSalgs_LastYear[i]);
                    }
                    AddRowFromList(tempList);


                    tempList = new List<object>();

                    tempList.Add(item.ProductNumber.ToString());
                    tempList.Add(item.ProductName);
                    if (checkBoxLastYear.Checked)
                    {
                        //Load Sales data per week
                        var salesLastYear = item.SalesRowsLastYear.GroupBy(r => new
                        {
                            r.ItemNumber,
                            r.Week
                        }).Select(r => new
                        {
                            Item = r.Key.ItemNumber,
                            Week = r.Key.Week,
                            Sum = r.Sum(x => x.Quantity)
                        });
                        tempList.Add("RealiseretSalg_LastYear");
                        for (int i = weekNumberStart; i <= (weekNumberStart + (2 * forecastNbrWeeks + 1)); i++)
                        {
                            if (i > 52)
                            {
                                k = i - 52;
                            }
                            else
                            {
                                k = i;
                            }
                            var sum = from line in salesLastYear
                                      where line.Week == k
                                      select line.Sum;
                            tempList.Add(sum.FirstOrDefault());
                            //tempList.Add(item.RealiseretSalgs_LastYear[i]);
                        }
                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("På lager: ");
                    if (checkBoxLastYear.Checked)
                    {
                        tempList.Add("RealiseretKampagn_LastYear");
                        for (int i = weekNumberStart; i <= (weekNumberStart + (2 * forecastNbrWeeks + 1)); i++)
                        {
                            if (i > 52)
                            {
                                k = i - 52;
                            }
                            else
                            {
                                k = i;
                            }
                            tempList.Add(item.RealiseretKampagn_LastYear[k]);
                        }
                    }
                    AddRowFromList(tempList);
                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("RealiseratSalg_ThisYear");
                    for (int i = weekNumberStart; i <= (weekNumberStart + (2 * forecastNbrWeeks + 1)); i++)
                    {
                        if (i > 52)
                        {
                            k = i - 52;
                        }
                        else
                        {
                            k = i;
                        }
                        var sum = from line in salesThisYear
                                  where line.Week == k
                                  select line.Sum;
                        tempList.Add(sum.FirstOrDefault());
                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Kampagn_ThisYear");
                    if (item.ShowCampaign)
                    {
                        for (int i = weekNumberStart; i <= (weekNumberStart + (2 * forecastNbrWeeks + 1)); i++)
                        {
                            if (i > 52)
                            {
                                k = i - 52;
                            }
                            else
                            {
                                k = i;
                            }
                            tempList.Add(item.Kampagn_ThisYear[k]);
                        }
                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Salgsbudget_ThisYear");
                    for (int i = weekNumberStart; i <= (weekNumberStart + (2 * forecastNbrWeeks + 1)); i++)
                    {
                        if (i > 52)
                        {
                            k = i - 52;
                        }
                        else
                        {
                            k = i;
                        }
                        tempList.Add(item.Salgsbudget_ThisYear[k]);
                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Reguleret_Comment");
                    for (int i = weekNumberStart; i <= (weekNumberStart + (2 * forecastNbrWeeks + 1)); i++)
                    {
                        if (i > 52)
                        {
                            k = i - 52;
                        }
                        else
                        {
                            k = i;
                        }
                        tempList.Add("");
                    }
                    AddRowFromList(tempList);

                    commentDict = item.SalgsbudgetReguleret_Comment;
                    DataGridViewRow tempRow = dataGridForecastInfo.Rows[dataGridForecastInfo.Rows.Count - 1];

                    for (int i = 3; i < tempRow.Cells.Count; i++)
                    {
                        String columnName = this.dataGridForecastInfo.Columns[i].Name;
                        int weekTest = StaticVariables.GetWeekFromName(columnName);
                        string tempComment = commentDict[weekTest];
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

                    for (int i = weekNumberStart; i <= (weekNumberStart + (2 * forecastNbrWeeks + 1)); i++)
                    {
                        if (i > 52)
                        {
                            k = i - 52;
                        }
                        else
                        {
                            k = i;
                        }
                        tempList.Add(item.SalgsbudgetReguleret_ThisYear[k]);
                    }

                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Salgsbudget_Summeret_ThisYear");

                    for (int i = weekNumberStart; i <= (weekNumberStart + (2 * forecastNbrWeeks + 1)); i++)
                    {
                        if (i > 52)
                        {
                            k = i - 52;
                        }
                        else
                        {
                            k = i;
                        }
                        tempList.Add(item.SalgsbudgetReguleret_ThisYear[k] + item.Salgsbudget_ThisYear[k]);
                    }

                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Köpsbudget_ThisYear");
                    for (int i = weekNumberStart; i <= (weekNumberStart + (2 * forecastNbrWeeks + 1)); i++)
                    {
                        if (i > 52)
                        {
                            k = i - 52;
                        }
                        else
                        {
                            k = i;
                        }
                        tempList.Add(item.Kopsbudget_ThisYear[k]);

                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Köpsorder_ThisYear");
                    for (int i = weekNumberStart; i <= (weekNumberStart + (2 * forecastNbrWeeks + 1)); i++)
                    {
                        if (i > 52)
                        {
                            k = i - 52;
                        }
                        else
                        {
                            k = i;
                        }
                        tempList.Add(item.Kopsorder_ThisYear[k]);

                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("InkommandeKöpsorder_ThisYear");
                    for (int i = weekNumberStart; i <= (weekNumberStart + (2 * forecastNbrWeeks + 1)); i++)
                    {
                        if (i > 52)
                        {
                            k = i - 52;
                        }
                        else
                        {
                            k = i;
                        }
                        tempList.Add(item.KopsorderExpected_ThisYear[k]);

                    }
                    AddRowFromList(tempList);
                }
                //Not Now chosen
                else
                {

                    weekToLock.Add(weekProdNBR, item.WeekToLockFrom + 2); //+ 2 is offset from cell number to week number
                    weekProdNBR++;

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Vecka");
                    for (int i = 1; i < 54; i++)
                    {
                        tempList.Add(i);
                        //tempList.Add(item.RealiseretSalgs_LastYear[i]);
                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();

                    tempList.Add(item.ProductNumber.ToString());
                    tempList.Add(item.ProductName);
                    if (checkBoxLastYear.Checked)
                    {
                        //Load Sales data per week
                        var salesLastYear = item.SalesRowsLastYear.GroupBy(r => new
                        {
                            r.ItemNumber,
                            r.Week
                        }).Select(r => new
                        {
                            Item = r.Key.ItemNumber,
                            Week = r.Key.Week,
                            Sum = r.Sum(x => x.Quantity)
                        });
                        tempList.Add("RealiseretSalg_LastYear");
                        for (int i = 1; i < 54; i++)
                        {
                            var sum = from line in salesLastYear
                                      where line.Week == i
                                      select line.Sum;
                            tempList.Add(sum.FirstOrDefault());
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
                        var sum = from line in salesThisYear
                                  where line.Week == i
                                  select line.Sum;
                        tempList.Add(sum.FirstOrDefault());
                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Kampagn_ThisYear");
                    if (item.ShowCampaign)
                    {
                        for (int i = 1; i < 54; i++)
                        {
                            tempList.Add(item.Kampagn_ThisYear[i]);
                        }
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

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("InkommandeKöpsorder_ThisYear");
                    for (int i = 1; i < 54; i++)
                    {
                        tempList.Add(item.KopsorderExpected_ThisYear[i]);
                    }
                    AddRowFromList(tempList);
                }
            }
            //Thread.Sleep(2000);
            weekProdNBR = 0;
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
                    for (int i = 3; i < row.Cells.Count; i++)
                    {                        
                        if ((selectedYear > 2000 && selectedYear < DateTime.Now.Year))
                        {
                            row.Cells[i].Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                        }
                        else
                        {
                            if (weekToLock[weekProdNBR] < i)
                            {
                                row.Cells[i].Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                            }
                            else
                            {
                                row.Cells[i].Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                            }

                            if (selectedYear > DateTime.Now.Year && weekToLock[weekProdNBR] <= 52)
                            {
                                row.Cells[i].Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                            }
                        }
                    }
                    weekProdNBR++;
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
                else if (Convert.ToString(row.Cells[2].Value) == "InkommandeKöpsorder_ThisYear")
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
                else if (Convert.ToString(row.Cells[2].Value) == "Vecka")
                {
                    row.DefaultCellStyle.ForeColor = Color.White;
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
                if (colNBR < 3 || !col.Name.Contains("20"))
                {
                    col.ReadOnly = true;
                }
                colNBR++;
            }
        }


        private void AddProductByNumber(string number)
        {
            string tempNumber = number.ToString();
            string prodName = "Unknown Name";
            //Clear purchase orders since we fetch new data from M3
            StaticVariables.PurchaseOrderLinesM3.Clear();
            StaticVariables.ExpectedPurchaseOrderLinesM3.Clear();
            if (StaticVariables.AllProductsNavDict.ContainsKey(tempNumber))
            {
                prodName = StaticVariables.AllProductsNavDict[tempNumber];
            }
            else if (StaticVariables.AllProductsM3Dict.ContainsKey(tempNumber))
            {
                prodName = StaticVariables.AllProductsM3Dict[tempNumber];
            }
            int status = 0;
            if (StaticVariables.dictItemStatus.ContainsKey(tempNumber))
            {
                status = StaticVariables.dictItemStatus[tempNumber];
            }
            PrognosInfoForSupply product1 = new PrognosInfoForSupply(prodName, tempNumber, status);
            product1.SetWhatToLoad(checkBoxLastYear.Checked, checkBoxKampgn.Checked);
            product1.FillNumbers(selectedYear);
            Products.Add(product1);
        }


        //What to do when a field is clicked
        private void dataGridForecastInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void SetFormSalesInstanse(FormSales form1)
        {
            formSalesInstance = form1;
        }

        //Go back to sales view
        private void buttonSalesView_Click(object sender, EventArgs e)
        {
            StaticVariables.AbortLoad = false;
            if (!infoboxSupply.Visible)
            {
                Console.WriteLine("User press Sales View");
                this.Visible = false;
                formSalesInstance.BringToFront();
                formSalesInstance.Location = this.Location;
                formSalesInstance.Show();
            }
            else
            {
                MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before changing view!");
            }
        }

        private void buttonGetProductsBySupplier_Click(object sender, EventArgs e)
        {
            StaticVariables.AbortLoad = false;
            dataGridForecastInfo.Visible = false;
            /*buttonGetProductByNumber.Enabled = false;
            buttonGetProductsBySupplier.Enabled = false;
            buttonGetSupplierFromNBR.Enabled = false;
            */
            SetStatus("Load Products");
            dataGridForecastInfo.ClearSelection();
            SetupColumns();
            buttonGetProductsBySupplier.Focus();
            checkBoxLastYear.Focus();
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
            FocusNowColumn();
            LoadReadyStatus();

        }

        private void SetStatus(string status)
        {
            labelStatus.Text = status;
            labelStatus.Visible = true;
            //dataGridForecastInfo.Visible = false;
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
            if (columnIndex >= 0)
            {
                String columnName = this.dataGridForecastInfo.Columns[e.ColumnIndex].Name;
                latestClickedWeek = StaticVariables.GetWeekFromName(columnName);
                latestClickedYear = StaticVariables.GetYearFromName(columnName);//columnIndex - 2;
                if (columnIndex == 0)
                {
                    Console.WriteLine("Here We are");

                    if (formSalesInstance.scrollInfoInstance != null)
                    {
                        formSalesInstance.scrollInfoInstance.Close();
                    }
                    formSalesInstance.scrollInfoInstance = new ScrollToProdNumber(formSalesInstance, false);
                    formSalesInstance.scrollInfoInstance.StartPosition = FormStartPosition.Manual;
                    formSalesInstance.scrollInfoInstance.Location = latestMouseClick;
                    formSalesInstance.scrollInfoInstance.Show();
                }
                //for the Sales info forecast show extra info
                if (GetValueFromGridAsString(rowIndex, columnIndex).Contains("lager") && columnIndex == 1)
                {
                    if (!infoboxSupply.Visible)
                    {
                        string itemNumber = GetProductNumberFromRow(rowIndex);
                        string message = "";
                        PrognosInfoForSupply tempInfo = GetProductInfoByNumber(itemNumber);

                        Console.WriteLine(" Product: " + itemNumber + " Week: " + latestClickedWeek);

                        List<BalanceId> balanceIdentities = tempInfo.GetStockInfo();

                        if (balanceIdentities == null)
                        {
                            MessageBox.Show("Kunde inte hämta lagersaldo. Försök igen");
                            return;
                        }
                        if (balanceIdentities.Count > 0)
                        {
                            var balanceIds = balanceIdentities.GroupBy(r => new
                            {
                                r.Warehouse,
                                r.ItemNumber,
                                r.BestBeforeDate
                            })
                            .OrderBy(x => x.Key.Warehouse)
                            .ThenBy(x => x.Key.BestBeforeDate)
                            .Select(r => new
                            {
                                Item = r.Key.ItemNumber,
                                Warehouse = r.Key.Warehouse,
                                BestBeforeDate = r.Key.BestBeforeDate,
                                AllocatableQuantity = r.Sum(x => x.AllocatableQuantity),
                                AllocatedQuantity = r.Sum(x => x.AllocatedQuantity),
                                OnHandBalance = r.Sum(x => x.OnHandBalance)
                            });
                            message = String.Format("{0}\t{1}\t{2}\t{3}\t{4}{5}",
                                    "BBD            ",
                                    "Lager",
                                    "Alk.bar",
                                    "Resvt",
                                    "Lager",
                                    Environment.NewLine
                                    );
                            foreach (var balanceId in balanceIds)
                            {
                                message = message + String.Format("{0}\t{1}\t{2}\t{3}\t{4}{5}",
                                    balanceId.BestBeforeDate.ToShortDateString(),
                                    balanceId.OnHandBalance,
                                    balanceId.AllocatableQuantity,
                                    balanceId.AllocatedQuantity,
                                    balanceId.Warehouse,
                                    Environment.NewLine);
                            }
                        }
                        else
                        {
                            message = "In Stock: 0  Nothing in stock!";
                        }
                        MessageBox.Show(message);
                    }
                    else
                    {
                        MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window to see info!");
                    }
                }
                if ((GetValueFromGridAsString(rowIndex, 2) == "Salgsbudget_ThisYear") && columnIndex > 2 && columnName.Contains("20"))
                {
                    if (!infoboxSupply.Visible)
                    {
                        string temp2 = GetProductNumberFromRow(rowIndex);
                        string tempNewName = temp2;
                        int latestWeek = latestClickedWeek;
                        latestClickedYear = StaticVariables.GetYearFromName(columnName);//columnIndex - 2;;
                        PrognosInfoForSupply tempInfo = GetProductInfoByNumber(temp2);
                        Console.WriteLine(" Product: " + temp2 + " Week: " + latestWeek);

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
                else if ((GetValueFromGridAsString(rowIndex, 2) == "Kampagn_ThisYear") && columnIndex > 2 && columnName.Contains("20"))
                {
                    if (!infoboxSupply.Visible)
                    {
                        string tempProdNBR = GetProductNumberFromRow(rowIndex);
                        string tempNewName = tempProdNBR;
                        int latestWeek = latestClickedWeek;
                        PrognosInfoForSupply tempInfo = GetProductInfoByNumber(tempProdNBR);
                        Console.WriteLine(" Product: " + tempProdNBR + " Week: " + latestWeek);

                        string infoText = "";
                        GetFromM3 m3Connection = new GetFromM3();
                        List<PromotionInfo> promotionLines = m3Connection.GetPromotionForAllCustomerAsListv2(tempProdNBR, selectedYear);

                        var result = from rows in promotionLines where rows.Week == latestWeek select rows;
                        NavSQLExecute sqlconn = new NavSQLExecute();
                        foreach (PromotionInfo line in result)
                        {
                            infoText = String.Format("{0, -30}\t{1, -20}\t{2, -20}\t{3, -20}{4}",
                                line.Id + ". " + line.Description,
                                line.ItemNumber,
                                line.Week,
                                line.Quantity,
                                Environment.NewLine) + infoText;
                        }
                        if (infoText.Length > 0)
                        {
                            infoText = String.Format("{0, -30}\t{1, -20}\t{2, -20}\t{3, -20}{4}",
                                "Kampagn",
                                "Produkt",
                                "Vecka",
                                "Antal",
                                Environment.NewLine) + infoText;
                            MessageBox.Show(infoText);
                        }
                    }
                    else
                    {
                        MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window to see info!");
                    }
                }
                else if ((GetValueFromGridAsString(rowIndex, 2) == "RealiseratSalg_ThisYear") && columnIndex > 2 && columnName.Contains("20"))
                {
                    if (!infoboxSupply.Visible)
                    {
                        string tempProdNBR = GetProductNumberFromRow(rowIndex);
                        //int productNumber = Convert.ToInt32(tempProdNBR);
                        int latestWeek = latestClickedWeek;
                        PrognosInfoForSupply tempInfo = GetProductInfoByNumber(tempProdNBR);

                        Console.WriteLine(" Product: " + tempProdNBR + " Week: " + latestWeek);
                        SaleInfo sf = new SaleInfo();
                        sf.LoadSaleInfo(tempInfo.SalesRowsThisYear, latestWeek);
                        sf.Show();
                    }
                }
                else if ((GetValueFromGridAsString(rowIndex, 2) == "RealiseretSalg_LastYear") && columnIndex > 2 && columnName.Contains("20"))
                {
                    if (!infoboxSupply.Visible)
                    {
                        string tempProdNBR = GetProductNumberFromRow(rowIndex);
                        //int productNumber = Convert.ToInt32(tempProdNBR);
                        int latestWeek = latestClickedWeek;
                        PrognosInfoForSupply tempInfo = GetProductInfoByNumber(tempProdNBR);

                        Console.WriteLine(" Product: " + tempProdNBR + " Week: " + latestWeek);
                        SaleInfo sf = new SaleInfo();
                        sf.LoadSaleInfo(tempInfo.SalesRowsLastYear, latestWeek);
                        sf.Show();
                    }
                }
                else if (GetValueFromGridAsString(rowIndex, 2).Contains("Comment") && columnName.Contains("20"))
                {
                    if (!infoboxSupply.Visible)
                    {

                        string temp = "";
                        string tempNewComment = GetValueFromGridAsString(rowIndex, columnIndex);



                        latestRow = rowIndex;
                        //latestClickedWeek = columnIndex - 2;
                        latestProductNumber = GetProductNumberFromRow(rowIndex);
                        string tempNewName = latestProductNumber;
                        if (latestClickedWeek > 0)
                        {
                            temp = Products.Where(x => x.ProductNumber.Equals(latestProductNumber)).Select(x => x.SalgsbudgetReguleret_Comment).First()[latestClickedWeek];
                            //temp = SupplierProducts[latestProductNumber].SalgsbudgetReguleret_Comment[latestWeek];

                            infoboxSupply.SetInfoText(this, tempNewComment, "Reguleret Product: " + tempNewName + " Week: " + latestClickedWeek);
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
                            infoboxSupply.FocusTextBox(this);
                        }
                    }
                    else
                    {
                        MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before open a new one!");
                    }
                }
                else if ((GetValueFromGridAsString(rowIndex, 2) == "Köpsorder_ThisYear" || GetValueFromGridAsString(rowIndex, 2) == "InkommandeKöpsorder_ThisYear" && columnName.Contains("20"))
                  && columnIndex > 2)
                {
                    bool received = GetValueFromGridAsString(rowIndex, 2) == "Köpsorder_ThisYear";
                    string message = "";
                    //latestClickedWeek = latestClickedWeek;
                    latestProductNumber = GetProductNumberFromRow(rowIndex);

                    if (latestClickedWeek > 0)
                    {
                        if (received)
                        {
                            var lines = from line in StaticVariables.PurchaseOrderLinesM3
                                        where line.Week == latestClickedWeek && line.ItemNumber == latestProductNumber
                                        select line;
                            foreach (var line in lines)
                            {
                                message = message + String.Format("{0, -20}\t{1, -20}\t{2, -20}\t{3, -8}{4}",
                                line.Warehouse,
                                StaticVariables.ReturnDanishFormat(line.Date.ToShortDateString()),
                                line.PONumber,
                                line.Quantity,
                                Environment.NewLine);
                            }
                        }
                        else
                        {
                            var lines = from line in StaticVariables.ExpectedPurchaseOrderLinesM3
                                        where line.Week == latestClickedWeek && line.ItemNumber == latestProductNumber
                                        select line;
                            foreach (var line in lines)
                            {
                                message = message + String.Format("{0, -20}\t{1, -20}\t{2, -20}\t{3, -8}{4}",
                                line.Warehouse + "   ",
                                StaticVariables.ReturnDanishFormat(line.Date.ToShortDateString()),
                                line.PONumber,
                                line.Quantity,
                                Environment.NewLine);
                            }
                        }
                        message = String.Format("{0, -20}\t{1, -20}\t{2, -20}\t{3, -8}{4}", "Lager", "Dato", "Order", "Antal",
                                    Environment.NewLine) + message;
                        MessageBox.Show(message);
                    }
                }
            }
        }

        public PrognosInfoForSupply GetProductInfoByNumber(string productNbr)
        {
            if (Products.Any(x => x.ProductNumber.Equals(productNbr)))
            {
                return Products.Where(x => x.ProductNumber.Equals(productNbr)).First();
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

            String columnName = this.dataGridForecastInfo.Columns[e.ColumnIndex].Name;
            int weekFromColName = StaticVariables.GetWeekFromName(columnName);
            int yearFromColName = StaticVariables.GetYearFromName(columnName);

            if ((columnIndex == 0 && rowIndex == 0) || Products.Count < 1)
            {
                Console.WriteLine("Validation while loading data skipped");
                return;
            }

            if ((GetValueFromGridAsString(rowIndex, 2) == "Köpsbudget_ThisYear") && columnIndex > 2 && columnName.Contains("20")) // 1 should be your column index
            {
                int i;
                string productNumber = GetProductNumberFromRow(rowIndex);
                int week = weekFromColName;
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
                        var kopsbudget = Products.
                            Where(x => x.ProductNumber.Equals(productNumber)).
                            Select(x => x.Kopsbudget_ThisYear).First();
                        int ammountToKop = Convert.ToInt32(e.FormattedValue) - kopsbudget[week];
                        SetKöpsbudget1102(week, productNumber.ToString(), Convert.ToInt32(e.FormattedValue));

                        //string startDato = "datum";
                        string comment = dataGridForecastInfo.Rows[rowIndex - 1].Cells[columnIndex].Value.ToString();
                        //DateTime tempDate = StaticVariables.FirstSaturdayBeforeWeakOne(yearFromColName);
                        DateTime kopDate = StaticVariables.GetForecastStartDateOfWeeknumber(yearFromColName, weekFromColName);
                        kopDate = kopDate.AddDays(2); //Monday
                        string format = "yyyy-MM-dd HH:mm:ss";    // modify the format depending upon input required in the column in database 


                        System.Threading.ThreadPool.QueueUserWorkItem(delegate
                        {
                            //Todo write to database

                            NavSQLExecute conn = new NavSQLExecute();
                            conn.InsertKöpsbudgetLine(productNumber.ToString(), kopDate.ToString(format), ammountToKop);

                        }, null);

                        //conn.InsertBudgetLine(tempCustNumber, productNumber, answer.ToString(format), ammount);

                    }
                }
            }
            else if ((GetValueFromGridAsString(rowIndex, 2) == "SalgsbudgetReguleret_TY") && columnIndex > 2 && columnName.Contains("20")) // 1 should be your column index
            {
                int i;
                string productNumber = GetProductNumberFromRow(rowIndex);
                int week = weekFromColName;
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
                        var salgsbudget = Products.
                            Where(x => x.ProductNumber.Equals(productNumber)).
                            Select(x => x.SalgsbudgetReguleret_ThisYear).First();
                        if (Convert.ToInt32(e.FormattedValue) != salgsbudget[week])
                        {
                            int ammount = Convert.ToInt32(e.FormattedValue) - salgsbudget[week];

                            if (dataGridForecastInfo.Rows[rowIndex - 1].Cells[columnIndex].Value.ToString() != "")
                            {
                                SetRegulerat1102(week, productNumber.ToString(), Convert.ToInt32(e.FormattedValue));

                                int sBudget = Convert.ToInt32(dataGridForecastInfo.Rows[rowIndex - 2].Cells[columnIndex].Value);
                                int regSbudget = Convert.ToInt32(e.FormattedValue);
                                if ((GetValueFromGridAsString(rowIndex + 1, 2).Contains("Salgsbudget_Summeret")))
                                {
                                    dataGridForecastInfo.Rows[rowIndex + 1].Cells[columnIndex].Value = sBudget + regSbudget;
                                }

                                string comment = dataGridForecastInfo.Rows[rowIndex - 1].Cells[columnIndex].Value.ToString();

                                //DateTime tempDate = StaticVariables.FirstSaturdayBeforeWeakOne(selectedYear);
                                //DateTime answer = tempDate.AddDays((week - 1) * 7);
                                DateTime kopDate = StaticVariables.GetForecastStartDateOfWeeknumber(yearFromColName, weekFromColName);
                                kopDate = kopDate.AddDays(2); //Monday
                                string format = "yyyy-MM-dd HH:mm:ss";    // modify the format depending upon input required in the column in database 

                                NavSQLExecute conn = new NavSQLExecute();
                                conn.InsertReguleretBudgetLine(productNumber.ToString(), kopDate.ToString(format), ammount, comment);

                                dataGridForecastInfo.Rows[rowIndex - 1].Cells[columnIndex].Value = "";
                                dataGridForecastInfo.Rows[rowIndex - 1].Cells[columnIndex].Style = new DataGridViewCellStyle { BackColor = Color.Yellow };

                            }
                            else
                            {
                                dataGridForecastInfo.Rows[rowIndex].Cells[columnIndex].Value = salgsbudget[week];
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
            PrognosInfoForSupply salgsbudget = Products.
                            Where(x => x.ProductNumber.Equals(prodNBR)).
                            First();
            salgsbudget.UpdateReguleretInfo(selectedYear);

            dataGridForecastInfo.Visible = true;
            LoadReadyStatus();
        }

        private void FormSupply_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.WriteLine("Close supply form");
            formSalesInstance.Close();
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
            //int prodNBRint = productNumber;
            if (Products.Any(x => x.ProductNumber.Equals(productNumber)))
            {
                Products.
                            Where(x => x.ProductNumber.Equals(productNumber)).
                            Select(x => x.Kopsbudget_ThisYear).First()[week] = Convert.ToInt32(value);
            }

        }

        public void SetRegulerat1102(int week, string productNumber, int value)
        {
            if (Products.Any(x => x.ProductNumber.Equals(productNumber)))
            {
                Products.
                            Where(x => x.ProductNumber.Equals(productNumber)).
                            Select(x => x.SalgsbudgetReguleret_ThisYear).First()[week] = Convert.ToInt32(value);

            }
        }

        public void SetProductRegComment(string comment)
        {
            if (selectedYear > 2000)
            {
                dataGridForecastInfo.Rows[latestRow].Cells[latestClickedWeek + 2].Value = comment;
            }
            else
            {
                int startYear = DateTime.Now.Year;
                int weeknumber = StaticVariables.GetForecastWeeknumberForDate(DateTime.Now);
                //weeknumber = 40;

                if (StaticVariables.TestWeek > 0)
                {
                    weeknumber = StaticVariables.TestWeek;
                }

                int startWeek = weeknumber - 20;
                if (startWeek < 1)
                {
                    startYear = startYear - 1;
                    startWeek = startWeek + 52;
                }

                int ClickedColumn = latestClickedWeek - startWeek + 3;
                if (ClickedColumn < 1)
                {
                    ClickedColumn = ClickedColumn + 52;
                }
                dataGridForecastInfo.Rows[latestRow].Cells[ClickedColumn].Value = comment;
            }
        }

        private void buttonGetSupplierFromNBR_Click(object sender, EventArgs e)
        {
            StaticVariables.AbortLoad = false;
            if (StaticVariables.AllSuppliersM3.ContainsKey(numericSupplyNBR.Value.ToString()))
            {
                comboBoxSupplier.SelectedItem = numericSupplyNBR.Value.ToString() + "  " + StaticVariables.AllSuppliersNameDict[numericSupplyNBR.Value.ToString()];
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
                FocusNowColumn();
                LoadReadyStatus();
            }
            else
            {
                MessageBox.Show(new Form() { TopMost = true }, "No supplier with that number");
            }

        }

        private void comboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxYear.SelectedItem != null && comboBoxYear.SelectedItem != "Now")
            {
                selectedYear = (int)Convert.ToInt32(comboBoxYear.SelectedItem);
            }
            else
            {
                selectedYear = 1000;
            }
        }

        private void buttonGetProductByNumber_Click(object sender, EventArgs e)
        {
            dataGridForecastInfo.Visible = false;
            SetStatus("Loading Products");
            dataGridForecastInfo.ClearSelection();
            SetupColumns();

            Products = new List<PrognosInfoForSupply>();
            this.dataGridForecastInfo.DataSource = null;

            this.dataGridForecastInfo.Rows.Clear();
            this.dataGridForecastInfo.AllowUserToAddRows = false;
            this.dataGridForecastInfo.AllowUserToDeleteRows = false;
            this.dataGridForecastInfo.AllowUserToOrderColumns = false;

            string prodNMBR = textBoxProdNBR.Text;
            AddProductByNumber(prodNMBR);
            PrepareGUI();
            dataGridForecastInfo.Visible = true;
            FocusNowColumn();
            LoadReadyStatus();
        }

        private void buttonCreateM3LactalisOrders_Click(object sender, EventArgs e)
        {
            // CreateOrderForProduct(5, 23303);
        }
        /*
        private void CreateOrderForProduct(int week, int prodNumber)
        {
            if ((SupplierProducts.Count > 0)
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
        }*/

        private void buttonCreateFile_Click(object sender, EventArgs e)
        {

        }

        private void CreateLinesForOneProduct(Dictionary<int, int> kopesBudgetFormNextWeek_TY, int[] weekPartPercentage, int lactalisProdNBR, int lactaNBRPerKolli)
        {
            int dayFromNow = 0;

            int PerKolli = Math.Max(lactaNBRPerKolli, 1);


            for (int i = 1; i < kopesBudgetFormNextWeek_TY.Count; i++)
            {
                int numberPerWeek = kopesBudgetFormNextWeek_TY[i] / PerKolli;
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
            return temp;
        }

        private void buttonCreateLactalisFile_Click(object sender, EventArgs e)
        {
            SetStatus("Products to Forecast France Creating File");
            Stopwatch stopwatch = Stopwatch.StartNew();
            dayOrderStrings = new List<string>();

            List<string> productsToSendKopbudget = new List<string>();

            GetFromM3 m3 = new GetFromM3();
            Dictionary<string, string> productsInSkaevinge = m3.GetAllSkaevingeProductsDict();
            Dictionary<string, string> productsInFile = new Dictionary<string, string>();

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

            foreach (KeyValuePair<string, string> item in productsInFile)
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

            labelCalculator.Location = new System.Drawing.Point(10, this.Height - 70);
            textBoxCalculator.Location = new System.Drawing.Point(125, this.Height - 70);

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

        private void comboBoxSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBoxProdNBR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonGetProductByNumber.PerformClick();
            }
        }

        private void buttonGetProductsBySupplier_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Om Escape trycks ned vid hämtning av produkter på kund avbryts hämtningen
            if (e.KeyChar == (char)27)
            {
                StaticVariables.AbortLoad = true;
            }
        }

        private void ShowFirstTen()
        {
            PrepareGUI();
            dataGridForecastInfo.Visible = true;
            FocusNowColumn();
            Application.DoEvents();
        }

        internal void ScrollToNumber(string number)
        {
            string tempNumber = number;

            number = number.Trim();

            for (int i = 0; i < dataGridForecastInfo.RowCount; i++)
            {
                string value = GetValueFromGridAsString(i, 0);
                if (value.Contains(number))
                {
                    dataGridForecastInfo.FirstDisplayedScrollingRowIndex = i;
                    return;
                }

            }

        }

        private void FocusNowColumn()
        {
            //Scroll to this week
            try
            {
                int columnNow = 0;
                for (int i = 0; i < dataGridForecastInfo.Columns.Count; i++)
                {
                    if (dataGridForecastInfo.Columns[i].Name.Contains("Now"))
                    {
                        columnNow = i;
                        break;
                    };
                }
                if (columnNow > 0)
                {
                    dataGridForecastInfo.FirstDisplayedScrollingColumnIndex = columnNow - 2;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Scroll Now error: " + ex.Message);
            }

        }


        private void textBoxCalculator_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                string temp = sender.GetType().ToString();
                if (temp.Contains("TextBox"))
                {
                    CountExpression(textBoxCalculator.Text);
                }
                //if (e.GetType() == System.Windows.Forms.TextBox)
            }
        }


        private void CountExpression(string calcString)
        {
            try
            {
                var loDataTable = new DataTable();
                var loDataColumn = new DataColumn("Eval", typeof(double), calcString);
                loDataTable.Columns.Add(loDataColumn);
                loDataTable.Rows.Add(0);
                double tempD = (double)loDataTable.Rows[0]["Eval"];
                tempD = Math.Floor(tempD);
                int answer = Convert.ToInt32(tempD);
                string temp = answer.ToString();
                textBoxCalculator.Text = temp;
                return;
            }
            catch
            {
                return;
            }
        }

        private void checkBoxLastYear_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
