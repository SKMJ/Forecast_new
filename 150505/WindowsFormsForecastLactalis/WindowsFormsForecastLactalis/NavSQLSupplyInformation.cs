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

        List<ISalesRow> salesRows = new List<ISalesRow>();

        Dictionary<int, int> relSalgQlick_LY = new Dictionary<int, int>();
        Dictionary<int, int> relSalgQlick_TY = new Dictionary<int, int>();

        Dictionary<int, int> kopsOrderTY = new Dictionary<int, int>();
        Dictionary<int, int> expKopsOrderTY = new Dictionary<int, int>();

        Dictionary<int, string> startDateStrings = new Dictionary<int, string>();
        Dictionary<int, string> endDateStrings = new Dictionary<int, string>();
        List<ISalesRow> salesRowsLast ;

        int[] weekPartPercentage;

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
            endDateStrings.Add(2017, @"2017/12/31");
            startDateStrings.Add(2018, @"2018/01/01");
            endDateStrings.Add(2018, @"2018/12/31");
            startDateStrings.Add(2019, @"2018/12/31");
            endDateStrings.Add(2019, @"2019/12/30");

            for (int k = 0; k < 54; k++)
            {
                salesBudget_TY.Add(k, 0);
                realKampagn_LY.Add(k, 0);
                kampagn_TY.Add(k, 0);
                salesBudgetREG_TY.Add(k, 0);
                kopesBudget_TY.Add(k, 0);
                kopsOrderTY.Add(k, 0);
                expKopsOrderTY.Add(k, 0);
                SalgsbudgetReg_Comment.Add(k, " ");
                kopesBudget_ForFile.Add(k, 0);
                relSalgQlick_LY.Add(k, 0);
                relSalgQlick_TY.Add(k, 0);
            }
        }

        //Load the sales budget numbers from SQL Database
        private void LoadSalesBudgetTYFromSQL()
        {
            string firstD = GetStartDate(currentSelectedYear);
            string endD = GetEndDate(currentSelectedYear);
            conn = new NavSQLExecute();

            string query = @"select Type, Antal, Navn_DebBogfGr, Startdato, Kommentar from " +
                StaticVariables.TableDebitorBudgetLinjePost + " where Varenr='XXXX' and startdato >= '" + firstD + "' and startdato < '" + endD + "' order by TasteDato";
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

                    DateTime tempDate = StaticVariables.GetDateTimeFromString(row["Startdato"].ToString());

                    int weekInt = StaticVariables.GetForecastWeeknumberForDate(tempDate);

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

                    //clear country
                    levKedja = levKedja.Replace("DK  ", "");
                    levKedja = levKedja.Replace("FI  ", "");
                    levKedja = levKedja.Replace("SE  ", "");
                    levKedja = levKedja.Replace("NO  ", "");
                    levKedja = levKedja.Replace("XX  ", "");


                    
                    DateTime tempDate = StaticVariables.GetDateTimeFromString(row["Startdato"].ToString());
                    int weekInt = StaticVariables.GetForecastWeeknumberForDate(tempDate);


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
                string currentLine = String.Format("{0, -20}\t{1, -80}\t{2}", "Value", "Customer", Environment.NewLine);
                infoString = infoString + currentLine;
                var items = from pair in infoDict
                            orderby pair.Key ascending
                            select pair;
                foreach (KeyValuePair<string, int> kvp in items)
                {
                    string temp = kvp.Key;

                    while (temp.Length > 0 && !Char.IsLetter(temp, 0))
                    {
                        temp = temp.Substring(1, temp.Length - 1);
                    }


                    currentLine = String.Format("{0, -20}\t{1, -80}\t{2}", kvp.Value, temp, Environment.NewLine);
    
                    //infoString = infoString + "\n " + temp + "  " + kvp.Value;
                    infoString = infoString  + currentLine;
                    

                }
                returnString = infoString;
            }
            return returnString;
        }


        //Load info for the RealiseretKampagnLY from SQL 
        private void LoadRealiseretKampagnLY_FromSQL()
        {
            conn = new NavSQLExecute();
            string firstD = GetStartDate(currentSelectedYear-1);
            string endD = GetEndDate(currentSelectedYear-1);

            string query = @"select startdato, Antal_Realiseret  from Afsl__Kampagnelinier where Varenr='XXXX' and startdato >= '" + firstD + "' and startdato < '" + endD + "' order by startdato";
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

                    DateTime tempDate = StaticVariables.GetDateTimeFromString(row["startdato"].ToString());

                    int weekInt = StaticVariables.GetForecastWeeknumberForDate(tempDate);

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
            if (currentSelectedYear > 2015 || currentSelectedYear < 2000)
            {
                GetFromM3 m3 = new GetFromM3();
                Dictionary<int, int> promotions = m3.GetPromotionsOfProducts(currentProdNumber, currentSelectedYear);
                foreach (var promotionItem in promotions)
                {
                    if (currentSelectedYear < 2000)
                    {

                        kampagn_TY[promotionItem.Key] += promotionItem.Value;
                        //DateTime tempKampagnDay = StaticVariables.GetForecastStartDateOfWeeknumber(promotionItem.Year, promotionItem.Week);
                        //bool withinLimit = StaticVariables.DateWithinNowForecastLimit(tempKampagnDay);
                        //if (withinLimit) //only witihin 20 weeks count
                        //{
                        //    kampagn_TY[promotionItem.Key] += promotionItem.Value;
                        //}
                    }
                    else
                    {
                        kampagn_TY[promotionItem.Key] += promotionItem.Value;
                    }
                }
            }
            if (currentSelectedYear < 2017)
            {
                LoadKampagnTY_FromSQL();
                PrepareKampagnTY_ForGUI();

            }
            return kampagn_TY;
        }


        private void LoadKampagnTY_FromSQL()
        {
            conn = new NavSQLExecute();
            string firstD = GetStartDate(currentSelectedYear);
            string endD = GetEndDate(currentSelectedYear);

            string query = @"select startdato, Antal_Realiseret  from Afsl__Kampagnelinier where Varenr='XXXX' and startdato >= '" + firstD + "' and startdato < '" + endD + "' order by startdato";
            query = query.Replace("XXXX", currentProdNumber);
            Console.WriteLine("LoadKampagnTY1 Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(query);

            query = @"select Dato, Antal_Budget, Beskrivelse  from Kampagnelinier_Fordelt where Varenr='XXXX' and Dato >= '" + firstD + "' and Dato < '" + endD + "' order by Dato";
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

        //Get info about salesbuget number for specific week
        public string GetKampagnWeekInfo(int week, int prodNumber)
        {
            conn = new NavSQLExecute();
            string firstD = GetStartDate(currentSelectedYear);
            string endD = GetEndDate(currentSelectedYear);

            string query = @"select *  from Kampagnelinier_Fordelt where Varenr='XXXX' and Dato >= '" + firstD + "' and Dato < '" + endD + "' order by Dato";
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
                   
                    DateTime tempDate = StaticVariables.GetDateTimeFromString(row["Startdato"].ToString());
                    int weekInt = StaticVariables.GetForecastWeeknumberForDate(tempDate);


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

        public List<ISalesRow> GetRealizedSalesByYear(int year)
        {
            List<ISalesRow> salesRows = new List<ISalesRow>();
            if (year < 2017)
            {
                salesRows.AddRange(LoadRelSalg_FromSQL(year, currentProdNumber));
            }
            if (year > 2015 || year < 2000)
            {
                salesRows.AddRange(LoadRealizedSalesFromM3Sales(year, currentProdNumber));
            }
            return salesRows;
        }

        public List<ISalesRow> GetRealizedSalesLastYear(int year)
        {
            List<ISalesRow> salesRows = new List<ISalesRow>();
            if (year < 2017)
            {
                salesRows.AddRange(LoadRelSalg_FromSQL(year, currentProdNumber));
            }
            if (year > 2015 || year < 2000)
            {
                salesRows.AddRange(GetRealizedSalesLastYear());
            }
            return salesRows;
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

            latestQueryQlickTable = conn.QueryExWithTableReturn(query);


            conn.Close();
        }

        private int GetWeek(DateTime date, int year)
        {
            int weekInt = StaticVariables.GetForecastWeeknumberForDate(date);
            return weekInt;

        }

        private List<ISalesRow> LoadRelSalg_FromSQL(int year, string ItemNumber)
        {
            conn = new NavSQLExecute();
            string query = "";

            query = @"SELECT DISTINCT Løbenr, Bogføringsdato, ISNULL(Styk_antal, Faktureret_antal) Styk_antal, LeverandørlagerOrdre, Posttype, Lokationskode, Varenr, AssortmentName
                    FROM  Varepost_Skyggetabel vs
                    LEFT OUTER JOIN dbo.AssortmentNav an ON Bogfgruppe = an.NavAssortment
                    LEFT OUTER JOIN " + StaticVariables.TableChainAssortment + @" AS a ON an.Assortment = a.AssortmentCode
            WHERE Varenr='XXXX' AND Bogføringsdato >= '" + startDateStrings[year] + "' AND Bogføringsdato < '" + endDateStrings[year] + "' ORDER BY Bogføringsdato";

            query = query.Replace("XXXX", currentProdNumber);
            Console.WriteLine("LoadRelSalgsbudget Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(query);
            List<ISalesRow> salesRows = new List<ISalesRow>();
            DataTable table = conn.QueryExWithTableReturn(query);
            DataRowCollection rows = table.Rows;
            foreach (DataRow row in rows)
            {
                bool levEgetLager = (bool)row["LeverandørlagerOrdre"];
                string lokKode = row["Lokationskode"].ToString();
                int postType = Convert.ToInt32(row["Posttype"].ToString());
                DateTime date = (DateTime)row["Bogføringsdato"];
                int week = GetWeek(date, year);
        
                DateTime tempDate = StaticVariables.GetDateTimeFromString(row["Bogføringsdato"].ToString());
                
                int weekInt = StaticVariables.GetForecastWeeknumberForDate(tempDate);
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
                    salesRows.Add(new SalesRowNavision()
                    {
                        Date = (DateTime)row["Bogføringsdato"],
                        SupplyOwnStock = (bool)row["LeverandørlagerOrdre"],
                        Quantity = (int)Math.Round(quantity, 0),
                        Posttype = Convert.ToInt32(row["Posttype"].ToString()),
                        ItemNumber = row["Varenr"].ToString(),
                        Week = weekInt,
                        CustomerName = row["AssortmentName"].ToString(),
                    });
                }
            }
            return salesRows;
        }

        private List<ISalesRow> LoadRealizedSalesFromM3Sales(int year, String itemNumber)
        {
            int salesYear = year;
            if(salesYear == 1000)
            {
                salesYear = DateTime.Now.Year;
            }
            else if (salesYear == 999)
            {
                salesYear = DateTime.Now.Year -1 ;
            }
            List<ISalesRow> salesRows = new List<ISalesRow>();
            salesRowsLast = new List<ISalesRow>();
            string tableSales = StaticVariables.TableM3Sales;
            string tableChainAssortment = StaticVariables.TableChainAssortment;
            string tableChainCustomer = StaticVariables.TableChainCustomer;
            conn = new NavSQLExecute();
            string query = "";
            query = String.Format(@"SELECT 
	                                    [UCORNO],
	                                    [UCDLDT],
                                        [MTTRQT],
	                                    [UCORNO],
	                                    [UCITNO],
	                                    [UCCUNO],
                                        [F1A130],
                                        [LMEXPI],
                                        [UCORQT],
	                                    CASE 
		                                    WHEN a.assortmentName IS NOT NULL	THEN a.assortmentName
		                                    WHEN a1.assortmentName IS NOT NULL	THEN a1.assortmentName
		                                    WHEN a2.assortmentName IS NOT NULL	THEN a2.assortmentName
		                                    WHEN a3.assortmentName IS NOT NULL	THEN a3.assortmentName
		                                    ELSE 'Øvrige'
	                                    END AssortmentName
                                        FROM " + tableSales + @" 
                                        LEFT OUTER JOIN " + tableChainCustomer + @" AS c ON " + tableSales + @".UCCUNO = c.Customer
                                        LEFT OUTER JOIN " + tableChainAssortment + @" AS a ON a.Chain = c.Chain
	                                    LEFT OUTER JOIN " + tableChainAssortment + @" a1 ON a1.Chain = UCCHL1
	                                    LEFT OUTER JOIN " + tableChainAssortment + @" a2 ON a2.Chain = UCCHL2
	                                    LEFT OUTER JOIN " + tableChainAssortment + @" a3 ON a3.Chain = UCCHL3
                                        WHERE UCITNO = '{0}' AND (LEFT(UCDLDT, 4) = {1} OR LEFT(UCDLDT, 4) = {2})", itemNumber, salesYear, salesYear-1);
            //Console.WriteLine("LoadRelSalg_FromM3SQL Query: ");
            //Console.WriteLine(query);
            DataTable table = conn.QueryExWithTableReturn(query);
            DataRowCollection rows = table.Rows;

            foreach (DataRow row in rows)
            {
                DateTime date = StaticVariables.ParseExactStringToDate(row["UCDLDT"].ToString(),
                                                    "yyyyMMdd",
                                                    CultureInfo.InvariantCulture);
                string mttrqt = row["MTTRQT"].ToString();
                string UCORQT = row["UCORQT"].ToString();
                int quantityOrdered = (int)Convert.ToDecimal(UCORQT);
                int quantity = (int)Convert.ToDecimal(mttrqt);
                int week = GetWeek(date, year);
                int wantedbbd = (int)Convert.ToInt32(row["F1A130"].ToString());
                int bbd = (int)Convert.ToInt32(row["LMEXPI"].ToString());

                bool lastYearInNextYear;
                bool inShowRange;
                lastYearInNextYear = false;
                inShowRange = true;
                double dateDiff = (DateTime.Now - date).TotalDays;

                if (year < 2000 && dateDiff > 23 * 7 ||
                    (year > date.Year))
                {
                    inShowRange = false;
                }

                if (inShowRange)
                {
                    salesRows.Add(new SalesRow()
                    {
                        Date = date,
                        OrderNumber = row["UCORNO"].ToString(),
                        Quantity = quantity,
                        QuantityOrdered = quantityOrdered,
                        Customer = row["UCCUNO"].ToString(),
                        ItemNumber = row["UCITNO"].ToString(),
                        Week = week,
                        BestBeforeDate = bbd,
                        WantedBestBeforeDate = wantedbbd,
                        CustomerName = row["AssortmentName"].ToString()
                    });
                }

                if (year < 2000 && dateDiff > 23 * 7 && dateDiff < 365)
                {
                    lastYearInNextYear = true;
                }


                if (date.Year == year - 1 || lastYearInNextYear)
                {

                    salesRowsLast.Add(new SalesRow()
                    {
                        Date = date,
                        OrderNumber = row["UCORNO"].ToString(),
                        Quantity = quantity,
                        QuantityOrdered = quantityOrdered,
                        Customer = row["UCCUNO"].ToString(),
                        ItemNumber = row["UCITNO"].ToString(),
                        Week = week,
                        BestBeforeDate = bbd,
                        WantedBestBeforeDate = wantedbbd,
                        CustomerName = row["AssortmentName"].ToString()
                    });
                }
                
            }
            Console.WriteLine("Last year sales row: " + salesRowsLast.Count + " This year Sales Row: " + salesRows.Count);

            return salesRows;
        }

        private List<ISalesRow> GetRealizedSalesLastYear()
        {
            return salesRowsLast;
        }



        internal Dictionary<string, Dictionary<int, int>> GetKopsorder_TY()
        {
            Stopwatch stopwatch2 = Stopwatch.StartNew();
            LoadKopsorder_TY_FromSQL();
            Console.WriteLine("Time5aa: " + stopwatch2.ElapsedMilliseconds);
            PrepareKopsorder_TYForGUI();
            Console.WriteLine("Time5bb: " + stopwatch2.ElapsedMilliseconds);

            GetFromM3 m3 = new GetFromM3();
            List<string> Warehouses = new List<string> { "LSK", "LDI" };
            //string Warehouse = "LSK";
            string itemNumber = currentProdNumber;
            Dictionary<string, int> unitToNBR = new Dictionary<string, int>();
            unitToNBR = m3.GetUnitToNBR(itemNumber);
            Console.WriteLine("Time5cc: " + stopwatch2.ElapsedMilliseconds);
            foreach (string Warehouse in Warehouses)
            {
                var poLines = m3.GetKopsorderFromM3(Warehouse, itemNumber, currentSelectedYear, unitToNBR);
                StaticVariables.PurchaseOrderLinesM3.AddRange(poLines["receivedLines"]);
                StaticVariables.ExpectedPurchaseOrderLinesM3.AddRange(poLines["expectedLines"]);
            }
            Console.WriteLine("Time5dd: " + stopwatch2.ElapsedMilliseconds);
            var linesPerWeek = StaticVariables.PurchaseOrderLinesM3.GroupBy(l => new
            {
                l.ItemNumber,
                l.Week
            }).Select(l => new
            {
                Week = l.Key.Week,
                Item = l.Key.ItemNumber,
                Sum = l.Sum(x => x.Quantity)
            });

            for (int i = 1; i < 54; i++)
            {
                var sum = from line in linesPerWeek
                          where line.Week == i && line.Item == itemNumber
                          select line.Sum;
                kopsOrderTY[i] += sum.FirstOrDefault();
            }

            linesPerWeek = StaticVariables.ExpectedPurchaseOrderLinesM3.GroupBy(l => new
            {
                l.ItemNumber,
                l.Week
            }).Select(l => new
            {
                Week = l.Key.Week,
                Item = l.Key.ItemNumber,
                Sum = l.Sum(x => x.Quantity)
            });
            for (int i = 1; i < 54; i++)
            {
                var sum = from line in linesPerWeek
                          where line.Week == i && line.Item == itemNumber
                          select line.Sum;
                expKopsOrderTY[i] += sum.FirstOrDefault();
            }


            //Confirmed
            Dictionary<int, int> expKonfirmed = new Dictionary<int, int>();
            for (int i = 0; i < 54; i++)
            {
                expKonfirmed.Add(i, 0);
            }
            
            var linesConfirmedPerWeek = StaticVariables.ExpectedPurchaseOrderLinesM3.GroupBy(l => new
            {
                l.ItemNumber,
                l.Week
            }).Select(l => new
            {
                Week = l.Key.Week,
                Item = l.Key.ItemNumber,
                Sum = l.Sum(x => x.ConfirmedQuantity)
            });
            for (int i = 1; i < 54; i++)
            {
                var sum = from line in linesConfirmedPerWeek
                          where line.Week == i && line.Item == itemNumber
                          select line.Sum;
                expKonfirmed[i] += sum.FirstOrDefault();
            }




            Dictionary<string, Dictionary<int, int>> kopsOrder = new Dictionary<string, Dictionary<int, int>>();
            kopsOrder["received"] = kopsOrderTY;
            kopsOrder["expected"] = expKopsOrderTY;
            kopsOrder["confirmed"] = expKonfirmed;
            return kopsOrder;
        }


        private void LoadKopsorder_TY_FromSQL()
        {
            conn = new NavSQLExecute();

            string firstD = GetStartDate(currentSelectedYear);
            string endD = GetEndDate(currentSelectedYear);

            string query = @"select Forventet_modtdato, Udestående_antal_basis  from Købslinie where Nummer='XXXX' and Forventet_modtdato >= '" + firstD + "' and Forventet_modtdato < '" + endD + "' order by Forventet_modtdato";
            query = query.Replace("XXXX", currentProdNumber);
            Console.WriteLine("LoadKopsorder1 Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(query);

            query = @"select Forventet_modtdato, Antal_basis  from dbo.Købsleverancelinie where Nummer='XXXX' and Forventet_modtdato >= '" + firstD + "' and Forventet_modtdato < '" + endD + "' order by Forventet_modtdato";
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

                    DateTime tempDate = StaticVariables.GetDateTimeFromString(row["Forventet_modtdato"].ToString());

                    int weekInt = StaticVariables.GetForecastWeeknumberForDate(tempDate);



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

                    DateTime tempDate = StaticVariables.GetDateTimeFromString(row["Forventet_modtdato"].ToString());

                    int weekInt = StaticVariables.GetForecastWeeknumberForDate(tempDate);



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
            if (Beskrivelse.Length > 1 && Lactalis_NBRPer_colli > 0)
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
            string query = @"select Type, Antal, Navn_DebBogfGr, Startdato, Kommentar from " +
                StaticVariables.TableDebitorBudgetLinjePost +
                " where Type = '6' and Varenr='XXXX' and startdato >= '" + dateKopesBudget[1] + "' and startdato < '" + dateKopesBudget[2] + "' order by startdato";
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


                        DateTime tempDate = StaticVariables.GetDateTimeFromString(row["Startdato"].ToString());
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
            int weekInt = StaticVariables.GetForecastWeeknumberForDate(tempDate);
            return weekInt;
        }

        internal Dictionary<int, int> GetKopesBudgetForFile()
        {
            LoadKopesBudgetForFileFromSQL();
            PrepareLoadedKopesBudgetForFile();
            return kopesBudget_ForFile;
        }

        public string GetEndDate(int year)
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

        public string GetStartDate(int year)
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
    }
}
