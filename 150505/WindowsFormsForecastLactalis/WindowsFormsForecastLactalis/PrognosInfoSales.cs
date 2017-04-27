///This file handles the data for the Sales forecast
///This is the class that keeps all the numbers product for product
///One instance of this calss for every product and customer
///the supply prognos info is connected to customer numbers

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;


namespace WindowsFormsForecastLactalis
{
    public class PrognosInfoSales : IComparable
    {
        public PrognosInfoSales(string name, string number, string customerNumber, int status)
        {
            ProductName = name;
            ProductNumber = number;
            CustomerNumberM3 = customerNumber;
            CustomerCodeNav = StaticVariables.GetCustNavCodes(customerNumber);
            Status = status;
        }

        public string ProductName = "";
        public string ProductNumber = "0";
        public string CustomerNumberM3 = "";
        public List<string> CustomerCodeNav = new List<string>();
        public int WeekToLockFrom = 0;
        public int Status;
        public bool showLastYear = false;
        public bool showLastKampagn = false;

        //public Dictionary<int, int> RealiseretKampagn_LastYear = new Dictionary<int, int>();
        public Dictionary<int, int> Kampagn_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Salgsbudget_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Salgsbudget_LastYear = new Dictionary<int, int>();
        public Dictionary<int, string> Salgsbudget_Comment = new Dictionary<int, string>();
        public Dictionary<int, string> Salgsbudget_CommentLY = new Dictionary<int, string>();
        public Dictionary<int, string> Salgsbudget_ChangeHistory = new Dictionary<int, string>();
        public List<PromotionInfo> PromotionLines = new List<PromotionInfo>();
        public List<ISalesRow> SalesRowsLastYear = new List<ISalesRow>();
        public List<ISalesRow> SalesRowsThisYear = new List<ISalesRow>();

        public Dictionary<int, int> Zeroed_ThisYear = new Dictionary<int, int>();


        int[] weekPartPercentage = new int[8]; //antal, mån, tis, ons....

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
            showLastKampagn = Campaign;
            showLastYear = lastY;
        }

        public void FillNumbers(int selectedYear)
        {
            Console.WriteLine("Fill info For Product Number: " + ProductNumber);
            Stopwatch stopwatch2 = Stopwatch.StartNew();
            NavSQLSupplyInformation sqlSupplyCalls = new NavSQLSupplyInformation(selectedYear, ProductNumber);
            Console.WriteLine("Time1: " + stopwatch2.ElapsedMilliseconds);
            sqlSupplyCalls.UpdateVareKort();
            int weektemp = sqlSupplyCalls.GetCurrentWeekNBR();
            string m3ProdNumber = GetM3ProdNumber();

            LockWeeksInfoFromM3(m3ProdNumber, weektemp);
            if(WeekToLockFrom>52)
            {
                Console.WriteLine(" Week over 52: prodnbr:" + ProductNumber + " week: " + WeekToLockFrom);
            }
            Console.WriteLine("Time2: " + stopwatch2.ElapsedMilliseconds);

            if (ProductName.Length < 2 || ProductName == "Name Unknown")
            {
                ProductName = sqlSupplyCalls.GetBeskrivelse();
            }

            SQLCallsSalesCustomerInfo sqlSalesCalls = new SQLCallsSalesCustomerInfo();
            sqlSalesCalls.SetYear(selectedYear);
            Console.WriteLine("Time3: " + stopwatch2.ElapsedMilliseconds);
            Dictionary<int, int> salesBudgetTY = sqlSalesCalls.GetSalesBudgetTY(ProductNumber, CustomerCodeNav);
            Dictionary<int, string> Sales_CommentTY = sqlSalesCalls.GetSalesComment_TY();
            Console.WriteLine("Time4: " + stopwatch2.ElapsedMilliseconds);
            Dictionary<int, int> salesBudgetLY = new Dictionary<int, int>();
            Dictionary<int, string> Sales_CommentLY = new Dictionary<int, string>();
            if (showLastYear)
            {
                salesBudgetLY = sqlSalesCalls.GetSalesBudget_LY(ProductNumber, CustomerCodeNav);
                Sales_CommentLY = sqlSalesCalls.GetSalesComment_LY();
                Console.WriteLine("Time5: " + stopwatch2.ElapsedMilliseconds);
            }
            
            //Load Promotion data
            Dictionary<int, int> KampagnTY;
            KampagnTY = new Dictionary<int, int>();
            for (int i = 0; i < 54; i++)
            {
                KampagnTY.Add(i, 0);
            }
            string stDateString = sqlSupplyCalls.GetStartDate(selectedYear).Replace("/","");
            string eDateString = sqlSupplyCalls.GetEndDate(selectedYear).Replace("/", "");

            Zeroed_ThisYear = GetZeroed(stDateString, eDateString);


            if (showLastKampagn)
            {
                if (selectedYear > 2015 || selectedYear < 2000)
                {
                    GetFromM3 m3 = new GetFromM3();
                    PromotionLines = m3.GetPromotionForAllCustomerAsListv2(ProductNumber, selectedYear);
                    foreach (PromotionInfo item in PromotionLines)
                    {
                        List<string> KadjeNiva = new List<string>();

                        //Get which chain the promotion is connected to
                        if (item.PromotionType == PromotionInfo.PromotionTypeEnum.CHAIN)
                        {
                            KadjeNiva = m3.GetAllKampaignLevelsByCuse(item.Chain);
                        }
                        else if (item.PromotionType == PromotionInfo.PromotionTypeEnum.CUSTOMER)
                        {
                            KadjeNiva = new ChainStructureHandler().GetChainsFromCustomer(item.Customer);
                        }

                        //This if statement checks if the campaign is connected to this assortment
                        int num = (from asch in StaticVariables.AssortmentM3_toKedjor[CustomerNumberM3]
                                   from ch in KadjeNiva
                                   where ch.StartsWith(asch)
                                   select ch).Count();
                        if (num > 0)
                        {
                            if (selectedYear < 2000)
                            {
                                DateTime tempKampagnDay = StaticVariables.GetForecastStartDateOfWeeknumber(item.Year, item.Week);
                                bool withinLimit = StaticVariables.DateWithinNowForecastLimit(tempKampagnDay);
                                if (withinLimit) //only witihin 20 weeks count
                                {
                                    KampagnTY[item.Week] = KampagnTY[item.Week] + item.Quantity;
                                }
                            }
                            else
                            {
                                KampagnTY[item.Week] = KampagnTY[item.Week] + item.Quantity;
                            }
                        }
                    }
                }
                if (selectedYear < 2017)
                {
                    Dictionary<int, int> promotions = sqlSalesCalls.GetKampagnTY(ProductNumber, CustomerCodeNav);
                    foreach (var promotionItems in promotions)
                    {
                        KampagnTY[promotionItems.Key] += promotionItems.Value;
                    }
                }
            }
            //Dictionary<int, int> realiseretKampagnLY = sqlSalesCalls.GetRealiseretKampagnLY(ProductNumber, CustomerCodeNav);
            Console.WriteLine("Time6: " + stopwatch2.ElapsedMilliseconds);

            //Load Sales data
            SalesRowsThisYear.AddRange(sqlSalesCalls.GetRealizedSalesByYear(selectedYear, ProductNumber, CustomerNumberM3, CustomerCodeNav));
            if (showLastYear)
            {
                SalesRowsLastYear.AddRange(sqlSalesCalls.GetLoadedLastYearSalesByYear());
            }
            Console.WriteLine("Time7: " + stopwatch2.ElapsedMilliseconds);
            if (ProductName.Length < 2)
            {
                ProductName = sqlSalesCalls.GetBeskrivelse();
            }

            Console.WriteLine("Time8: " + stopwatch2.ElapsedMilliseconds);
            for (int i = 0; i < 54; i++)
            {
                if (showLastYear)
                {
                    Salgsbudget_LastYear[i] = salesBudgetLY[i];
                    Salgsbudget_CommentLY[i] = Sales_CommentLY[i];
                }
                Salgsbudget_ThisYear[i] = salesBudgetTY[i];
                //RealiseretKampagn_LastYear[i] = realiseretKampagnLY[i];
                Kampagn_ThisYear[i] = KampagnTY[i];
                Salgsbudget_Comment[i] = Sales_CommentTY[i];
                Salgsbudget_ChangeHistory[i] = "";
            }
            
            stopwatch2.Stop();
            double timeQuerySeconds = stopwatch2.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Time for For Filling productifo : " + timeQuerySeconds.ToString() + " For Product Number: " + ProductNumber);
        }

        private Dictionary<int, int> GetZeroed(string p1, string p2)
        {
            Dictionary<int, int> Zeroed = new Dictionary<int, int>();

            try
            {
                SQL_M3Direct m3SQL = new SQL_M3Direct();
                Zeroed = m3SQL.GetZeroedSpecificCustomerWholeYear(p1, p2, CustomerNumberM3, ProductNumber);
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

       

        /// <summary>
        /// Load from m3 is timebound since it sometimes hangs 
        /// If it hangs redo after one second
        /// </summary>
        /// <param name="m3ProdNumber"></param>
        /// <param name="currWeek"></param>
        private void LockWeeksInfoFromM3(string m3ProdNumber, int currWeek)
        {
            GetFromM3 m3Info = new GetFromM3();

            bool Completed = false;
            int laps = 1;
            while (!Completed && laps<3)
            {
                //Completed = ExecuteWithTimeLimit(TimeSpan.FromMilliseconds(1000), () =>
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
                        WeekToLockFrom = tempWeeksToLOCK + currWeek; ;
                    }
                }
                //);

                Console.WriteLine("Completed: " + Completed + " lap: " + laps);
                laps++;
            }
        }

        private string GetM3ProdNumber()
        {
            string tempProdNBr = ProductNumber;

            return tempProdNBr;
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
                Console.WriteLine("Exception in time bound code Sales");
                throw ae.InnerExceptions[0];
            }
        }

        internal void UpdateSalesInfo(int selectedYear)
        {
            SQLCallsSalesCustomerInfo sqlSalesCalls = new SQLCallsSalesCustomerInfo();
            sqlSalesCalls.SetYear(selectedYear);
            Dictionary<int, int> salesBudgetTY = sqlSalesCalls.GetSalesBudgetTY(ProductNumber, CustomerCodeNav);
            Dictionary<int, string> Sales_CommentTY = sqlSalesCalls.GetSalesComment_TY();
            Console.WriteLine("APAP");

            for (int i = 0; i < 54; i++)
            {
                Salgsbudget_ThisYear[i] = salesBudgetTY[i];
                Salgsbudget_Comment[i] = Sales_CommentTY[i];
            }
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
    }
}
