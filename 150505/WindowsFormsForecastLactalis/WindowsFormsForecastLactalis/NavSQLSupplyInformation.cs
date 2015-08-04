using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    class NavSQLSupplyInformation
    {
        DataTable latestQueryTable; //Holds latest loaded Query output
        DataTable latestQueryTable2; //Holds latest loaded Query output if 2 needed

        string Beskrivelse = "A";
        NavSQLExecute conn;
        Dictionary<string, string> infoDict = new Dictionary<string, string>();
        Dictionary<int, int> salesBudget_TY = new Dictionary<int, int>();
        Dictionary<int, int> salesBudgetREG_TY = new Dictionary<int, int>();
        Dictionary<int, int> kopesBudget_TY = new Dictionary<int, int>();

        Dictionary<int, int> realKampagn_LY = new Dictionary<int, int>();

        Dictionary<int, int> kampagn_TY = new Dictionary<int, int>();
        Dictionary<int, int> relSalg_LY = new Dictionary<int, int>();
        Dictionary<int, int> relSalg_TY = new Dictionary<int, int>();

        Dictionary<int, int> kopsOrderTY = new Dictionary<int, int>();

        Dictionary<int, string> startDateStrings = new Dictionary<int, string>();
        Dictionary<int, string> endDateStrings = new Dictionary<int, string>();

        Dictionary<int, DateTime> startDate = new Dictionary<int, DateTime>();

        int debug1 = 0;
        int debug2 = 0;
        int debug3 = 0;

        int currentSelectedYear = 2015;
        int currentProdNumber = 2015;

        enum TypeEnum
        {
            SalgsBudget = 4,
            SalgsBudget_Regulering = 5,
            KøbsBudget = 6
        };


        //Constructor
        public NavSQLSupplyInformation(int yearForInfo, int prodNumberForInfo)
        {
            InitiateProperties();
            currentProdNumber = prodNumberForInfo;
            currentSelectedYear = yearForInfo;

        }

        private void InitiateProperties()
        {
            string st = "12/29/2014";
            startDate.Add(2015, DateTime.Parse(st));
            st = "12/30/2013";
            startDate.Add(2014, DateTime.Parse(st));
            st = "01/04/2016";
            startDate.Add(2016, DateTime.Parse(st));
            st = "01/02/2017";
            startDate.Add(2017, DateTime.Parse(st));

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
            }
        }

        //Load the sales budget numbers from SQL Database
        private void LoadSalesBudgetTYFromSQL()
        {
            conn = new NavSQLExecute();

            string queryString = @"select Type, Antal, Navn_DebBogfGr, Startdato, Kommentar from Debitor_Budgetlinjepost where Varenr='XXXX' and startdato >= '" + startDateStrings[currentSelectedYear] + "' and startdato < '" + endDateStrings[currentSelectedYear] + "' order by startdato";
            queryString = queryString.Replace("XXXX", currentProdNumber.ToString());
            Console.WriteLine("Load Sales Budget Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(queryString);
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

                    string levKedja = row["Navn_DebBogfGr"].ToString();

                    DateTime tempDate = DateTime.Parse(row["Startdato"].ToString());
                    double week = (tempDate - startDate[currentSelectedYear]).TotalDays;
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

                    DateTime tempDate = DateTime.Parse(row["Startdato"].ToString());
                    double week1 = (tempDate - startDate[currentSelectedYear]).TotalDays;
                    double weekNBR = week1 / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;

                    if (intType == (int)TypeEnum.SalgsBudget && weekInt == week)
                    {
                        if (!infoDict.ContainsKey(levKedja))
                        {
                            infoDict.Add(levKedja, levAntal + comment);
                        }
                        else
                        {
                            i++;
                            string temp = i.ToString() + " " + levKedja;
                            infoDict.Add(temp, levAntal + "  " + comment);
                        }
                    }
                }
                string infoString = "";
                foreach (KeyValuePair<string, string> kvp in infoDict)
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


        //Load info for the RealiseretKampagnLY from SQL 
        private void LoadRealiseretKampagnLY_FromSQL()
        {
            conn = new NavSQLExecute();

            string queryString = @"select startdato, Antal_Realiseret  from Afsl__Kampagnelinier where Varenr='XXXX' and startdato >= '" + startDateStrings[currentSelectedYear - 1] + "' and startdato < '" + endDateStrings[currentSelectedYear - 1] + "' order by startdato";
            queryString = queryString.Replace("XXXX", currentProdNumber.ToString());
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
                    double week = (tempDate - startDate[currentSelectedYear - 1]).TotalDays;
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
            LoadKampagnTY_FromSQL();
            PrepareKampagnTY_ForGUI();
            return kampagn_TY;
        }


        private void LoadKampagnTY_FromSQL()
        {
            conn = new NavSQLExecute();

            string queryString = @"select startdato, Antal_Realiseret  from Afsl__Kampagnelinier where Varenr='XXXX' and startdato >= '" + startDateStrings[currentSelectedYear] + "' and startdato < '" + endDateStrings[currentSelectedYear] + "' order by startdato";
            queryString = queryString.Replace("XXXX", currentProdNumber.ToString());
            Console.WriteLine("LoadKampagnTY1 Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(queryString);

            queryString = @"select Dato, Antal_Budget, Beskrivelse  from Kampagnelinier_Fordelt where Varenr='XXXX' and Dato >= '" + startDateStrings[currentSelectedYear] + "' and Dato < '" + endDateStrings[currentSelectedYear] + "' order by Dato";
            queryString = queryString.Replace("XXXX", currentProdNumber.ToString());
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
                    double week = (tempDate - startDate[currentSelectedYear]).TotalDays;
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
                    double week = (tempDate - startDate[currentSelectedYear]).TotalDays;
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


            string queryString = @"select *  from Kampagnelinier_Fordelt where Varenr='XXXX' and Dato >= '" + startDateStrings[currentSelectedYear] + "' and Dato < '" + endDateStrings[currentSelectedYear] + "' order by Dato";
            queryString = queryString.Replace("XXXX", prodNumber.ToString());
            Console.WriteLine("LoadKampagnTY3 Query: ");
            latestQueryTable2 = conn.QueryExWithTableReturn(queryString);

            conn.Close();

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
                    string levAntal = row["Antal_Budget"].ToString();
                    int Antal = Convert.ToInt32(levAntal);
                    string levKedja = row["Kæde_Kilde"].ToString();
                    //string comment = row["Kommentar"].ToString();

                    DateTime tempDate = DateTime.Parse(row["Startdato"].ToString());
                    double week1 = (tempDate - startDate[currentSelectedYear]).TotalDays;
                    double weekNBR = week1 / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;

                    if (weekInt == week)
                    {
                        if (!infoDict.ContainsKey(levKedja))
                        {
                            infoDict.Add(levKedja, levAntal);
                        }
                        else
                        {
                            i++;
                            string temp = i.ToString() + " " + levKedja;
                            infoDict.Add(temp, levAntal);
                        }
                    }
                }
                string infoString = "";
                foreach (KeyValuePair<string, string> kvp in infoDict)
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

        internal Dictionary<int, int> GetRelSalg_LY()
        {
            LoadRelSalg_LY_FromSQL(currentSelectedYear - 1);
            Console.WriteLine("Call 1 REl salg LY: " + currentSelectedYear + " prodNBR " + currentProdNumber);
            PrepareRelSalg_LYForGUI(currentSelectedYear - 1);
            return relSalg_LY;
        }

        internal Dictionary<int, int> GetRelSalg_TY()
        {
            LoadRelSalg_LY_FromSQL(currentSelectedYear);
            Console.WriteLine("Call 2 REl salg LY: " + currentSelectedYear + " prodNBR " + currentProdNumber);
            PrepareRelSalg_LYForGUI(currentSelectedYear);
            int sum = debug2 - debug3;
            Console.WriteLine(" var1: " + debug1 + " var2: " + debug2 + " var3: " + debug3 + " sum: " + sum);
            return relSalg_TY;
        }

        private void PrepareRelSalg_LYForGUI(int year)
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

                    dayOFYear = (tempDate - startDate[year]).TotalDays;

                    double weekNBR = dayOFYear / 7.0;
                    weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;

                    //if (currentProdNumber == 1442 && weekInt == 4)
                    //{
                    //    //Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" + year + " Realiseret Salg: week: " + weekInt + " Antal: " + Antal + " posttype: " + postType + " bool: " + levEgetLager + " lokCode: " + lokKode);
                    //    //Console.WriteLine("antal Lokekod100: " + Antal);
                    //    debug2 = debug2 + Antal;
                    //}


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
                                        if (currentProdNumber == 1442 && weekInt == 4)
                                        {
                                            debug1 = debug1 - Antal;
                                        }

                                    }

                                    else if (lokKode.Equals("100"))
                                    {
                                        relSalg_TY[weekInt] = relSalg_TY[weekInt] + Antal;
                                        if (currentProdNumber == 1442 && weekInt == 4)
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
                                    if (currentProdNumber == 1442 && weekInt == 4)
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
                                //Console.WriteLine(tempDate + "Realiseret Salg: week: " + weekInt + " Antal: " + Antal + " posttype: " + postTypeString + " bool: " + levEgetLager + " lokCode: " + lokKode);

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

        private void LoadRelSalg_LY_FromSQL(int year)
        {
            conn = new NavSQLExecute();
            string queryString = "";

            queryString = @"select Bogføringsdato, Faktureret_antal, LeverandørlagerOrdre, Posttype, Lokationskode  from  Varepost_Skyggetabel where Varenr='XXXX' and Bogføringsdato >= '" + startDateStrings[year] + "' and Bogføringsdato < '" + endDateStrings[year] + "' order by Bogføringsdato";




            queryString = queryString.Replace("XXXX", currentProdNumber.ToString());
            Console.WriteLine("LoadRelSalgsbudget Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(queryString);
            //throw new NotImplementedException();
        }

        internal Dictionary<int, int> GetKopsorder_TY()
        {
            LoadKopsorder_TY_FromSQL();
            PrepareKopsorder_TYForGUI();
            return kopsOrderTY;
        }


        private void LoadKopsorder_TY_FromSQL()
        {
            conn = new NavSQLExecute();

            string queryString = @"select Forventet_modtdato, Udestående_antal_basis  from Købslinie where Nummer='XXXX' and Forventet_modtdato >= '" + startDateStrings[currentSelectedYear] + "' and Forventet_modtdato < '" + endDateStrings[currentSelectedYear] + "' order by Forventet_modtdato";
            queryString = queryString.Replace("XXXX", currentProdNumber.ToString());
            Console.WriteLine("LoadKopsorder1 Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(queryString);

            queryString = @"select Forventet_modtdato, Antal_basis  from dbo.Købsleverancelinie where Nummer='XXXX' and Forventet_modtdato >= '" + startDateStrings[currentSelectedYear] + "' and Forventet_modtdato < '" + endDateStrings[currentSelectedYear] + "' order by Forventet_modtdato";
            queryString = queryString.Replace("XXXX", currentProdNumber.ToString());
            Console.WriteLine("LoadKopsorder2 Query: ");
            latestQueryTable2 = conn.QueryExWithTableReturn(queryString);

            conn.Close();
        }


        private void PrepareKopsorder_TYForGUI()
        {
            DataRow[] currentRows = latestQueryTable.Select(null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
            {
                Console.WriteLine("No Kopsorder Line Found");
            }
            else
            {
                //loop trough all rows and write in tabs
                foreach (DataRow row in currentRows)
                {
                    string levAntal = row["Udestående_antal_basis"].ToString();
                    int Antal = Convert.ToInt32(levAntal);

                    DateTime tempDate = DateTime.Parse(row["Forventet_modtdato"].ToString());
                    double week = (tempDate - startDate[currentSelectedYear]).TotalDays;
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
                Console.WriteLine("No Kopsorder Line Found");
            }
            else
            {
                //loop trough all rows and write in tabs
                foreach (DataRow row in currentRows)
                {
                    string levAntal = row["Antal_basis"].ToString();
                    int Antal = Convert.ToInt32(levAntal);

                    DateTime tempDate = DateTime.Parse(row["Forventet_modtdato"].ToString());
                    double week = (tempDate - startDate[currentSelectedYear]).TotalDays;
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
            else
            {
                return "A";
            }
        }
    }
}
