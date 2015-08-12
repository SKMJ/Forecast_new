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
        public PrognosInfoForSupply(string name, int number, bool showLastYear)
        {
            ProductName = name;
            ProductNumber = number;
            ShowLastYear = showLastYear;
        }

        public string ProductName = "";
        public string WareHouse = "";
        public string Supplier = "";
        public string PrepLocation = "";
        public bool ShowLastYear = false;

        int[] weekPartPercentage = new int[8];


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
            sqlSupplyCalls.UpdateVareKort();
            weekPartPercentage = sqlSupplyCalls.GetPercentageWeekArray();

            if (ProductName.Length <2 ||ProductName == "Unknown Name")
            {
                ProductName = sqlSupplyCalls.GetBeskrivelse();
            }

            Dictionary<int, int> salesBudgetTY = sqlSupplyCalls.GetSalesBudget();
            Dictionary<int, int> salesBudget_REG_TY = sqlSupplyCalls.GetSalesBudgetREG_TY();
            Dictionary<int, string> Reguleret_CommentTY = sqlSupplyCalls.GetRegComment_TY();
            Dictionary<int, int> kopesBudgetTY = sqlSupplyCalls.GetKopesBudget_TY();


            Dictionary<int, int> KampagnTY = sqlSupplyCalls.GetKampagnTY();
            Dictionary<int, int> realiseretKampagnLY = new Dictionary<int,int>();
            Dictionary<int, int> relaiseratSalgsbudget_LY = new Dictionary<int, int>();
            if (ShowLastYear)
            {
                realiseretKampagnLY = sqlSupplyCalls.GetRealiseretKampagnLY();
                relaiseratSalgsbudget_LY = sqlSupplyCalls.GetRelSalg_LY();
            }

            Dictionary<int, int> relaiseratSalgsbudget_TY = sqlSupplyCalls.GetRelSalg_TY();
            Dictionary<int, int> kopsOrder_TY = sqlSupplyCalls.GetKopsorder_TY();

            for (int i = 0; i < 54; i++)
            {
                if (ShowLastYear)
                {
                    RealiseretKampagn_LastYear[i] = realiseretKampagnLY[i];
                    RealiseretSalg_LastYear[i] = relaiseratSalgsbudget_LY[i];
                }
                Kampagn_ThisYear[i] = KampagnTY[i];
                //Salgsbudget_LastYear[i] = 0;
                Salgsbudget_ThisYear[i] = salesBudgetTY[i];
                RealiseratSalg_ThisYear[i] = relaiseratSalgsbudget_TY[i];
                SalgsbudgetReguleret_ThisYear[i] = salesBudget_REG_TY[i];
                Kopsbudget_ThisYear[i] = kopesBudgetTY[i];
                Kopsorder_ThisYear[i] = kopsOrder_TY[i];
                //Salgsbudget_Comment[i] = "Comment";
                SalgsbudgetReguleret_Comment[i] = Reguleret_CommentTY[i];
                //Salgsbudget_ChangeHistory[i] = "";
            }
            stopwatch2.Stop();
            double timeQuerySeconds = stopwatch2.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Time for For Filling productifo : " + timeQuerySeconds.ToString() + " For Product Number: " + ProductNumber);
        }
    }
}
