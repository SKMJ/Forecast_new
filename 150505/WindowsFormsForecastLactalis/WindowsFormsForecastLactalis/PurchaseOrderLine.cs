using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    public class PurchaseOrderLine
    {
        public string PONumber { get; set; }
        public string ItemNumber { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public int Week { get; set; }
        public string Warehouse { get; set; }

        public override string ToString()
        {
            return String.Format("PO:{0} Item:{1}, qty:{2}, date:{3}, week:{4}", PONumber, ItemNumber, Quantity, Date.ToShortDateString(), Week);
        }
    }
}
