///This is the GUI File for the Sales view 
///This GUI runs all action for Sales to focus on
///Download numbers and save ForeCasts based on customers

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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsForecastLactalis
{
    public partial class FormSales : Form
    {
        private static List<PrognosInfoSales> Products = new List<PrognosInfoSales>();
        public FormSupply supplyViewInstance;
        public ScrollToProdNumber scrollInfoInstance;
        private string latestComboBoxText = "";
        GetFromM3 m3Info = new GetFromM3();
        TextBoxForm infoboxSales = new TextBoxForm();
        private string latestProductNumber;
        private string latestCustomerLoaded;
        private int latestClickedWeek;
        private int latestClickedYear;
        Point latestMouseClick;
        int selectedYear;
        bool AssortmentFromM3 = true;
        StaticVariables.WritePermission OnlyLook;

        int forecastNbrWeeks;

        private bool loadingNewProductsOngoing;

        public Dictionary<string, string> allProductsDict = new Dictionary<string, string>();

        public Dictionary<string, string> allProductsM3Dict = new Dictionary<string, string>();
        Dictionary<string, List<string>> allSuppliersM3 = new Dictionary<string, List<string>>();
        public Dictionary<string, string> newNumberDict = new Dictionary<string, string>();

        Dictionary<string, List<string>> allkedjaToCustM3 = new Dictionary<string, List<string>>();
        int latestCommentRow;
        string newCommentProdNBR = "";

        public FormSales()
        {
            var loginForm = new FormLogin();

            AssortmentFromM3 = true;
            InitializeComponent();
            Console.WriteLine("Start Form1!");


            forecastNbrWeeks = 20;
            latestClickedWeek = 1;
            latestClickedYear = 1000;
            m3Info.TestM3Connection();

            loadingNewProductsOngoing = false;
            //test with Coop and test customer
            FixCustomerChoices();


            SetupColumns();
            allProductsM3Dict = m3Info.GetAllSkaevingeProductsDict();
            allSuppliersM3 = m3Info.GetSupplierWithItemsDict();

            LoadAllProductDict();


            dataGridForecastInfo.Width = this.Width - 50;
            dataGridForecastInfo.Height = this.Height - 250;

            labelCalculator.Location = new System.Drawing.Point(10, this.Height - 70); ;
            textBoxCalculator.Location = new System.Drawing.Point(125, this.Height - 70);


            StaticVariables.SetAllProductsNavDict(allProductsDict);
            StaticVariables.InitiateDate();

            DateTime dateTemp = StaticVariables.GetForecastStartDateOfWeeknumber(2016, 31);
            Console.WriteLine("Wanted day1: " + dateTemp.ToString("MMMM dd, yyyy"));
            dateTemp = StaticVariables.GetForecastStartDateOfWeeknumber(2017, 1);
            Console.WriteLine("Wanted day2: " + dateTemp.ToString("MMMM dd, yyyy"));
            

        }


        //Populate the drop downlist wit customers
        private void FixCustomerChoices()
        {
            StaticVariables.SetCustDictionary();

            comboBoxAssortment.DropDownStyle = ComboBoxStyle.DropDownList;
            if (!AssortmentFromM3)
            {
                comboBoxAssortment.DataSource = new BindingSource(StaticVariables.AssortmentDictionaryNav, null);
                comboBoxAssortment.DisplayMember = "Key";
                comboBoxAssortment.ValueMember = "Key";
            }
            else
            {
                Dictionary<string, string> assormentList = m3Info.GetListOfAssortments();
                Dictionary<string, string> assormentListNation = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> item in assormentList)
                {
                    string nation = StaticVariables.GetNationForAssortment(item.Key);
                    assormentListNation.Add(item.Key, nation + "  " + item.Value);
                }
                if (assormentList != null)
                {
                    //Sort the assortment list by description
                    var sortedList = from entry in assormentListNation orderby entry.Value ascending select entry;
                    comboBoxAssortment.DataSource = new BindingSource(sortedList, null);
                }
                comboBoxAssortment.DisplayMember = "Value";
                comboBoxAssortment.ValueMember = "Key";
            }
            List<string> yearList = new List<string>();
            yearList.Add("Now");
            yearList.Add("2015");
            yearList.Add("2016");
            yearList.Add("2017");
            yearList.Add("2018");
            yearList.Add("2019");

            comboBoxYear.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxYear.DataSource = new BindingSource(yearList, null);
            comboBoxYear.SelectedIndex = comboBoxYear.Items.IndexOf("Now");
            checkBoxLastYear.Checked = true;
            checkBoxKampgn.Checked = true;
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

                        for (int i = (forecastNbrWeeks * 2 + 2); i < dataGridForecastInfo.Columns.Count-2; i++)
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

        Comparer _comparer = new Comparer(System.Globalization.CultureInfo.CurrentCulture);
        public int Compare(PrognosInfoSales x, PrognosInfoSales y)
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
            //Products.Sort();
            Products.Sort(Compare);
            foreach (PrognosInfoSales item in Products)
            {
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

                    int lockfromColumn = (item.WeekToLockFrom + 3 - weekNumberStart);
                    if (lockfromColumn < 3)
                    { 
                        lockfromColumn = lockfromColumn + 52;
                    }

                    weekToLock.Add(weekProdNBR, lockfromColumn); //+ 3 is offset from cell number to week number
                    weekProdNBR++;
                    List<object> tempList = new List<object>();
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

                    tempList.Add("RealiseretSalg_LastYear");
                    if (item.showLastYear)
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
                            var sum = from line in salesLastYear
                                      where line.Week == k
                                      select line.Sum;
                            tempList.Add(sum.FirstOrDefault());
                            //tempList.Add(item.RealiseretSalgs_LastYear[i]);
                        }
                    }
                    AddRowFromList(tempList);

                    //tempList = new List<object>();
                    //tempList.Add("");
                    //tempList.Add("");
                    //tempList.Add("RealiseretKampagn_LastYear");
                    //for (int i = weekNumberStart; i <= (weekNumberStart + (2 * forecastNbrWeeks + 1)); i++)
                    //{
                    //    if (i > 52)
                    //    {
                    //        k = i - 52;
                    //    }
                    //    else
                    //    {
                    //        k = i;
                    //    }
                    //    tempList.Add(item.RealiseretKampagn_LastYear[k]);
                    //}
                    //AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Salgsbudget_LastYear");
                    if (item.showLastYear)
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
                            tempList.Add(item.Salgsbudget_LastYear[k]);
                        }
                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Comment_LastYear");
                    if (item.showLastYear)
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
                            tempList.Add(item.Salgsbudget_CommentLY[k]);
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
                        //tempList.Add(item.RealiseratSalg_ThisYear[i]);
                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Nollat_ThisYear");
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
                        tempList.Add(item.Zeroed_ThisYear[k]);
                        //tempList.Add(item.RealiseratSalg_ThisYear[i]);
                    }
                    AddRowFromList(tempList);


                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Kampagn_ThisYear");
                    if (item.showLastKampagn)
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
                    tempList.Add("Salgsbudget_Comment");
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

                    commentDict = item.Salgsbudget_Comment;
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
                    tempList.Add("Salgsbudget_ChangeHistory");
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
                        tempList.Add(item.Salgsbudget_ChangeHistory[k]);
                    }
                    AddRowFromList(tempList);
                    commentDict = item.Salgsbudget_Comment;
                }
                //Selected week not now
                else
                {
                    weekToLock.Add(weekProdNBR, item.WeekToLockFrom + 2); //+ 2 is offset from cell number to week number
                    weekProdNBR++;
                    List<object> tempList = new List<object>();
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

                    tempList.Add("RealiseretSalg_LastYear");
                    if (item.showLastYear)
                    {
                        for (int i = 1; i < 54; i++)
                        {
                            var sum = from line in salesLastYear
                                      where line.Week == i
                                      select line.Sum;
                            tempList.Add(sum.FirstOrDefault());
                            //tempList.Add(item.RealiseretSalgs_LastYear[i]);
                        }
                    }
                    AddRowFromList(tempList);

                    //tempList = new List<object>();
                    //tempList.Add("");
                    //tempList.Add("");
                    //tempList.Add("RealiseretKampagn_LastYear");
                    //for (int i = 1; i < 54; i++)
                    //{
                    //    tempList.Add(item.RealiseretKampagn_LastYear[i]);
                    //}
                    //AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Salgsbudget_LastYear");
                    if (item.showLastYear)
                    {
                        for (int i = 1; i < 54; i++)
                        {
                            tempList.Add(item.Salgsbudget_LastYear[i]);
                        }
                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Comment_LastYear");
                    if (item.showLastYear)
                    {

                        for (int i = 1; i < 54; i++)
                        {
                            tempList.Add(item.Salgsbudget_CommentLY[i]);
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
                        //tempList.Add(item.RealiseratSalg_ThisYear[i]);
                    }
                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Nollat_ThisYear");

                    for (int i = 1; i < 54; i++)
                    {
                        tempList.Add(item.Zeroed_ThisYear[i]);
                    }

                    AddRowFromList(tempList);

                    tempList = new List<object>();
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("Kampagn_ThisYear");
                    if (item.showLastKampagn)
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
            }

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
                else if (Convert.ToString(row.Cells[2].Value).Contains("Nollat"))
                {
                    row.DefaultCellStyle.ForeColor = Color.Chocolate;
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
                    row.Cells[2].ReadOnly = true;
                    row.Cells[2].Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                    bool writingForbidden = !(OnlyLook == StaticVariables.WritePermission.SaleWrite || OnlyLook == StaticVariables.WritePermission.Write);
                    for (int i = 3; i < row.Cells.Count; i++)
                    {
                        if ((selectedYear > 2000 && selectedYear < DateTime.Now.Year) || writingForbidden) //past years
                        {
                            row.Cells[i].ReadOnly = true;
                            row.Cells[i].Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                        }
                        else if (selectedYear == DateTime.Now.Year || selectedYear == 1000)
                        {
                            if (weekToLock[weekProdNBR] < i )
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
                        else if (selectedYear - 1 > DateTime.Now.Year)
                        {
                            //if year more then one year in future always allow writing
                            row.Cells[i].ReadOnly = false;
                            row.Cells[i].Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                        }
                        else if (selectedYear - 1 == DateTime.Now.Year )
                        {
                            if (weekToLock[weekProdNBR] - 52 < i)
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



                    }
                    weekProdNBR++;
                }
                else if (Convert.ToString(row.Cells[2].Value).Contains("Commen"))
                {
                    row.ReadOnly = true;
                    if (Convert.ToString(row.Cells[2].Value).Contains("Last"))
                    {
                        row.DefaultCellStyle.ForeColor = Color.Blue;
                    }
                }
                else if (Convert.ToString(row.Cells[2].Value) == "RealiseratSalg_ThisYear")
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

        public string GetNameFromLoadedProducts(string prodNBR)
        {
            string temp = "Name Unknown";
            //First try to see if you find the name in M3 on Warehouse
            //If not try with old Forecastdatabase, if not found Name Unknown
            if (StaticVariables.AllProductsM3Dict.ContainsKey(prodNBR))
            {
                temp = StaticVariables.AllProductsM3Dict[prodNBR];
            }
            else if (StaticVariables.AllProductsNavDict.ContainsKey(prodNBR))
            {
                temp = StaticVariables.AllProductsNavDict[prodNBR];
            }
            return temp;
        }

        //Add testinfo Products only for test purposes
        private void CreateProducts()
        {
            string tempCustNumber = StaticVariables.AssortmentDictionaryNav[comboBoxAssortment.Text];
            //PrognosInfoSales product1 = new PrognosInfoSales(GetNameFromLoadedProducts("2432"), "2432", tempCustNumber, 20);
            //PrognosInfoSales product2 = new PrognosInfoSales(GetNameFromLoadedProducts("1442"), "1442", tempCustNumber, 20);
            //PrognosInfoSales product3 = new PrognosInfoSales(GetNameFromLoadedProducts("1238"), "1238", tempCustNumber, 20);
            //PrognosInfoSales product4 = new PrognosInfoSales(GetNameFromLoadedProducts("2442"), "2442", tempCustNumber, 20);
            //PrognosInfoSales product5 = new PrognosInfoSales(GetNameFromLoadedProducts("2735"), "2735", tempCustNumber, 20);

            //latestCustomerLoaded = tempCustNumber;
            //SetStatus("Products loading 1/5");
            //product1.FillNumbers(selectedYear);
            //SetStatus("Products loading 2/5");
            //product2.FillNumbers(selectedYear);
            //SetStatus("Products loading 3/5");
            //product3.FillNumbers(selectedYear);
            //SetStatus("Products loading 4/5");
            //product4.FillNumbers(selectedYear);
            //SetStatus("Products loading 5/5");
            //product5.FillNumbers(selectedYear);

            //Products.Add(product1);
            //Products.Add(product2);
            //Products.Add(product3);
            //Products.Add(product4);
            //Products.Add(product5);
            labelStatus.Visible = false;
        }

        private void SetStatus(string status)
        {
            labelStatus.Text = status;
            buttonSupplyView.Enabled = false;
            button1.Enabled = false;
            buttonGetProductByNumber.Enabled = false;
            comboBoxYear.Enabled = false;
            labelStatus.Invalidate();
            labelStatus.Update();
            labelStatus.Refresh();
            Application.DoEvents();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
                if (OnlyLook == StaticVariables.WritePermission.SupplWrite || OnlyLook == StaticVariables.WritePermission.Write)
                {
                    supplyViewInstance.SetOnlyLook(false);
                }
                else
                {
                    supplyViewInstance.SetOnlyLook(true);
                }

                this.Visible = false;
                supplyViewInstance.SetFormSalesInstanse(this);
                supplyViewInstance.BringToFront();
                supplyViewInstance.Show();
            }
            else
            {
                MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before changing view!");
            }
        }

        //Load products by customer
        private void button1_Click(object sender, EventArgs e)
        {
            StaticVariables.AbortLoad = false;
            if (comboBoxYear.SelectedItem != null && comboBoxYear.SelectedItem.ToString() != "Now")
            {
                selectedYear = (int)Convert.ToInt32(comboBoxYear.SelectedItem);
            }
            else
            {
                selectedYear = 1000;
            }


            labelStatus.Visible = true;
            dataGridForecastInfo.Visible = false;
            SetStatus("Loading Products");

            loadingNewProductsOngoing = true;
            Stopwatch stopwatch = Stopwatch.StartNew();

            SetupColumns();

            if (!infoboxSales.Visible)
            {
                if (!AssortmentFromM3)
                {
                    Console.WriteLine("Hittepåkund vald");
                    Products = new List<PrognosInfoSales>();
                    CreateProducts();

                    FillSalesGUIInfo();
                }
                else
                {
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
                            //Avbryt hämtning av fler artiklar om användaren trycker på Escape
                            if (StaticVariables.AbortLoad || labelStatus.Visible == false)
                            {
                                StaticVariables.AbortLoad = false;
                                break;
                            }
                            i++;
                            string temp = GetNameFromLoadedProducts(item.ToString());
                            int status = 0;
                            if (StaticVariables.dictItemStatus.ContainsKey(item))
                            {
                                status = StaticVariables.dictItemStatus[item];
                            }
                            PrognosInfoSales product1 = new PrognosInfoSales(temp, item, tempString, status);
                            product1.SetWhatToLoad(checkBoxLastYear.Checked, checkBoxKampgn.Checked);
                            product1.FillNumbers(selectedYear);
                            Products.Add(product1);
                            if (i == 1 || i == 10 || i == 3)
                            {
                                ShowFirstTen();
                            }
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
            FocusNowColumn();
        }


        //Set comment from outside this form (textbox)
        public void SetProductComment(string comment)
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
            dataGridForecastInfo.Rows[latestCommentRow].Cells[ClickedColumn].Value = comment;
        }

        public void SetProductCommentImmediately(string comment)
        {
            string tempComment = comment;
            string cleanComment = System.Text.RegularExpressions.Regex.Replace(tempComment, "[áàäâãåÁÀÄÂÃÅ]", "a");
            cleanComment = System.Text.RegularExpressions.Regex.Replace(cleanComment, "[óòöôõÓÒÖÔÕ]", "o");
            string thisDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            cleanComment = ": " + cleanComment;
            if (cleanComment.Length > 49)
            {
                cleanComment = cleanComment.Substring(0, 49);
            }
            string tempCustNumber = StaticVariables.GetCustNavCodeFirst(latestCustomerLoaded); ;
            string productNumber = GetProductNumberFromRow(latestCommentRow);
            DateTime tempDate = StaticVariables.FirstSaturdayBeforeWeakOne(latestClickedYear);

            DateTime dateForComment = StaticVariables.GetForecastStartDateOfWeeknumber(latestClickedYear, latestClickedWeek);
            dateForComment = dateForComment.AddDays(2); //Monday
            //DateTime answer = tempDate.AddDays((week - 1) * 7+2);
            string format = "yyyy-MM-dd HH:mm:ss";
            DateTime time = DateTime.Now;
            Console.WriteLine("TasteDato: " + time.ToString(format));
            string inputNow = time.ToString(format);
            string tempAssortment = comboBoxAssortment.Text;
            NavSQLExecute conn = new NavSQLExecute();
            conn.InsertBudgetLine(tempCustNumber, tempAssortment, productNumber, dateForComment.ToString(format), 0, inputNow, cleanComment);
            this.SetCommentWithoutForecastValue(latestClickedWeek, productNumber, cleanComment);
        }


        private void buttonSupplyView_MouseClick(object sender, MouseEventArgs e)
        {

        }


        private void Form1_MouseClick(object sender, MouseEventArgs e)
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
        }


        //Velidate that input is valid value
        private void dataGridForecastInfo_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            String columnName = this.dataGridForecastInfo.Columns[e.ColumnIndex].Name;

            //Do not validate while new products is loading
            //if (!loadingNewProductsOngoing && GetValueFromGridAsString(rowIndex, 2) == "Salgsbudget_ThisYear" && columnIndex > 2 && columnName.Contains("20"))//(rowIndex % 7 == 5 && columnIndex > 2) // 1 should be your column index
            if (GetValueFromGridAsString(rowIndex, 2) == "Salgsbudget_ThisYear" && columnIndex > 2 && columnName.Contains("20"))//(rowIndex % 7 == 5 && columnIndex > 2) // 1 should be your column index
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
                                //String columnName = this.dataGridForecastInfo.Columns[columnIndex].Name;
                                int weekTest = StaticVariables.GetWeekFromName(columnName);
                                int yearTest = StaticVariables.GetYearFromName(columnName);
                                //here a new budget_line post should be added to the database
                                int ammount = Convert.ToInt32(e.FormattedValue) - item.Salgsbudget_ThisYear[weekTest];
                                if (ammount != 0)
                                {
                                    DateTime budgetDate = DateTime.Now;
                                    string tempCustNumber = StaticVariables.GetCustNavCodeFirst(latestCustomerLoaded); ;
                                    if (selectedYear > 2000)
                                    {

                                        DateTime tempDate = StaticVariables.GetDateTimeFromString(StaticVariables.StartDate[selectedYear].ToString());
                                        tempDate = StaticVariables.GetDateTimeFromString(StaticVariables.StartDate[selectedYear].ToString());
                                        budgetDate = tempDate.AddDays((week - 1) * 7);

                                        //DateTime budgetDate = StaticVariables.GetForecastStartDateOfWeeknumber(latestClickedYear, latestClickedWeek);
                                        budgetDate = budgetDate.AddDays(2); //monday
                                    }


                                    DateTime budgetDate2 = StaticVariables.GetForecastStartDateOfWeeknumber(yearTest, weekTest);
                                    budgetDate2 = budgetDate2.AddDays(2); //monday

                                    if (selectedYear > 2000)
                                    {
                                        Console.WriteLine("Budget date old: " + budgetDate.ToString() + "   Budget date new: " + budgetDate2.ToString());

                                    }
                                    string format = "yyyy-MM-dd HH:mm:ss";    // modify the format depending upon input required in the column in database 
                                    string tempAssortment = comboBoxAssortment.Text;
                                    DateTime time = DateTime.Now;              // Use current time

                                    Console.WriteLine("TasteDato: " + time.ToString(format));
                                    string inputNow = time.ToString(format);
                                    string tempComment = GetValueFromGridAsString(rowIndex + 1, columnIndex);
                                    dataGridForecastInfo.Rows[rowIndex + 1].Cells[columnIndex].Value = "";
                                    string cleanComment = System.Text.RegularExpressions.Regex.Replace(tempComment, "[áàäâãåÁÀÄÂÃÅ]", "a");
                                    cleanComment = System.Text.RegularExpressions.Regex.Replace(cleanComment, "[óòöôõÓÒÖÔÕ]", "o");
                                    string thisDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    cleanComment = thisDate.Substring(5, 11) + ": " + cleanComment;
                                    if (cleanComment.Length > 49)
                                    {
                                        cleanComment = cleanComment.Substring(0, 49);
                                    }
                                    if (tempComment.Length <= 1)
                                    {
                                        System.Threading.ThreadPool.QueueUserWorkItem(delegate
                                        {
                                            NavSQLExecute conn = new NavSQLExecute();
                                            conn.InsertBudgetLine(tempCustNumber, tempAssortment, productNumber, budgetDate2.ToString(format), ammount, inputNow, "");
                                        }, null);
                                    }
                                    else
                                    {
                                        newCommentProdNBR = productNumber;
                                        NavSQLExecute conn = new NavSQLExecute();
                                        conn.InsertBudgetLine(tempCustNumber, tempAssortment, productNumber, budgetDate2.ToString(format), ammount, inputNow, "");
                                    }

                                    SetKöpsbudget(weekTest, productNumber, Convert.ToInt32(e.FormattedValue), "", ammount);
                                }
                            }
                        }
                    }
                }
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

            if (columnIndex >= 0)
            {
                String columnName = this.dataGridForecastInfo.Columns[e.ColumnIndex].Name;

                if (comboBoxYear.SelectedItem != null && comboBoxYear.SelectedItem != "Now")
                {
                    selectedYear = (int)Convert.ToInt32(comboBoxYear.SelectedItem);
                }
                else
                {
                    selectedYear = 1000;
                }

                latestMouseClick = System.Windows.Forms.Cursor.Position;
                Console.WriteLine("Value clicked... Column index: " + columnIndex + "  rowIndex: " + rowIndex);
                if (columnIndex == 0)
                {
                    Console.WriteLine("Here We are");
                    if (scrollInfoInstance != null)
                    {
                        scrollInfoInstance.Close();
                    }
                    scrollInfoInstance = new ScrollToProdNumber(this, true);
                    scrollInfoInstance.StartPosition = FormStartPosition.Manual;
                    scrollInfoInstance.Location = latestMouseClick;
                    scrollInfoInstance.Show();
                }
                if (columnIndex > 2 && columnName.Contains("20"))
                {
                    if (GetValueFromGridAsString(rowIndex, 2).Contains("Comment"))
                    {

                        if (!infoboxSales.Visible)
                        {
                            latestCommentRow = rowIndex;
                            string temp = GetValueFromGridAsString(rowIndex, columnIndex);
                            latestClickedWeek = StaticVariables.GetWeekFromName(columnName);
                            latestClickedYear = StaticVariables.GetYearFromName(columnName);//columnIndex - 2;
                            latestProductNumber = GetProductNumberFromRow(rowIndex);

                            foreach (PrognosInfoSales item in Products)
                            {
                                if (item.ProductNumber.ToString() == latestProductNumber)
                                {
                                    if (GetValueFromGridAsString(rowIndex, 2).Contains("Last"))
                                    {
                                        if (item.showLastYear)
                                        {
                                            temp = item.Salgsbudget_CommentLY[latestClickedWeek];
                                        }
                                    }
                                    else
                                    {
                                        temp = item.Salgsbudget_Comment[latestClickedWeek];
                                    }
                                }
                            }
                            infoboxSales.SetInfoText(this, "", " Product: " + latestProductNumber + " Week: " + latestClickedWeek);
                            infoboxSales.SetOldInfo(temp);
                            if (GetValueFromGridAsString(rowIndex, 2).Contains("Last"))
                            {
                                infoboxSales.IsReadOnly(true);
                            }
                            else
                            {
                                infoboxSales.IsReadOnly(CellIsReadOnly(rowIndex - 1, columnIndex));
                            }
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
                            infoboxSales.FocusTextBox(this);
                            infoboxSales.LastYearNoSave(GetValueFromGridAsString(rowIndex, 2).Contains("Last"));
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
                            latestClickedWeek = StaticVariables.GetWeekFromName(columnName);
                            latestClickedYear = StaticVariables.GetYearFromName(columnName);
                            string message = "";
                            string productNumber = GetProductNumberFromRow(rowIndex);
                            PrognosInfoSales prognosInfo = Products.FirstOrDefault(p => p.ProductNumber == productNumber);
                            var result = from rows in prognosInfo.PromotionLines where (rows.Week == latestClickedWeek && rows.Year == latestClickedYear) select rows;

                            GetFromM3 m3 = new GetFromM3();
                            ChainStructureHandler chainHandler = new ChainStructureHandler();
                            foreach (PromotionInfo promotion in result)
                            {
                                List<string> KadjeNiva = new List<string>();
                                if (promotion.PromotionType == PromotionInfo.PromotionTypeEnum.CHAIN)
                                {
                                    KadjeNiva = m3.GetAllKampaignLevelsByCuse(promotion.Chain);
                                }
                                else if (promotion.PromotionType == PromotionInfo.PromotionTypeEnum.CUSTOMER)
                                {
                                    KadjeNiva = chainHandler.GetChainsFromCustomer(promotion.Customer);
                                }
                                string value = comboBoxAssortment.SelectedValue.ToString();

                                //This if statement checks if the campaign is connected to this assortment
                                //if (KadjeNiva.Intersect(StaticVariables.AssortmentM3_toKedjor[value]).Any())
                                int num = (from asch in StaticVariables.AssortmentM3_toKedjor[value]
                                           from ch in KadjeNiva
                                           where ch.StartsWith(asch)
                                           select ch).Count();
                                if (num > 0)
                                {
                                    message = String.Format("{0, -30}\t{1, -20}\t{2, -20}\t{3, -20}{4}",
                                        promotion.Id + ". " + promotion.Description,
                                        promotion.ItemNumber,
                                        promotion.Week,
                                        promotion.Quantity,
                                        Environment.NewLine) + message;
                                }
                            }
                            if (message.Length > 0)
                            {
                                message = String.Format("{0, -30}\t{1, -20}\t{2, -20}\t{3, -20}{4}",
                                        "Kampagn             ",
                                        "Produkt",
                                        "Vecka",
                                        "Antal",
                                        Environment.NewLine) + message;
                                MessageBox.Show(message);
                            }
                        }
                    }
                    else if (GetValueFromGridAsString(rowIndex, 2).Contains("History"))
                    {
                        if (!infoboxSales.Visible)
                        {
                            string prodNBRTemp = GetProductNumberFromRow(rowIndex);
                            string tempNewName = prodNBRTemp;
                            int latestWeek = StaticVariables.GetWeekFromName(columnName);
                            string temp = "";

                            SQLCallsSalesCustomerInfo sqlConnection = new SQLCallsSalesCustomerInfo();
                            sqlConnection.SetYear(selectedYear);
                            //string tempCustNumber = ClassStaticVaribles.CustDictionary[comboBoxAssortment.Text];
                            temp = sqlConnection.GetSalesBudgetWeekInfo(latestWeek, prodNBRTemp, StaticVariables.GetCustNavCodes(latestCustomerLoaded));

                            temp = "Salgsbudget" + " Product: " + tempNewName + " Week: " + latestWeek + "\n\n" + temp;
                            MessageBox.Show(temp);
                        }
                        else
                        {
                            MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window to see info!");
                        }
                    }
                    else if (GetValueFromGridAsString(rowIndex, 2).Contains("RealiseratSalg_ThisYear"))
                    {
                        if (!infoboxSales.Visible)// && GetValueFromGridAsString(rowIndex, columnIndex) != "0")
                        {
                            latestClickedWeek = StaticVariables.GetWeekFromName(columnName);
                            latestClickedYear = StaticVariables.GetYearFromName(columnName);
                            string productNumber = GetProductNumberFromRow(rowIndex);
                            PrognosInfoSales prognosInfo = Products.FirstOrDefault(p => p.ProductNumber == productNumber);
                            Console.WriteLine(" Product: " + productNumber + " Week: " + latestClickedWeek);
                            DateTime startDate = StaticVariables.GetForecastStartDateOfWeeknumber(latestClickedYear, latestClickedWeek);
                            DateTime endDate = startDate.AddDays(7);
                            int nolladeTotalt = 0;
                            try
                            {
                                SQL_M3Direct m3Sql = new SQL_M3Direct();
                                nolladeTotalt = m3Sql.GetZeroedForSpecificCustomer(startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd"), prognosInfo.CustomerNumberM3, prognosInfo.ProductNumber);
                                m3Sql.Close();
                            }
                            catch
                            {
                                MessageBox.Show("Please install Client Access to be able to se totally Zeroed sales-orders");
                            }

                            SaleInfo sf = new SaleInfo(" Product: " + productNumber + " Week: " + latestClickedWeek);
                            sf.LoadSaleInfo(prognosInfo.SalesRowsThisYear, latestClickedWeek, nolladeTotalt, false);
                            sf.Show();
                        }
                    }
                    else if (GetValueFromGridAsString(rowIndex, 2).Contains("RealiseretSalg_LastYear"))
                    {
                        if (!infoboxSales.Visible)
                        {
                            latestClickedWeek = StaticVariables.GetWeekFromName(columnName);
                            latestClickedYear = StaticVariables.GetYearFromName(columnName);
                            string productNumber = GetProductNumberFromRow(rowIndex);
                            PrognosInfoSales prognosInfo = Products.FirstOrDefault(p => p.ProductNumber == productNumber);
                            Console.WriteLine(" Product: " + productNumber + " Week: " + latestClickedWeek);
                            DateTime startDate = StaticVariables.GetForecastStartDateOfWeeknumber(latestClickedYear - 1, latestClickedWeek);
                            DateTime endDate = startDate.AddDays(7);

                            int nolladeTotalt = 0;
                            try
                            {
                                SQL_M3Direct m3Sql = new SQL_M3Direct();
                                nolladeTotalt = m3Sql.GetZeroedForSpecificCustomer(startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd"), prognosInfo.CustomerNumberM3, prognosInfo.ProductNumber);
                                m3Sql.Close();
                            }
                            catch
                            {
                                MessageBox.Show("Please install Client Access to be able to se  totally Zeroed sales-orders");
                            }


                            SaleInfo sf = new SaleInfo(" Product: " + productNumber + " Week: " + latestClickedWeek);
                            sf.LoadSaleInfo(prognosInfo.SalesRowsLastYear, latestClickedWeek, nolladeTotalt, false);
                            sf.Show();

                        }
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

        private bool CellIsReadOnly(int row, int col)
        {
            return dataGridForecastInfo.Rows[row].Cells[col].ReadOnly;
        }

        public void SetKöpsbudget(int week, string productNumber, int value, string comment, int changeValue)
        {
            foreach (PrognosInfoSales item in Products)
            {
                if (item.ProductNumber.ToString() == productNumber)
                {
                    item.Salgsbudget_ThisYear[week] = Convert.ToInt32(value);
                    //item.Salgsbudget_Comment[week] = item.Salgsbudget_Comment[week] + "\n " + Convert.ToInt32(changeValue) + ' ' + comment;
                }
            }
        }

        public void SetCommentWithoutForecastValue(int week, string productNumber, string comment)
        {
            foreach (PrognosInfoSales item in Products)
            {
                if (item.ProductNumber.ToString() == productNumber)
                {
                    item.Salgsbudget_Comment[week] = item.Salgsbudget_Comment[week] + "\n 0 " + comment;
                }
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
            string tempString = comboBoxAssortment.SelectedItem.ToString();
            tempString = tempString.Substring(1);
            string[] firstPart = tempString.Split(',');
            tempString = firstPart[0];

            string tempCustNumber = tempString;
            string temp = GetNameFromLoadedProducts(prodNMBR);

            List<string> productList = m3Info.GetListOfProductsNbrByAssortment(tempString);

            if (productList != null && productList.Contains(prodNMBR))
            {

                PrognosInfoSales product1 = new PrognosInfoSales(temp, prodNMBR, tempCustNumber, 20);
                product1.SetWhatToLoad(checkBoxLastYear.Checked, checkBoxKampgn.Checked);
                latestCustomerLoaded = tempCustNumber;
                product1.FillNumbers(selectedYear);
                Products.Add(product1);
                FillSalesGUIInfo();
                LoadingReady();
                FocusNowColumn();
            }
            else
            {
                MessageBox.Show("Product not in Assortment for this customer");
                LoadingReady();
            }
        }

        private void LoadingReady()
        {
            loadingNewProductsOngoing = false;
            dataGridForecastInfo.Visible = true;
            labelStatus.Visible = false;
            buttonSupplyView.Enabled = true;
            button1.Enabled = true;
            buttonGetProductByNumber.Enabled = true;
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


            labelCalculator.Location = new System.Drawing.Point(10, this.Height - 70);
            textBoxCalculator.Location = new System.Drawing.Point(125, this.Height - 70);
            dataGridForecastInfo.Width = this.Width - 50;
            dataGridForecastInfo.Height = this.Height - 250;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("Close Sales form");
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

            foreach (PrognosInfoSales prognos in Products)
            {
                if (prognos.ProductNumber == newCommentProdNBR)
                {
                    prognos.UpdateSalesInfo(selectedYear);
                }
            }

            LoadingReady();
        }

        internal void SetOnlyLook(StaticVariables.WritePermission permission)
        {
            OnlyLook = permission;
        }

        internal void SetProdOrTestHeading(bool prod)
        {
            if (prod)
            {
                this.Text = "Forecast Sales Production Environment  " + Application.ProductVersion;
            }
            else
            {
                this.Text = "Forecast Sales Test Environment  " + Application.ProductVersion; ;
                this.BackColor = Color.WhiteSmoke;
            }
        }

        private void textBoxProdNBR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonGetProductByNumber.PerformClick();
            }
        }


        private void FormSales_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Om Escape trycks ned vid hämtning av produkter på kund avbryts hämtningen
            if (e.KeyChar == (char)27)
            {
                StaticVariables.AbortLoad = true;
            }
        }


        private void ShowFirstTen()
        {
            FillSalesGUIInfo();
            dataGridForecastInfo.Visible = true;
            FocusNowColumn();
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

        private void textBoxCalculator_Enter(object sender, EventArgs e)
        {

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

        private void comboBoxLand_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxAssortment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
