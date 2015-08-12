using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    class PrognosInfoSales
    {
         public PrognosInfoSales(string name, int number, string customerNumber)
        {
            ProductName = name;
            ProductNumber = number;
            CustomerNumber = customerNumber;
        }

        
        public string ProductName = "";
        public int ProductNumber = 0;
        public string CustomerNumber = "";
        public Dictionary<int, int> RealiseretKampagn_LastYear = new Dictionary<int, int>();
        public Dictionary<int, int> RealiseretSalgs_LastYear = new Dictionary<int, int>();
        public Dictionary<int, int> Kampagn_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Salgsbudget_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Salgsbudget_LastYear = new Dictionary<int, int>();
        public Dictionary<int, string> Salgsbudget_Comment = new Dictionary<int, string>();
        public Dictionary<int, string> Salgsbudget_ChangeHistory = new Dictionary<int, string>();
        public Dictionary<int, int> RealiseratSalg_ThisYear = new Dictionary<int, int>();

        int[] weekPartPercentage = new int[8];





        public void FillNumbers(int selectedYear)
        {
            Console.WriteLine("Fill info For Product Number: " + ProductNumber);
            Stopwatch stopwatch2 = Stopwatch.StartNew();

            NavSQLSupplyInformation sqlSupplyCalls = new NavSQLSupplyInformation(selectedYear, ProductNumber);
            sqlSupplyCalls.UpdateVareKort();
            weekPartPercentage = sqlSupplyCalls.GetPercentageWeekArray();

            if (ProductName.Length < 2 || ProductName == "Name Unknown")
            {
                ProductName = sqlSupplyCalls.GetBeskrivelse();
            }


            SQLCallsSalesCustomerInfo  sqlSalesCalls = new SQLCallsSalesCustomerInfo();
            sqlSalesCalls.SetYear(selectedYear);

            Dictionary<int, int> salesBudgetTY = sqlSalesCalls.GetSalesBudgetTY(ProductNumber, CustomerNumber);
            Dictionary<int, string> Sales_CommentTY = sqlSalesCalls.GetSalesComment_TY();
            //sqlSalesCalls = new SQLCallsSalesCustomerInfo();
            Dictionary<int, int> salesBudgetLY = sqlSalesCalls.GetSalesBudget_LY(ProductNumber, CustomerNumber);

            //sqlSalesCalls = new SQLCallsSalesCustomerInfo();
            Dictionary<int, int> realiseretKampagnLY = sqlSalesCalls.GetRealiseretKampagnLY(ProductNumber, CustomerNumber);


            //sqlSalesCalls = new SQLCallsSalesCustomerInfo();
            Dictionary<int, int> KampagnTY = sqlSalesCalls.GetKampagnTY(ProductNumber, CustomerNumber);




            //sqlSalesCalls = new SQLCallsSalesCustomerInfo();

            Dictionary<int, int> relaiseratSalg_LY = sqlSalesCalls.GetRelSalg_LY(ProductNumber, CustomerNumber);

            //sqlSalesCalls = new SQLCallsSalesCustomerInfo();
            Dictionary<int, int> relaiseratSalg_TY = sqlSalesCalls.GetRelSalg_TY(ProductNumber, CustomerNumber);

            if (ProductName.Length < 2)
            {
                ProductName = sqlSalesCalls.GetBeskrivelse();
            }

            //Todo add the code for the otther fileds.

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

            stopwatch2.Stop();
            double timeQuerySeconds = stopwatch2.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Time for For Filling productifo : " + timeQuerySeconds.ToString() + " For Product Number: " + ProductNumber);
        }
    }
}
