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
         public PrognosInfoSales(string name, int number, int customerNBR)
        {
            ProductName = name;
            ProductNumber = number;
            CustomerNumber = customerNBR;
        }

        
        public string ProductName = "";
        public int ProductNumber = 0;
        public int CustomerNumber = 0;
        public Dictionary<int, int> RealiseretKampagn_LastYear = new Dictionary<int, int>();
        public Dictionary<int, int> RealiseretSalgsbudget_LastYear = new Dictionary<int, int>();
        public Dictionary<int, int> Kampagn_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Salgsbudget_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Salgsbudget_LastYear = new Dictionary<int, int>();
        public Dictionary<int, string> Salgsbudget_Comment = new Dictionary<int, string>();
        public Dictionary<int, string> Salgsbudget_ChangeHistory = new Dictionary<int, string>();
        public Dictionary<int, int> Realiserat_ThisYear = new Dictionary<int, int>();





        public void FillNumbers(int prodNumber)
        {
            Console.WriteLine("Fill info For Product Number: " + prodNumber);
            Stopwatch stopwatch2 = Stopwatch.StartNew();



            NavSQLSupplyInformation sqlSupplyCalls = new NavSQLSupplyInformation();
            Dictionary<int, int> salesBudgetTY = sqlSupplyCalls.GetSalesBudget(prodNumber);


            sqlSupplyCalls = new NavSQLSupplyInformation();
            Dictionary<int, int> realiseretKampagnLY = sqlSupplyCalls.GetRealiseretKampagnLY(prodNumber);

            sqlSupplyCalls = new NavSQLSupplyInformation();
            Dictionary<int, int> KampagnTY = sqlSupplyCalls.GetKampagnTY(prodNumber);

            sqlSupplyCalls = new NavSQLSupplyInformation();
            Dictionary<int, int> relaiseratSalgsbudget_LY = sqlSupplyCalls.GetRelSalgsbudget_LY(prodNumber);

            sqlSupplyCalls = new NavSQLSupplyInformation();
            Dictionary<int, int> relaiseratSalgsbudget_TY = sqlSupplyCalls.GetRelSalgsbudget_TY(prodNumber);



            //Todo add the code for the otther fileds.

            for (int i = 0; i < 53; i++)
            {
                RealiseretKampagn_LastYear[i] = realiseretKampagnLY[i];
                RealiseretSalgsbudget_LastYear[i] = relaiseratSalgsbudget_LY[i];
                Kampagn_ThisYear[i] = KampagnTY[i];
                Salgsbudget_LastYear[i] = 0;
                Salgsbudget_ThisYear[i] = salesBudgetTY[i];
                Realiserat_ThisYear[i] = relaiseratSalgsbudget_TY[i];
                Salgsbudget_Comment[i] = "Comment";
                Salgsbudget_ChangeHistory[i] = "";
            }

            stopwatch2.Stop();
            double timeQuerySeconds = stopwatch2.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Time for For Filling productifo : " + timeQuerySeconds.ToString() + " For Product Number: " + prodNumber);
        }
    }
}
