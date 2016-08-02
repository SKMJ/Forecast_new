using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    class SalesRowNavision : ISalesRow
    {
        /* Posttype */
        public int Posttype { get; set; }
        
        /* Varenr */
        public string ItemNumber { get; set; }

        /* Bokforingsdato */
        public DateTime Date { get; set; }
        //public DateTime PostingDate { get; set; }

        /* Faktureret antal */
        public int Quantity { get; set; }

        /* LeverandörlagerOrdre */
        public bool SupplyOwnStock { get; set; }

        public int Week { get; set; }
        public string CustomerName { get; set; }
    }
}
