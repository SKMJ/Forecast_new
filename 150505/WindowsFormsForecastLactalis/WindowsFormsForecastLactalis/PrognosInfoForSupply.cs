///This file handles the data for the Supply forecast
///This is the class that keeps all the numbers product for product
///One instance of this calss for every product
///the supply prognos info not connected to any customer


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{

    public class PrognosInfoForSupply : IComparable
    {
        public PrognosInfoForSupply(string name, string number, int status)
        {
            ProductName = name;
            ProductNumber = number;
            Status = status;
        }

        public string ProductName = "";
        public string WareHouse = "";
        public string Supplier = "";
        public int Status;
        public string PrepLocation = "";
        public bool InLactaFranceFile = false;
        public bool ShowLastYear = false;
        public bool ShowCampaign = false;
        public int WeekToLockFrom = 0;

        public int NBRonStock = 0;

        int[] weekPartPercentage = new int[8]; //antal, mån, tis, ons....


        public string ProductNumber = "0";
        public Dictionary<int, int> RealiseretKampagn_LastYear = new Dictionary<int, int>();
        public Dictionary<int, int> Kampagn_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Salgsbudget_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, string> Salgsbudget_Comment = new Dictionary<int, string>();
        public Dictionary<int, string> Salgsbudget_ChangeHistory = new Dictionary<int, string>();
        public Dictionary<int, int> SalgsbudgetReguleret_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, string> SalgsbudgetReguleret_Comment = new Dictionary<int, string>();
        public Dictionary<int, int> SalgsbudgetReguleret_LastYear = new Dictionary<int, int>();
        public Dictionary<int, string> SalgsbudgetReguleret_CommentLY = new Dictionary<int, string>();
        public Dictionary<int, int> Kopsbudget_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Kopsorder_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> KopsorderExpected_ThisYear = new Dictionary<int, int>();
        public List<ISalesRow> SalesRowsLastYear = new List<ISalesRow>();
        public List<ISalesRow> SalesRowsThisYear = new List<ISalesRow>();
        public Dictionary<string, Dictionary<int, int>> childSales = new Dictionary<string, Dictionary<int, int>>();

        public Dictionary<int, int> Zeroed_ThisYear = new Dictionary<int, int>();

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            PrognosInfoSales otherPrognosInfo = obj as PrognosInfoSales;
            if (otherPrognosInfo != null)
            {
                return this.ProductNumber.CompareTo(otherPrognosInfo.ProductNumber);
            }
            else
            {
                throw new ArgumentException("Object is not a PrognosInfo");
            }
        }

        public void SetWhatToLoad(bool lastY, bool Campaign)
        {
            ShowCampaign = Campaign;
            ShowLastYear = lastY;
        }

        public void FillNumbers(int selectedYear)
        {
            Console.WriteLine("Fill info For Product Number: " + ProductNumber);

            if (ProductNumber.Contains("1088"))
            {
                Console.WriteLine("time");
            }
            Stopwatch stopwatch2 = Stopwatch.StartNew();

            string m3ProdNumber = GetM3ProdNumber();

            Console.WriteLine("Time1: " + stopwatch2.ElapsedMilliseconds);
            Console.WriteLine("WareHouse: " + WareHouse + " PrepLocation: " + PrepLocation + " Supplier: " + Supplier);

            NavSQLSupplyInformation sqlSupplyCalls = new NavSQLSupplyInformation(selectedYear, ProductNumber);
            sqlSupplyCalls.SetSelectedYear(selectedYear);
            sqlSupplyCalls.UpdateVareKort();
            Supplier = GetSupplierFromProduct();

            Console.WriteLine("Time3: " + stopwatch2.ElapsedMilliseconds);

            Dictionary<int, int> salesBudgetTY = sqlSupplyCalls.GetSalesBudget();
            if (StaticVariables.ProdMotherChildRelation.ContainsKey(ProductNumber))
            {
                Console.WriteLine("prod " + ProductNumber + "  " + StaticVariables.ProdMotherChildRelation[ProductNumber]);
                Dictionary<string, double> parts = new Dictionary<string, double>();
                childSales = new Dictionary<string, Dictionary<int, int>>();
                string[] listChilds = StaticVariables.ProdMotherChildRelation[ProductNumber].Split('=');
                foreach(string child in listChilds)
                {
                    string[] valueKvot = child.Split(';');
                    parts.Add(valueKvot[0], Convert.ToDouble(valueKvot[1]));

                }
                foreach(KeyValuePair<string,double> items in parts)
                {
                    Console.WriteLine("Barn: " + items.Key + " kvot: " + items.Value);
                    NavSQLSupplyInformation sqlSupplyCallsChild = new NavSQLSupplyInformation(selectedYear, items.Key);
                    sqlSupplyCallsChild.SetSelectedYear(selectedYear);
                    sqlSupplyCallsChild.UpdateVareKort();
                    //Supplier = GetSupplierFromProduct();

                    Console.WriteLine("Time3: " + stopwatch2.ElapsedMilliseconds);

                    Dictionary<int, int> salesBudgetChildTY = sqlSupplyCallsChild.GetSalesBudget();
                    Dictionary<int, int> salesBudgetChildNumberWhole = new Dictionary<int,int>();
                    foreach(KeyValuePair<int,int> salesNumber in salesBudgetChildTY)
                    {
                        double antalWhole = items.Value * salesNumber.Value;
                        antalWhole  = Math.Ceiling(antalWhole);
                        int tempInt = Convert.ToInt32(antalWhole);
                        salesBudgetChildNumberWhole.Add(salesNumber.Key, tempInt);
                    }
                    childSales.Add(items.Key, salesBudgetChildNumberWhole);
                }

            }
            Dictionary<int, int> salesBudget_REG_TY = sqlSupplyCalls.GetSalesBudgetREG_TY();
            Dictionary<int, string> Reguleret_CommentTY = sqlSupplyCalls.GetRegComment_TY();
            Dictionary<int, int> kopesBudgetTY = sqlSupplyCalls.GetKopesBudget_TY();
            Console.WriteLine("Time4: " + stopwatch2.ElapsedMilliseconds);

            // Load Promotions
            Dictionary<int, int> KampagnTY = new Dictionary<int, int>();
            if (ShowCampaign)
            {
                KampagnTY = sqlSupplyCalls.GetKampagnTY();
            }
            Dictionary<int, int> realiseretKampagnLY = new Dictionary<int, int>();
            if (ShowLastYear)
            {
                realiseretKampagnLY = sqlSupplyCalls.GetRealiseretKampagnLY();
            }
            Console.WriteLine("Time5: " + stopwatch2.ElapsedMilliseconds);

            // Load Salesdata
            SalesRowsThisYear.AddRange(sqlSupplyCalls.GetRealizedSalesByYear(selectedYear));
            if (ShowLastYear)
            {
                SalesRowsLastYear.AddRange(sqlSupplyCalls.GetRealizedSalesLastYear(selectedYear - 1));
            }
            Console.WriteLine("Time5A: " + stopwatch2.ElapsedMilliseconds);

            // Load Purchase order
            Dictionary<string, Dictionary<int, int>> kopsorder = sqlSupplyCalls.GetKopsorder_TY();
            Dictionary<int, int> kopsOrder_TY = kopsorder["received"];
            Dictionary<int, int> kopsOrderExp_TY = kopsorder["expected"];

            Console.WriteLine("Time5B: " + stopwatch2.ElapsedMilliseconds);

            for (int i = 0; i < 54; i++)
            {
                if (ShowLastYear)
                {
                    RealiseretKampagn_LastYear[i] = realiseretKampagnLY[i];

                }
                if (ShowCampaign)
                {
                    Kampagn_ThisYear[i] = KampagnTY[i];
                }
                Salgsbudget_ThisYear[i] = salesBudgetTY[i];

                SalgsbudgetReguleret_ThisYear[i] = salesBudget_REG_TY[i];
                Kopsbudget_ThisYear[i] = kopesBudgetTY[i];
                Kopsorder_ThisYear[i] = kopsOrder_TY[i];
                KopsorderExpected_ThisYear[i] = kopsOrderExp_TY[i];
                SalgsbudgetReguleret_Comment[i] = Reguleret_CommentTY[i];
            }
            Console.WriteLine("Time6: " + stopwatch2.ElapsedMilliseconds);
            string ProductNameTemp = sqlSupplyCalls.GetBeskrivelse();
            if (ProductName.Length > 2 && ProductName != "Unknown Name")
            {
                Console.WriteLine("Fill info For Product Number: " + ProductNumber + " LactaNAme: " + ProductNameTemp + " SKMJNAme: " + ProductName);
            }
            if (ProductNameTemp.Length > 2)
            {
                ProductName = ProductNameTemp;
            }
            string stDateString = sqlSupplyCalls.GetStartDate(selectedYear).Replace("/", "");
            string eDateString = sqlSupplyCalls.GetEndDate(selectedYear).Replace("/", "");
            Zeroed_ThisYear = GetZeroed(stDateString, eDateString);
            int weektemp = sqlSupplyCalls.GetCurrentWeekNBR();
            LockWeeksInfoFromM3(m3ProdNumber, weektemp);
            stopwatch2.Stop();
            double timeQuerySeconds = stopwatch2.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Time for For Filling productifo : " + timeQuerySeconds.ToString() + " For Product Number: " + ProductNumber);
        }

        private Dictionary<int, int> GetZeroed(string stDateString, string eDateString)
        {
            Dictionary<int, int> Zeroed = new Dictionary<int, int>();

            try
            {
                SQL_M3Direct m3SQL = new SQL_M3Direct();
                Zeroed = m3SQL.GetZeroedAllCustomersWholeYear(stDateString, eDateString , ProductNumber);
                m3SQL.Close();
            }
            catch
            {
                Zeroed = new Dictionary<int, int>();
                for (int i = 0; i <= 53; i++)
                {
                    Zeroed.Add(i, 0);
                }
            }
            return Zeroed;
        }

        public void UpdateReguleretInfo(int selectedYear)
        {
            NavSQLSupplyInformation sqlSupplyCalls = new NavSQLSupplyInformation(selectedYear, ProductNumber);
            sqlSupplyCalls.GetSalesBudget();
            Dictionary<int, int> salesBudget_REG_TY = sqlSupplyCalls.GetSalesBudgetREG_TY();
            Dictionary<int, string> Reguleret_CommentTY = sqlSupplyCalls.GetRegComment_TY();


            for (int i = 0; i < 54; i++)
            {
                SalgsbudgetReguleret_ThisYear[i] = salesBudget_REG_TY[i];
                SalgsbudgetReguleret_Comment[i] = Reguleret_CommentTY[i];
            }
        }

        private void GetAllForeCastSpecialFieldsFromM3(int selectedYear, string m3ProdNumber)
        {
            GetFromM3 m3Info = new GetFromM3();
            NavSQLSupplyInformation sqlSupply = new NavSQLSupplyInformation(selectedYear, ProductNumber);
            bool Completed = false;
            int laps = 1;
            while (!Completed && laps <= 3)
            {
                Completed = ExecuteWithTimeLimit(TimeSpan.FromMilliseconds(1000), () =>
                {
                    Dictionary<string, string> info = m3Info.GetItemInfoByItemNumber(m3ProdNumber);
                    if (info != null && info.Count > 0 && info["prepLocation"].Length > 0)
                    {
                        WareHouse = info["whsLocation"];
                        PrepLocation = info["prepLocation"];

                        if (Convert.ToInt32(info["INLActaFranceFile"]) > 0)
                        {
                            InLactaFranceFile = true;
                        }

                        weekPartPercentage = new int[] { Convert.ToInt32(info["forecastWeek"]), Convert.ToInt32(info["FCMO"]), Convert.ToInt32(info["FCTU"]), Convert.ToInt32(info["FCWE"]), Convert.ToInt32(info["FCTH"]), Convert.ToInt32(info["FCFR"]), Convert.ToInt32(info["FCSA"]), Convert.ToInt32(info["FCSU"]) };
                    }
                    else
                    {
                        weekPartPercentage = sqlSupply.GetPercentageWeekArray();
                    }

                });
                Console.WriteLine("SpecialFileds Completed: " + Completed + " lap: " + laps);
                if (!Completed)
                {
                    System.Threading.Thread.Sleep(1500);
                }

                laps++;
            }
        }

        //Load from m3 is timebound since it sometimes hangs 
        // If it hangs: Redo after one second
        private List<BalanceId> GetM3StockInfo(string m3ProdNumber)
        {
            List<String> warehouses = new List<string> { "LDI", "LSK" };
            List<BalanceId> balanceIdentities = new List<BalanceId>();
            List<BalanceId> tempBalance = new List<BalanceId>();
            GetFromM3 m3Info = new GetFromM3();
            foreach(string warehouse in warehouses)
            {
                //Försök 3 gånger om man får null då API:et tydligen är lite ostadigt
                tempBalance = m3Info.GetStockInfo(m3ProdNumber, warehouse);
                if(tempBalance == null)
                {
                    tempBalance = m3Info.GetStockInfo(m3ProdNumber, warehouse);
                }
                if (tempBalance == null)
                {
                    tempBalance = m3Info.GetStockInfo(m3ProdNumber, warehouse);
                }
                if (tempBalance == null)
                {
                    tempBalance = m3Info.GetStockInfo(m3ProdNumber, warehouse);
                }

                if (tempBalance != null)
                {
                    balanceIdentities.AddRange(tempBalance);
                }
            }
            return balanceIdentities;
        }

        private string GetM3ProdNumber()
        {
            string tempProdNBr = ProductNumber;

            return tempProdNBr;
        }

        private string GetSupplierFromProduct()
        {
            string returnSuppl = "";
            string tempProdNBr = GetM3ProdNumber();

            if (StaticVariables.ProdToSupplDict.ContainsKey(tempProdNBr))
            {
                returnSuppl = StaticVariables.ProdToSupplDict[tempProdNBr];
            }
            return returnSuppl;
        }

        public static bool ExecuteWithTimeLimit(TimeSpan timeSpan, Action codeBlock)
        {
            try
            {
                Task task = Task.Factory.StartNew(() => codeBlock());
                task.Wait(timeSpan);
                return task.IsCompleted;
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("Exception in time bound code Supply");
                throw ae.InnerExceptions[0];
            }
        }

        internal List<BalanceId> GetStockInfo()
        {
            string m3ProdNumber = GetM3ProdNumber();
            return GetM3StockInfo(m3ProdNumber);
        }

        /// <summary>
        /// Load from m3 is timebound since it sometimes hangs 
        /// If it hangs redo after one second
        /// </summary>
        /// <param name="m3ProdNumber"></param>
        /// <param name="weektemp"></param>
        private void LockWeeksInfoFromM3(string m3ProdNumber, int currWeek)
        {
            GetFromM3 m3Info = new GetFromM3();

            bool Completed = false;
            int laps = 1;
            while (!Completed && laps < 3)
            {
                Completed = ExecuteWithTimeLimit(TimeSpan.FromMilliseconds(1000), () =>
                {
                    //
                    // Write your time bounded code here
                    // 
                    Dictionary<string, string> info = m3Info.GetItemInfoByItemNumber(m3ProdNumber);
                    if (info != null && info.Count > 0 && Convert.ToInt32(info["FCLockSale"]) > 0)
                    {
                        int tempDaysLock = Convert.ToInt32(info["FCLockSale"]);

                        if (tempDaysLock <= 0)
                        {
                            
                            tempDaysLock = Convert.ToInt32(m3Info.GetSalesWeekLockFromSupplier(GetSupplierFromProduct()));

                        }
                        int tempWeeksToLOCK = tempDaysLock / 7;
                        WeekToLockFrom = tempWeeksToLOCK + currWeek;
                    }
                    else
                    {
                        int tempDaysLock = 0;
                        if (tempDaysLock <= 0)
                        {
                            tempDaysLock = Convert.ToInt32(m3Info.GetSalesWeekLockFromSupplier(GetSupplierFromProduct()));

                        }
                        int tempWeeksToLOCK = tempDaysLock / 7;
                        WeekToLockFrom = tempWeeksToLOCK + currWeek; 
                    }
                });

                Console.WriteLine("Completed: " + Completed + " lap: " + laps);
                laps++;
            }
        }

    }
}
