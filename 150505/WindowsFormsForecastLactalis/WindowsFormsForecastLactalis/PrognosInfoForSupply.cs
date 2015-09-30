using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    //this is the supply prognos info not connected to any customer
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
        public bool ShowLastYear = false;

        int[] weekPartPercentage = new int[8]; //antal, mån, tis, ons....


        public string ProductNumber = "0";
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

            GetFromM3 m3Info = new GetFromM3();

            string m3ProdNumber = GetM3ProdNumber();

            Dictionary<string, string> info = m3Info.GetItemInfoByItemNumber(m3ProdNumber);
            WareHouse = info["whsLocation"];
            PrepLocation = info["prepLocation"];
            Supplier = GetSupplierFromProduct();

            Console.WriteLine("WareHouse: " + WareHouse + " PrepLocation: " + PrepLocation + " Supplier: " + Supplier);


            NavSQLSupplyInformation sqlSupplyCalls = new NavSQLSupplyInformation(selectedYear, ProductNumber);
            sqlSupplyCalls.SetSelectedYear(selectedYear);
            sqlSupplyCalls.UpdateVareKort();
            weekPartPercentage = sqlSupplyCalls.GetPercentageWeekArray();


            string ProductNameTemp = sqlSupplyCalls.GetBeskrivelse();
            if (ProductName.Length > 2 && ProductName != "Unknown Name")
            {
                Console.WriteLine("Fill info For Product Number: " + ProductNumber + " LactaNAme: " + ProductNameTemp + " SKMJNAme: " + ProductName);
            }
            if (ProductNameTemp.Length > 2)
            {

                ProductName = ProductNameTemp;
            }

            Dictionary<int, int> salesBudgetTY = sqlSupplyCalls.GetSalesBudget();
            Dictionary<int, int> salesBudget_REG_TY = sqlSupplyCalls.GetSalesBudgetREG_TY();
            Dictionary<int, string> Reguleret_CommentTY = sqlSupplyCalls.GetRegComment_TY();
            Dictionary<int, int> kopesBudgetTY = sqlSupplyCalls.GetKopesBudget_TY();


            Dictionary<int, int> KampagnTY = sqlSupplyCalls.GetKampagnTY();
            Dictionary<int, int> realiseretKampagnLY = new Dictionary<int, int>();
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


        private string GetSupplierFromProduct()
        {
            string returnSuppl = "";
            string tempProdNBr = GetM3ProdNumber() ;

            if (ClassStaticVaribles.ProdToSupplDict.ContainsKey(tempProdNBr))
            {
                returnSuppl = ClassStaticVaribles.ProdToSupplDict[tempProdNBr];
            }


            return returnSuppl;
        }
    }
}
