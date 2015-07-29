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





        public void FillNumbers()
        {
            Console.WriteLine("Fill info For Product Number: " + ProductNumber);
            Stopwatch stopwatch2 = Stopwatch.StartNew();


            SQLCallsSalesCustomerInfo  sqlSalesCalls = new SQLCallsSalesCustomerInfo();
            Dictionary<int, int> salesBudgetTY = sqlSalesCalls.GetSalesBudgetTY(ProductNumber, CustomerNumber);

            sqlSalesCalls = new SQLCallsSalesCustomerInfo();
            Dictionary<int, int> salesBudgetLY = sqlSalesCalls.GetSalesBudget_LY(ProductNumber, CustomerNumber);

            sqlSalesCalls = new SQLCallsSalesCustomerInfo();
            Dictionary<int, int> realiseretKampagnLY = sqlSalesCalls.GetRealiseretKampagnLY(ProductNumber, CustomerNumber);


            sqlSalesCalls = new SQLCallsSalesCustomerInfo();
            Dictionary<int, int> KampagnTY = sqlSalesCalls.GetKampagnTY(ProductNumber, CustomerNumber);




            NavSQLSupplyInformation sqlSupplyCalls;
            sqlSupplyCalls = new NavSQLSupplyInformation();
            Dictionary<int, int> relaiseratSalg_LY = sqlSupplyCalls.GetRelSalg_LY(ProductNumber);

            sqlSupplyCalls = new NavSQLSupplyInformation();
            Dictionary<int, int> relaiseratSalg_TY = sqlSupplyCalls.GetRelSalg_TY(ProductNumber);



            //Todo add the code for the otther fileds.

            for (int i = 0; i < 53; i++)
            {
                Salgsbudget_LastYear[i] = salesBudgetLY[i];
                Salgsbudget_ThisYear[i] = salesBudgetTY[i];

                RealiseretKampagn_LastYear[i] = realiseretKampagnLY[i];
                RealiseretSalgs_LastYear[i] = -1;//relaiseratSalgsbudget_LY[i];
                Kampagn_ThisYear[i] = KampagnTY[i];
                RealiseratSalg_ThisYear[i] = -1;//relaiseratSalgsbudget_TY[i];
                Salgsbudget_Comment[i] = "Comment";
                Salgsbudget_ChangeHistory[i] = "";
            }

            stopwatch2.Stop();
            double timeQuerySeconds = stopwatch2.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Time for For Filling productifo : " + timeQuerySeconds.ToString() + " For Product Number: " + ProductNumber);
        }
    }
}
