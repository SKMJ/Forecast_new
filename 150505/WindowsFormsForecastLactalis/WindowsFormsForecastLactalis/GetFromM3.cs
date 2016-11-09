///The file that handles all the M3 interface calls

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lawson.M3.MvxSock;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsForecastLactalis
{
    public class GetFromM3
    {

        //ConnectionString
        string ipNummer = "172.31.157.25";//M3 Test //dev
        //string ipNummer = "172.31.157.14";//M3 produktion

        int portNumber = 16205; //M3 Test
        //int portNumber = 16105; //M3 Prod
        //int portNumber = 16305; //M3 Dev
        String userName = "mi310";
        String userPsw = "MIPGM99";

        Dictionary<string, List<string>> dictSupplier;
        

        /// <summary>
        /// This function is just to test M3 Connection
        /// </summary>
        /// <returns></returns>
        public bool TestM3Connection()
        {
            //List<int> productList = new List<int>();

            string returnString = "";
            {
                int itemNbr = 1075;
                SERVER_ID sid = new SERVER_ID();
                uint rc;
                rc = ConnectToM3Interface(ref sid, "MMS200MI");
                if (rc != 0)
                {
                    Console.WriteLine("Test M3 Connection Failed!");
                    MessageBox.Show("Test M3 Connection Failed! \r\n Application Will not work without M3  \r\n Error: " + rc);
                    return false;
                }
                //Set the field without need to know position Start from this customer 00752
                MvxSock.SetField(ref sid, "ITNO", itemNbr.ToString());
                MvxSock.SetField(ref sid, "CONO", "001");
                rc = MvxSock.Access(ref sid, "GetItmBasic");
                if (rc != 0)
                {
                    //MvxSock.ShowLastError(ref sid, "Error in get Name by productsNbr: " + rc + "\n");
                    MvxSock.Close(ref sid);
                    return false;
                }
                returnString = MvxSock.GetField(ref sid, "ITDS");
                Console.WriteLine("Test M3 works ProductNBR: " + itemNbr + " Name: " + returnString);
                MvxSock.Close(ref sid);
                return true;
            }
        }

        public List<string> GetListOfProductsNbrByAssortment(string assortment)
        {
            List<string> productList = new List<string>();
            {
                SERVER_ID sid = new SERVER_ID();

                uint rc = 0;
                rc = ConnectToM3Interface(ref sid, "CRS105MI");
                if (rc != 0)
                {
                    return null;
                }
                SetMaxList(sid, 2000);
                //Set the field without need to know position Start from this customer 00752
                MvxSock.SetField(ref sid, "ASCD", assortment);
                MvxSock.SetField(ref sid, "CONO", "001");
                rc = MvxSock.Access(ref sid, "LstAssmItem");
                if (rc != 0)
                {
                    MvxSock.ShowLastError(ref sid, "Error in get products no " + rc + "\n");
                    MvxSock.Close(ref sid);
                    return null;
                }

                while (MvxSock.More(ref sid))
                {
                    string tempItemNbr = MvxSock.GetField(ref sid, "ITNO");
                    Console.Write("Item nr: " + tempItemNbr);
                    Console.WriteLine("Kedja: " + MvxSock.GetField(ref sid, "ASCD"));
                    //Mooves to next row
                    MvxSock.Access(ref sid, null);

                    productList.Add(tempItemNbr);

                }

                MvxSock.Close(ref sid);

                Console.WriteLine("M3 communication: SUCCESS!!");
                return productList;
            }
        }

        public Dictionary<int, int> GetPromotionsOfProducts(string itno, int year)
        {
            Dictionary<int, int> promotions = new Dictionary<int, int>();
            for (int i = 0; i < 54; i++)
            {
                promotions.Add(i, 0);
            }

            int yearExtra = 0;

            if (year > 2000)
            {
                promotions = GetPromotionsOfProductsByDivision(itno, year, "310", promotions, false);
                promotions = GetPromotionsOfProductsByDivision(itno, year, "320", promotions, false);
                promotions = GetPromotionsOfProductsByDivision(itno, year, "710", promotions, false);
                promotions = GetPromotionsOfProductsByDivision(itno, year, "720", promotions, false);
            }
            else if (year < 2000)
            {
                promotions = GetPromotionsOfProductsByDivision(itno, DateTime.Now.Year, "310", promotions, true);
                promotions = GetPromotionsOfProductsByDivision(itno, DateTime.Now.Year, "320", promotions, true);
                promotions = GetPromotionsOfProductsByDivision(itno, DateTime.Now.Year, "710", promotions, true);
                promotions = GetPromotionsOfProductsByDivision(itno, DateTime.Now.Year, "720", promotions, true);
                if (StaticVariables.GetForecastWeeknumberForDate(DateTime.Now) > 32)
                {
                    yearExtra = DateTime.Now.Year + 1;
                    promotions = GetPromotionsOfProductsByDivision(itno, yearExtra, "310", promotions, true);
                    promotions = GetPromotionsOfProductsByDivision(itno, yearExtra, "320", promotions, true);
                    promotions = GetPromotionsOfProductsByDivision(itno, yearExtra, "710", promotions, true);
                    promotions = GetPromotionsOfProductsByDivision(itno, yearExtra, "720", promotions, true);
                }
                else if (StaticVariables.GetForecastWeeknumberForDate(DateTime.Now) < 21)
                {
                    yearExtra = DateTime.Now.Year - 1;
                    promotions = GetPromotionsOfProductsByDivision(itno, yearExtra, "310", promotions, true);
                    promotions = GetPromotionsOfProductsByDivision(itno, yearExtra, "320", promotions, true);
                    promotions = GetPromotionsOfProductsByDivision(itno, yearExtra, "710", promotions, true);
                    promotions = GetPromotionsOfProductsByDivision(itno, yearExtra, "720", promotions, true);
                }
            }
            return promotions;
        }

        public Dictionary<int, int> GetPromotionsOfProductsByDivision(string itno, int year, string division, Dictionary<int, int> promotions, bool nowSelected)
        {
            SERVER_ID sid = new SERVER_ID();

            uint rc = 0;
            rc = ConnectToM3Interface(ref sid, "OIS840MI");
            if (rc != 0)
            {
                return null;
            }
            Console.WriteLine("Get Item campaign: " + itno);
            SetMaxList(sid, 2000);
            //Set the field without need to know position Start from this customer 00752
            MvxSock.SetField(ref sid, "YEAR", year.ToString());
            MvxSock.SetField(ref sid, "CONO", "001");
            MvxSock.SetField(ref sid, "DIVI", division);
            MvxSock.SetField(ref sid, "ITNO", itno);
            rc = MvxSock.Access(ref sid, "LstPromItem");
            if (rc != 0)
            {
                MvxSock.ShowLastError(ref sid, "Error in get products no " + rc + "\n");
                MvxSock.Close(ref sid);
                return null;
            }

            while (MvxSock.More(ref sid))
            {
                string tempstartDate = MvxSock.GetField(ref sid, "FRD2");
                string tempEndDate = MvxSock.GetField(ref sid, "TOD2");
                string tempAntal = MvxSock.GetField(ref sid, "BUQT");
                string promotionID = MvxSock.GetField(ref sid, "PIDE");
                string[] temp = tempAntal.Split('.');
                int Antal = Convert.ToInt32(temp[0]);
                string tempCampCuse = MvxSock.GetField(ref sid, "CUSE");
                Console.WriteLine("Kedja: " + tempCampCuse + " start: " + tempstartDate + " end: " + tempEndDate + " antal: " + tempAntal);
                //Mooves to next row
                MvxSock.Access(ref sid, null);
                try
                {
                    DateTime tempDate = StaticVariables.ParseExactStringToDate(tempstartDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

                    tempDate = StaticVariables.ParseExactStringToDate(tempstartDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

                    //Kampanjer ska ligga veckan innan kampanjstart
                    tempDate = tempDate.AddDays(-7);
                    int weekInt = StaticVariables.GetWeek2(tempDate);
                    if (weekInt > 0 && weekInt < 54)
                    {
                        if (nowSelected)
                        {
                            bool withinLimit = StaticVariables.DateWithinNowForecastLimit(tempDate);
                            if (withinLimit) //only witihin 20 weeks count
                            {
                                promotions[weekInt] = promotions[weekInt] + Antal;
                            }
                        }
                        else
                        {
                            promotions[weekInt] = promotions[weekInt] + Antal;
                        }
                                                    
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    MessageBox.Show("Kan inte visa kampanj " + promotionID + " på division " + division);
                }
                
            }
            MvxSock.Close(ref sid);

            Console.WriteLine("M3 communication: SUCCESS!!");
            return promotions;

        }

        public Dictionary<string, string> GetListOfAssortments()
        {
            Dictionary<string, string> returnStrings = new Dictionary<string, string>();
            {
                SERVER_ID sid = new SERVER_ID();
                uint rc = 0;
                rc = ConnectToM3Interface(ref sid, "CRS105MI");
                if (rc != 0)
                {
                    return null;
                }
                SetMaxList(sid, 2000);
                //Set the field without need to know position Start from this customer 00752
                //MvxSock.SetField(ref sid, "ASCD", assortment);
                MvxSock.SetField(ref sid, "CONO", "001");
                rc = MvxSock.Access(ref sid, "LstAssmHead");
                if (rc != 0)
                {
                    MvxSock.ShowLastError(ref sid, "Error in get products no " + rc + "\n");
                    MvxSock.Close(ref sid);
                    return null;
                }

                while (MvxSock.More(ref sid))
                {
                    string m3_kod = MvxSock.GetField(ref sid, "ASCD");
                    string tx40 = MvxSock.GetField(ref sid, "TX40");
                    string tx15 = MvxSock.GetField(ref sid, "TX15");
                    Console.WriteLine("Kedja: " + m3_kod + " tx40: " + tx40 + " tx15: " + tx15);
                    if (StaticVariables.AssortmentM3_toNav.ContainsKey(m3_kod))
                    {
                        Console.WriteLine("M3kod till NavKod: " + StaticVariables.AssortmentM3_toNav[m3_kod][0]);
                    }
                    else
                    {
                        Console.WriteLine("saknas Nav-Kod");
                    }
                    //Mooves to next row
                    MvxSock.Access(ref sid, null);

                    List<string> temp = new List<string>();
                    if (tx15.Contains(';'))
                    {

                        string[] tempArray = tx15.Split(';');
                        for (int i = 0; i < tempArray.Length; i++)
                        {
                            temp.Add(tempArray[i]);
                        }
                    }
                    else
                    {
                        temp.Add(m3_kod);
                    }

                    StaticVariables.AssortmentM3_toKedjor.Add(m3_kod, temp);
                    returnStrings.Add(m3_kod, tx40);

                }

                MvxSock.Close(ref sid);

                Console.WriteLine("M3 communication: SUCCESS!!");

                StaticVariables.SetCustAssortmentListM3(returnStrings);
                return returnStrings;
            }
        }

        //[Obsolete("No longer used")]
        //public void GetDictOfKedjaToCustNBR()
        //{
        //    Dictionary<string, List<string>> returnStrings = new Dictionary<string, List<string>>();
        //    {
        //        Stopwatch stopwatch2 = Stopwatch.StartNew();
        //        SERVER_ID sid = new SERVER_ID();
                
        //        uint rc = 0;
        //        rc = ConnectToM3Interface(ref sid, "OIS038MI");
                
        //        if (rc != 0)
        //        {
        //            return;
        //        }
        //        SetMaxList(sid, 20000);
        //        //Set the field without need to know position Start from this customer 00752
        //        //MvxSock.SetField(ref sid, "ASCD", assortment);
        //        MvxSock.SetField(ref sid, "CONO", "001");
        //        rc = MvxSock.Access(ref sid, "LstBusChainCust");
        //        if (rc != 0)
        //        {
        //            MvxSock.ShowLastError(ref sid, "Error in get products no " + rc + "\n");
        //            MvxSock.Close(ref sid);
        //            return;
        //        }

        //        while (MvxSock.More(ref sid))
        //        {
        //            string cuno = MvxSock.GetField(ref sid, "CUNO");
        //            string chl = MvxSock.GetField(ref sid, "CHL4");
        //            if(chl.Trim().Length < 1)
        //                chl = MvxSock.GetField(ref sid, "CHL3");
        //            if (chl.Trim().Length < 1)
        //                chl = MvxSock.GetField(ref sid, "CHL2");
        //            if (chl.Trim().Length < 1)
        //                chl = MvxSock.GetField(ref sid, "CHL1");
        //            Console.WriteLine("cuno: " + cuno + " chl1: " + chl);
        //            if (returnStrings.ContainsKey(chl))
        //            {
        //                Console.WriteLine("kedja: " + chl + " existerar i returnStrings");
        //                returnStrings[chl].Add(cuno);
        //            }
        //            else
        //            {
        //                Console.WriteLine("kedja: " + chl + " saknas i returnStrings");
        //                List<string> cunoTemp = new List<string>();
        //                cunoTemp.Add(cuno);
        //                returnStrings.Add(chl, cunoTemp);
        //            }
        //            //Mooves to next row
        //            MvxSock.Access(ref sid, null);
        //        }

        //        MvxSock.Close(ref sid);

        //        Console.WriteLine("M3 communication: SUCCESS!!");

        //        StaticVariables.SetKedjaToCunoListM3(returnStrings);
        //        Console.WriteLine("Antal Kedjor: " + returnStrings.Count);
        //        Console.WriteLine("Time KedjaToCuno: " + stopwatch2.ElapsedMilliseconds);
        //        //return returnStrings;
        //    }
        //}

        public Dictionary<string, string> GetItemInfoByItemNumber(string itemNbr)
        {
            //string returnString = "";
            Dictionary<string, string> returnStrings = new Dictionary<string, string>();
            {
                SERVER_ID sid = new SERVER_ID();
                uint rc;
                rc = ConnectToM3Interface(ref sid, "ZMM001MI");
                if (rc != 0)
                {
                    return null;
                }
                ////Set the field without need to know position Start from this customer 00752
                MvxSock.SetField(ref sid, "ITNO", itemNbr);
                //MvxSock.SetField(ref sid, "CONO", "001");
                rc = MvxSock.Access(ref sid, "GetItemInfo");

                if (rc != 0)
                {
                    //MvxSock.ShowLastError(ref sid, "Error in get ForeCast info by productsNbr: " + rc + "\n");
                    MvxSock.Close(ref sid);
                    return null;
                }

                ////Division, Prep location och Whs location
                string div = MvxSock.GetField(ref sid, "LDIV");
                returnStrings.Add("div", div);
                string prepLocation = MvxSock.GetField(ref sid, "PRLO");
                returnStrings.Add("prepLocation", prepLocation);
                string whsLocation = MvxSock.GetField(ref sid, "WALO");
                returnStrings.Add("whsLocation", whsLocation);
                string forecastWeek = MvxSock.GetField(ref sid, "FCWK");
                returnStrings.Add("forecastWeek", forecastWeek);
                string FCMO = MvxSock.GetField(ref sid, "FCMO");
                returnStrings.Add("FCMO", FCMO);
                string FCTU = MvxSock.GetField(ref sid, "FCTU");
                returnStrings.Add("FCTU", FCTU);
                string FCWE = MvxSock.GetField(ref sid, "FCWE");
                returnStrings.Add("FCWE", FCWE);
                string FCTH = MvxSock.GetField(ref sid, "FCTH");
                returnStrings.Add("FCTH", FCTH);
                string FCFR = MvxSock.GetField(ref sid, "FCFR");
                returnStrings.Add("FCFR", FCFR);
                string FCSA = MvxSock.GetField(ref sid, "FCSA");
                returnStrings.Add("FCSA", FCSA);
                string FCSU = MvxSock.GetField(ref sid, "FCSU");
                returnStrings.Add("FCSU", FCSU);
                string INLActaFranceFile = MvxSock.GetField(ref sid, "INFC");
                returnStrings.Add("INLActaFranceFile", INLActaFranceFile);
                string FCLockSale = MvxSock.GetField(ref sid, "FCSL");
                returnStrings.Add("FCLockSale", FCLockSale);

                //Console.WriteLine("ProductNBR: " + itemNbr + " Name: " + returnString);
                MvxSock.Close(ref sid);

                Console.WriteLine("M3 Forecast info communication: SUCCESS!!");
                return returnStrings;
            }
        }

        //CRS620MI med transaktion GetBasicData 
        public string GetSalesWeekLockFromSupplier(string supplNBR)
        {
            //string returnString = "";
            Dictionary<string, string> returnStrings = new Dictionary<string, string>();
            {
                SERVER_ID sid = new SERVER_ID();
                uint rc;
                rc = ConnectToM3Interface(ref sid, "CRS620MI");
                if (rc != 0)
                {
                    return null;
                }
                ////Set the field without need to know position Start from this customer 00752
                MvxSock.SetField(ref sid, "SUNO", supplNBR);
                MvxSock.SetField(ref sid, "CONO", "001");
                rc = MvxSock.Access(ref sid, "GetBasicData");

                if (rc != 0)
                {
                    //MvxSock.ShowLastError(ref sid, "Error in get ForeCast info by productsNbr: " + rc + "\n");
                    MvxSock.Close(ref sid);
                    return null;
                }

                ////Division, Prep location och Whs location
                string FCLockSale = MvxSock.GetField(ref sid, "CFI3");
                FCLockSale = FCLockSale.Trim();
                if (FCLockSale.Length < 1)
                {
                    FCLockSale = "0";
                }

                MvxSock.Close(ref sid);

                Console.WriteLine("Get Lock days. Supplier: " + supplNBR + " days: " + FCLockSale );

                Console.WriteLine("M3 Forecast info communication get supplier days: SUCCESS!!");
                return FCLockSale;
            }
        }

        public string GetSupplierNameByNumber(string supplierNbr)
        {
            string returnString = "";
            {
                SERVER_ID sid = new SERVER_ID();

                uint rc;
                rc = ConnectToM3Interface(ref sid, "CRS620MI");
                if (rc != 0)
                {
                    //MvxSock.ShowLastError(ref sid, "Error no " + rc + "\n");
                    return returnString;
                }

                //Set the field without need to know position Start from this customer 00752
                MvxSock.SetField(ref sid, "SUNO", supplierNbr);
                MvxSock.SetField(ref sid, "CONO", "001");
                rc = MvxSock.Access(ref sid, "GetBasicData");
                if (rc != 0)
                {
                    //MvxSock.ShowLastError(ref sid, "Error in get Supplier by number " + rc + "\n");
                    MvxSock.Close(ref sid);
                    return returnString;
                }

                //string tempItemNBR = MvxSock.GetField(ref sid, "ITNO") + "\t\t";
                returnString = MvxSock.GetField(ref sid, "SUNM");
                //Console.WriteLine("SupplierNBR: " + supplierNbr + " Name: " + returnString);
                MvxSock.Close(ref sid);
                return returnString;

            }
        }

        public List<string> GetAllKampaignLevelsByCuse(string CUSE)
        {
            List<string> returnString = new List<string>();

            SERVER_ID sid = new SERVER_ID();

            uint rc;
            rc = ConnectToM3Interface(ref sid, "OIS038MI");
            
            if (rc != 0)
            {
                //MvxSock.ShowLastError(ref sid, "Error no " + rc + "\n");
                return returnString;
            }

            //Set the field without need to know position Start from this customer 00752
            MvxSock.SetField(ref sid, "CHAI", CUSE);
            MvxSock.SetField(ref sid, "CONO", "001");
            rc = MvxSock.Access(ref sid, "LstBusChainStr");
            if (rc != 0)
            {
                //MvxSock.ShowLastError(ref sid, "Error in get Supplier by number " + rc + "\n");
                MvxSock.Close(ref sid);
                return returnString;
            }

            //string tempItemNBR = MvxSock.GetField(ref sid, "ITNO") + "\t\t";
            returnString.Add(MvxSock.GetField(ref sid, "CHL1"));
            returnString.Add(MvxSock.GetField(ref sid, "CHL2"));
            returnString.Add(MvxSock.GetField(ref sid, "CHL3"));
            returnString.Add(MvxSock.GetField(ref sid, "CHL4"));

            //Console.WriteLine("SupplierNBR: " + supplierNbr + " Name: " + returnString);
            MvxSock.Close(ref sid);
            return returnString;
        }

        private uint ConnectToM3Interface(ref SERVER_ID sid, string m3Interface)
        {
            //Console.WriteLine("Before Connect to M3 Interface: " + M3Interface);
            if(StaticVariables.Production)
            { 
                ipNummer = "172.31.157.14";
                portNumber = 16105;
            }
            
            uint rc = 0;
            try
            {
                rc = MvxSock.Connect(ref sid, ipNummer, portNumber, userName, userPsw, m3Interface, null);
                //Console.WriteLine("After Connect to M3 Interface: " + m3Interface);
            }
            catch (Exception ex)
            {
                MessageBox.Show("fail M3 communication Connect! 2: MI: " + m3Interface);
                MessageBox.Show("Exception in M3 connection: " + ex.Message);
                Console.WriteLine("Exception in M3 connection: " + ex.Message);
                return rc;
            }
            if (rc != 0)
            {
                //MessageBox.Show("Not Connected to M3!! communication Fail! Application will not work!");
                //MvxSock.ShowLastError(ref sid, "Error no " + rc + "\n");
                Console.WriteLine("Exception in M3 connection: ");
                
            }
            return rc;
        }

        private void SetMaxList(SERVER_ID sid, int maxInt)
        {
            //Console.WriteLine("SetMaxList: " + maxInt);
            StringBuilder ret2 = new StringBuilder(1024);
            uint size2 = (uint)ret2.Capacity;

            String cmd2 = "SetLstMaxRec   " + maxInt;
            try
            {
                //MvxSock.Access(ref server_id, cmd);
                MvxSock.Trans(ref sid, cmd2, ret2, ref size2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("List set MAX number Error" + ex.Message);
            }
        }

        //Get Msgn code for new order
        public string GetNewOrderCode()
        {
            //Catrin helps to get this new code
            string returnString = "";
            uint rc;
            SERVER_ID sid = new SERVER_ID();

            rc = ConnectToM3Interface(ref sid, "PPS370MI");

            if (rc != 0)
            {
                MvxSock.ShowLastError(ref sid, "Error no " + rc + "\n");
                return returnString;
            }

            //Set the field without need to know position Start from this customer 00752
            MvxSock.SetField(ref sid, "BAOR", "NORMAL");
            rc = MvxSock.Access(ref sid, "StartEntry");

            if (rc != 0)
            {
                MvxSock.ShowLastError(ref sid, "Error in get Supplier by number " + rc + "\n");
                MvxSock.Close(ref sid);
                return returnString;
            }
            returnString = MvxSock.GetField(ref sid, "MSGN");
            Console.WriteLine("Message Number: " + returnString);
            MvxSock.Close(ref sid);
            return returnString;
        }

        //Creates order purposal line.
        //Added line to ol order if same MSGN
        public string CreateNewOrderProposal(string MSGN, string ITNO, int ordQuant, string SUNO, string Date)
        {
            string returnString = "";
            uint rc;
            SERVER_ID sid = new SERVER_ID();

            rc = ConnectToM3Interface(ref sid, "PPS370MI");

            if (rc != 0)
            {
                MvxSock.ShowLastError(ref sid, "Error no " + rc + "\n");
                return returnString;
            }

            //Set the field without need to know position Start from this customer 00752
            MvxSock.SetField(ref sid, "MSGN", MSGN);
            MvxSock.SetField(ref sid, "ITNO", ITNO);
            MvxSock.SetField(ref sid, "ORQA", ordQuant.ToString());
            MvxSock.SetField(ref sid, "FACI", "LAD");
            MvxSock.SetField(ref sid, "WHLO", "LSK");
            MvxSock.SetField(ref sid, "SUNO", SUNO);
            MvxSock.SetField(ref sid, "DWDT", Date);
            MvxSock.SetField(ref sid, "BUYE", "DK045104");

            rc = MvxSock.Access(ref sid, "AddLine");

            if (rc != 0)
            {
                MvxSock.ShowLastError(ref sid, "Error in get Supplier by number " + rc + "\n");
                MvxSock.Close(ref sid);
                return returnString;
            }
            string orderNumber = MvxSock.GetField(ref sid, "PUNO");
            returnString = MvxSock.GetField(ref sid, "PNLI");
            Console.WriteLine("Order Number: " + orderNumber + " Order Number: " + returnString);
            MvxSock.Close(ref sid);
            return returnString;
        }

        public Dictionary<string, string> GetAllSkaevingeProductsDict()
        {
            //Catrin helps to get this new code
            Dictionary<string, string> dictItems = new Dictionary<string, string>();
            dictSupplier = new Dictionary<string, List<string>>();
            StaticVariables.dictItemStatus = new Dictionary<string, int>();
            {
                SERVER_ID sid = new SERVER_ID();
                uint rc;
                rc = ConnectToM3Interface(ref sid, "MMS200MI");

                if (rc != 0)
                {
                    //MvxSock.ShowLastError(ref sid, "Error no " + rc + "\n");
                    return null;
                }
                SetMaxList(sid, 2000);
                //Set the field without need to know position Start from this customer 00752
                //MvxSock.SetField(ref sid, "ASCD", Assortment);
                MvxSock.SetField(ref sid, "CONO", "001");
                MvxSock.SetField(ref sid, "WHLO", "LSK");
                rc = MvxSock.Access(ref sid, "LstItmWhsByWhs");
                if (rc != 0)
                {
                    MvxSock.ShowLastError(ref sid, "Error in get suppliers no " + rc + "\n");
                    MvxSock.Close(ref sid);
                    return null;
                }

                while (MvxSock.More(ref sid))
                {
                    string itemNBR = MvxSock.GetField(ref sid, "ITNO");// + "\t\t";
                    string cutting = MvxSock.GetField(ref sid, "PUIT");// + "\t\t";
                    if (cutting.Contains("1") && !StaticVariables.AllCuttingProducts.Contains(itemNBR))
                    {
                        StaticVariables.AllCuttingProducts.Add(itemNBR);
                    }

                    string itemName = MvxSock.GetField(ref sid, "ITDS");// + "\t\t";
                    int status = int.Parse(MvxSock.GetField(ref sid, "STAT"));

                    string itemSupplier = MvxSock.GetField(ref sid, "SUNO");// + "\t\t";
                    if (itemSupplier.Length > 0) // && Convert.ToInt32(itemSupplier) > 99) SUNO är ej ett numeriskt fält
                    {
                        //Console.Write("Supplier nr: " + tempSupplierNBR);
                        //Console.WriteLine("Kedja: " + MvxSock.GetField(ref sid, "ASCD"));
                        //supplierList.Add(Convert.ToInt32(tempSupplierNbr));
                        if (!dictItems.ContainsKey(itemNBR))
                        {
                            dictItems.Add(itemNBR, itemName);
                        }
                        if (!dictSupplier.ContainsKey(itemSupplier))
                        {
                            List<string> tempList = new List<string>();
                            tempList.Add(itemNBR);
                            dictSupplier.Add(itemSupplier, tempList);
                        }
                        else
                        {
                            dictSupplier[itemSupplier].Add(itemNBR);
                        }
                        //Lägg till artikelstatus
                        if (!StaticVariables.dictItemStatus.ContainsKey(itemNBR))
                        {
                            StaticVariables.dictItemStatus.Add(itemNBR, status);
                        }
                    }
                    //Mooves to next row
                    MvxSock.Access(ref sid, null);
                }
                MvxSock.Close(ref sid);
                StaticVariables.SetAllProductsM3Dict(dictItems);
                return dictItems;
            }
        }

        public Dictionary<string, List<string>> GetSupplierWithItemsDict()
        {
            StaticVariables.SetAllSuppliersM3(dictSupplier);
            Dictionary<string, string> AllSuppliersNameDict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, List<string>> item in dictSupplier)
            {
                AllSuppliersNameDict.Add(item.Key, this.GetSupplierNameByNumber(item.Key));
            }
            StaticVariables.SetAllSuppliersNameDict(AllSuppliersNameDict);

            return dictSupplier;
        }

        public List<BalanceId> GetStockInfo(string m3ProdNumber, string warehouse)
        {
            List<BalanceId> balanceIdentities = new List<BalanceId>();
            if (m3ProdNumber.Length > 0)
            {
                SERVER_ID sid = new SERVER_ID();
                uint rc;
                rc = ConnectToM3Interface(ref sid, "MMS060MI");
                if (rc != 0)
                {
                    return null;
                }
                MvxSock.SetField(ref sid, "ITNO", m3ProdNumber);
                MvxSock.SetField(ref sid, "CONO", "001");

                MvxSock.SetField(ref sid, "WHLO", warehouse);
                rc = MvxSock.Access(ref sid, "List");

                if (rc != 0)
                {
                    Console.WriteLine("Error in get Stock info by productsNbr: " + rc + "\n");
                    MvxSock.Close(ref sid);
                    return balanceIdentities;
                }
                
                while (MvxSock.More(ref sid))
                {
                    DateTime bestBeforeDate = StaticVariables.ParseExactStringToDate(MvxSock.GetField(ref sid, "PRDT"), 
                                                                            "yyyyMMdd", 
                                                                            System.Globalization.CultureInfo.InvariantCulture);
                    int onHandBalance = Convert.ToInt32(MvxSock.GetField(ref sid, "STQT"));
                    int allocatable = Convert.ToInt16(MvxSock.GetField(ref sid, "ALOC"));
                    int allocatedQuantity = Convert.ToInt32(MvxSock.GetField(ref sid, "ALQT"));
                    string allokplats = MvxSock.GetField(ref sid, "WHSL");

                    if(allokplats != "IRMA" && allocatable == 1)
                    {
                        balanceIdentities.Add(new BalanceId
                        {
                            ItemNumber = m3ProdNumber,
                            Warehouse = warehouse,
                            OnHandBalance = onHandBalance,
                            AllocatedQuantity = allocatedQuantity,
                            BestBeforeDate = bestBeforeDate
                        });
                    }
                    //Moves to next row
                    MvxSock.Access(ref sid, null);
                }
                MvxSock.Close(ref sid);
            }
            return balanceIdentities;
        }
        
        internal List<PromotionInfo> GetPromotionForAllCustomerAsListv2(string itno, int year)
        {
            List<PromotionInfo> promotionList = new List<PromotionInfo>();
            if (year > 2000)
            { 
                promotionList.AddRange(GetPromotionForAllCustomerByDiv(itno, year, "310"));
                promotionList.AddRange(GetPromotionForAllCustomerByDiv(itno, year, "320"));
                promotionList.AddRange(GetPromotionForAllCustomerByDiv(itno, year, "710"));
                promotionList.AddRange(GetPromotionForAllCustomerByDiv(itno, year, "720"));
            }           
            else if (year < 2000)
            {
                promotionList.AddRange(GetPromotionForAllCustomerByDiv(itno, DateTime.Now.Year, "310"));
                promotionList.AddRange(GetPromotionForAllCustomerByDiv(itno, DateTime.Now.Year, "320"));
                promotionList.AddRange(GetPromotionForAllCustomerByDiv(itno, DateTime.Now.Year, "710"));
                promotionList.AddRange(GetPromotionForAllCustomerByDiv(itno, DateTime.Now.Year, "720"));
                if (StaticVariables.GetForecastWeeknumberForDate(DateTime.Now) > 32)
                {
                    promotionList.AddRange(GetPromotionForAllCustomerByDiv(itno, DateTime.Now.Year + 1, "310"));
                    promotionList.AddRange(GetPromotionForAllCustomerByDiv(itno, DateTime.Now.Year + 1, "320"));
                    promotionList.AddRange(GetPromotionForAllCustomerByDiv(itno, DateTime.Now.Year + 1, "710"));
                    promotionList.AddRange(GetPromotionForAllCustomerByDiv(itno, DateTime.Now.Year + 1, "720"));
                }
                else if (StaticVariables.GetForecastWeeknumberForDate(DateTime.Now) < 21)
                {
                    promotionList.AddRange(GetPromotionForAllCustomerByDiv(itno, DateTime.Now.Year - 1, "310"));
                    promotionList.AddRange(GetPromotionForAllCustomerByDiv(itno, DateTime.Now.Year - 1, "320"));
                    promotionList.AddRange(GetPromotionForAllCustomerByDiv(itno, DateTime.Now.Year - 1, "710"));
                    promotionList.AddRange(GetPromotionForAllCustomerByDiv(itno, DateTime.Now.Year - 1, "720"));
                }
            }

            return promotionList;
        }

        internal List<PromotionInfo> GetPromotionForAllCustomerByDiv(string itno, int year, string division)
        {
            List<PromotionInfo> answerList = new List<PromotionInfo>();
            SERVER_ID sid = new SERVER_ID();

            uint rc = 0;
            rc = ConnectToM3Interface(ref sid, "OIS840MI");
            if (rc != 0)
            {
                return null;
            }
            SetMaxList(sid, 2000);
            Console.WriteLine("Get Item campaign: " + itno);
            //Set the field without need to know position Start from this customer 00752
            MvxSock.SetField(ref sid, "YEAR", year.ToString());
            MvxSock.SetField(ref sid, "CONO", "001");
            MvxSock.SetField(ref sid, "DIVI", division);
            MvxSock.SetField(ref sid, "ITNO", itno);
            rc = MvxSock.Access(ref sid, "LstPromItem");
            if (rc != 0)
            {
                MvxSock.ShowLastError(ref sid, "Error in get products no " + rc + "\n");
                MvxSock.Close(ref sid);
                return null;
            }

            while (MvxSock.More(ref sid))
            {
                string promotionId = MvxSock.GetField(ref sid, "PIDE");

                string tempstartDate = MvxSock.GetField(ref sid, "FRD2");
                string tempEndDate = MvxSock.GetField(ref sid, "TOD2");
                string quantity = MvxSock.GetField(ref sid, "BUQT");
                string[] temp = quantity.Split('.');
                int Antal = Convert.ToInt32(temp[0]);
                Console.WriteLine(" start: " + tempstartDate + " end: " + tempEndDate + " antal: " + quantity);
                MvxSock.Access(ref sid, null);
                try { 
                    DateTime tempDate = StaticVariables.ParseExactStringToDate(tempstartDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    int yearForDate1 = tempDate.Year;
                    //Kampanjer ska presenteras en vecka innan kampanjstart
                    tempDate = tempDate.AddDays(-7);
                    int yearForDate2 = tempDate.Year;

                    int yeardiff = yearForDate1 - yearForDate2;
                    int weekInt = StaticVariables.GetWeek2(tempDate);
                    if (weekInt > 0 && weekInt < 54)
                    {
                        answerList.Add(new PromotionInfo()
                        {
                            Week = weekInt,
                            Quantity = Antal,
                            ItemNumber = itno,
                            Id = promotionId,
                            Division = division,
                            Year = year - yeardiff
                        });
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    MessageBox.Show("Kan inte visa kampanj " + promotionId + " på division " + division);
                }
            }
            MvxSock.Close(ref sid);

            foreach(PromotionInfo promotion in answerList)
            {
                rc = ConnectToM3Interface(ref sid, "OIS840MI");
                if (rc != 0)
                {
                    return null;
                }
                SetMaxList(sid, 2000);

                MvxSock.SetField(ref sid, "CONO", "001");
                MvxSock.SetField(ref sid, "DIVI", division);
                MvxSock.SetField(ref sid, "PIDE", promotion.Id);
                rc = MvxSock.Access(ref sid, "GetPromHead");
                if (rc == 0)
                {
                    string customer = MvxSock.GetField(ref sid, "CUNO");
                    string chain = MvxSock.GetField(ref sid, "CHAI");
                    string description = MvxSock.GetField(ref sid, "TX15");
                    if (customer.Trim().Length > 0)
                    {
                        promotion.Customer = customer;
                    }
                    else
                    {
                        promotion.Chain = chain;
                    }
                    promotion.Description = description;
                }
                MvxSock.Close(ref sid);
            }

            Console.WriteLine("M3 communication: SUCCESS!!");
            return answerList;
        }

        internal Dictionary<string, List<PurchaseOrderLine>> GetKopsorderFromM3(string Warehouse, string itemNumber, int year, Dictionary<string, int> unitToNBR)
        {
            Stopwatch stopwatch2 = Stopwatch.StartNew();
            List<PurchaseOrderLine> poLineList = new List<PurchaseOrderLine>();
            List<PurchaseOrderLine> expPoLineList = new List<PurchaseOrderLine>();
            SERVER_ID sid = new SERVER_ID();
            //Dictionary<int, int> kampagn_TY = new Dictionary<int, int>();

            uint rc = 0;
            rc = ConnectToM3Interface(ref sid, "MDBREADMI");
            Console.WriteLine("Time5aaa: " + stopwatch2.ElapsedMilliseconds);
            if (rc != 0)
            {
                return null;
            }
            SetMaxList(sid, 2000);
            Console.WriteLine("Get Item Kopsorder: " + itemNumber);
            MvxSock.SetField(ref sid, "WHLO", Warehouse);
            MvxSock.SetField(ref sid, "ITNO", itemNumber);
            rc = MvxSock.Access(ref sid, "LstMPLINEV2");
            Console.WriteLine("Time5bbb: " + stopwatch2.ElapsedMilliseconds);
            if (rc != 0)
            {
                MvxSock.ShowLastError(ref sid, "Error in get Kopsorder no " + rc + "\n");
                MvxSock.Close(ref sid);
                return null;
            }

            Console.WriteLine("Time5ccc: " + stopwatch2.ElapsedMilliseconds);
            while (MvxSock.More(ref sid))
            {
                string miWarehouse = MvxSock.GetField(ref sid, "WHLO");
                //Om annat lagerställe returneras, bryt while-loop
                if (!miWarehouse.Equals(Warehouse))
                {
                    break;
                }
                string orderNumber = MvxSock.GetField(ref sid, "PUNO");
                string rcdt = MvxSock.GetField(ref sid, "RCDT");
                string receivedQuantity = MvxSock.GetField(ref sid, "RVQA");
                string pldt = MvxSock.GetField(ref sid, "PLDT");
                string orderQuantity = MvxSock.GetField(ref sid, "ORQA");

                string confirmedQuantity = MvxSock.GetField(ref sid, "CFQA");
                string quantity = "0.0";
                string expectedQuantity = "0.0";
                string plannedDate = "", confirmedDate = "";

                string purchaseUnit = MvxSock.GetField(ref sid, "PUUN");
                bool isReceived = false;
                if (rcdt.Length > 3 || pldt.Length > 3)
                {
                    // Hämta Önskad/bekräftad kvantitet och datum
                    if(confirmedQuantity.StartsWith("0.0"))
                    {
                        expectedQuantity = orderQuantity;
                    }
                    else
                    {
                        expectedQuantity = confirmedQuantity;
                    }   
                    plannedDate = pldt;
                    if(!rcdt.Equals("0"))
                    {
                        //Mottagen
                        quantity = receivedQuantity;
                        confirmedDate = rcdt;
                        isReceived = true;
                    }

                    DateTime tempDate = StaticVariables.ParseExactStringToDate(plannedDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

                    if (plannedDate.Substring(0, 4).Equals(year.ToString()) ||
                        year < 2000 && StaticVariables.DateWithinNowForecastLimit(tempDate))
                    {                        
                        //Räkna om till grundenhet ST 
                        int nbrPerUnit = 1; 
                        if(unitToNBR.ContainsKey("ANT"))
                        {
                            nbrPerUnit = unitToNBR["ANT"];
                        }
                        else if (unitToNBR.ContainsKey(purchaseUnit))
                        { 
                            nbrPerUnit = unitToNBR[purchaseUnit];
                        }
                        string[] temp = quantity.Split('.');
                        int quantityReceived = Convert.ToInt32(temp[0]) * nbrPerUnit;

                        temp = expectedQuantity.Split('.');
                        int quantiyExpected = Convert.ToInt32(temp[0]) * nbrPerUnit;
                        Console.WriteLine("Datum: " + rcdt + " antal: " + quantity);

                        tempDate = StaticVariables.ParseExactStringToDate(plannedDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                        tempDate = tempDate.AddMinutes(10);
                        int weekInt = StaticVariables.GetWeek2(tempDate);

                        if (weekInt > 0 && weekInt < 54)
                        {
                            var poExpLine = new PurchaseOrderLine()
                            {
                                PONumber = orderNumber,
                                Quantity = quantiyExpected,
                                Date = tempDate.Date,
                                Week = weekInt,
                                ItemNumber = itemNumber,
                                Warehouse = miWarehouse
                            };
                            expPoLineList.Add(poExpLine);

                            if (isReceived && confirmedDate.Length > 7)
                            {
                                tempDate = StaticVariables.ParseExactStringToDate(confirmedDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                bool nowAndWithLimit = (year < 2000 && StaticVariables.DateWithinNowForecastLimit(tempDate));
                                if (confirmedDate.Substring(0, 4).Equals(year.ToString()) || (nowAndWithLimit))
                                {
                                    var poLine = new PurchaseOrderLine()
                                    {
                                        PONumber = orderNumber,
                                        Quantity = quantityReceived,
                                        Date = tempDate.Date,
                                        Week = weekInt,
                                        ItemNumber = itemNumber,
                                        Warehouse = miWarehouse
                                    };
                                    poLineList.Add(poLine);
                                }
                            }
                            
                        }
                    }
                }
                MvxSock.Access(ref sid, null);
            }
            Console.WriteLine("Time5ddd: " + stopwatch2.ElapsedMilliseconds);
            MvxSock.Close(ref sid);

            Console.WriteLine("M3 communication: SUCCESS!!");
            Dictionary<string, List<PurchaseOrderLine>> returnDictionary = new Dictionary<string, List<PurchaseOrderLine>>();
            returnDictionary.Add("receivedLines", poLineList);
            returnDictionary.Add("expectedLines", expPoLineList);
            return returnDictionary;
        }

        internal Dictionary<string, int> GetUnitToNBR(string itemNumber)
        {
            Dictionary<string, int> answerDict = new Dictionary<string, int>();
            SERVER_ID sid = new SERVER_ID();
            //Dictionary<int, int> kampagn_TY = new Dictionary<int, int>();

            uint rc = 0;
            rc = ConnectToM3Interface(ref sid, "MMS200MI");
            if (rc != 0)
            {
                return null;
            }
            SetMaxList(sid, 2000);
            Console.WriteLine("Get UInt to nbr: " + itemNumber);
            //Set the field without need to know position Start from this customer 00752
            //MvxSock.SetField(ref sid, "YEAR", year.ToString());
            MvxSock.SetField(ref sid, "CONO", "001");
            MvxSock.SetField(ref sid, "FITN", itemNumber);
            MvxSock.SetField(ref sid, "TITN", itemNumber);
            MvxSock.SetField(ref sid, "FAUT", "1");
            MvxSock.SetField(ref sid, "TAUT", "1");

            rc = MvxSock.Access(ref sid, "LstItmAltUnitMs");
            if (rc != 0)
            {
                MvxSock.ShowLastError(ref sid, "Error in get Kopsorder no " + rc + "\n");
                MvxSock.Close(ref sid);
                return null;
            }

            while (MvxSock.More(ref sid))
            {
                string nameOfUnit = MvxSock.GetField(ref sid, "ALUN");
                string tempAntal = MvxSock.GetField(ref sid, "COFA");
                string[] temp = tempAntal.Split('.');
                int Antal = Convert.ToInt32(temp[0]);
                Console.WriteLine("NAme Of unit: " + nameOfUnit + " Antal: " + tempAntal);

                if (!answerDict.ContainsKey(nameOfUnit))
                {
                    answerDict.Add(nameOfUnit, Antal);
                }
                MvxSock.Access(ref sid, null);
            }

            MvxSock.Close(ref sid);

            Console.WriteLine("M3 communication: SUCCESS!!");
            return answerDict;
        }
    }
}
