using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    class FileToSendInfoForPoduct
    {
        public FileToSendInfoForPoduct(string prodNumber, int lactaFranceProdNumber)
        {
            ProductNumber = prodNumber;
            LactalisFranceProductNumber = lactaFranceProdNumber;

        }

        public string ProductName = "No Name";


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
            weekPartPercentage = sqlSupplyCalls.GetPercentageWeekArray();
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
    }
}
