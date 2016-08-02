using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    public interface ISalesRow
    {
        DateTime Date { get; set; }
        string ItemNumber { get; set; }
        int Quantity { get; set; }
        int Week { get; set; }
        String CustomerName { get; set; }
    }
}
