using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    public class PrognosInfo
    {
        public PrognosInfo(string name, int number)
        {
            ProductName = name;
            ProductNumber = number;
        }


        public string ProductName = "";
        public int ProductNumber = 0;
        public Dictionary<int, int> RealiseretKampagn_LastYear = new Dictionary<int, int>();
        public Dictionary<int, int> RealiseretSalgsbudget_LastYear = new Dictionary<int, int>();
        public Dictionary<int, int> Kampagn_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Salgsbudget_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, string> Salgsbudget_Comment = new Dictionary<int, string>();
        public Dictionary<int, int> Realiserat_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> SalgsbudgetReguleret_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Kopsbudget_ThisYear = new Dictionary<int, int>();
        public Dictionary<int, int> Kopsorder_ThisYear = new Dictionary<int, int>();




        internal void FillNumbers()
        {
            for (int i = 0; i < 53; i++)
            {
                RealiseretKampagn_LastYear[i] = i * 10;
                RealiseretSalgsbudget_LastYear[i] = i * 20;
                Kampagn_ThisYear[i] = i * 30;
                Salgsbudget_ThisYear[i] = i * 40;
                Realiserat_ThisYear[i] = i * 50;
                SalgsbudgetReguleret_ThisYear[i] = 0;
                Kopsbudget_ThisYear[i] = 0;
                Kopsorder_ThisYear[i] = 0;
                Salgsbudget_Comment[i] = "Comment";
            }
        }
    }
}
