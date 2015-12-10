///This file handles all loading of supply information
///The file is some kind of layer between between the PrognosInfoForSupply.cs  and the actual SQL calls
///handle in NavSQLExecute.cs 


using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WindowsFormsForecastLactalis
{
    class NavSQLSupplyInformation
    {
        DataTable latestQueryTable; //Holds latest loaded Query output
        DataTable latestQueryTable2; //Holds latest loaded Query output if 2 needed
        DataTable latestQueryQlickTable;

        string Beskrivelse = "A";

        int LactalisVareNummer = 0;
        int Lactalis_NBRPer_colli = 0;



        NavSQLExecute conn;
        Dictionary<string, int> infoDict = new Dictionary<string, int>();
        Dictionary<int, int> salesBudget_TY = new Dictionary<int, int>();
        Dictionary<int, int> salesBudgetREG_TY = new Dictionary<int, int>();
        Dictionary<int, int> kopesBudget_TY = new Dictionary<int, int>();

        Dictionary<int, int> kopesBudget_ForFile = new Dictionary<int, int>();

        public Dictionary<int, string> SalgsbudgetReg_Comment = new Dictionary<int, string>();

        Dictionary<int, int> realKampagn_LY = new Dictionary<int, int>();

        Dictionary<int, int> kampagn_TY = new Dictionary<int, int>();
        Dictionary<int, int> relSalg_LY = new Dictionary<int, int>();
        Dictionary<int, int> relSalg_TY = new Dictionary<int, int>();

        Dictionary<int, int> relSalgQlick_LY = new Dictionary<int, int>();
        Dictionary<int, int> relSalgQlick_TY = new Dictionary<int, int>();

        Dictionary<int, int> kopsOrderTY = new Dictionary<int, int>();

        Dictionary<int, string> startDateStrings = new Dictionary<int, string>();
        Dictionary<int, string> endDateStrings = new Dictionary<int, string>();

        //Dictionary<int, DateTime> startDate = new Dictionary<int, DateTime>();
        int[] weekPartPercentage;


        int debug1 = 0;
        int debug2 = 0;
        int debug3 = 0;

        int currentSelectedYear = 2015;
        string currentProdNumber = "0";

        enum TypeEnum
        {
            SalgsBudget = 4,
            SalgsBudget_Regulering = 5,
            KøbsBudget = 6
        };


        //Constructor
        public NavSQLSupplyInformation(int yearForInfo, string prodNumberForInfo)
        {
            //MessageBox.Show("a1");
            InitiateProperties();
            //MessageBox.Show("b1");
            currentProdNumber = prodNumberForInfo;
            currentSelectedYear = yearForInfo;


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
                salesBudget_TY.Add(k, 0);
                realKampagn_LY.Add(k, 0);
                kampagn_TY.Add(k, 0);
                salesBudgetREG_TY.Add(k, 0);
                kopesBudget_TY.Add(k, 0);
                relSalg_LY.Add(k, 0);
                relSalg_TY.Add(k, 0);
                kopsOrderTY.Add(k, 0);
                SalgsbudgetReg_Comment.Add(k, " ");
                kopesBudget_ForFile.Add(k, 0);
                relSalgQlick_LY.Add(k, 0);
                relSalgQlick_TY.Add(k, 0);
            }
        }

        //Load the sales budget numbers from SQL Database
        private void LoadSalesBudgetTYFromSQL()
        {
            conn = new NavSQLExecute();

            string query = @"select Type, Antal, Navn_DebBogfGr, Startdato, Kommentar from Debitor_Budgetlinjepost where Varenr='XXXX' and startdato >= '" + startDateStrings[currentSelectedYear] + "' and startdato < '" + endDateStrings[currentSelectedYear] + "' order by TasteDato";
            query = query.Replace("XXXX", currentProdNumber);
            Console.WriteLine("Load Sales Budget Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(query);
            conn.Close();
        }




        //Get the data you need from sales budget 
        //Prepare the data for GUI
        private void PrepareLoadedSalesBudgetForGUI()
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

                    string levKedja = row["Navn_DebBogfGr"].ToString();

                    DateTime tempDate = DateTime.Parse(row["Startdato"].ToString());
                    double week = (tempDate - ClassStaticVaribles.StartDate[currentSelectedYear]).TotalDays;
                    double weekNBR = week / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;
                    if (weekInt > 0 && weekInt < 54)
                    {
                        if (intType == (int)TypeEnum.SalgsBudget)
                        {
                            salesBudget_TY[weekInt] = salesBudget_TY[weekInt] + Antal;

                            antalTot = antalTot + Antal;
                        }
                        else if (intType == (int)TypeEnum.SalgsBudget_Regulering)
                        {
                            salesBudgetREG_TY[weekInt] = salesBudgetREG_TY[weekInt] + Antal;
                            SalgsbudgetReg_Comment[weekInt] = SalgsbudgetReg_Comment[weekInt] + " \n " + Antal + "  " + comment;

                            //Console.WriteLine("SalesBudgetREg week: " + weekInt + " Antal: " + Antal);
                        }
                        else if (intType == (int)TypeEnum.KøbsBudget)
                        {
                            kopesBudget_TY[weekInt] = kopesBudget_TY[weekInt] + Antal;
                            //Console.WriteLine("KopesBudget week: " + weekInt + " Antal: " + Antal);
                        }
                    }
                }

            }
        }

        //Get sales buget data as dictionary
        public Dictionary<int, int> GetSalesBudget()
        {
            LoadSalesBudgetTYFromSQL();
            PrepareLoadedSalesBudgetForGUI();
            return salesBudget_TY;
        }


        //Get info about salesbuget number for specific week
        public string GetSalesBudgetWeekInfo(int week)
        {
            LoadSalesBudgetTYFromSQL();
            infoDict = new Dictionary<string, int>();
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

                    DateTime tempDate = DateTime.Parse(row["Startdato"].ToString());
                    double week1 = (tempDate - ClassStaticVaribles.StartDate[currentSelectedYear]).TotalDays;
                    double weekNBR = week1 / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;

                    if (intType == (int)TypeEnum.SalgsBudget && weekInt == week)
                    {
                        if (!infoDict.ContainsKey(levKedja))
                        {
                            //infoDict.Add(levKedja, levAntal + comment);
                            infoDict.Add(levKedja, Antal);
                        }
                        else
                        {
                            i++;
                            string temp = i.ToString() + " " + levKedja;
                            infoDict[levKedja] = infoDict[levKedja] + Antal;
                            //infoDict.Add(temp, infoDict[levKedja]);
                        }
                    }
                }
                string infoString = "";
                foreach (KeyValuePair<string, int> kvp in infoDict)
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


        //Load info for the RealiseretKampagnLY from SQL 
        private void LoadRealiseretKampagnLY_FromSQL()
        {
            conn = new NavSQLExecute();

            string query = @"select startdato, Antal_Realiseret  from Afsl__Kampagnelinier where Varenr='XXXX' and startdato >= '" + startDateStrings[currentSelectedYear - 1] + "' and startdato < '" + endDateStrings[currentSelectedYear - 1] + "' order by startdato";
            query = query.Replace("XXXX", currentProdNumber);
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

                    DateTime tempDate = DateTime.Parse(row["startdato"].ToString());
                    double week = (tempDate - ClassStaticVaribles.StartDate[currentSelectedYear - 1]).TotalDays;
                    double weekNBR = week / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);


                    if (weekInt > 0 && weekInt < 54)
                    {
                        realKampagn_LY[weekInt] = realKampagn_LY[weekInt] + Antal;
                    }
                }
            }
        }

        public Dictionary<int, int> GetRealiseretKampagnLY()
        {
            LoadRealiseretKampagnLY_FromSQL();
            PrepareRealiseretKampagnLY_ForGUI();
            return realKampagn_LY;
        }

        internal Dictionary<int, int> GetKampagnTY()
        {
            if (currentSelectedYear >= 2015)
            {
                GetFromM3 m3 = new GetFromM3();
                return m3.GetCampaignsOfProducts(currentProdNumber, currentSelectedYear, "");
            }
            else
            {
                LoadKampagnTY_FromSQL();
                PrepareKampagnTY_ForGUI();
                return kampagn_TY;
            }
        }


        private void LoadKampagnTY_FromSQL()
        {
            conn = new NavSQLExecute();

            string query = @"select startdato, Antal_Realiseret  from Afsl__Kampagnelinier where Varenr='XXXX' and startdato >= '" + startDateStrings[currentSelectedYear] + "' and startdato < '" + endDateStrings[currentSelectedYear] + "' order by startdato";
            query = query.Replace("XXXX", currentProdNumber);
            Console.WriteLine("LoadKampagnTY1 Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(query);

            query = @"select Dato, Antal_Budget, Beskrivelse  from Kampagnelinier_Fordelt where Varenr='XXXX' and Dato >= '" + startDateStrings[currentSelectedYear] + "' and Dato < '" + endDateStrings[currentSelectedYear] + "' order by Dato";
            query = query.Replace("XXXX", currentProdNumber);
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

        //Get info about salesbuget number for specific week
        public string GetKampagnWeekInfo(int week, int prodNumber)
        {
            conn = new NavSQLExecute();


            string query = @"select *  from Kampagnelinier_Fordelt where Varenr='XXXX' and Dato >= '" + startDateStrings[currentSelectedYear] + "' and Dato < '" + endDateStrings[currentSelectedYear] + "' order by Dato";
            query = query.Replace("XXXX", prodNumber.ToString());
            Console.WriteLine("LoadKampagnTY3 Query: ");
            latestQueryTable2 = conn.QueryExWithTableReturn(query);

            conn.Close();

            infoDict = new Dictionary<string, int>();
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
                    string levAntal = row["Antal_Budget"].ToString();
                    int Antal = Convert.ToInt32(levAntal);
                    string levKedja = row["Kæde_Kilde"].ToString();
                    //string comment = row["Kommentar"].ToString();

                    DateTime tempDate = DateTime.Parse(row["Startdato"].ToString());
                    double week1 = (tempDate - ClassStaticVaribles.StartDate[currentSelectedYear]).TotalDays;
                    double weekNBR = week1 / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;

                    if (weekInt == week)
                    {
                        if (!infoDict.ContainsKey(levKedja))
                        {
                            infoDict.Add(levKedja, Antal);
                        }
                        else
                        {
                            i++;
                            string temp = i.ToString() + " " + levKedja;
                            infoDict.Add(temp, Antal);
                        }
                    }
                }
                string infoString = "";
                foreach (KeyValuePair<string, int> kvp in infoDict)
                {
                    string temp = kvp.Key;
                    while (!Char.IsLetter(temp, 0))
                    {
                        temp = temp.Substring(1, temp.Length - 1);
                    }
                    infoString = infoString + "\n " + temp + "  " + kvp.Value;
                }
                returnString = infoString;
            }
            return returnString;
        }


        //Always call after GetSalesBudget
        public Dictionary<int, int> GetSalesBudgetREG_TY()
        {
            return salesBudgetREG_TY;
        }


        internal Dictionary<int, int> GetKopesBudget_TY()
        {
            return kopesBudget_TY;
        }

        internal Dictionary<int, int> GetRelSalg_LY(bool fromNewQlickview)
        {
            if (currentSelectedYear < 2017)
            {
                LoadRelSalg_FromSQL(currentSelectedYear - 1);
                Console.WriteLine("Call 1 REl salg LY: " + currentSelectedYear + " prodNBR " + currentProdNumber);
                PrepareRelSalg_ForGUI(currentSelectedYear - 1);
            }
            if (currentSelectedYear > 2015)
            {
                //Get sales numbers from Qlickview
                //if (latestQueryQlickTable == null || latestQueryQlickTable.Rows.Count <= 0)
                //{
                //    LoadRelSalg_FromQlick();
                //}
                //ComputeNumberPerWeekDict(currentSelectedYear - 1);

            }
            return relSalg_LY;
        }

        internal Dictionary<int, int> GetRelSalg_TY(bool fromNewQlickview)
        {
            if (currentSelectedYear < 2016)
            {
                LoadRelSalg_FromSQL(currentSelectedYear);
                Console.WriteLine("Call 2 REl salg LY: " + currentSelectedYear + " prodNBR " + currentProdNumber);
                PrepareRelSalg_ForGUI(currentSelectedYear);
                int sum = debug2 - debug3;
                //Console.WriteLine(" var1: " + debug1 + " var2: " + debug2 + " var3: " + debug3 + " sum: " + sum); 
            }
            if (currentSelectedYear > 2014)
            {
                //Get sales numbers from Qlickview
                //if (latestQueryQlickTable == null || latestQueryQlickTable.Rows.Count <= 0)
                //{
                //    LoadRelSalg_FromQlick();
                //}
                //ComputeNumberPerWeekDict(currentSelectedYear);
            }
            return relSalg_TY;
        }

        public void LoadRelSalg_FromQlick()
        {
            ClassSQLQlickViewDataLayer conn = new ClassSQLQlickViewDataLayer();
            string query = "";
            query = query + @"SELECT ";
            query = query + @"SKMJDWDataMarts.dbo.FactFörsäljning.Kvantitet, SKMJDWDataMarts.dbo.FactFörsäljning.artikelid, SKMJDWDataMarts.dbo.FactFörsäljning.kundid, SKMJDWDataMarts.dbo.FactFörsäljning.KalenderIDBekräftadLeveransDatum, SKMJDWDataMarts.dbo.FactFörsäljning.FörsäljningID, SKMJDWDataMarts.dbo.DimArtikel.ArtikelNr , SKMJDWDataMarts.dbo.DimArtikel.ArtikelNamn ";
            query = query + @"FROM SKMJDWDataMarts.dbo.FactFörsäljning ";
            query = query + @"Inner join SKMJDWDataMarts.dbo.DimArtikel ";
            query = query + @"on SKMJDWDataMarts.dbo.DimArtikel.ArtikelId = SKMJDWDataMarts.dbo.FactFörsäljning.ArtikelId ";
            query = query + @"WHERE FörsäljningID > '60000000' ";
            query = query + @"AND SKMJDWDataMarts.dbo.DimArtikel.ArtikelNR =  'XXXXX' ";

            query = query.Replace("XXXXX", currentProdNumber);
            //queryString = queryString.Replace("YYYYY", custNumber);
            //Console.WriteLine("LoadRealiseretKampagnLY Query Qlickview: " + queryString2);
            latestQueryQlickTable = conn.QueryExWithTableReturn(query);
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


        private void PrepareRelSalg_ForGUI(int year)
        {
            int weekInt;
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
                    weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;


                    if (currentSelectedYear == year)
                    {
                        if (weekInt > 0 && weekInt < 54)
                        {

                            if (levEgetLager)
                            {
                                if (postType == 2)
                                {
                                    if (lokKode.Equals("8"))
                                    {
                                        relSalg_TY[weekInt] = relSalg_TY[weekInt] + Antal;
                                        if (currentProdNumber == "1442" && weekInt == 4)
                                        {
                                            debug1 = debug1 - Antal;
                                        }

                                    }

                                    else if (lokKode.Equals("100"))
                                    {
                                        relSalg_TY[weekInt] = relSalg_TY[weekInt] + Antal;
                                        if (currentProdNumber == "1442" && weekInt == 4)
                                        {
                                            Console.WriteLine(year + " Realiseret Salg: week: " + weekInt + " Antal: " + Antal + " posttype: " + postType + " bool: " + levEgetLager + " lokCode: " + lokKode);
                                            Console.WriteLine("antal Lokekod100: " + Antal);
                                            debug2 = debug2 + Antal;
                                        }

                                    }
                                }
                            }
                            else
                            {
                                //Console.WriteLine(tempDate + "Realiseret Salg: week: " + weekInt + " Antal: " + Antal + " posttype: " + postTypeString + " bool: " + levEgetLager + " lokCode: " + lokKode);

                                if (postType == 1)
                                {
                                    relSalg_TY[weekInt] = relSalg_TY[weekInt] - Antal;
                                    if (currentProdNumber == "1442" && weekInt == 4)
                                    {
                                        debug3 = debug3 + Antal;
                                    }
                                }
                            }
                        }
                    }
                    else if (currentSelectedYear == year + 1)
                    {
                        if (weekInt > 0 && weekInt < 54)
                        {
                            if (levEgetLager)
                            {
                                if (postType == 2)
                                {
                                    if (lokKode.Equals("8"))
                                    {
                                        relSalg_LY[weekInt] = relSalg_LY[weekInt] + Antal;
                                    }
                                    else if (lokKode.Equals("100"))
                                    {
                                        relSalg_LY[weekInt] = relSalg_LY[weekInt] + Antal;
                                    }
                                }
                            }
                            else
                            {
                                if (postType == 1)
                                {
                                    relSalg_LY[weekInt] = relSalg_LY[weekInt] - Antal;
                                }
                            }
                        }
                    }
                }
            }


        }

        private void LoadRelSalg_FromSQL(int year)
        {
            conn = new NavSQLExecute();
            string query = "";

            query = @"select Bogføringsdato, Faktureret_antal, LeverandørlagerOrdre, Posttype, Lokationskode  from  Varepost_Skyggetabel where Varenr='XXXX' and Bogføringsdato >= '" + startDateStrings[year] + "' and Bogføringsdato < '" + endDateStrings[year] + "' order by Bogføringsdato";




            query = query.Replace("XXXX", currentProdNumber);
            Console.WriteLine("LoadRelSalgsbudget Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(query);
            //throw new NotImplementedException();
        }

        internal Dictionary<int, int> GetKopsorder_TY()
        {
            Stopwatch stopwatch2 = Stopwatch.StartNew();
            LoadKopsorder_TY_FromSQL();
            Console.WriteLine("Time5aa: " + stopwatch2.ElapsedMilliseconds);
            PrepareKopsorder_TYForGUI();
            Console.WriteLine("Time5bb: " + stopwatch2.ElapsedMilliseconds);

            GetFromM3 m3 = new GetFromM3();
            string Warehouse = "LSK";
            string itemNumber = currentProdNumber;
            Dictionary<string, int> unitToNBR = new Dictionary<string, int>();
            unitToNBR = m3.GetUnitToNBR(itemNumber);
            Console.WriteLine("Time5cc: " + stopwatch2.ElapsedMilliseconds);

            Dictionary<int, int> kopsOrderM3 = m3.GetKopsorderFromM3(Warehouse, itemNumber, currentSelectedYear, unitToNBR);
            Console.WriteLine("Time5dd: " + stopwatch2.ElapsedMilliseconds);

            for (int i = 1; i < 54; i++)
            {
                kopsOrderTY[i] = kopsOrderTY[i] + kopsOrderM3[i];
            }
            return kopsOrderTY;            
        }


        private void LoadKopsorder_TY_FromSQL()
        {
            conn = new NavSQLExecute();

            string query = @"select Forventet_modtdato, Udestående_antal_basis  from Købslinie where Nummer='XXXX' and Forventet_modtdato >= '" + startDateStrings[currentSelectedYear] + "' and Forventet_modtdato < '" + endDateStrings[currentSelectedYear] + "' order by Forventet_modtdato";
            query = query.Replace("XXXX", currentProdNumber);
            Console.WriteLine("LoadKopsorder1 Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(query);

            query = @"select Forventet_modtdato, Antal_basis  from dbo.Købsleverancelinie where Nummer='XXXX' and Forventet_modtdato >= '" + startDateStrings[currentSelectedYear] + "' and Forventet_modtdato < '" + endDateStrings[currentSelectedYear] + "' order by Forventet_modtdato";
            query = query.Replace("XXXX", currentProdNumber);
            Console.WriteLine("LoadKopsorder2 Query: ");
            latestQueryTable2 = conn.QueryExWithTableReturn(query);

            conn.Close();
        }


        private void PrepareKopsorder_TYForGUI()
        {
            DataRow[] currentRows = latestQueryTable.Select(null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
            {
                Console.WriteLine("No Kopsorder Line Found 2");
            }
            else
            {
                //loop trough all rows and write in tabs
                foreach (DataRow row in currentRows)
                {
                    string levAntal = row["Udestående_antal_basis"].ToString();
                    int Antal = Convert.ToInt32(levAntal);

                    DateTime tempDate = DateTime.Parse(row["Forventet_modtdato"].ToString());
                    double week = (tempDate - ClassStaticVaribles.StartDate[currentSelectedYear]).TotalDays;
                    double weekNBR = week / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;


                    if (weekInt > 0 && weekInt < 54)
                    {
                        kopsOrderTY[weekInt] = kopsOrderTY[weekInt] + Antal;
                    }
                }
            }

            currentRows = latestQueryTable2.Select(null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
            {
                Console.WriteLine("No Kopsorder Line Found 3");
            }
            else
            {
                //loop trough all rows and write in tabs
                foreach (DataRow row in currentRows)
                {
                    string levAntal = row["Antal_basis"].ToString();
                    int Antal = Convert.ToInt32(levAntal);

                    DateTime tempDate = DateTime.Parse(row["Forventet_modtdato"].ToString());
                    double week = (tempDate - ClassStaticVaribles.StartDate[currentSelectedYear]).TotalDays;
                    double weekNBR = week / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;


                    if (weekInt > 0 && weekInt < 54)
                    {
                        kopsOrderTY[weekInt] = kopsOrderTY[weekInt] + Antal;
                    }
                }
            }

        }

        internal void SetSelectedYear(int selectedYear)
        {
            currentSelectedYear = selectedYear;
        }

        internal string GetBeskrivelse()
        {
            if (Beskrivelse.Length > 1)
            {
                return Beskrivelse;
            }
            return "A";

        }

        public int GetLactaVareNR()
        {
            if (Beskrivelse.Length > 1)
            {
                return LactalisVareNummer;
            }
            return 0;

        }




        public int GetLactalis_NBRPer_colli()
        {
            if (Beskrivelse.Length > 1)
            {
                return Lactalis_NBRPer_colli;
            }
            return 1;
        }


        public void UpdateVareKort()
        {
            loadVareKortTable();
            PrepareVareKort();
        }

        private void PrepareVareKort()
        {
            DataRow[] currentRows = latestQueryTable.Select(null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
            {
                Console.WriteLine("No Varekort Line Found 2");
            }
            else
            {
                //loop trough all rows and write in tabs
                foreach (DataRow row in currentRows)
                {
                    string beskrivelse = row["Beskrivelse"].ToString();
                    int Antal = Convert.ToInt32(row["Antal_forecast_uger"].ToString());
                    int FCMan = Convert.ToInt32(row["FC_man"].ToString());
                    int FCTis = Convert.ToInt32(row["FC_tis"].ToString());
                    int FCOns = Convert.ToInt32(row["FC_ons"].ToString());
                    int FCTors = Convert.ToInt32(row["FC_tor"].ToString());
                    int FCFre = Convert.ToInt32(row["FC_fre"].ToString());
                    int FCLor = Convert.ToInt32(row["FC_lor"].ToString());
                    int FCSon = Convert.ToInt32(row["FC_son"].ToString());
                    string temp = row["lactalis_varenr"].ToString();

                    if (temp.Length > 0)
                    {
                        LactalisVareNummer = Convert.ToInt32(temp);
                    }

                    Lactalis_NBRPer_colli = Convert.ToInt32(row["Antal_pr_kolli"].ToString());

                    if (FCMan + FCTis + FCOns + FCTors + FCFre + FCLor + FCSon != 100)
                    {
                        FCMan = 100;
                        FCTis = 0;
                        FCOns = 0;
                        FCTors = 0;
                        FCFre = 0;
                        FCLor = 0;
                        FCSon = 0;
                    }

                    weekPartPercentage = new int[] { Antal, FCMan, FCTis, FCOns, FCTors, FCFre, FCLor, FCSon };
                    Console.WriteLine("VareKort: beskriv: " + beskrivelse + " Antal Forecast Veckor:  " + Antal + " , Procent: " + FCMan +
                        ", " + FCTis + ", " + FCOns + ", " + FCTors + ", " + FCFre + ", " + FCLor + ", " + FCSon);
                    Beskrivelse = beskrivelse;
                }

            }
        }

        private void loadVareKortTable()
        {
            string query = @"select Antal_pr_kolli, Nummer, Beskrivelse, Antal_Forecast_uger, Forecast_fordelingsC_Mandag as FC_man, Forecast_fordelingsC_Tirsdag as FC_tis, Forecast_fordelingsC_Onsdag as FC_ons, Forecast_fordelingsC_Torsdag as FC_tor, Forecast_fordelingsC_Fredag as FC_fre, Forecast_fordelingsC_Lørdag as FC_lor, Forecast_fordelingsC_Søndag as FC_son, lactalis_varenr   from vare where nummer = 'XXXX' ";

            conn = new NavSQLExecute();


            query = query.Replace("XXXX", currentProdNumber);
            Console.WriteLine("LoadVarekort Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(query);


            conn.Close();
        }

        //Load the sales budget numbers from SQL Database
        private void LoadKopesBudgetForFileFromSQL()
        {
            conn = new NavSQLExecute();
            Dictionary<int, string> dateKopesBudget = new Dictionary<int, string>();
            dateKopesBudget.Add(1, @"2015/09/07");
            dateKopesBudget.Add(2, @"2019/09/07");
            string query = @"select Type, Antal, Navn_DebBogfGr, Startdato, Kommentar from Debitor_Budgetlinjepost where Type = '6' and Varenr='XXXX' and startdato >= '" + dateKopesBudget[1] + "' and startdato < '" + dateKopesBudget[2] + "' order by startdato";
            query = query.Replace("XXXX", currentProdNumber);
            Console.WriteLine("Load Sales Budget Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(query);
            conn.Close();
        }


        private void PrepareLoadedKopesBudgetForFile()
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
                    string stringType = row["Type"].ToString();
                    int intType = Convert.ToInt32(stringType);
                    string levAntal = row["Antal"].ToString();
                    int Antal = Convert.ToInt32(levAntal);
                    int Antal2 = Convert.ToInt32(row["Antal"]);
                    string comment = row["Kommentar"].ToString();

                    string levKedja = row["Navn_DebBogfGr"].ToString();

                    int dayFromNow = 0;
                    int thisWeekday = 100;

                    DateTime today = DateTime.Now;
                    while (thisWeekday != 0 && dayFromNow < 10)
                    {

                        DateTime answer = today.AddDays(dayFromNow);

                        //Console.WriteLine("day nmbr: " + (int)answer.DayOfWeek + " is: {0:dddd}", answer);
                        thisWeekday = (int)answer.DayOfWeek;
                        dayFromNow++;
                    }
                    DateTime dateNextMonday = today.AddDays(dayFromNow);
                    thisWeekday = (int)dateNextMonday.DayOfWeek;
                    if (dayFromNow > 0 && dayFromNow < 8 && thisWeekday == 1)
                    {


                        DateTime tempDate = DateTime.Parse(row["Startdato"].ToString());
                        double week = (tempDate - dateNextMonday).TotalDays;
                        double weekNBR = week / 7.0;
                        int weekNBRfromNowInt = (int)Math.Floor(weekNBR);
                        weekNBRfromNowInt = weekNBRfromNowInt + 2; //Next week is week 1

                        if (weekNBRfromNowInt >= 0 && weekNBRfromNowInt < 54)
                        {
                            if (intType == (int)TypeEnum.KøbsBudget)
                            {
                                kopesBudget_ForFile[weekNBRfromNowInt] = kopesBudget_ForFile[weekNBRfromNowInt] + Antal;
                                //Console.WriteLine("KopesBudget week: " + weekInt + " Antal: " + Antal);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error day is not monday!! this day: " + thisWeekday + "  days fromNow to next monday: " + dayFromNow);
                    }
                }

            }
        }




        internal Dictionary<int, string> GetRegComment_TY()
        {
            return SalgsbudgetReg_Comment;
        }

        internal int[] GetPercentageWeekArray()
        {
            if (weekPartPercentage == null)
            {
                weekPartPercentage = new int[] { 0, 100, 0, 0, 0, 0, 0, 0 };
            }
            return weekPartPercentage;
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

        internal Dictionary<int, int> GetKopesBudgetForFile()
        {
            LoadKopesBudgetForFileFromSQL();
            PrepareLoadedKopesBudgetForFile();
            return kopesBudget_ForFile;
        }
    }
}
