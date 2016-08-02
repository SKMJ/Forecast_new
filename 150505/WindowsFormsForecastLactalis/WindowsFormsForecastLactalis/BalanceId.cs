using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    public class BalanceId
    {
        public string ItemNumber { get; set; }
        public string Warehouse { get; set; }
        public int OnHandBalance { get; set; }
        public int AllocatedQuantity { get; set; }
        public int AllocatableQuantity
        {
            get
            {
                return OnHandBalance - AllocatedQuantity;
            }
        }
        public DateTime BestBeforeDate { get; set; }
    }
}
