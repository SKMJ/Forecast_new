using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    class Get_FromSimulatedM3
    {
        bool firstTime = true;
        Dictionary<int, string> productName = new Dictionary<int, string>();

        public List<int> GetListOfProductsNbrByAssortment(string Assortment)
        {
            List<int> productList = new List<int>();


            Console.WriteLine("fake M3 communication: 1");
            productList.Add(Convert.ToInt32("23046"));
            productList.Add(Convert.ToInt32("23409"));
            productList.Add(Convert.ToInt32("28053"));
            productList.Add(Convert.ToInt32("28341"));
            productList.Add(Convert.ToInt32("28655"));
            return productList;
        }


        public string GetNameByItemNumber(int itemNbr)
        {


            string returnString = "";
            List<int> productList = new List<int>();


            Console.WriteLine("fake M3 communication! 2");
            if (firstTime)
            {
                productName.Add(23046, "Galbani Paesano Italiano 40g");
                productName.Add(23409, "Brie President 500g");
                productName.Add(28053, "Briellant Bleu 180 g");
                productName.Add(28341, "Brie De Meaux OP (26 CO) 700 g");
                productName.Add(28655, "Brie Ermitage 200 g");
                firstTime = false;
            }
            returnString = productName[itemNbr];
            return returnString;
        }

    }
}
