using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    class SQLCallsSalesCustomerInfo
    {
        DataTable latestQueryTable; //Holds latest loaded Query output
        DataTable latestQueryTable2; //Holds latest loaded Query output if 2 needed
        NavSQLExecute conn;

        Dictionary<string, string> allCustomers = new Dictionary<string, string>();
        Dictionary<int, int> salesBudgetTY = new Dictionary<int, int>();
        Dictionary<int, int> salesBudgetLY = new Dictionary<int, int>();
        Dictionary<int, int> realKampagn_LY = new Dictionary<int, int>();
        Dictionary<int, int> kampagn_TY = new Dictionary<int, int>();
        

        DateTime startdate2016; //Week one day one
        DateTime startdate2015; //Week one day one
        DateTime startdate2014;//Week one day one
        int thisYear = 2015;

        enum TypeEnum
        {
            SalgsBudget = 4,
            SalgsBudget_Regulering = 5,
            KøbsBudget = 6
        };


        //Constructor
        public SQLCallsSalesCustomerInfo()
        {
            string st = "12/29/2014";
            startdate2015 = DateTime.Parse(st);
            st = "12/30/2013";
            startdate2014 = DateTime.Parse(st);
            st = "01/04/2016";
            startdate2016 = DateTime.Parse(st);

            for (int k = 0; k < 53; k++)
            {
                salesBudgetTY.Add(k, 0);
                salesBudgetLY.Add(k, 0);
                realKampagn_LY.Add(k, 0);
                kampagn_TY.Add(k, 0);
                //salesBudgetREG.Add(k, 0);
                //kopesBudget.Add(k, 0);
                //relSalg_LY.Add(k, 0);
                //kopsOrderTY.Add(k, 0);
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

            string queryString = @"select Navn_DebBogfGr, DebitorBogføringsruppe  from Debitor_Budgetlinjepost where  startdato >= '2014/12/29' and startdato < '2016/01/05' order by Navn_DebBogfGr";
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
        public Dictionary<int, int> GetSalesBudgetTY(int prodNumber, string customerName)
        {
            LoadSalesBudgetForProductFromSQL(prodNumber, customerName, thisYear);
            PrepareLoadedSalesBudgetForGUI(thisYear);
            return salesBudgetTY;
        }

        //Load the sales budget numbers from SQL Database
        private void LoadSalesBudgetForProductFromSQL(int prodNumber, string customerName, int year)
        {
            conn = new NavSQLExecute();
            string queryString = "";
            if(year == 2015)
            {
                queryString = @"select Type, Antal, Debitorbogføringsruppe, Startdato, Kommentar from Debitor_Budgetlinjepost where Debitorbogføringsruppe='YYYY' and Varenr='XXXX' and startdato >= '2014/12/29' and startdato < '2016/01/05' order by startdato";
            } 
            else if (year == 2014)
            {
                queryString = @"select Type, Antal, Debitorbogføringsruppe, Startdato, Kommentar from Debitor_Budgetlinjepost where Debitorbogføringsruppe='YYYY' and Varenr='XXXX' and startdato >= '2013/12/30' and startdato < '2014/12/30' order by startdato";
            }
            else if (year == 2016)
            {
                queryString = @"select Type, Antal, Debitorbogføringsruppe, Startdato, Kommentar from Debitor_Budgetlinjepost where Debitorbogføringsruppe='YYYY' and Varenr='XXXX' and startdato >= '2016/01/04' and startdato < '2017/01/01' order by startdato";
            } 

            
            queryString = queryString.Replace("XXXX", prodNumber.ToString());
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

                    //string levKedja = row["Navn_DebBogfGr"].ToString();

                    DateTime tempDate = DateTime.Parse(row["Startdato"].ToString());
                    double week = (tempDate - startdate2014).TotalDays; 
                    if (year == 2015)
                    {
                        week = (tempDate - startdate2015).TotalDays;  //queryString = @"select Type, Antal, Navn_DebBogfGr, Startdato, Kommentar from Debitor_Budgetlinjepost where Navn_DebBogfGr='YYYY' and Varenr='XXXX' and startdato >= '2014/12/29' and startdato < '2016/01/05' order by startdato";
                    }
                    else if (year == 2016)
                    {
                        week = (tempDate - startdate2016).TotalDays;  //queryString = @"select Type, Antal, Navn_DebBogfGr, Startdato, Kommentar from Debitor_Budgetlinjepost where Navn_DebBogfGr='YYYY' and Varenr='XXXX' and startdato >= '2014/12/29' and startdato < '2016/01/05' order by startdato";
                    }

                    
                    double weekNBR = week / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);
                    weekInt = weekInt + 1;

                    if (intType == (int)TypeEnum.SalgsBudget && weekInt < 53)
                    {
                        if (year == thisYear)
                        {
                            salesBudgetTY[weekInt] = salesBudgetTY[weekInt] + Antal;
                        }
                        else
                        {
                            salesBudgetLY[weekInt] = salesBudgetLY[weekInt] + Antal;
                        }

                        antalTot = antalTot + Antal;
                    }
                }

            }
        }




        internal Dictionary<int, int> GetSalesBudget_LY(int ProductNumber, string CustomerName)
        {
            LoadSalesBudgetForProductFromSQL(ProductNumber, CustomerName, thisYear-1);
            PrepareLoadedSalesBudgetForGUI(thisYear - 1);
            return salesBudgetLY;
        }



        public Dictionary<int, int> GetRealiseretKampagnLY(int prodNumber, string custNumber)
        {
            LoadRealiseretKampagnLY_FromSQL(prodNumber, custNumber);
            PrepareRealiseretKampagnLY_ForGUI();
            return realKampagn_LY;
        }



        //Load info for the RealiseretKampagnLY from SQL 
        private void LoadRealiseretKampagnLY_FromSQL(int prodNumber, string custNumber)
        {
            conn = new NavSQLExecute();

            string queryString = @"select startdato, Antal_Realiseret  from Afsl__Kampagnelinier where Debitorbogføringsgruppe='YYYY' and Varenr='XXXX' and startdato >= '2013/12/30' and startdato < '2014/12/30' order by startdato";
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
                    double week = (tempDate - startdate2014).TotalDays;
                    double weekNBR = week / 7.0;
                    int weekInt = (int)Math.Floor(weekNBR);


                    if (weekInt > 0 && weekInt < 53)
                    {
                        Console.WriteLine("Antal Kampagn Rel: " + Antal);
                        realKampagn_LY[weekInt] = realKampagn_LY[weekInt] + Antal;
                    }
                }
            }
        }


        internal Dictionary<int, int> GetKampagnTY(int prodNumber, string custNumber)
        {
            LoadKampagnTY_FromSQL(prodNumber, custNumber);
            PrepareKampagnTY_ForGUI();
            return kampagn_TY;
        }


        private void LoadKampagnTY_FromSQL(int prodNumber, string custNumber)
        {
            conn = new NavSQLExecute();

            string queryString = @"select startdato, Antal_Realiseret  from Afsl__Kampagnelinier where Debitorbogføringsgruppe='YYYY' and Varenr='XXXX' and startdato >= '2014/12/29' and startdato < '2016/01/05' order by startdato";
            queryString = queryString.Replace("XXXX", prodNumber.ToString());
            queryString = queryString.Replace("YYYY", custNumber);
            Console.WriteLine("LoadKampagnTY1 Query: ");
            latestQueryTable = conn.QueryExWithTableReturn(queryString);

            queryString = @"select Dato, Antal_Budget  from Kampagnelinier_Fordelt where Debitorbogføringsgruppe='YYYY' and Varenr='XXXX' and Dato >= '2014/12/29' and Dato < '2016/01/05' order by Dato";
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
    }
}
