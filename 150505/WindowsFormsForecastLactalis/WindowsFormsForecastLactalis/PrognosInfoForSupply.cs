using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    //this is the supply prognos info not connected to any customer
    public class PrognosInfoForSupply
    {
        public PrognosInfoForSupply(string name, int number)
        {
            ProductName = name;
            ProductNumber = number;
        }

        public string ProductName = "";
        public int ProductNumber = 0;
        public Dictionary<int, int> RealiseretKampagn_LastYear = new Dictionary<int, int>();
        public Dictionary<int, int> RealiseretSalg_LastYear = new Dictionary<int, int>();
        public Dictionary<int, int> Kampagn_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Salgsbudget_ThisYear = new Dictionary<int, int>();
        //public Dictionary<int, int> Salgsbudget_LastYear = new Dictionary<int, int>();
        public Dictionary<int, string> Salgsbudget_Comment = new Dictionary<int, string>();
        public Dictionary<int, string> Salgsbudget_ChangeHistory = new Dictionary<int, string>();
        public Dictionary<int, int> RealiseratSalg_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> SalgsbudgetReguleret_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, string> SalgsbudgetReguleret_Comment = new Dictionary<int, string>();
        public Dictionary<int, int> Kopsbudget_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Kopsorder_ThisYear = new Dictionary<int, int>();
        //private PrognosInfo item;




        public void FillNumbers(int selectedYear)
        {
            Console.WriteLine("Fill info For Product Number: " + ProductNumber);
            Stopwatch stopwatch2 = Stopwatch.StartNew();



            NavSQLSupplyInformation sqlSupplyCalls = new NavSQLSupplyInformation(selectedYear, ProductNumber);
            sqlSupplyCalls.SetSelectedYear(selectedYear);
            Dictionary<int, int> salesBudgetTY = sqlSupplyCalls.GetSalesBudget();
            Dictionary<int, int> salesBudget_REG_TY = sqlSupplyCalls.GetSalesBudgetREG_TY();
            Dictionary<int, int> kopesBudgetTY = sqlSupplyCalls.GetKopesBudget_TY();


            //sqlSupplyCalls = new NavSQLSupplyInformation();
            Dictionary<int, int> realiseretKampagnLY = sqlSupplyCalls.GetRealiseretKampagnLY();

            //sqlSupplyCalls = new NavSQLSupplyInformation();
            Dictionary<int, int> KampagnTY = sqlSupplyCalls.GetKampagnTY();

            //sqlSupplyCalls = new NavSQLSupplyInformation();
            Dictionary<int, int> relaiseratSalgsbudget_LY = sqlSupplyCalls.GetRelSalg_LY();

            //sqlSupplyCalls = new NavSQLSupplyInformation();
            Dictionary<int, int> relaiseratSalgsbudget_TY = sqlSupplyCalls.GetRelSalg_TY();

            //sqlSupplyCalls = new NavSQLSupplyInformation();
            Dictionary<int, int> kopsOrder_TY = sqlSupplyCalls.GetKopsorder_TY();


            //Todo add the code for the otther fileds.

            for (int i = 0; i < 54; i++)
            {
                RealiseretKampagn_LastYear[i] = realiseretKampagnLY[i];
                RealiseretSalg_LastYear[i] = relaiseratSalgsbudget_LY[i];
                Kampagn_ThisYear[i] = KampagnTY[i];
                //Salgsbudget_LastYear[i] = 0;
                Salgsbudget_ThisYear[i] = salesBudgetTY[i];
                RealiseratSalg_ThisYear[i] = relaiseratSalgsbudget_TY[i];
                SalgsbudgetReguleret_ThisYear[i] = salesBudget_REG_TY[i];
                Kopsbudget_ThisYear[i] = kopesBudgetTY[i];
                Kopsorder_ThisYear[i] = kopsOrder_TY[i];
                //Salgsbudget_Comment[i] = "Comment";
                SalgsbudgetReguleret_Comment[i] = "Comment";
                //Salgsbudget_ChangeHistory[i] = "";
            }

            stopwatch2.Stop();
            double timeQuerySeconds = stopwatch2.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Time for For Filling productifo : " + timeQuerySeconds.ToString() + " For Product Number: " + ProductNumber);
        }
    }
}
