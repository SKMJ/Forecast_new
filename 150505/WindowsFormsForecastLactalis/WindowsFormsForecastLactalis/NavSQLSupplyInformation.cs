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
        NavSQLExecute conn;
        Dictionary<string, string> infoDict = new Dictionary<string, string>();
        Dictionary<int, int> salesBudget = new Dictionary<int, int>();
        Dictionary<int, int> salesBudgetREG = new Dictionary<int, int>();
        Dictionary<int, int> kopesBudget = new Dictionary<int, int>();

        Dictionary<int, int> realKampagn_LY = new Dictionary<int, int>();

        Dictionary<int, int> kampagn_TY = new Dictionary<int, int>();
        Dictionary<int, int> relSalg_LY = new Dictionary<int, int>();
        Dictionary<int, int> kopsOrderTY = new Dictionary<int, int>();
        DateTime startdate2015; //Week one day one
        DateTime startdate2014;//Week one day one

        enum TypeEnum
        {
            SalgsBudget = 4,
            SalgsBudget_Regulering = 5,
            KøbsBudget = 6
        };


        //Constructor
        public NavSQLSupplyInformation()
        {
            string st = "12/29/2014";
            startdate2015 = DateTime.Parse(st);
            st = "12/30/2013";
            startdate2014 = DateTime.Parse(st);

            for (int k = 0; k < 53; k++)
            {
                salesBudget.Add(k, 0);
                realKampagn_LY.Add(k, 0);
                kampagn_TY.Add(k, 0);
                salesBudgetREG.Add(k, 0);
                kopesBudget.Add(k, 0);
                relSalg_LY.Add(k, 0);
                kopsOrderTY.Add(k, 0);
            }
        }

        //Load the sales budget numbers from SQL Database
        private void LoadSalesBudgetForProductFromSQL(int prodNumber)
        {
            conn = new NavSQLExecute();

            string queryString = @"select Type, Antal, Navn_DebBogfGr, Startdato, Kommentar from Debitor_Budgetlinjepost where Varenr='XXXX' and startdato >= '2014/12/29' and startdato < '2016/01/05' order by startdato";
            queryString = queryString.Replace("XXXX", prodNumber.ToString());
            Console.WriteLine("Load Sales Budget Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(queryString);
            conn.Close();
        }

         //Load the sales budget numbers from SQL Database

       


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
                    double week = (tempDate - startdate2015).TotalDays;
                    double weekNBR = week / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;

                    if (intType == (int)TypeEnum.SalgsBudget)
                    {
                        salesBudget[weekInt] = salesBudget[weekInt] + Antal;

                        antalTot = antalTot + Antal;
                    }
                    else if (intType == (int)TypeEnum.SalgsBudget_Regulering)
                    {
                        salesBudgetREG[weekInt] = salesBudgetREG[weekInt] + Antal;
                        //Console.WriteLine("SalesBudgetREg week: " + weekInt + " Antal: " + Antal);
                    }
                    else if (intType == (int)TypeEnum.KøbsBudget)
                    {
                        kopesBudget[weekInt] = kopesBudget[weekInt] + Antal;
                        //Console.WriteLine("KopesBudget week: " + weekInt + " Antal: " + Antal);
                    }
                }

            }
        }

        //Get sales buget data as dictionary
        public Dictionary<int, int> GetSalesBudget(int prodNumber)
        {
            LoadSalesBudgetForProductFromSQL(prodNumber);
            PrepareLoadedSalesBudgetForGUI();
            return salesBudget;
        }


        //Get info about salesbuget number for specific week
        public string GetSalesBudgetWeekInfo(int week, int prodNumber)
        {
            LoadSalesBudgetForProductFromSQL(prodNumber);
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
                    double week1 = (tempDate - startdate2015).TotalDays;
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
        private void LoadRealiseretKampagnLY_FromSQL(int prodNumber)
        {
            conn = new NavSQLExecute();

            string queryString = @"select startdato, Antal_Realiseret  from Afsl__Kampagnelinier where Varenr='XXXX' and startdato >= '2013/12/30' and startdato < '2014/12/30' order by startdato";
            queryString = queryString.Replace("XXXX", prodNumber.ToString());
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
                    double week = (tempDate - startdate2014).TotalDays;
                    double weekNBR = week / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);


                    if (weekInt > 0 && weekInt < 53)
                    {
                        realKampagn_LY[weekInt] = realKampagn_LY[weekInt] + Antal;
                    }
                }
            }
        }

        public Dictionary<int, int> GetRealiseretKampagnLY(int prodNumber)
        {
            LoadRealiseretKampagnLY_FromSQL(prodNumber);
            PrepareRealiseretKampagnLY_ForGUI();
            return realKampagn_LY;
        }

        internal Dictionary<int, int> GetKampagnTY(int prodNumber)
        {
            LoadKampagnTY_FromSQL(prodNumber);
            PrepareKampagnTY_ForGUI();
            return kampagn_TY;
        }


        private void LoadKampagnTY_FromSQL(int prodNumber)
        {
            conn = new NavSQLExecute();

            string queryString = @"select startdato, Antal_Realiseret  from Afsl__Kampagnelinier where Varenr='XXXX' and startdato >= '2014/12/29' and startdato < '2016/01/05' order by startdato";
            queryString = queryString.Replace("XXXX", prodNumber.ToString());
            Console.WriteLine("LoadKampagnTY1 Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(queryString);

            queryString = @"select Dato, Antal_Budget  from Kampagnelinier_Fordelt where Varenr='XXXX' and Dato >= '2014/12/29' and Dato < '2016/01/05' order by Dato";
            queryString = queryString.Replace("XXXX", prodNumber.ToString());
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
                    double week = (tempDate - startdate2015).TotalDays;
                    double weekNBR = week / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);


                    if (weekInt > 0 && weekInt < 53)
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
                    double week = (tempDate - startdate2015).TotalDays;
                    double weekNBR = week / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);


                    if (weekInt > 0 && weekInt < 53)
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


            string queryString = @"select *  from Kampagnelinier_Fordelt where Varenr='XXXX' and Dato >= '2014/12/29' and Dato < '2016/01/05' order by Dato";
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
                    double week1 = (tempDate - startdate2015).TotalDays;
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
        public Dictionary<int, int> GetSalesBudgetREG(int prodNumber)
        {
            return salesBudgetREG;
        }


        internal Dictionary<int, int> GetKopesBudget(int prodNumber)
        {
            return kopesBudget;
        }

        internal Dictionary<int, int> GetRelSalg_LY(int prodNumber)
        {
            LoadRelSalgsbudget_LY_FromSQL(prodNumber, 2014);
            PrepareRelSalgsbudget_LYForGUI(2014);
            return relSalg_LY;
        }

        internal Dictionary<int, int> GetRelSalg_TY(int prodNumber)
        {
            LoadRelSalgsbudget_LY_FromSQL(prodNumber, 2015);
            PrepareRelSalgsbudget_LYForGUI(2015);
            return relSalg_LY;
        }

        private void PrepareRelSalgsbudget_LYForGUI(int year)
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
                    if (year == 2014)
                    {
                        dayOFYear = (tempDate - startdate2014).TotalDays;
                    }
                    else
                    {
                        dayOFYear = (tempDate - startdate2015).TotalDays;
                    }
                    double weekNBR = dayOFYear / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;

                    if (Antal > 0)
                    {

                        //Console.WriteLine(tempDate + "Realiseret Salg: week: " + weekInt + " Antal: " + Antal + " posttype: " + postTypeString + " bool: " + levEgetLager + " lokCode: " + lokKode);
                    }


                    if (weekInt > 0 && weekInt < 53)
                    {
                        if (levEgetLager)
                        {
                            if (lokKode.Equals("8"))
                            {
                                relSalg_LY[weekInt] = relSalg_LY[weekInt] - Antal;
                            }
                            else if (lokKode.Equals("100"))
                            {
                                relSalg_LY[weekInt] = relSalg_LY[weekInt] - Antal;
                            }
                        }
                        else
                        {
                            //Console.WriteLine(tempDate + "Realiseret Salg: week: " + weekInt + " Antal: " + Antal + " posttype: " + postTypeString + " bool: " + levEgetLager + " lokCode: " + lokKode);

                            if (postType == 1)
                            {
                                relSalg_LY[weekInt] = relSalg_LY[weekInt] + Math.Abs(Antal);
                            }
                        }
                    }
                }
            }


        }

        private void LoadRelSalgsbudget_LY_FromSQL(int prodNumber, int year)
        {
            conn = new NavSQLExecute();
            string queryString = "";
            if (year == 2014)
            {
                queryString = @"select Bogføringsdato, Faktureret_antal, LeverandørlagerOrdre, Posttype, Lokationskode  from  Varepost_Skyggetabel where Varenr='XXXX' and Bogføringsdato >= '2013/12/30' and Bogføringsdato < '2014/12/30'";

            }
            else
            {
                queryString = @"select Bogføringsdato, Faktureret_antal, LeverandørlagerOrdre, Posttype, Lokationskode  from  Varepost_Skyggetabel where Varenr='XXXX' and Bogføringsdato >= '2014/12/29' and Bogføringsdato < '2016/01/05'";

            }

            queryString = queryString.Replace("XXXX", prodNumber.ToString());
            Console.WriteLine("LoadRelSalgsbudget Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(queryString);
            //throw new NotImplementedException();
        }

        internal Dictionary<int, int> GetKopsorder_TY(int prodNumber)
        {
            LoadKopsorder_TY_FromSQL(prodNumber);
            PrepareKopsorder_TYForGUI();
            return kopsOrderTY;
        }


        private void LoadKopsorder_TY_FromSQL(int prodNumber)
        {
            conn = new NavSQLExecute();

            string queryString = @"select Forventet_modtdato, Udestående_antal_basis  from Købslinie where Nummer='XXXX' and Forventet_modtdato >= '2014/12/29' and Forventet_modtdato < '2016/01/05' order by Forventet_modtdato";
            queryString = queryString.Replace("XXXX", prodNumber.ToString());
            Console.WriteLine("LoadKopsorder1 Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(queryString);

            queryString = @"select Forventet_modtdato, Antal_basis  from dbo.Købsleverancelinie where Nummer='XXXX' and Forventet_modtdato >= '2014/12/29' and Forventet_modtdato < '2016/01/05' order by Forventet_modtdato";
            queryString = queryString.Replace("XXXX", prodNumber.ToString());
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
                    double week = (tempDate - startdate2015).TotalDays;
                    double weekNBR = week / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;


                    if (weekInt > 0 && weekInt < 53)
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
                    double week = (tempDate - startdate2015).TotalDays;
                    double weekNBR = week / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);


                    if (weekInt > 0 && weekInt < 53)
                    {
                        kopsOrderTY[weekInt] = kopsOrderTY[weekInt] + Antal;
                    }
                }
            }

        }
    }
}
