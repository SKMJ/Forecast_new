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
    class PrognosInfoSales : IComparable
    {
        public PrognosInfoSales(string name, string number, string customerNumber)
        {
            ProductName = name;
            ProductNumber = number;
            CustomerNumberM3 = customerNumber;
            CustomerCodeNav = ClassStaticVaribles.GetCustNavCodeFirst(customerNumber);
        }


        public string ProductName = "";
        public string ProductNumber = "0";
        public string CustomerNumberM3 = "";
        public string CustomerCodeNav = "";
        public int WeekToLockFrom = 0;

        public Dictionary<int, int> RealiseretKampagn_LastYear = new Dictionary<int, int>();
        public Dictionary<int, int> RealiseretSalgs_LastYear = new Dictionary<int, int>();
        public Dictionary<int, int> Kampagn_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Salgsbudget_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Salgsbudget_LastYear = new Dictionary<int, int>();
        public Dictionary<int, string> Salgsbudget_Comment = new Dictionary<int, string>();
        public Dictionary<int, string> Salgsbudget_ChangeHistory = new Dictionary<int, string>();
        public Dictionary<int, int> RealiseratSalg_ThisYear = new Dictionary<int, int>();

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


        public void FillNumbers(int selectedYear)
        {
            Console.WriteLine("Fill info For Product Number: " + ProductNumber);
            Stopwatch stopwatch2 = Stopwatch.StartNew();
           

            //MessageBox.Show("Place 3");

            NavSQLSupplyInformation sqlSupplyCalls = new NavSQLSupplyInformation(selectedYear, ProductNumber);
            Console.WriteLine("Time1: " + stopwatch2.ElapsedMilliseconds);
            //MessageBox.Show("Place AA");
            sqlSupplyCalls.UpdateVareKort();
            //MessageBox.Show("Place BB");
            weekPartPercentage = sqlSupplyCalls.GetPercentageWeekArray();
            int weektemp = sqlSupplyCalls.GetCurrentWeekNBR();
            string m3ProdNumber = GetM3ProdNumber();

            LockWeeksInfoFromM3(m3ProdNumber, weektemp);
            Console.WriteLine("Time2: " + stopwatch2.ElapsedMilliseconds);
            Console.WriteLine("Produkt " + ProductNumber + " Varukort,  Vecka nu: " + weektemp + " Weektolock from: " + WeekToLockFrom + " Antal att låsa: " + weekPartPercentage[0]);

            if (ProductName.Length < 2 || ProductName == "Name Unknown")
            {
                ProductName = sqlSupplyCalls.GetBeskrivelse();
            }
            //MessageBox.Show("Place 6");

            SQLCallsSalesCustomerInfo sqlSalesCalls = new SQLCallsSalesCustomerInfo();
            sqlSalesCalls.SetYear(selectedYear);
            Console.WriteLine("Time3: " + stopwatch2.ElapsedMilliseconds);
            Dictionary<int, int> salesBudgetTY = sqlSalesCalls.GetSalesBudgetTY(ProductNumber, CustomerCodeNav);
            Dictionary<int, string> Sales_CommentTY = sqlSalesCalls.GetSalesComment_TY();
            Console.WriteLine("Time4: " + stopwatch2.ElapsedMilliseconds);
            //sqlSalesCalls = new SQLCallsSalesCustomerInfo();
            Dictionary<int, int> salesBudgetLY = sqlSalesCalls.GetSalesBudget_LY(ProductNumber, CustomerCodeNav);
            // MessageBox.Show("Place 7");

            //sqlSalesCalls = new SQLCallsSalesCustomerInfo();
            Dictionary<int, int> realiseretKampagnLY = sqlSalesCalls.GetRealiseretKampagnLY(ProductNumber, CustomerCodeNav);

            Console.WriteLine("Time5: " + stopwatch2.ElapsedMilliseconds);
            //sqlSalesCalls = new SQLCallsSalesCustomerInfo();
            Dictionary<int, int> KampagnTY;
            if (selectedYear >= 2015)
            {
                GetFromM3 m3 = new GetFromM3();
                KampagnTY = m3.GetCampaignsOfProducts(ProductNumber, selectedYear, CustomerNumberM3);
            }
            else
            {
                KampagnTY = sqlSalesCalls.GetKampagnTY(ProductNumber, CustomerNumberM3);
            }
            Console.WriteLine("Time6: " + stopwatch2.ElapsedMilliseconds);

            // MessageBox.Show("Place 8");



            //sqlSalesCalls = new SQLCallsSalesCustomerInfo();

            Dictionary<int, int> relaiseratSalg_LY = sqlSalesCalls.GetRelSalg_LY(ProductNumber, CustomerCodeNav);

            //sqlSalesCalls = new SQLCallsSalesCustomerInfo();
            Dictionary<int, int> relaiseratSalg_TY = sqlSalesCalls.GetRelSalg_TY(ProductNumber, CustomerCodeNav);
            Console.WriteLine("Time7: " + stopwatch2.ElapsedMilliseconds);
            if (ProductName.Length < 2)
            {
                ProductName = sqlSalesCalls.GetBeskrivelse();
            }

            // MessageBox.Show("Place 9");

            //Todo add the code for the otther fileds.
            Console.WriteLine("Time8: " + stopwatch2.ElapsedMilliseconds);
            for (int i = 0; i < 54; i++)
            {
                Salgsbudget_LastYear[i] = salesBudgetLY[i];
                Salgsbudget_ThisYear[i] = salesBudgetTY[i];

                RealiseretKampagn_LastYear[i] = realiseretKampagnLY[i];
                RealiseretSalgs_LastYear[i] = relaiseratSalg_LY[i];
                Kampagn_ThisYear[i] = KampagnTY[i];
                RealiseratSalg_ThisYear[i] = relaiseratSalg_TY[i];
                Salgsbudget_Comment[i] = Sales_CommentTY[i];
                Salgsbudget_ChangeHistory[i] = "";
            }
            
            // MessageBox.Show("Place AA");
            stopwatch2.Stop();
            double timeQuerySeconds = stopwatch2.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Time for For Filling productifo : " + timeQuerySeconds.ToString() + " For Product Number: " + ProductNumber);

            //MessageBox.Show("Place BB");
        }

        
        /// <summary>
        /// Load from m3 is timebound since it sometimes hangs 
        /// If it hangs redo after one second
        /// </summary>
        /// <param name="m3ProdNumber"></param>
        /// <param name="weektemp"></param>
        private void LockWeeksInfoFromM3(string m3ProdNumber, int weektemp)
        {

            GetFromM3 m3Info = new GetFromM3();

            bool Completed = false;
            int laps = 1;
            while (!Completed && laps<3)
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
                        int tempWeeksToLOCK = tempDaysLock / 7;
                        WeekToLockFrom = tempWeeksToLOCK + weektemp;
                    }
                    else
                    {
                        WeekToLockFrom = weektemp;
                    }
                });

                Console.WriteLine("Completed: " + Completed + " lap: " + laps);
                laps++;
            }
        }

        private string GetM3ProdNumber()
        {
            string tempProdNBr = ProductNumber;

            if (ClassStaticVaribles.NewNumberDictNavKey.ContainsKey(ProductNumber))
            {
                tempProdNBr = ClassStaticVaribles.NewNumberDictNavKey[ProductNumber];

                //Console.WriteLine("Search for pr");
            }
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

            for (int i = 0; i < 54; i++)
            {
                Salgsbudget_ThisYear[i] = salesBudgetTY[i];
                Salgsbudget_Comment[i] = Sales_CommentTY[i];
            }
        }
    }
}
