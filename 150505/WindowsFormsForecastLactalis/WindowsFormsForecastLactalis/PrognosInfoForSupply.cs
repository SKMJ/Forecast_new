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
        public PrognosInfoForSupply(string name, string number, bool showLastYear)
        {
            ProductName = name;
            ProductNumber = number;
            ShowLastYear = showLastYear;
        }

        public string ProductName = "";
        public string WareHouse = "";
        public string Supplier = "";
        public string PrepLocation = "";
        public bool InLactaFranceFile = false;
        public bool ShowLastYear = false;

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
        public Dictionary<int, int> Kopsbudget_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Kopsorder_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> KopsorderExpected_ThisYear = new Dictionary<int, int>();
        public List<ISalesRow> SalesRowsLastYear = new List<ISalesRow>();
        public List<ISalesRow> SalesRowsThisYear = new List<ISalesRow>();

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
            Dictionary<int, int> salesBudget_REG_TY = sqlSupplyCalls.GetSalesBudgetREG_TY();
            Dictionary<int, string> Reguleret_CommentTY = sqlSupplyCalls.GetRegComment_TY();
            Dictionary<int, int> kopesBudgetTY = sqlSupplyCalls.GetKopesBudget_TY();
            Console.WriteLine("Time4: " + stopwatch2.ElapsedMilliseconds);

            // Load Promotions
            Dictionary<int, int> KampagnTY = sqlSupplyCalls.GetKampagnTY();
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
                SalesRowsLastYear.AddRange(sqlSupplyCalls.GetRealizedSalesByYear(selectedYear - 1));
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
                Kampagn_ThisYear[i] = KampagnTY[i];
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
            stopwatch2.Stop();
            double timeQuerySeconds = stopwatch2.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Time for For Filling productifo : " + timeQuerySeconds.ToString() + " For Product Number: " + ProductNumber);
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
    }
}
