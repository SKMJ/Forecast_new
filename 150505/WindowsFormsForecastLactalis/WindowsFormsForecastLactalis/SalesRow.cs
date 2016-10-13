using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    /// <summary>
    /// This class represents a sales statistics row from local Forecast Table
    /// </summary>
    public class SalesRow : ISalesRow
    {
        public String OrderNumber { get; set; }
        public int OrderRow { get; set; }
        public string ItemNumber { get; set; }
        public DateTime Date { get; set; }
        //public DateTime DeliveryDate { get; set; }
        public String Customer { get; set; }
        public int Quantity { get; set; }
        public int QuantityOrdered { get; set; }
        public int Week { get; set; }
        public int BestBeforeDate { get; set; }
        public int WantedBestBeforeDate { get; set; }

        public string CustomerName { get; set; }
        /*Bogføringsdato, Faktureret_antal, LeverandørlagerOrdre, Posttype, Lokationskode*/
    }
}
