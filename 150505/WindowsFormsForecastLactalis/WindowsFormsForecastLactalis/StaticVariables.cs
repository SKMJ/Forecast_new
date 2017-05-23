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
    public static class StaticVariables
    {
        public static Dictionary<string, string> AssortmentDictionaryNav;
        public static Dictionary<string, string> AssortmentDictionaryM3;
        public static Dictionary<string, int> dictItemStatus;
        public static Dictionary<string, string> dictPolicyKanal = new Dictionary<string, string>();

        // private bool loadingNewProductsOngoing;
        public static Dictionary<string, string> AssortmentNation = new Dictionary<string, string>();

        public static Dictionary<string, string> AllProductsNavDict = new Dictionary<string, string>();

        public static Dictionary<string, string> ProdToSupplDict = new Dictionary<string, string>();

        public static Dictionary<string, string> ProdMotherChildRelation = new Dictionary<string, string>();

        public static List<string> AllCuttingProducts = new List<string>();

        public static Dictionary<string, string> AllProductsM3Dict = new Dictionary<string, string>();
        public static Dictionary<string, List<string>> AllSuppliersM3 = new Dictionary<string, List<string>>();
        public static Dictionary<string, string> AllSuppliersNameDict = new Dictionary<string, string>();

        //public static Dictionary<string, string> NewNumberDictNavKey = new Dictionary<string, string>();
        //public static Dictionary<string, string> NewNumberDictM3Key = new Dictionary<string, string>();

        public static Dictionary<string, List<string>> AssortmentM3_toNav = new Dictionary<string, List<string>>();
        public static Dictionary<string, string> AssortmentNav = new Dictionary<string, string>();

        public static Dictionary<string, List<string>> AssortmentM3_toKedjor = new Dictionary<string, List<string>>();

        public static Dictionary<string, List<string>> Kedjor_toCUNO = new Dictionary<string, List<string>>();
        public static List<PurchaseOrderLine> PurchaseOrderLinesM3 = new List<PurchaseOrderLine>();
        public static List<PurchaseOrderLine> ExpectedPurchaseOrderLinesM3 = new List<PurchaseOrderLine>();

        public static Dictionary<int, DateTime> StartDate = new Dictionary<int, DateTime>(); //Dict First Date of week 1 in Year
        public static bool Production = true;

        private static bool AllProductsNavFirst = true;
        private static bool AllProductsM3DictFirst = true;
        private static bool AllSuppliersM3First = true;
        //private static bool NewNumberDictNavKeyFirst = true;

        private static bool CustDictionaryFirst = true;
        private static bool StartDateFirst = true;
        public static int TestWeek = 0;

        public enum WritePermission { Read = 1, Write = 2, SaleWrite = 3, SupplWrite = 4 };


        public static void SetMotherChildDictionary()
        {
            try
            {
                if (ProdMotherChildRelation.Count < 1)
                {

                    SQL_M3Direct m3Sql = new SQL_M3Direct();

                    ProdMotherChildRelation = m3Sql.GetMotherChildren();
                    m3Sql.Close();
                }
            }
            catch
            {
                ProdMotherChildRelation = new Dictionary<string, string>();
                Console.WriteLine("Mother Children does not work. Probalby no Client access");
            }

        }

        public static void SetAssortmentDictionary()
        {
            if (CustDictionaryFirst)
            {
                CustDictionaryFirst = false;
                Dictionary<string, string> allAssortments = new Dictionary<string, string>();
                AssortmentDictionaryNav = new Dictionary<string, string>();

                allAssortments.Add("KGVG", "23");
                allAssortments.Add("LUWW", "26,9500");
                allAssortments.Add("YEYD", "102,51500");
                allAssortments.Add("KDVC", "4,9102");
                allAssortments.Add("KDVA", "15");
                allAssortments.Add("KRVI", "28,6312");
                allAssortments.Add("KIVM", "6,1810");
                allAssortments.Add("KCVD", "52,9705");
                allAssortments.Add("LUWX", "30,31");
                allAssortments.Add("KCVF", "IRMA,9800");

                allAssortments.Add("LDWA", "47");
                allAssortments.Add("LI", "19");
                allAssortments.Add("LH", "45");
                allAssortments.Add("LZ", "58");
                allAssortments.Add("LDWF", "56");
                allAssortments.Add("LOWQ", "60,63,65,43,69");
                allAssortments.Add("LDWZ", "41");
                allAssortments.Add("LDWG", "27,71");
                allAssortments.Add("LDWE", "73");
                allAssortments.Add("LOWT", "44");
                allAssortments.Add("LSWU", "10,42");
                allAssortments.Add("LOWR", "40");
                allAssortments.Add("LOWS", "76");
                //allAssortments.Add("ZGZG", "971");
                //allAssortments.Add("NR", "988");
                //allAssortments.Add("OZOZ", "96");
                allAssortments.Add("OIOI", "92");
                //allAssortments.Add("OZ", "93");
                allAssortments.Add("YEYB", "103");
                //allAssortments.Add("DD", "975");

                allAssortments.Add("DZDZ", "986,984,979,989,971");
                allAssortments.Add("DDDCLC", "975");
                allAssortments.Add("DBDBLC", "976");
                allAssortments.Add("DBBCBC", "977");
                allAssortments.Add("DKCKLC", "974");
                allAssortments.Add("DIILLC", "973");
                allAssortments.Add("ICABP", "983");
                allAssortments.Add("DNZNLC", "969");

                allAssortments.Add("OEOE", "96");
                allAssortments.Add("OAOA", "90");
                allAssortments.Add("OZOZ", "93");
                allAssortments.Add("SKM", "904");
                allAssortments.Add("NRNR", "988");
                allAssortments.Add("NNNN", "980");
                allAssortments.Add("NINI", "982");
                allAssortments.Add("NCNC", "964");
                allAssortments.Add("YEYC", "94");
                //Dictionary<string, string> allCustomersSwitched = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> item in allAssortments)
                {
                    List<string> tempList = new List<string>();
                    string[] tempList2 = item.Value.Split(',');
                    for (int i = 0; i < tempList2.Length; i++)
                    {
                        tempList.Add(tempList2[i]);
                        AssortmentNav.Add(tempList2[i], item.Key);
                    }

                    AssortmentM3_toNav.Add(item.Key, tempList);
                }
                FillNationDictionary();
                Console.WriteLine("Assortment ready" + AssortmentM3_toNav.ToString());

            }
        }

        private static void FillNationDictionary()
        {
            //Danmark :
            AssortmentNation.Add("YEYD", "DK");// ALDI tx15: KAVK

            AssortmentNation.Add("KAVK", "DK");// ALDI tx15: KAVK

            AssortmentNation.Add("KCVD", "DK");// Coop Danmark tx15: KCVD

            AssortmentNation.Add("KCVF", "DK");// Irma tx15: KCVF;LUWY

            AssortmentNation.Add("KDVA", "DK");// Dansk Supermarked tx15: KDVA;KDVB

            AssortmentNation.Add("KDVC", "DK");// Netto Danmark tx15: KDVC

            AssortmentNation.Add("KGVG", "DK");// Dagrofa Danmark tx15: KGVG

            AssortmentNation.Add("KIVM", "DK");// Intervare tx15: KIVM

            AssortmentNation.Add("KRVI", "DK");// Reitan tx15: KRVI;KRVJ

            AssortmentNation.Add("LDWA", "DK");// AB Catering, BC Catering, Inco CC tx15: LDWA;LDWB;LDWC

            AssortmentNation.Add("LH", "DK");// Hørkram tx15: LH

            AssortmentNation.Add("LI", "DK");// HKI tx15: LI

            AssortmentNation.Add("LOWE", "DK");// Meyer Kantiner Ny tx15: LOWE

            AssortmentNation.Add("LZ", "DK");// Grossister tx15: LZ

            AssortmentNation.Add("LOWQ", "DK");// Hoteller tx15: LOWQ

            AssortmentNation.Add("LOWR", "DK");// Restauranter tx15: LOWR

            AssortmentNation.Add("LOWS", "DK");// Sodexo tx15: LOWS

            AssortmentNation.Add("LOWT", "DK");// Øvrige Out of Home tx15: LOWT

            AssortmentNation.Add("LSWU", "DK");// Ostehandlere tx15: LSWU

            AssortmentNation.Add("LUWW", "DK");// GNF DAGROFA tx15: LUWW;KGVH

            AssortmentNation.Add("LUWX", "DK");// Coop Danmark Direkt tx15: LUWX;KCVE

            AssortmentNation.Add("YEYC", "DK");// Arla Grönland tx15: YEYC



            //Finland:
            AssortmentNation.Add("OAOA", "FI");// Helsinki Foodstock tx15: OAOAOA

            AssortmentNation.Add("OEOE", "FI");// KESKO KESPRO Finland tx15: OEOEOE;OPOPOP

            AssortmentNation.Add("OIOI", "FI");// INEX Finland tx15: OIOIOI

            AssortmentNation.Add("OZOZ", "FI");// Other Customers Finland tx15: OZOZOZ;OTOTOT


            //Norge:
            AssortmentNation.Add("NCNC", "NO");// COOP Norge tx15: NCNCNC

            AssortmentNation.Add("NINI", "NO");// ICA Norge tx15: NININI

            AssortmentNation.Add("NNNN", "NO");// Unil tx15: NNNNNN

            AssortmentNation.Add("NRNR", "NO");// REMA Norge tx15: NRNRNR

            //Others:

            AssortmentNation.Add("SKM", "XX");// SKM tx15: Styrs via FC

            AssortmentNation.Add("YEVL", "XX");// ISLAND tx15: YEVL

            AssortmentNation.Add("YEYB", "XX");// NETTO Polen tx15: YEYB
            AssortmentNation.Add("NETTO", "XX");// Netto sortiment tx15: NETTO

            AssortmentNation.Add("OKQ8", "XX");// OKQ8 sortiment tx15: OKQ8 sortiment


            //Sverige:

            AssortmentNation.Add("AXFOOD", "SE");// Axfoods sortiment Storhushåll tx15: Axfoods sortime

            AssortmentNation.Add("BITOST", "SE");// Bitost i wellkartong tx15: Bitost i wellka

            AssortmentNation.Add("DBBCBC", "SE");// Bergendahls butikspack tx15: DBBCBC

            AssortmentNation.Add("DBDBLC", "SE");// Bergendahls tx15: DBDBLC

            AssortmentNation.Add("DDDCLC", "SE");// Axfood tx15: DDDCLC

            AssortmentNation.Add("DIILLC", "SE");// ICA Sverige tx15: DIILLC

            AssortmentNation.Add("DKCKLC", "SE");// Coop Sverige tx15: DKCKLC

            AssortmentNation.Add("DNZNLC", "SE");// Netto Sverige tx15: DNZNLC

            AssortmentNation.Add("DZDZ", "SE");// Direkt Sverige tx15: Styrs i FC

            AssortmentNation.Add("ICABP", "SE");// ICA Butikspack tx15: Styrs via FC

        }

        public static void SetCustDictionary()
        {
            if (CustDictionaryFirst)
            {
                SetAssortmentDictionary();
                CustDictionaryFirst = false;
                Dictionary<string, string> allCustomers = new Dictionary<string, string>();
                AssortmentDictionaryNav = new Dictionary<string, string>();
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
                    AssortmentDictionaryNav.Add(item.Value, item.Key);
                }
            }
        }

        public static void SetCustAssortmentListM3(Dictionary<string, string> inputDict)
        {
            AssortmentDictionaryM3 = new Dictionary<string, string>();

            AssortmentDictionaryM3 = inputDict;

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
                //StartDateFirst = false;
                string s = "2014-12-29";
                //string s = "2015-01-03";

                DateTime dt =
                    StaticVariables.ParseExactStringToDate(s, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                StartDate.Add(2015, dt);
                //MessageBox.Show("after");

                s = "2013-12-30";
                //s = "2013-12-28";

                dt =
                    StaticVariables.ParseExactStringToDate(s, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                StartDate.Add(2014, dt);

                //st = "01/04/2016";
                s = "2016-01-04";
                s = "2016-01-03";

                dt =
                    StaticVariables.ParseExactStringToDate(s, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                StartDate.Add(2016, dt);

                //startDate.Add(2016, StaticVariables.GetDateTimeFromString(st));
                //st = "01/02/2017";
                //s = "2017-01-02";
                s = "2016-12-31";
                dt =
                    StaticVariables.ParseExactStringToDate(s, "yyyy-MM-dd", CultureInfo.InvariantCulture);
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

            }
        }

        private static void CreateProdToSupplDict()
        {

            //string returnSuppl = "";
            foreach (KeyValuePair<string, List<string>> item in StaticVariables.AllSuppliersM3)
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


        //Set all products in WHS Skaevinge
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

        internal static void SetAllSuppliersNameDict(Dictionary<string, string> AllSuppliersName)
        {
            AllSuppliersNameDict = AllSuppliersName;
        }

        internal static string GetCustNavCodeFirst(string M3code)
        {
            string returnString = M3code;
            string tempFirst = "";
            if (AssortmentM3_toNav.ContainsKey(M3code))
            {
                foreach (string item in AssortmentM3_toNav[M3code])
                {
                    tempFirst = item;
                    break;
                }


                returnString = tempFirst;
            }
            return returnString;
        }

        internal static List<string> GetCustNavCodes(string customerNumber)
        {
            if (AssortmentM3_toNav.ContainsKey(customerNumber))
            {
                return AssortmentM3_toNav[customerNumber];
            }
            else
            {
                Console.WriteLine("Assotment ERROR code missing");
                List<string> temp = new List<string>();
                temp.Add(customerNumber);
                return temp;
            }
        }

        internal static void SetKedjaToCunoListM3(Dictionary<string, List<string>> returnStrings)
        {
            Kedjor_toCUNO = returnStrings;
        }

        internal static void SetProdOrTest(bool prod)
        {
            Production = prod;
        }

        public static int GetWeek(DateTime date, int year)
        {
            int weekInt = StaticVariables.GetForecastWeeknumberForDate(date);
            return weekInt;
        }

        public static int GetWeek2(DateTime date)
        {
            var day = (int)CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(date);
            int week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date.AddDays(4 - (day == 0 ? 7 : day)), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            //Om söndag returnera nästa ISO-vecka
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                int year = date.Year;
                if (date.Month == 1 && week > 1)
                {
                    year = year - 1;
                }
                week = week + 1;
                DateTime maxday = new DateTime(year, 12, 28);
                var maxWeekday = (int)CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(maxday);
                int numberOfWeeks = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(maxday.AddDays(4 - (maxWeekday == 0 ? 7 : maxWeekday)), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                week = week > numberOfWeeks ? 1 : week;
            }
            return week;
        }

        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3 - 1);
        }

        public static bool AbortLoad { get; set; }

        public static string TableM3Sales
        {
            get { return Production ? "M3_Sales" : "M3_Sales_Test"; }
        }

        public static string TableChainCustomer
        {
            get { return Production ? "Chain_Customer" : "Chain_Customer_Test"; }
        }

        public static string TableChainAssortment
        {
            get { return Production ? "Chain_Assortment" : "Chain_Assortment_Test"; }
        }

        public static string TableDebitorBudgetLinjePost
        {
            get { return Production ? "Debitor_Budgetlinjepost" : "Debitor_Budgetlinjepost_Test"; }
        }

        internal static DateTime FirstMonYearWeakOne(int wantedYear)
        {
            string year = wantedYear.ToString();

            string s = year + "-01-04";
            //string s = "2015-01-03";

            DateTime dt =
                StaticVariables.ParseExactStringToDate(s, "yyyy-MM-dd", CultureInfo.InvariantCulture);


            int loopError = 0;
            while (dt.DayOfWeek != DayOfWeek.Monday && loopError < 10)
            {
                dt = dt.AddDays(-1);
                loopError++;
            }


            if (loopError > 7)
            {
                //Error: start year with 4th jan
                Console.WriteLine("Loop Error day");
                dt =
                StaticVariables.ParseExactStringToDate(s, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            return dt;
        }

        internal static DateTime FirstSaturdayBeforeWeakOne(int wantedYear)
        {
            DateTime dt = FirstMonYearWeakOne(wantedYear);
            dt = dt.AddDays(-2);
            return dt;
        }

        internal static int GetForecastWeeknumberForDate(DateTime inputDate)
        {

            int year = inputDate.Year;
            DateTime dtFirst = FirstSaturdayBeforeWeakOne(year);

            if ((inputDate - dtFirst).TotalDays < 0)
            {
                dtFirst = FirstSaturdayBeforeWeakOne(year - 1);
            }

            double week1 = (inputDate - dtFirst).TotalDays;
            double weekNBR = week1 / 7.0;
            int weekInt = (int)Math.Floor(weekNBR);
            weekInt = weekInt + 1;

            return weekInt;
        }


        internal static DateTime GetForecastStartDateOfWeeknumber(int year, int week)
        {


            DateTime dtFirst = FirstSaturdayBeforeWeakOne(year);

            dtFirst = dtFirst.AddDays(7 * (week - 1));

            return dtFirst;
        }

        internal static int GetWeekFromName(string columnName)
        {
            int weekNbr = 0;
            if (columnName.Contains(".") && columnName.Contains("20"))
            {
                string[] temp = columnName.Split('.');
                weekNbr = Convert.ToInt32(temp[0]);
            }
            return weekNbr;
        }

        internal static int GetYearFromName(string columnName)
        {
            int yearNbr = 0;
            if (columnName.Contains(".") && columnName.Contains("20"))
            {
                string[] temp = columnName.Split('.');
                string yearString = temp[1];
                yearString = yearString.Substring(0, 4);
                yearNbr = Convert.ToInt32(yearString);
            }

            return yearNbr;
        }

        internal static bool DateWithinNowForecastLimit(DateTime tempDate)
        {
            bool returnBool = false;
            TimeSpan span = tempDate.Subtract(DateTime.Now);
            int daysDifference = (int)Math.Abs(span.TotalDays);
            if (21 * 7 > daysDifference) //only witihin 20 weeks count
            {
                returnBool = true;
            }
            return returnBool;
        }


        internal static string ReturnDanishFormat(string tempDate)
        {
            string outDate = tempDate.Replace("-", "");
            string year;
            string month;
            string day;
            if (outDate.StartsWith("20") && outDate.Length == 8)
            {
                year = outDate.Substring(0, 4);
                month = outDate.Substring(4, 2);
                day = outDate.Substring(6, 2);
                outDate = day + "-" + month + "-" + year;

            }
            return outDate;
        }


        internal static DateTime GetDateTimeFromString(string dateString)
        {
            DateTime date = new DateTime();
            try
            {
                date = DateTime.Parse(dateString);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error transforming string to date: " + dateString + "\r\n" + "Send this info to Christofer Hellberg in print screen" + "\r\n" + ex.Message);
            }
            return date;
        }


        internal static DateTime ParseExactStringToDate(string dateString, string format, CultureInfo cultureInfo)
        {
            DateTime date = new DateTime();
            string dateStringLocal = dateString.Replace(".", "");
            dateStringLocal = dateStringLocal.Replace(",", "");

            try
            {
                date = DateTime.ParseExact(dateStringLocal, format, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error transforming string to date Exact: " + dateString + "\r\n" + " format: " + format + "\r\n" + "Send this info to Christofer Hellberg in print screen" + "\r\n" + ex.Message);
            }
            return date;
        }

        internal static string GetNationForAssortment(string assortment)
        {
            string nationCode = "XX";
            if (AssortmentNation.ContainsKey(assortment))
            {
                nationCode = AssortmentNation[assortment];
            }
            return nationCode;
        }



        public static Dictionary<string, string> GetDictKanalCode()
        {
            if (dictPolicyKanal.Count < 1)
            {


                SQL_ModdedM3 m3Mod = new SQL_ModdedM3();
                dictPolicyKanal = m3Mod.GetDictKanalCode();
            }
            return dictPolicyKanal;

        }
    }
}
