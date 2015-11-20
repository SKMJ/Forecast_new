///This file handles the data for the file sent to Lactalis France
///This is the class that keeps all the numbers product for product
///One instance of this calss for every product

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    public class FileToSendInfoForPoduct
    {
        public FileToSendInfoForPoduct(string prodNumber, int lactaFranceProdNumber)
        {
            ProductNumber = prodNumber;
            LactalisFranceProductNumber = lactaFranceProdNumber;

        }

        public string ProductName = "No Name";
        public bool InLactaFranceFile = false;


        public int[] weekPartPercentage = new int[8]; //antal, mån, tis, ons....


        public string ProductNumber = "0";
        public int LactalisFranceProductNumber = 0;

        public int Antal_pr_kolli = 1;

        public Dictionary<int, int> Kopsbudget_ThisYear = new Dictionary<int, int>();





        public void FillNumbers()
        {
            Console.WriteLine("Fill info To File For Product Number: " + ProductNumber);

            NavSQLSupplyInformation sqlSupplyCalls = new NavSQLSupplyInformation(2015, ProductNumber);
            sqlSupplyCalls.SetSelectedYear(2015);
            sqlSupplyCalls.UpdateVareKort();
            GetFromM3 m3Info = new GetFromM3();
            string m3ProdNumber = GetM3ProdNumber();
            //Dictionary<string, string> info = m3Info.GetItemInfoByItemNumber(m3ProdNumber);
            //if (info != null && info.Count > 0 && Convert.ToInt32(info["INLActaFranceFile"]) > 0)
            //{

            //    InLactaFranceFile = true;

            //    Console.WriteLine("M3 prod infofields: " + m3ProdNumber);
            //    weekPartPercentage = new int[] { Convert.ToInt32(info["forecastWeek"]), Convert.ToInt32(info["FCMO"]), Convert.ToInt32(info["FCTU"]), Convert.ToInt32(info["FCWE"]), Convert.ToInt32(info["FCTH"]), Convert.ToInt32(info["FCFR"]), Convert.ToInt32(info["FCSA"]), Convert.ToInt32(info["FCSU"]) };
            //}
            //else
            //{
            //    weekPartPercentage = sqlSupplyCalls.GetPercentageWeekArray();
            //}
            LactalisFranceProductNumber = sqlSupplyCalls.GetLactaVareNR();
            Antal_pr_kolli = sqlSupplyCalls.GetLactalis_NBRPer_colli();


            if (ProductName.Length < 2 || ProductName == "No Name")
            {
                ProductName = sqlSupplyCalls.GetBeskrivelse();
            }


            Dictionary<int, int> kopesBudgetCurrent = sqlSupplyCalls.GetKopesBudgetForFile();

            for (int i = 0; i < 54; i++)
            {

                Kopsbudget_ThisYear[i] = kopesBudgetCurrent[i];
            }
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
    }
}
