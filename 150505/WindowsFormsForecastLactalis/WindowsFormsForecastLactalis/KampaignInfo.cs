using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsForecastLactalis
{
    public class KampaignInfo
    {
        public string itemNBR= "";
        public int weekNBR = 0;
        public int Antal= 0;
        public string tempCampCuse = "";


        public KampaignInfo(string tempCampCuse, int Antal, int weekInt, string itemNBR)
        {
            // TODO: Complete member initialization
            this.itemNBR = itemNBR;
            this.tempCampCuse = tempCampCuse;
            this.Antal = Antal;
            this.weekNBR = weekInt;
        }
    }
}
