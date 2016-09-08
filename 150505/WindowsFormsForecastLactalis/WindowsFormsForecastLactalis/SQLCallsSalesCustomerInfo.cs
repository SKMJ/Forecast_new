///This file handles all loading of Sales information
///The file is some kind of layer between between the PrognosInfoSales.cs  and the actual SQL calls
///handle in NavSQLExecute.cs 

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    public class SQLCallsSalesCustomerInfo
    {
        DataTable latestQueryTable; //Holds latest loaded Query output
        DataTable latestQueryTable2; //Holds latest loaded Query output if 2 needed
        NavSQLExecute conn;
        String Beskrivelse = "A";

        Dictionary<string, string> allCustomers = new Dictionary<string, string>();
        Dictionary<int, int> salesBudgetTY = new Dictionary<int, int>();
        Dictionary<int, int> salesBudgetLY = new Dictionary<int, int>();
        Dictionary<int, int> realKampagn_LY = new Dictionary<int, int>();
        Dictionary<int, int> kampagn_TY = new Dictionary<int, int>();

        Dictionary<int, string> startDateStrings = new Dictionary<int, string>();
        Dictionary<int, string> endDateStrings = new Dictionary<int, string>();

        public Dictionary<int, string> Salgsbudget_Comment = new Dictionary<int, string>();
        List<ISalesRow> lastYearRowsM3 = new List<ISalesRow>();
        List<ISalesRow> lastYearRowsNAV = new List<ISalesRow>();

        Dictionary<string, string> infoDict = new Dictionary<string, string>();
        int currentSelectedYear = 2015;

        enum TypeEnum
        {
            SalgsBudget = 4,
            SalgsBudget_Regulering = 5,
            KøbsBudget = 6
        };


        //Constructor
        public SQLCallsSalesCustomerInfo()
        {
            InitiateProperties();

        }

        private void InitiateProperties()
        {
            startDateStrings.Add(2014, @"2013/12/29");
            endDateStrings.Add(2014, @"2014/12/29");
            startDateStrings.Add(2015, @"2014/12/28");
            endDateStrings.Add(2015, @"2016/01/04");
            startDateStrings.Add(2016, @"2016/01/03");
            endDateStrings.Add(2016, @"2016/12/31");
            startDateStrings.Add(2017, @"2017/01/01");
            endDateStrings.Add(2017, @"2017/12/31");
            startDateStrings.Add(2018, @"2018/01/01");
            endDateStrings.Add(2018, @"2018/12/31");
            startDateStrings.Add(2019, @"2018/12/31");
            endDateStrings.Add(2019, @"2019/12/30");

            //startDateStrings.Add(2014, @"2013/12/30");
            //endDateStrings.Add(2014, @"2014/12/30");
            //startDateStrings.Add(2015, @"2014/12/29");
            //endDateStrings.Add(2015, @"2016/01/05");
            //startDateStrings.Add(2016, @"2016/01/04");
            //endDateStrings.Add(2016, @"2017/01/01");
            //startDateStrings.Add(2017, @"2017/01/02");
            //endDateStrings.Add(2017, @"2018/01/01");

            for (int k = 0; k < 54; k++)
            {
                salesBudgetTY.Add(k, 0);
                salesBudgetLY.Add(k, 0);
                realKampagn_LY.Add(k, 0);
                kampagn_TY.Add(k, 0);
                Salgsbudget_Comment.Add(k, " ");
            }
        }

        public Dictionary<string, string> GetAllCustomers()
        {
            LoadAllCustomers();
            PrepareAllCustomers();
            return allCustomers;
        }

        private void LoadAllCustomers()
        {
            conn = new NavSQLExecute();

            string firstD = GetStartDate(currentSelectedYear);
            string endD = GetEndDate(currentSelectedYear);

            string query = @"select Navn_DebBogfGr, DebitorBogføringsruppe  from " +
                StaticVariables.TableDebitorBudgetLinjePost + " where  startdato >= '" + firstD + "' and startdato < '" + endD + "' order by Navn_DebBogfGr";
            Console.WriteLine("Load All Customers: ");
            latestQueryTable = conn.QueryExWithTableReturn(query);
            conn.Close();
        }

        private void PrepareAllCustomers()
        {
            DataRow[] currentRows = latestQueryTable.Select(null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
            {
                Console.WriteLine("No Supplier Current Rows Found");
            }
            else
            {
                //loop trough all rows and write in tabs
                foreach (DataRow row in currentRows)
                {
                    string custName = row["Navn_DebBogfGr"].ToString();
                    string custNumber = row["DebitorBogføringsruppe"].ToString();

                    if (!allCustomers.ContainsKey(custNumber))
                    {
                        allCustomers.Add(custNumber, custName);
                    }
                }
            }
        }

        private string GetEndDate(int year)
        {
            string endDate = "";
            int endYear = DateTime.Now.Year;
            int weeknumber = StaticVariables.GetForecastWeeknumberForDate(DateTime.Now);
            //weeknumber = 40;
            if (StaticVariables.TestWeek > 0)
            {
                weeknumber = StaticVariables.TestWeek;
            }
            if (endDateStrings.ContainsKey(year))
            {
                endDate = endDateStrings[year];

            }
            else
            {
                int endWeek = weeknumber + 20;
                if (endWeek > 52)
                {
                    endYear = endYear + 1;
                    endWeek = endWeek - 52;
                }

                if (year == 1000)
                {
                    DateTime endDatedate = StaticVariables.GetForecastStartDateOfWeeknumber(endYear, endWeek);
                    endDatedate = endDatedate.AddDays(7);
                    string date = endDatedate.ToString("yyyy/MM/dd");
                    date = date.Replace("-", @"/");
                    endDateStrings.Add(year, date);
                    endDate = date;
                }
                else if (year == 999)
                {
                    DateTime endDatedate = StaticVariables.GetForecastStartDateOfWeeknumber(endYear - 1, endWeek);
                    endDatedate = endDatedate.AddDays(7);
                    string date = endDatedate.ToString("yyyy/MM/dd");
                    date = date.Replace("-", @"/");
                    endDateStrings.Add(year, date);
                    endDate = date;
                }


            }

            return endDate;
        }

        private string GetStartDate(int year)
        {

            int weeknumber;
            string startDate = "";

            if (startDateStrings.ContainsKey(year))
            {
                startDate = startDateStrings[year];

            }
            else
            {
                int startYear = DateTime.Now.Year;
                weeknumber = StaticVariables.GetForecastWeeknumberForDate(DateTime.Now);
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


                if (year == 1000)
                {
                    DateTime stDate = StaticVariables.GetForecastStartDateOfWeeknumber(startYear, startWeek);
                    string date = stDate.ToString("yyyy/MM/dd");
                    date = date.Replace("-", @"/");
                    startDateStrings.Add(year, date);
                    startDate = date;

                    //Add also last year
                    stDate = StaticVariables.GetForecastStartDateOfWeeknumber(startYear - 1, startWeek);
                    date = stDate.ToString("yyyy/MM/dd");
                    date = date.Replace("-", @"/");
                    startDateStrings.Add(year-1, date);
                }
                else if (year == 999)
                {
                    DateTime stDate = StaticVariables.GetForecastStartDateOfWeeknumber(startYear - 1, startWeek);
                    string date = stDate.ToString("yyyy/MM/dd");
                    date = date.Replace("-", @"/");
                    startDateStrings.Add(year, date);
                    startDate = date;
                }

            }
            return startDate;
        }


        //Get sales buget data as dictionary
        public Dictionary<int, int> GetSalesBudgetTY(string prodNumber, List<string> customerName)
        {
            LoadSalesBudgetForProductFromSQL(prodNumber, customerName, currentSelectedYear);
            PrepareLoadedSalesBudgetForGUI(currentSelectedYear);
            return salesBudgetTY;
        }

        //Load the sales budget numbers from SQL Database
        private void LoadSalesBudgetForProductFromSQL(string prodNumber, List<string> customerName, int year)
        {
            conn = new NavSQLExecute();
            string query = "";
            string firstD = GetStartDate(year);
            string endD = GetEndDate(year);

            query = @"select Type, Antal, Debitorbogføringsruppe, Startdato, Navn_DebBogfGr, Tastedato, Kommentar from " +
                StaticVariables.TableDebitorBudgetLinjePost + " where ";
            bool first = true;
            foreach (string item in customerName)
            {
                if (first)
                {
                    first = false;
                    query = query + @"( Debitorbogføringsruppe='" + item + "' ";
                }
                else
                {
                    query = query + @" or Debitorbogføringsruppe='" + item + "' ";
                }
            }
            query = query + @") and Varenr='XXXX' and startdato >= '" + firstD + "' and startdato < '" + endD + "' order by Tastedato";
            query = query.Replace("XXXX", prodNumber);
            Console.WriteLine("Load Sales Budget Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(query);
            conn.Close();
        }

        //Get the data you need from sales budget 
        //Prepare the data for GUI
        private void PrepareLoadedSalesBudgetForGUI(int year)
        {
            DataRow[] currentRows = latestQueryTable.Select(null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
            {
                Console.WriteLine("No Supplier Current Rows Found");
            }
            else
            {
                //loop trough all rows and write in tabs
                int antalTot = 0;
                foreach (DataRow row in currentRows)
                {
                    string stringType = row["Type"].ToString();
                    int intType = Convert.ToInt32(stringType);
                    string levAntal = row["Antal"].ToString();
                    int Antal = Convert.ToInt32(levAntal);
                    int Antal2 = Convert.ToInt32(row["Antal"]);
                    string comment = row["Kommentar"].ToString();
                    string temp = "2016-11-11 11:11:11";
                    if (comment.StartsWith("2016-") && temp.Length < comment.Length)
                    {

                        comment = comment.Substring(temp.Length);
                    }
                    string dateforNumber = row["Tastedato"].ToString();
                    //string levKedja = row["Navn_DebBogfGr"].ToString();


                    DateTime tempDate = StaticVariables.GetDateTimeFromString(row["Startdato"].ToString());
                    int weekInt = StaticVariables.GetForecastWeeknumberForDate(tempDate);

                    if (intType == (int)TypeEnum.SalgsBudget && weekInt < 54 && weekInt >= 0)
                    {
                        if (year == currentSelectedYear)
                        {
                            salesBudgetTY[weekInt] = salesBudgetTY[weekInt] + Antal;
                            if (comment.Length > 0)
                            {
                                Salgsbudget_Comment[weekInt] = Salgsbudget_Comment[weekInt] + " \n " + Antal + " " + dateforNumber + " " + comment;
                            }
                        }
                        else if (year == currentSelectedYear - 1)
                        {
                            salesBudgetLY[weekInt] = salesBudgetLY[weekInt] + Antal;
                        }

                        antalTot = antalTot + Antal;
                    }
                }

            }
        }

        internal Dictionary<int, int> GetSalesBudget_LY(string ProductNumber, List<string> CustomerNumber)
        {
            LoadSalesBudgetForProductFromSQL(ProductNumber, CustomerNumber, currentSelectedYear - 1);
            PrepareLoadedSalesBudgetForGUI(currentSelectedYear - 1);
            return salesBudgetLY;
        }

        public Dictionary<int, int> GetRealiseretKampagnLY(string prodNumber, List<string> custNumber)
        {
            LoadRealiseretKampagnLY_FromSQL(prodNumber, custNumber);
            PrepareRealiseretKampagnLY_ForGUI();
            return realKampagn_LY;
        }

        //Load info for the RealiseretKampagnLY from SQL 
        private void LoadRealiseretKampagnLY_FromSQL(string prodNumber, List<string> custNumber)
        {
            conn = new NavSQLExecute();
            string firstD = GetStartDate(currentSelectedYear - 1);
            string endD = GetEndDate(currentSelectedYear - 1);

            string query = @"select startdato, Antal_Realiseret  from Afsl__Kampagnelinier where ";
            bool first = true;
            foreach (string item in custNumber)
            {
                if (first)
                {
                    first = false;
                    query = query + @"( Debitorbogføringsgruppe='" + item + "' ";
                }
                else
                {
                    query = query + @" or Debitorbogføringsgruppe='" + item + "' ";
                }
            }
            query = query + @") and Varenr='XXXX' and startdato >= '" + firstD + "' and startdato < '" + endD + "' order by startdato";
            query = query.Replace("XXXX", prodNumber.ToString());
            //queryString = queryString.Replace("YYYY", custNumber[0]);
            Console.WriteLine("LoadRealiseretKampagnLY Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(query);

            conn.Close();
        }

        private void PrepareRealiseretKampagnLY_ForGUI()
        {
            DataRow[] currentRows = latestQueryTable.Select(null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
            {
                Console.WriteLine("No Supplier Current Rows Found");
            }
            else
            {
                //loop trough all rows and write in tabs
                foreach (DataRow row in currentRows)
                {
                    string levAntal = row["Antal_Realiseret"].ToString();
                    int Antal = Convert.ToInt32(levAntal);


                    DateTime tempDate = StaticVariables.GetDateTimeFromString(row["startdato"].ToString());
                    int weekInt = StaticVariables.GetForecastWeeknumberForDate(tempDate);

                    if (weekInt > 0 && weekInt < 54)
                    {
                        Console.WriteLine("Antal Kampagn Rel: " + Antal);
                        realKampagn_LY[weekInt] = realKampagn_LY[weekInt] + Antal;
                    }
                }
            }
        }

        internal Dictionary<int, int> GetKampagnTY(string prodNumber, List<string> custNumber)
        {
            LoadKampagnTY_FromSQL(prodNumber, custNumber);
            PrepareKampagnTY_ForGUI();
            return kampagn_TY;
        }

        private void LoadKampagnTY_FromSQL(string prodNumber, List<string> custNumber)
        {
            conn = new NavSQLExecute();


            string firstD = GetStartDate(currentSelectedYear);
            string endD = GetEndDate(currentSelectedYear);

            string query = @"select startdato, Antal_Realiseret  from Afsl__Kampagnelinier where ";
            bool first = true;
            foreach (string item in custNumber)
            {
                if (first)
                {
                    first = false;
                    query = query + @"( Debitorbogføringsgruppe='" + item + "' ";
                }
                else
                {
                    query = query + @" or Debitorbogføringsgruppe='" + item + "' ";
                }
            }
            query = query + @")  and Varenr='XXXX' and startdato >= '" + firstD + "' and startdato < '" + endD + "' order by startdato";
            query = query.Replace("XXXX", prodNumber.ToString());
            //queryString = queryString.Replace("YYYY", custNumber[0]);
            Console.WriteLine("LoadKampagnTY1 Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(query);

            query = @"select Beskrivelse, Dato, Antal_Budget  from Kampagnelinier_Fordelt where ";
            first = true;
            foreach (string item in custNumber)
            {
                if (first)
                {
                    first = false;
                    query = query + @"( Debitorbogføringsgruppe='" + item + "' ";
                }
                else
                {
                    query = query + @" or Debitorbogføringsgruppe='" + item + "' ";
                }
            }
            query = query + @")  and Varenr='XXXX' and Dato >= '" + firstD + "' and Dato < '" + endD + "' order by Dato";
            query = query.Replace("XXXX", prodNumber.ToString());
            //queryString = queryString.Replace("YYYY", custNumber[0]);
            Console.WriteLine("LoadKampagnTY2 Query: ");
            latestQueryTable2 = conn.QueryExWithTableReturn(query);

            conn.Close();
        }

        private void PrepareKampagnTY_ForGUI()
        {
            DataRow[] currentRows = latestQueryTable.Select(null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
            {
                Console.WriteLine("No prepare kampagn Rows Found");
            }
            else
            {
                //loop trough all rows and write in tabs
                foreach (DataRow row in currentRows)
                {
                    string levAntal = row["Antal_Realiseret"].ToString();
                    int Antal = Convert.ToInt32(levAntal);

                    DateTime tempDate = StaticVariables.GetDateTimeFromString(row["startdato"].ToString());
                    int weekInt = StaticVariables.GetForecastWeeknumberForDate(tempDate);

                    if (weekInt > 0 && weekInt < 54)
                    {
                        kampagn_TY[weekInt] = kampagn_TY[weekInt] + Antal;
                    }
                }
            }

            currentRows = latestQueryTable2.Select(null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
            {
                Console.WriteLine("No prepare kampagn2 Rows Found");
            }
            else
            {
                //loop trough all rows and write in tabs
                foreach (DataRow row in currentRows)
                {
                    string levAntal = row["Antal_Budget"].ToString();
                    int Antal = Convert.ToInt32(levAntal);

                    DateTime tempDate = StaticVariables.GetDateTimeFromString(row["Dato"].ToString());
                    int weekInt = StaticVariables.GetForecastWeeknumberForDate(tempDate);
                    Beskrivelse = row["Beskrivelse"].ToString();


                    if (weekInt > 0 && weekInt < 54)
                    {
                        kampagn_TY[weekInt] = kampagn_TY[weekInt] + Antal;
                    }
                }

            }
        }

        public string GetSalesBudgetWeekInfo(int week, string prodNumber, List<string> customerName)
        {
            LoadSalesBudgetForProductFromSQL(prodNumber, customerName, currentSelectedYear);
            infoDict = new Dictionary<string, string>();
            string returnString = "";
            DataRow[] currentRows = latestQueryTable.Select(null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
            {
                Console.WriteLine("No Supplier Current Rows Found");
            }
            else
            {
                //loop trough all rows and write in tabs
                int i = 1;
                foreach (DataRow row in currentRows)
                {
                    string stringType = row["Type"].ToString();
                    int intType = Convert.ToInt32(stringType);
                    string levAntal = row["Antal"].ToString();
                    int Antal = Convert.ToInt32(levAntal);
                    string levKedja = row["Navn_DebBogfGr"].ToString();
                    string comment = row["Kommentar"].ToString();
                    string dateforNumber = row["Tastedato"].ToString();


                    DateTime tempDate = StaticVariables.GetDateTimeFromString(row["Startdato"].ToString());
                    int weekInt = StaticVariables.GetForecastWeeknumberForDate(tempDate);

                    if (intType == (int)TypeEnum.SalgsBudget && weekInt == week)
                    {
                        if (!infoDict.ContainsKey(levKedja))
                        {
                            infoDict.Add(levKedja, dateforNumber + "    " + levAntal + "  " + comment);
                        }
                        else
                        {
                            i++;
                            string temp = i.ToString() + " " + levKedja;
                            infoDict.Add(temp, dateforNumber + "    " + levAntal + "  " + comment);
                        }
                    }
                }
                string infoString = "";
                foreach (KeyValuePair<string, string> kvp in infoDict)
                {
                    string temp = kvp.Key;
                    while (temp.Length > 0 && !Char.IsLetter(temp, 0))
                    {
                        temp = temp.Substring(1, temp.Length - 1);
                    }
                    infoString = infoString + "\n " + temp + "  " + kvp.Value;
                }
                returnString = infoString;
            }
            return returnString;
        }

        public List<ISalesRow> GetRealizedSalesByYear(int year, string itemNumber, String customerNumber, List<string> customerNumberNav)
        {
            List<ISalesRow> salesRows = new List<ISalesRow>();
            int callyear = year;
            if (year < 2000)
            {
                if(year == 1000)
                {
                    callyear = DateTime.Now.Year;
                }
                else if(year == 999)
                {
                    callyear = DateTime.Now.Year-1;
                }
                
            }
            if (callyear <= 2017)
            {
                salesRows.AddRange(LoadRelSalg_FromSQL(itemNumber, year, customerNumberNav));
            }
            if (callyear > 2015)
            {
                salesRows.AddRange(LoadRealizedSalesFromM3Sales(itemNumber, year, customerNumber));
            }
            return salesRows;
        }

        public List<ISalesRow> GetLoadedLastYearSalesByYear()
        {
            List<ISalesRow> salesRows = new List<ISalesRow>();

            salesRows.AddRange(GetLastSalesFromNav());

            salesRows.AddRange(GetRealizedSalesLastYear());

            return salesRows;
        }


        //public void LoadRelSalg_FromQlick(string prodNumber, int year, string custNumber)
        //{
        //    Console.WriteLine("Load Rel SAlg: prodnr" + prodNumber + " year: " + year + " custNumber: " + custNumber);
        //    ClassSQLQlickViewDataLayer conn = new ClassSQLQlickViewDataLayer();
        //    string query = "";
        //    query = query + @"SELECT ";
        //    query = query + @"SKMJDWDataMarts.dbo.FactFörsäljning.Kvantitet, SKMJDWDataMarts.dbo.FactFörsäljning.artikelid, SKMJDWDataMarts.dbo.FactFörsäljning.kundid, SKMJDWDataMarts.dbo.FactFörsäljning.KalenderIDBekräftadLeveransDatum, SKMJDWDataMarts.dbo.FactFörsäljning.FörsäljningID, SKMJDWDataMarts.dbo.DimArtikel.ArtikelNr , SKMJDWDataMarts.dbo.DimArtikel.ArtikelNamn, SKMJDWDataMarts.dbo.FactFörsäljning.KundID ";
        //    query = query + @"FROM SKMJDWDataMarts.dbo.FactFörsäljning ";
        //    query = query + @"Inner join SKMJDWDataMarts.dbo.DimArtikel ";
        //    query = query + @"on SKMJDWDataMarts.dbo.DimArtikel.ArtikelId = SKMJDWDataMarts.dbo.FactFörsäljning.ArtikelId ";
        //    query = query + @"Inner join SKMJDWDataMarts.dbo.DimKund ";
        //    query = query + @"on SKMJDWDataMarts.dbo.DimKund.KundID = SKMJDWDataMarts.dbo.FactFörsäljning.KundID ";
        //    query = query + @"WHERE FörsäljningID > '60000000' ";
        //    query = query + @"AND SKMJDWDataMarts.dbo.DimArtikel.ArtikelNR =  'XXXXX' ";
        //    query = query + @"AND SKMJDWDataMarts.dbo.DimKund.kundNR = 'YYYYY' ";

        //    query = query.Replace("XXXXX", prodNumber);
        //    query = query.Replace("YYYYY", custNumber);
        //   // Console.WriteLine("LoadRealiseretKampagnLY Query Qlickview: " + queryString2);
        //    latestQueryQlickTable = conn.QueryExWithTableReturn(query);
        //    //dataGridViewQlickviewData.DataSource = latestQueryTable;

        //    conn.Close();
        //}


        //private void ComputeNumberPerWeekDict(int year)
        //{
        //    DataRow[] currentRows = latestQueryQlickTable.Select(null, null, DataViewRowState.CurrentRows);

        //    if (currentRows.Length < 1)
        //    {
        //        Console.WriteLine("No prepare salg Rows Found" + year);
        //    }
        //    else
        //    {
        //        //loop trough all rows and write in tabs
        //        foreach (DataRow row in currentRows)
        //        {
        //            string levAntal = row["Kvantitet"].ToString();
        //            double ant = Math.Floor(Convert.ToDouble(levAntal));
        //            int Antal = Convert.ToInt32(ant);

        //            //int postType = Convert.ToInt32(row["Posttype"].ToString());
        //            //string lokKode = row["Lokationskode"].ToString();
        //            //bool levEgetLager = (bool)row["LeverandørlagerOrdre"];
        //            string dateString = row["KalenderIDBekräftadLeveransDatum"].ToString();
        //            DateTime tempDate = StaticVariables.ParseExactStringToDate(dateString,
        //                                                    "yyyyMMdd",
        //                                                    CultureInfo.InvariantCulture,
        //                                                    DateTimeStyles.None);

        //            string tempNameString = row["ArtikelNamn"].ToString();
        //            if (Beskrivelse.Length < 2)
        //            {
        //                Beskrivelse = tempNameString;
        //                Console.WriteLine("Get Name from Qlickview: " + Beskrivelse);
        //            }
        //            double dayOFYear = 0;

        //            dayOFYear = (tempDate - StaticVariables.StartDate[year]).TotalDays;

        //            double weekNBR = dayOFYear / 7.0;
        //            int weekInt = (int)Math.Floor(weekNBR);
        //            weekInt = weekInt + 1;

        //            if (weekInt > 0 && weekInt < 54)
        //            {
        //                if (currentSelectedYear == year)
        //                {
        //                    if (Antal > 0)
        //                    {
        //                        relSalg_TY[weekInt] = relSalg_TY[weekInt] + Antal;
        //                    }
        //                }
        //                else if (currentSelectedYear - 1 == year)
        //                {
        //                    if (Antal > 0)
        //                    {
        //                        relSalg_LY[weekInt] = relSalg_LY[weekInt] + Antal;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        private List<ISalesRow> LoadRelSalg_FromSQL(string prodNumber, int year, List<string> custNumber)
        {
            conn = new NavSQLExecute();
            lastYearRowsNAV = new List<ISalesRow>();
            string query = "";
            query = @"select DISTINCT Løbenr, Bogføringsdato, ISNULL(Styk_antal, Faktureret_antal) Styk_antal, LeverandørlagerOrdre, Posttype, Lokationskode, Varenr, AssortmentName
                        from  Varepost_Skyggetabel vs
                        LEFT OUTER JOIN dbo.AssortmentNav an ON Bogfgruppe = an.NavAssortment
                        LEFT OUTER JOIN " + StaticVariables.TableChainAssortment + @" AS a ON an.Assortment = a.AssortmentCode
                        where ";
            bool first = true;
            foreach (string item in custNumber)
            {
                if (first)
                {
                    first = false;
                    query = query + @"( Bogfgruppe='" + item + "' ";
                }
                else
                {
                    query = query + @" or Bogfgruppe='" + item + "' ";
                }
            }
            query = query + @")  and Varenr='XXXX' and Bogføringsdato >= '" + startDateStrings[year-1] + "' and Bogføringsdato < '" + endDateStrings[year] + "' order by Bogføringsdato";
            query = query.Replace("XXXX", prodNumber.ToString());
            Console.WriteLine("LoadRelSalgsbudget Query: ");

            List<ISalesRow> salesRows = new List<ISalesRow>();
            DataTable table = conn.QueryExWithTableReturn(query);
            DataRowCollection rows = table.Rows;
            string tempDate = GetStartDate(year).Replace(@"/", "");
            DateTime tempDateThisStart = StaticVariables.ParseExactStringToDate(tempDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            tempDate = GetEndDate(year).Replace(@"/", "");
            DateTime tempDateThisEnd = StaticVariables.ParseExactStringToDate(tempDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            tempDate = GetStartDate(year - 1).Replace(@"/", "");
            DateTime tempDateLastStart = StaticVariables.ParseExactStringToDate(tempDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            tempDate = GetEndDate(year - 1).Replace(@"/", "");
            DateTime tempDateLastEnd = StaticVariables.ParseExactStringToDate(tempDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            foreach (DataRow row in rows)
            {
                bool levEgetLager = (bool)row["LeverandørlagerOrdre"];
                string lokKode = row["Lokationskode"].ToString();
                int postType = Convert.ToInt32(row["Posttype"].ToString());
                DateTime date = (DateTime)row["Bogføringsdato"];
                int week = GetWeek(date);
                double quantity = Convert.ToDouble(row["Styk_antal"].ToString());
                if (levEgetLager && postType == 2)
                {
                    if (lokKode.Equals("8") || lokKode.Equals("100"))
                    {
                        //Placeholder
                    }
                }
                else if (!levEgetLager && postType == 1)
                {
                    quantity = -1 * quantity;
                }
                else
                {
                    quantity = 0;
                }
                if (quantity > 0)
                {
                    

                    if (date > tempDateThisStart && tempDateThisEnd > date)
                    {
                        salesRows.Add(new SalesRowNavision()
                        {
                            Date = (DateTime)row["Bogføringsdato"],
                            SupplyOwnStock = (bool)row["LeverandørlagerOrdre"],
                            Quantity = (int)Math.Round(quantity, 0),
                            Posttype = Convert.ToInt32(row["Posttype"].ToString()),
                            ItemNumber = row["Varenr"].ToString(),
                            Week = week,
                            CustomerName = row["AssortmentName"].ToString(),
                        });
                    }

                    if (date > tempDateLastStart && tempDateLastEnd > date)
                    {
                        lastYearRowsNAV.Add(new SalesRowNavision()
                        {
                            Date = (DateTime)row["Bogføringsdato"],
                            SupplyOwnStock = (bool)row["LeverandørlagerOrdre"],
                            Quantity = (int)Math.Round(quantity, 0),
                            Posttype = Convert.ToInt32(row["Posttype"].ToString()),
                            ItemNumber = row["Varenr"].ToString(),
                            Week = week,
                            CustomerName = row["AssortmentName"].ToString(),
                        });
                    }
                }
            }
            return salesRows;
        }

        private List<ISalesRow> GetLastSalesFromNav()
        {
            return lastYearRowsNAV;
        }

        public int GetWeek(DateTime date)
        {

            int weekInt = StaticVariables.GetForecastWeeknumberForDate(date);
            return weekInt;
        }

        private List<ISalesRow> LoadRealizedSalesFromM3Sales(string prodNumber, int year, string custNumber)
        {
            int salesYear = year;
            if (salesYear == 1000)
            {
                salesYear = DateTime.Now.Year;
            }
            else if (salesYear == 999)
            {
                salesYear = DateTime.Now.Year - 1;
            }
            conn = new NavSQLExecute();
            List<ISalesRow> salesRows = new List<ISalesRow>();
            List<ISalesRow> salesRowsLast = new List<ISalesRow>();
            string query = "";
            string tableSales = StaticVariables.TableM3Sales;
            string tableChainCustomer = StaticVariables.TableChainCustomer;
            string tableChainAssortment = StaticVariables.TableChainAssortment;
            query = String.Format(@"SELECT * FROM (SELECT
	                                [UCORNO],
	                                [UCDLDT],
                                    [MTTRQT],
	                                [UCITNO],
	                                [UCCUNO],
                                    [F1A130],
                                    [LMEXPI],
	                                CASE 
		                                WHEN a.assortmentName IS NOT NULL	THEN a.assortmentName
		                                WHEN a1.assortmentName IS NOT NULL	THEN a1.assortmentName
		                                WHEN a2.assortmentName IS NOT NULL	THEN a2.assortmentName
		                                WHEN a3.assortmentName IS NOT NULL	THEN a3.assortmentName
		                                ELSE 'Øvrige'
	                                END AssortmentName,
	                                CASE 
		                                WHEN a.assortmentCode IS NOT NULL	THEN a.assortmentCode
		                                WHEN a1.assortmentCode IS NOT NULL	THEN a1.assortmentCode
		                                WHEN a2.assortmentCode IS NOT NULL	THEN a2.assortmentCode
		                                WHEN a3.assortmentCode IS NOT NULL	THEN a3.assortmentCode
		                                ELSE 'Øvrige'
	                                END AssortmentCode
                                  FROM " + tableSales + @" 
                                    LEFT OUTER JOIN " + tableChainCustomer + @" AS c ON " + tableSales + @".UCCUNO = c.Customer
                                    LEFT OUTER JOIN " + tableChainAssortment + @" AS a ON a.Chain = c.Chain
	                                LEFT OUTER JOIN " + tableChainAssortment + @" a1 ON a1.Chain = UCCHL1
	                                LEFT OUTER JOIN " + tableChainAssortment + @" a2 ON a2.Chain = UCCHL2
	                                LEFT OUTER JOIN " + tableChainAssortment + @" a3 ON a3.Chain = UCCHL3
                                    WHERE UCITNO = '{0}' AND (LEFT(UCDLDT, 4) = {1} OR LEFT(UCDLDT, 4) = {3})) A WHERE AssortmentCode LIKE '{2}%'", prodNumber, salesYear, custNumber, salesYear - 1);

            DataTable table = conn.QueryExWithTableReturn(query);
            DataRowCollection rows = table.Rows;

            foreach (DataRow row in rows)
            {
                DateTime date = StaticVariables.ParseExactStringToDate(row["UCDLDT"].ToString(),
                                                    "yyyyMMdd",
                                                    CultureInfo.InvariantCulture);
                int quantity = (int)Convert.ToDecimal(row["MTTRQT"].ToString());
                int week = StaticVariables.GetWeek2(date);
                int wantedbbd = (int)Convert.ToInt32(row["F1A130"].ToString());
                int bbd = (int)Convert.ToInt32(row["LMEXPI"].ToString());
                if (date.Year == salesYear)
                {
                    salesRows.Add(new SalesRow()
                    {
                        Date = date,
                        OrderNumber = row["UCORNO"].ToString(),
                        Quantity = quantity,
                        Customer = row["UCCUNO"].ToString(),
                        ItemNumber = row["UCITNO"].ToString(),
                        Week = week,
                        BestBeforeDate = bbd,
                        WantedBestBeforeDate = wantedbbd,
                        CustomerName = row["AssortmentName"].ToString()
                    });
                }

                if (date.Year == salesYear - 1)
                {
                    lastYearRowsM3.Add(new SalesRow()
                    {
                        Date = date,
                        OrderNumber = row["UCORNO"].ToString(),
                        Quantity = quantity,
                        Customer = row["UCCUNO"].ToString(),
                        ItemNumber = row["UCITNO"].ToString(),
                        Week = week,
                        BestBeforeDate = bbd,
                        WantedBestBeforeDate = wantedbbd,
                        CustomerName = row["AssortmentName"].ToString()
                    });
                }

            }
            //query = query + " order by UCDLDT";

            //Console.WriteLine("LoadRelSalg_FromM3SQL Query: ");
            //Console.WriteLine(query);
            latestQueryTable = conn.QueryExWithTableReturn(query);
            Console.WriteLine("Last year sales row: " + lastYearRowsM3.Count + " This year Sales Row: " + salesRows.Count);
            return salesRows;
            //throw new NotImplementedException();
        }

        private List<ISalesRow> GetRealizedSalesLastYear()
        {
            return lastYearRowsM3;
        }


        //
        //        private void LoadRelSalg_FromM3SQL(string prodNumber, int year, List<string> custNumber)
        //        {
        //            conn = new NavSQLExecute();
        //            string query = "";
        //            query = String.Format(@"SELECT UCDLDT, UCIVQT, UCORNO, UCITNO, UCORNO, UCCUNO, AssortmentName, AssortmentCode FROM " +
        //                                    StaticVariables.TableM3Sales + @" 
        //                                    INNER JOIN " + StaticVariables.TableChainCustomer + @" c ON Customer = UCCUNO
        //                                    LEFT OUTER JOIN " + StaticVariables.TableChainAssortment + @" AS a ON c.Chain = a.Chain 
        //                                    WHERE UCITNO = '{0}' AND LEFT(UCDLDT, 4) = {1}", prodNumber, year);
        //            bool first = true;
        //            foreach (string customer in custNumber)
        //            {
        //                if (first)
        //                {
        //                    first = false;
        //                    query = query + String.Format(" AND AssortmentCode LIKE '{0}%' ", customer);
        //                }
        //                else
        //                {
        //                    query = query + String.Format(" OR AssortmentCode LIKE '{0}%' ", customer);
        //                }
        //            }
        //            query = query + " order by UCDLDT";

        //            Console.WriteLine("LoadRelSalg_FromM3SQL Query: ");
        //            Console.WriteLine(query);
        //            latestQueryTable = conn.QueryExWithTableReturn(query);
        //        }


        internal void SetYear(int selectedYear)
        {

            currentSelectedYear = selectedYear;

        }

        internal string GetBeskrivelse()
        {
            return Beskrivelse;
        }


        internal Dictionary<int, string> GetSalesComment_TY()
        {
            return Salgsbudget_Comment;
        }

        internal int GetCurrentWeekNBR()
        {
            DateTime tempDate = DateTime.Now;
            int weekInt = StaticVariables.GetForecastWeeknumberForDate(tempDate);
            return weekInt;
        }
    }
}
