///Thhis file is used for keeping all the numbers that is downloaded on startup
///Keeps numbers as product_number, Customernumbers and so on so we don't have to load this every time.
///One load on start up then it is saved locally in this file

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    public static class ClassStaticVaribles
    {
        public static Dictionary<string, string> CustDictionary;

        // private bool loadingNewProductsOngoing;

        public static Dictionary<string, string> AllProductsNavDict = new Dictionary<string, string>();

        public static Dictionary<string, string> ProdToSupplDict = new Dictionary<string, string>();

        public static List<string> AllCuttingProducts = new List<string>();

        public static Dictionary<string, string> AllProductsM3Dict = new Dictionary<string, string>();
        public static Dictionary<string, List<string>> AllSuppliersM3 = new Dictionary<string, List<string>>();
        public static Dictionary<string,string> AllSuppliersNameDict = new Dictionary<string, string>();

        public static Dictionary<string, string> NewNumberDictNavKey = new Dictionary<string, string>();
        public static Dictionary<string, string> NewNumberDictM3Key = new Dictionary<string, string>();

        public static Dictionary<string, string> AssortmentM3_toNav = new Dictionary<string, string>();

        public static Dictionary<int, DateTime> StartDate = new Dictionary<int, DateTime>(); //Dict First Date of week 1 in Year

        private static bool AllProductsNavFirst = true;
        private static bool AllProductsM3DictFirst = true;
        private static bool AllSuppliersM3First = true;
        private static bool NewNumberDictNavKeyFirst = true;

        private static bool CustDictionaryFirst = true;
        private static bool StartDateFirst = true;



        public static void SetAssortmentDictionary()
        {
            if (CustDictionaryFirst)
            {
                CustDictionaryFirst = false;
                Dictionary<string, string> allAssortments = new Dictionary<string, string>();
                CustDictionary = new Dictionary<string, string>();

                allAssortments.Add("KCWF", "IRMA");
                allAssortments.Add("KD", "15");
                allAssortments.Add("DS", "15");
                allAssortments.Add("KC", "52");
                allAssortments.Add("KDVC", "4");
                allAssortments.Add("KI", "6");
                allAssortments.Add("KR", "28");
                allAssortments.Add("LD", "47");
                allAssortments.Add("YEYD", "102");


                //Dictionary<string, string> allCustomersSwitched = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> item in allAssortments)
                {
                    AssortmentM3_toNav.Add(item.Key, item.Value);
                }
            }
        }

        public static void SetCustDictionary()
        {
            if (CustDictionaryFirst)
            {
                SetAssortmentDictionary();
                CustDictionaryFirst = false;
                Dictionary<string, string> allCustomers = new Dictionary<string, string>();
                CustDictionary = new Dictionary<string, string>();
                allCustomers.Add("0", " ");
                allCustomers.Add("904", "914-KristianstadsOstföräd.EUR");
                allCustomers.Add("981", "ArlaNorge");
                allCustomers.Add("980", "ASKOLactalisNorge");
                allCustomers.Add("975", "AxFood");
                allCustomers.Add("986", "AxfooddirektebutiksLev.");
                allCustomers.Add("976", "Bergendahls");
                allCustomers.Add("60", "BESTWESTERN");
                allCustomers.Add("56", "CHEVALBLANCKANTINER");
                allCustomers.Add("67", "CHOISEHOTELS");
                allCustomers.Add("977", "CityGross");
                allCustomers.Add("52", "COOPCENTRALLAGER");
                allCustomers.Add("984", "Coopdirekteleverancer");
                allCustomers.Add("974", "COOPSverige");
                allCustomers.Add("41", "COORKANTINER");
                allCustomers.Add("47", "DANSKCATER");
                allCustomers.Add("68", "DANSKKROFERIE");
                allCustomers.Add("63", "DANSKEKONF.CENT.BOOK.SERVICE");
                allCustomers.Add("90", "Div.Finland");
                allCustomers.Add("51", "DIVERSE");
                allCustomers.Add("15", "DSCENTRAL-LAGER");
                allCustomers.Add("989", "Gekås");
                allCustomers.Add("26", "GENNEMFAKTVIASUPERGROS");
                allCustomers.Add("58", "grossist");
                allCustomers.Add("19", "HKI(tidlS-engros)");
                allCustomers.Add("65", "HOTELLER");
                allCustomers.Add("45", "HØRKRAM");
                allCustomers.Add("973", "ICA");
                allCustomers.Add("983", "ICAbutikspakketCentraltLev.");
                allCustomers.Add("982", "ICANorge");
                allCustomers.Add("71", "INCOCaterKBHgennemfak");
                allCustomers.Add("73", "INCOgennemfakMeyer");
                allCustomers.Add("92", "INEXFinland");
                allCustomers.Add("6", "Intervare");
                allCustomers.Add("IRMA", "IRMA");
                allCustomers.Add("101", "Island");
                allCustomers.Add("44", "KANTINER");
                allCustomers.Add("96", "KESKO");
                allCustomers.Add("43", "KURSUSEJD.");
                allCustomers.Add("34", "METRO");
                allCustomers.Add("4", "NETTO");
                allCustomers.Add("969", "NettoSverigeAB");
                allCustomers.Add("42", "OSTEHANDEL");
                allCustomers.Add("32", "OstehandlereSverige");
                allCustomers.Add("103", "Polen");
                allCustomers.Add("28", "REITAN");
                allCustomers.Add("988", "RemaNorge");
                allCustomers.Add("40", "RESTAURANTER");
                allCustomers.Add("979", "SalgsbilskunderiSverige");
                allCustomers.Add("10", "SALLING");
                allCustomers.Add("69", "SASRADDISON");
                allCustomers.Add("76", "Sodexo");
                allCustomers.Add("23", "SUPERGROSCENTRALLAGER");
                allCustomers.Add("967", "SvenskeKunde");
                allCustomers.Add("102", "Tyskland");
                allCustomers.Add("27", "ØVRIGEDANSKCATER-GENNEMFAK");
                allCustomers.Add("94", "Øvrigeexport(grønland)");
                allCustomers.Add("93", "ØvrigeFinland");

                //Dictionary<string, string> allCustomersSwitched = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> item in allCustomers)
                {
                    CustDictionary.Add(item.Value, item.Key);
                }
            }
        }

        public static void SetAllProductsNavDict(Dictionary<string, string> dict)
        {
            if (AllProductsNavFirst)
            {
                AllProductsNavFirst = false;
                foreach (KeyValuePair<string, string> item in dict)
                {
                    AllProductsNavDict.Add(item.Key, item.Value);
                }
            }
        }

        public static void InitiateDate()
        {
           // Dictionary<int, DateTime> StartDate = new Dictionary<int, DateTime>();
            if (StartDateFirst)
            {
                StartDateFirst = false;
                string s = "2014-12-29 00:01";

                DateTime dt =
                    DateTime.ParseExact(s, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                StartDate.Add(2015, dt);
                //MessageBox.Show("after");

                s = "2013-12-30 00:01";

                dt =
                    DateTime.ParseExact(s, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                StartDate.Add(2014, dt);

                //st = "01/04/2016";
                s = "2016-01-04 00:01";

                dt =
                    DateTime.ParseExact(s, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                StartDate.Add(2016, dt);

                //startDate.Add(2016, DateTime.Parse(st));
                //st = "01/02/2017";
                s = "2017-01-02 00:01";

                dt =
                    DateTime.ParseExact(s, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                StartDate.Add(2017, dt);
            }
        }

        public static void SetAllSuppliersM3(Dictionary<string, List<string>> dict)
        {
            if (AllSuppliersM3First)
            {
                
                AllSuppliersM3First = false;
                foreach (KeyValuePair<string, List<string>> item in dict)
                {

                    AllSuppliersM3.Add(item.Key, item.Value);

                }
                CreateProdToSupplDict();

                AllSuppliersM3.Add("Cutting", AllCuttingProducts);

                List<string> TestStockProducts = new List<string>();
                TestStockProducts.Add("56170");
                TestStockProducts.Add("60268");
                TestStockProducts.Add("56108");

                AllSuppliersM3.Add("TestStock", TestStockProducts);
            }
        }

        private static void CreateProdToSupplDict()
        {

            //string returnSuppl = "";
            foreach (KeyValuePair<string, List<string>> item in ClassStaticVaribles.AllSuppliersM3)
            {
                foreach (string prodNBR in item.Value)
                {
                    //returnSuppl = item.Key;
                    if (!ProdToSupplDict.ContainsKey(prodNBR))
                    {
                        ProdToSupplDict.Add(prodNBR, item.Key);
                    }
                }
            }
        }


        public static void SetAllProductsM3Dict(Dictionary<string, string> dict)
        {
            if (AllProductsM3DictFirst)
            {
                AllProductsM3DictFirst = false;
                foreach (KeyValuePair<string, string> item in dict)
                {
                    AllProductsM3Dict.Add(item.Key, item.Value);
                }
                Console.WriteLine("AllNumberProducts: " + AllProductsM3Dict.Count());
            }
        }

        public static void SetNewNumberDict()
        {
            if (NewNumberDictNavKeyFirst)
            {
                NewNumberDictNavKeyFirst = false;
                Dictionary<int, int> dict = new Dictionary<int, int>();
                dict.Add(28011, 20004);
                dict.Add(1120, 20012);
                dict.Add(1140, 20013);
                dict.Add(1094, 20014);
                dict.Add(28190, 20005);
                dict.Add(1050, 20020);
                dict.Add(1051, 20021);
                dict.Add(1052, 20022);
                dict.Add(1053, 20023);

                dict.Add(1060, 20015);
                dict.Add(1088, 20016);
                dict.Add(1110, 20017);
                dict.Add(1070, 20018);
                dict.Add(1040, 20025);
                dict.Add(1041, 20026);
                dict.Add(1098, 20030);

                dict.Add(2754, 20019);
                dict.Add(2763, 20006);
                dict.Add(2761, 20007);
                dict.Add(2760, 20008);
                dict.Add(2765, 20027);
                dict.Add(2402, 20028);
                dict.Add(1190, 20009);
                dict.Add(1192, 20010);
                dict.Add(1072, 20029);
                dict.Add(1193, 20032);
                dict.Add(2770, 20033);
                dict.Add(2748, 20034);
                dict.Add(1185, 20035);
                dict.Add(1191, 20036);
                dict.Add(1030, 20011);
                dict.Add(3832, 20037);
                dict.Add(2460, 20040);
                dict.Add(1310, 20038);
                dict.Add(1194, 20039);
                dict.Add(2864, 20041);
                dict.Add(1012, 20042);
                dict.Add(1320, 20043);
                dict.Add(1010, 20044);
                dict.Add(1075, 20045);
                dict.Add(2766, 20046);
                dict.Add(42730, 20047);
                dict.Add(2620, 20048);
                dict.Add(1338, 20049);
                dict.Add(1721, 20060);
                dict.Add(111128, 20061);

                foreach (KeyValuePair<int, int> item in dict)
                {
                    NewNumberDictNavKey.Add(item.Key.ToString(), item.Value.ToString());
                    NewNumberDictM3Key.Add(item.Value.ToString(), item.Key.ToString());
                }
                NewNumberDictNavKey.Add("Ny", "20024");
                NewNumberDictM3Key.Add("20024", "Ny");
                NewNumberDictM3Key.Add("20031", "1098");
            }
        }




        internal static void SetAllSuppliersNameDict(Dictionary<string, string> AllSuppliersName)
        {
            AllSuppliersNameDict = AllSuppliersName;
        }

        internal static string GetCustNavCode(string M3code)
        {
            string returnString = M3code;
            if(AssortmentM3_toNav.ContainsKey(M3code))
            {
                returnString = AssortmentM3_toNav[M3code];
            }
            return returnString;
        }
    }
}
