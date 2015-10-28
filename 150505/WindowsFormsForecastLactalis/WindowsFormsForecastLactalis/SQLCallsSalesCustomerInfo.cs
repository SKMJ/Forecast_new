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
    class SQLCallsSalesCustomerInfo
    {
        DataTable latestQueryTable; //Holds latest loaded Query output
        DataTable latestQueryTable2; //Holds latest loaded Query output if 2 needed
        DataTable latestQueryQlickTable;
        NavSQLExecute conn;
        String Beskrivelse = "A";

        Dictionary<string, string> allCustomers = new Dictionary<string, string>();
        Dictionary<int, int> salesBudgetTY = new Dictionary<int, int>();
        Dictionary<int, int> salesBudgetLY = new Dictionary<int, int>();
        Dictionary<int, int> realKampagn_LY = new Dictionary<int, int>();
        Dictionary<int, int> kampagn_TY = new Dictionary<int, int>();
        Dictionary<int, int> relSalg_LY = new Dictionary<int, int>();
        Dictionary<int, int> relSalg_TY = new Dictionary<int, int>();

        Dictionary<int, string> startDateStrings = new Dictionary<int, string>();
        Dictionary<int, string> endDateStrings = new Dictionary<int, string>();
        //Dictionary<int, DateTime> startDate = new Dictionary<int, DateTime>();

        public Dictionary<int, string> Salgsbudget_Comment = new Dictionary<int, string>();

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

           
            startDateStrings.Add(2014, @"2013/12/30");
            endDateStrings.Add(2014, @"2014/12/30");
            startDateStrings.Add(2015, @"2014/12/29");
            endDateStrings.Add(2015, @"2016/01/05");
            startDateStrings.Add(2016, @"2016/01/04");
            endDateStrings.Add(2016, @"2017/01/01");
            startDateStrings.Add(2017, @"2017/01/02");
            endDateStrings.Add(2017, @"2018/01/01");


            for (int k = 0; k < 54; k++)
            {
                salesBudgetTY.Add(k, 0);
                salesBudgetLY.Add(k, 0);
                realKampagn_LY.Add(k, 0);
                kampagn_TY.Add(k, 0);
                relSalg_LY.Add(k, 0);
                relSalg_TY.Add(k, 0);
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

            string queryString = @"select Navn_DebBogfGr, DebitorBogføringsruppe  from Debitor_Budgetlinjepost where  startdato >= '" + startDateStrings[currentSelectedYear] + "' and startdato < '" + endDateStrings[currentSelectedYear] + "' order by Navn_DebBogfGr";
            Console.WriteLine("Load All Customers: ");
            latestQueryTable = conn.QueryExWithTableReturn(queryString);
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


       

        //Get sales buget data as dictionary
        public Dictionary<int, int> GetSalesBudgetTY(string prodNumber, string customerName)
        {
            LoadSalesBudgetForProductFromSQL(prodNumber, customerName, currentSelectedYear);
            PrepareLoadedSalesBudgetForGUI(currentSelectedYear);
            return salesBudgetTY;
        }

        //Load the sales budget numbers from SQL Database
        private void LoadSalesBudgetForProductFromSQL(string prodNumber, string customerName, int year)
        {
            conn = new NavSQLExecute();
            string queryString = "";

            queryString = @"select Type, Antal, Debitorbogføringsruppe, Startdato, Navn_DebBogfGr, Tastedato, Kommentar from Debitor_Budgetlinjepost where Debitorbogføringsruppe='YYYY' and Varenr='XXXX' and startdato >= '" + startDateStrings[year] + "' and startdato < '" + endDateStrings[year] + "' order by Tastedato";


            
            queryString = queryString.Replace("XXXX", prodNumber);
            queryString = queryString.Replace("YYYY", customerName);
            Console.WriteLine("Load Sales Budget Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(queryString);
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
                    //string levKedja = row["Navn_DebBogfGr"].ToString();

                    DateTime tempDate = DateTime.Parse(row["Startdato"].ToString());
                    double week = (tempDate - ClassStaticVaribles.StartDate[year]).TotalDays; 
                    

                    
                    double weekNBR = week / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;

                    if (intType == (int)TypeEnum.SalgsBudget && weekInt < 54)
                    {
                        if (year == currentSelectedYear)
                        {
                            salesBudgetTY[weekInt] = salesBudgetTY[weekInt] + Antal;
                            if (comment.Length > 0)
                            {
                                Salgsbudget_Comment[weekInt] = Salgsbudget_Comment[weekInt] + " \n " + Antal + "  " + comment;
                            }
                        }
                        else if (year == currentSelectedYear-1)
                        {
                            salesBudgetLY[weekInt] = salesBudgetLY[weekInt] + Antal;
                        }

                        antalTot = antalTot + Antal;
                    }
                }

            }
        }




        internal Dictionary<int, int> GetSalesBudget_LY(string ProductNumber, string CustomerNumber)
        {
            LoadSalesBudgetForProductFromSQL(ProductNumber, CustomerNumber, currentSelectedYear-1);
            PrepareLoadedSalesBudgetForGUI(currentSelectedYear - 1);
            return salesBudgetLY;
        }



        public Dictionary<int, int> GetRealiseretKampagnLY(string prodNumber, string custNumber)
        {
            LoadRealiseretKampagnLY_FromSQL(prodNumber, custNumber);
            PrepareRealiseretKampagnLY_ForGUI();
            return realKampagn_LY;
        }



        //Load info for the RealiseretKampagnLY from SQL 
        private void LoadRealiseretKampagnLY_FromSQL(string prodNumber, string custNumber)
        {
            conn = new NavSQLExecute();

            string queryString = @"select startdato, Antal_Realiseret  from Afsl__Kampagnelinier where Debitorbogføringsgruppe='YYYY' and Varenr='XXXX' and startdato >= '" + startDateStrings[currentSelectedYear - 1] + "' and startdato < '" + endDateStrings[currentSelectedYear - 1] + "' order by startdato";
            queryString = queryString.Replace("XXXX", prodNumber.ToString());
            queryString = queryString.Replace("YYYY", custNumber);
            Console.WriteLine("LoadRealiseretKampagnLY Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(queryString);

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

                    DateTime tempDate = DateTime.Parse(row["startdato"].ToString());
                    double week = (tempDate - ClassStaticVaribles.StartDate[currentSelectedYear - 1]).TotalDays;
                    double weekNBR = week / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);


                    if (weekInt > 0 && weekInt < 54)
                    {
                        Console.WriteLine("Antal Kampagn Rel: " + Antal);
                        realKampagn_LY[weekInt] = realKampagn_LY[weekInt] + Antal;
                    }
                }
            }
        }


        internal Dictionary<int, int> GetKampagnTY(string prodNumber, string custNumber)
        {
            LoadKampagnTY_FromSQL(prodNumber, custNumber);
            PrepareKampagnTY_ForGUI();
            return kampagn_TY;
        }


        private void LoadKampagnTY_FromSQL(string prodNumber, string custNumber)
        {
            conn = new NavSQLExecute();

            string queryString = @"select startdato, Antal_Realiseret  from Afsl__Kampagnelinier where Debitorbogføringsgruppe='YYYY' and Varenr='XXXX' and startdato >= '" + startDateStrings[currentSelectedYear] + "' and startdato < '" + endDateStrings[currentSelectedYear] + "' order by startdato";
            queryString = queryString.Replace("XXXX", prodNumber.ToString());
            queryString = queryString.Replace("YYYY", custNumber);
            Console.WriteLine("LoadKampagnTY1 Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(queryString);

            queryString = @"select Beskrivelse, Dato, Antal_Budget  from Kampagnelinier_Fordelt where Debitorbogføringsgruppe='YYYY' and Varenr='XXXX' and Dato >= '" + startDateStrings[currentSelectedYear] + "' and Dato < '" + endDateStrings[currentSelectedYear] + "' order by Dato";
            queryString = queryString.Replace("XXXX", prodNumber.ToString());
            queryString = queryString.Replace("YYYY", custNumber);
            Console.WriteLine("LoadKampagnTY2 Query: ");
            latestQueryTable2 = conn.QueryExWithTableReturn(queryString);

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

                    DateTime tempDate = DateTime.Parse(row["startdato"].ToString());
                    double week = (tempDate - ClassStaticVaribles.StartDate[currentSelectedYear]).TotalDays;
                    double weekNBR = week / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);


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

                    DateTime tempDate = DateTime.Parse(row["Dato"].ToString());
                    double week = (tempDate - ClassStaticVaribles.StartDate[currentSelectedYear]).TotalDays;
                    double weekNBR = week / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);
                    Beskrivelse = row["Beskrivelse"].ToString();


                    if (weekInt > 0 && weekInt < 54)
                    {
                        kampagn_TY[weekInt] = kampagn_TY[weekInt] + Antal;
                    }
                }

            }
        }


        public string GetSalesBudgetWeekInfo(int week, string prodNumber, string customerName)
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

                    DateTime tempDate = DateTime.Parse(row["Startdato"].ToString());
                    double week1 = (tempDate - ClassStaticVaribles.StartDate[currentSelectedYear]).TotalDays;
                    double weekNBR = week1 / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;

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
        

        internal Dictionary<int, int> GetRelSalg_LY(string prodNumber, string custNumber)
        {
            if (currentSelectedYear < 2017)
            {
                LoadRelSalg_FromSQL(prodNumber, currentSelectedYear - 1, custNumber);
                PrepareRelSalg_LYorTYForGUI(currentSelectedYear - 1, custNumber, prodNumber);
            }
            if (currentSelectedYear > 2015)
            {
                //Get sales numbers from Qlickview
                if (latestQueryQlickTable == null || latestQueryQlickTable.Rows.Count <= 0)
                {
                    LoadRelSalg_FromQlick(prodNumber, currentSelectedYear - 1, "15701");
                }
                ComputeNumberPerWeekDict(currentSelectedYear - 1);

            }

            return relSalg_LY;
        }

        public void LoadRelSalg_FromQlick(string prodNumber, int year, string custNumber)
        {
            ClassSQLQlickViewDataLayer conn = new ClassSQLQlickViewDataLayer();
            string queryString2 = "";
            queryString2 = queryString2 + @"SELECT ";
            queryString2 = queryString2 + @"SKMJDWDataMarts.dbo.FactFörsäljning.Kvantitet, SKMJDWDataMarts.dbo.FactFörsäljning.artikelid, SKMJDWDataMarts.dbo.FactFörsäljning.kundid, SKMJDWDataMarts.dbo.FactFörsäljning.KalenderIDBekräftadLeveransDatum, SKMJDWDataMarts.dbo.FactFörsäljning.FörsäljningID, SKMJDWDataMarts.dbo.DimArtikel.ArtikelNr , SKMJDWDataMarts.dbo.DimArtikel.ArtikelNamn, SKMJDWDataMarts.dbo.FactFörsäljning.KundID ";
            queryString2 = queryString2 + @"FROM SKMJDWDataMarts.dbo.FactFörsäljning ";
            queryString2 = queryString2 + @"Inner join SKMJDWDataMarts.dbo.DimArtikel ";
            queryString2 = queryString2 + @"on SKMJDWDataMarts.dbo.DimArtikel.ArtikelId = SKMJDWDataMarts.dbo.FactFörsäljning.ArtikelId ";
            queryString2 = queryString2 + @"Inner join SKMJDWDataMarts.dbo.DimKund ";
            queryString2 = queryString2 + @"on SKMJDWDataMarts.dbo.DimKund.KundID = SKMJDWDataMarts.dbo.FactFörsäljning.KundID ";
            queryString2 = queryString2 + @"WHERE FörsäljningID > '60000000' ";
            queryString2 = queryString2 + @"AND SKMJDWDataMarts.dbo.DimArtikel.ArtikelNR =  'XXXXX' ";
            queryString2 = queryString2 + @"AND SKMJDWDataMarts.dbo.DimKund.kundNR = 'YYYYY' ";

            queryString2 = queryString2.Replace("XXXXX", prodNumber);
            queryString2 = queryString2.Replace("YYYYY", custNumber);
           // Console.WriteLine("LoadRealiseretKampagnLY Query Qlickview: " + queryString2);
            latestQueryQlickTable = conn.QueryExWithTableReturn(queryString2);
            //dataGridViewQlickviewData.DataSource = latestQueryTable;

            conn.Close();
        }


        private void ComputeNumberPerWeekDict(int year)
        {
            DataRow[] currentRows = latestQueryQlickTable.Select(null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
            {
                Console.WriteLine("No prepare salg Rows Found" + year);
            }
            else
            {
                //loop trough all rows and write in tabs
                foreach (DataRow row in currentRows)
                {
                    string levAntal = row["Kvantitet"].ToString();
                    double ant = Math.Floor(Convert.ToDouble(levAntal));
                    int Antal = Convert.ToInt32(ant);

                    //int postType = Convert.ToInt32(row["Posttype"].ToString());
                    //string lokKode = row["Lokationskode"].ToString();
                    //bool levEgetLager = (bool)row["LeverandørlagerOrdre"];
                    string dateString = row["KalenderIDBekräftadLeveransDatum"].ToString();
                    DateTime tempDate = DateTime.ParseExact(dateString,
                                                            "yyyyMMdd",
                                                            CultureInfo.InvariantCulture,
                                                            DateTimeStyles.None);

                    string tempNameString = row["ArtikelNamn"].ToString();
                    if (Beskrivelse.Length < 2)
                    {
                        Beskrivelse = tempNameString;
                        Console.WriteLine("Get Name from Qlickview: " + Beskrivelse);
                    }
                    double dayOFYear = 0;

                    dayOFYear = (tempDate - ClassStaticVaribles.StartDate[year]).TotalDays;

                    double weekNBR = dayOFYear / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;

                    if (weekInt > 0 && weekInt < 54)
                    {
                        if (currentSelectedYear == year)
                        {
                            if (Antal > 0)
                            {
                                relSalg_TY[weekInt] = relSalg_TY[weekInt] + Antal;
                            }
                        }
                        else if (currentSelectedYear - 1 == year)
                        {
                            if (Antal > 0)
                            {
                                relSalg_LY[weekInt] = relSalg_LY[weekInt] + Antal;
                            }
                        }
                    }
                }
            }
        }

        internal Dictionary<int, int> GetRelSalg_TY(string prodNumber, string custNumber)
        {
            
            

            if (currentSelectedYear < 2016)
            {
                LoadRelSalg_FromSQL(prodNumber, currentSelectedYear, custNumber);
                PrepareRelSalg_LYorTYForGUI(currentSelectedYear, custNumber, prodNumber);
            }
            if (currentSelectedYear > 2014)
            {
                //Get sales numbers from Qlickview
                if (latestQueryQlickTable == null || latestQueryQlickTable.Rows.Count <= 0)
                {
                    LoadRelSalg_FromQlick(prodNumber, currentSelectedYear , "15701");
                }
                ComputeNumberPerWeekDict(currentSelectedYear);

            }
            return relSalg_TY;
        }

        private void PrepareRelSalg_LYorTYForGUI(int year, string custNumber, string prodNumber)
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
                    string levAntal = row["Faktureret_antal"].ToString();
                    int Antal = Convert.ToInt32(levAntal);

                    int postType = Convert.ToInt32(row["Posttype"].ToString());
                    string lokKode = row["Lokationskode"].ToString();
                    bool levEgetLager = (bool)row["LeverandørlagerOrdre"];

                    DateTime tempDate = DateTime.Parse(row["Bogføringsdato"].ToString());
                    double dayOFYear = 0;

                    dayOFYear = (tempDate - ClassStaticVaribles.StartDate[year]).TotalDays;

                    double weekNBR = dayOFYear / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;

                    if (Antal > 0)
                    {

                        //Console.WriteLine(tempDate + "Realiseret Salg: week: " + weekInt + " Antal: " + Antal + " posttype: " + postTypeString + " bool: " + levEgetLager + " lokCode: " + lokKode);
                    }
                    if (weekInt > 0 && weekInt < 54)
                    {
                        if (year == currentSelectedYear - 1)
                        {
                            //if (prodNumber == 2735 && weekInt == 1 || weekInt == 2 || weekInt == 8)
                            //{
                                if (levEgetLager)
                                {
                                    if (postType == 2)
                                    {
                                        if (lokKode.Equals("8"))
                                        {
                                           // Console.WriteLine("Antal 1: " + Antal + " week: " + weekInt);
                                            relSalg_LY[weekInt] = relSalg_LY[weekInt] + Antal;
                                        }
                                        else if (lokKode.Equals("100"))
                                        {
                                           // Console.WriteLine("Antal 2: " + Antal + " week: " + weekInt);
                                            relSalg_LY[weekInt] = relSalg_LY[weekInt] + Antal;
                                        }
                                    }
                                }
                                else
                                {
                                    //Console.WriteLine(tempDate + "Realiseret Salg: week: " + weekInt + " Antal: " + Antal + " posttype: " + postTypeString + " bool: " + levEgetLager + " lokCode: " + lokKode);

                                    if (postType == 1)
                                    {
                                        //Console.WriteLine("Antal 3: " + Antal + " week: " + weekInt);
                                        relSalg_LY[weekInt] = relSalg_LY[weekInt] - Antal; 
                                    }
                                }
                            //}

                        }
                        else if (year == currentSelectedYear)
                        {

                            if (levEgetLager)
                            {
                                if (postType == 2)
                                {
                                    if (lokKode.Equals("8"))
                                    {
                                        relSalg_TY[weekInt] = relSalg_TY[weekInt] + Antal;
                                    }
                                    else if (lokKode.Equals("100"))
                                    {
                                        relSalg_TY[weekInt] = relSalg_TY[weekInt] + Antal;
                                    }
                                }
                            }
                            else
                            {
                                //Console.WriteLine(tempDate + "Realiseret Salg: week: " + weekInt + " Antal: " + Antal + " posttype: " + postTypeString + " bool: " + levEgetLager + " lokCode: " + lokKode);

                                if (postType == 1)
                                {
                                    relSalg_TY[weekInt] = relSalg_TY[weekInt] - Antal;
                                }
                            }
                        }


                    }
                }
            }
        }

        private void LoadRelSalg_FromSQL(string prodNumber, int year, string custNumber)
        {
            conn = new NavSQLExecute();
            string queryString = "";
            queryString = @"select Bogføringsdato, Faktureret_antal, LeverandørlagerOrdre, Posttype, Lokationskode  from  Varepost_Skyggetabel where Bogfgruppe='YYYY' and Varenr='XXXX' and Bogføringsdato >= '" + startDateStrings[year] + "' and Bogføringsdato < '" + endDateStrings[year] + "' order by Bogføringsdato";
            //if (year == 2014)
            //{
            //    queryString = @"select Bogføringsdato, Faktureret_antal, LeverandørlagerOrdre, Posttype, Lokationskode  from  Varepost_Skyggetabel where Bogfgruppe='YYYY' and Varenr='XXXX' and Bogføringsdato >= '2013/12/30' and Bogføringsdato < '2014/12/30'";

            //}
            //else
            //{
            //    queryString = @"select Bogføringsdato, Faktureret_antal, LeverandørlagerOrdre, Posttype, Lokationskode  from  Varepost_Skyggetabel where  Bogfgruppe='YYYY' and Varenr='XXXX' and Bogføringsdato >= '2014/12/29' and Bogføringsdato < '2016/01/05'";

            //}

            queryString = queryString.Replace("XXXX", prodNumber.ToString());
            queryString = queryString.Replace("YYYY", custNumber);
            Console.WriteLine("LoadRelSalgsbudget Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(queryString);
            //throw new NotImplementedException();
        }

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
            double week1 = (tempDate - ClassStaticVaribles.StartDate[currentSelectedYear]).TotalDays;
            double weekNBR = week1 / 7.0;
            int weekInt = (int)Math.Floor(weekNBR);
            weekInt = weekInt + 1;
            return weekInt;
        }
    }
}
