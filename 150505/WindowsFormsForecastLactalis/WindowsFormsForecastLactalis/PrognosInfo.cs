using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    public class PrognosInfo
    {
        public PrognosInfo(string name, int number, int customerNbr)
        {
            ProductName = name;
            ProductNumber = number;
            CustomerNumber = customerNbr;
        }

        public PrognosInfo(PrognosInfo item)
        {
            // TODO: Complete member initialization
            this.ProductName = item.ProductName;
            this.ProductNumber = item.ProductNumber;
            this.CustomerNumber = item.CustomerNumber;


            this.RealiseretKampagn_LastYear = item.RealiseretKampagn_LastYear;
            this.RealiseretSalgsbudget_LastYear = item.RealiseretSalgsbudget_LastYear;
            this.Kampagn_ThisYear = item.Kampagn_ThisYear;
            this.Salgsbudget_ThisYear = item.Salgsbudget_ThisYear;
            this.Salgsbudget_LastYear = item.Salgsbudget_LastYear;
            this.Salgsbudget_Comment = item.Salgsbudget_Comment;
            this.Realiserat_ThisYear = item.Realiserat_ThisYear;
            this.SalgsbudgetReguleret_ThisYear = item.SalgsbudgetReguleret_ThisYear;
            this.SalgsbudgetReguleret_Comment = item.SalgsbudgetReguleret_Comment;
            this.Kopsbudget_ThisYear = item.Kopsbudget_ThisYear;
            this.Kopsorder_ThisYear = item.Kopsorder_ThisYear;
            this.Salgsbudget_ChangeHistory = item.Salgsbudget_ChangeHistory;
            //this.item = item.item;


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
        public Dictionary<int, int> SalgsbudgetReguleret_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, string> SalgsbudgetReguleret_Comment = new Dictionary<int, string>();
        public Dictionary<int, int> Kopsbudget_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Kopsorder_ThisYear = new Dictionary<int, int>();
        //private PrognosInfo item;




        public void FillNumbers()
        {
            for (int i = 0; i < 53; i++)
            {
                RealiseretKampagn_LastYear[i] = i * 10;
                RealiseretSalgsbudget_LastYear[i] = i * 20;
                Kampagn_ThisYear[i] = i * 30;
                Salgsbudget_LastYear[i] = i * 35;
                Salgsbudget_ThisYear[i] = i * 40;
                Realiserat_ThisYear[i] = i * 50;
                SalgsbudgetReguleret_ThisYear[i] = 0;
                Kopsbudget_ThisYear[i] = 0;
                Kopsorder_ThisYear[i] = 0;
                Salgsbudget_Comment[i] = "Comment";
                SalgsbudgetReguleret_Comment[i] = "Comment";
                Salgsbudget_ChangeHistory[i] = "";
            }
        }
    }
}
