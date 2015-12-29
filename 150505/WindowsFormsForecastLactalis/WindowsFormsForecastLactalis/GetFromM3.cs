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


        public Dictionary<int, int> GetCampaignsOfProducts(string itno, int year, string customer)
        {

            SERVER_ID sid = new SERVER_ID();
            Dictionary<int, int> kampagn_TY = new Dictionary<int, int>();

            uint rc = 0;
            rc = ConnectToM3Interface(ref sid, "OIS840MI");
            if (rc != 0)
            {
                return null;
            }
            Console.WriteLine("Get Item campaign: " + itno);
            SetMaxList(sid, 2000);
            //Set the field without need to know position Start from this customer 00752
            MvxSock.SetField(ref sid, "YEAR", "2015");
            MvxSock.SetField(ref sid, "CONO", "001");
            MvxSock.SetField(ref sid, "DIVI", "710");
            MvxSock.SetField(ref sid, "ITNO", itno);
            rc = MvxSock.Access(ref sid, "LstPromItem");
            if (rc != 0)
            {
                MvxSock.ShowLastError(ref sid, "Error in get products no " + rc + "\n");
                MvxSock.Close(ref sid);
                return null;
            }

            for (int i = 0; i < 54; i++)
            {
                kampagn_TY.Add(i, 0);
            }

            while (MvxSock.More(ref sid))
            {
                string tempstartDate = MvxSock.GetField(ref sid, "FVDT");
                string tempEndDate = MvxSock.GetField(ref sid, "LVDT");
                string tempAntal = MvxSock.GetField(ref sid, "BUQT");
                string[] temp = tempAntal.Split('.');
                int Antal = Convert.ToInt32(temp[0]);
                string tempCampCuse = MvxSock.GetField(ref sid, "CUSE");
                Console.WriteLine("Kedja: " + tempCampCuse + " start: " + tempstartDate + " end: " + tempEndDate + " antal: " + tempAntal);
                //Mooves to next row
                MvxSock.Access(ref sid, null);


                DateTime tempDate = DateTime.ParseExact(tempstartDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                tempDate = tempDate.AddMinutes(10);
                double weekStart = (tempDate - ClassStaticVaribles.StartDate[year]).TotalDays;
                double weekStartNBR = weekStart / 7.0;
                int weekStartInt = (int)Math.Floor(weekStartNBR);

                int weekInt = weekStartInt;
                int thisWeekday = (int)tempDate.DayOfWeek;
                int weekDay;
                // this code is (monday = 1, teuseday = 2,.... sunday=7
                // answer from day of week code is (sunday=7 = 0, monday = 1, teuseday = 2,.... 
                if (thisWeekday > 0) //if not sunday
                {
                    weekDay = thisWeekday;
                }
                else
                {
                    weekDay = 7; //sunday
                }
                if (weekDay > 5)
                {
                    weekInt = weekInt + 1;
                }

                if (weekInt > 0 && weekInt < 54)
                {
                    if (customer.Length == 0 || customer == tempCampCuse)
                    {
                        kampagn_TY[weekInt] = kampagn_TY[weekInt] + Antal;
                    }
                }
            }

            MvxSock.Close(ref sid);

            Console.WriteLine("M3 communication: SUCCESS!!");
            return kampagn_TY;

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
                    if (ClassStaticVaribles.AssortmentM3_toNav.ContainsKey(m3_kod))
                    {
                        Console.WriteLine("M3kod till NavKod: " + ClassStaticVaribles.AssortmentM3_toNav[m3_kod][0]);
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

                    ClassStaticVaribles.AssortmentM3_toKedjor.Add(m3_kod, temp);
                    returnStrings.Add(m3_kod, tx40);

                }

                MvxSock.Close(ref sid);

                Console.WriteLine("M3 communication: SUCCESS!!");

                ClassStaticVaribles.SetCustAssortmentListM3(returnStrings);
                return returnStrings;
            }
        }

        public void GetDictOfKedjaToCustNBR()
        {
            Dictionary<string, List<string>> returnStrings = new Dictionary<string, List<string>>();
            {
                Stopwatch stopwatch2 = Stopwatch.StartNew();
                SERVER_ID sid = new SERVER_ID();
                
                uint rc = 0;
                rc = ConnectToM3Interface(ref sid, "OIS038MI");
                
                if (rc != 0)
                {
                    return;
                }
                SetMaxList(sid, 20000);
                //Set the field without need to know position Start from this customer 00752
                //MvxSock.SetField(ref sid, "ASCD", assortment);
                MvxSock.SetField(ref sid, "CONO", "001");
                rc = MvxSock.Access(ref sid, "LstBusChainCust");
                if (rc != 0)
                {
                    MvxSock.ShowLastError(ref sid, "Error in get products no " + rc + "\n");
                    MvxSock.Close(ref sid);
                    return;
                }

                while (MvxSock.More(ref sid))
                {
                    string cuno = MvxSock.GetField(ref sid, "CUNO");
                    string chl = MvxSock.GetField(ref sid, "CHL4");
                    if(chl.Trim().Length < 1)
                        chl = MvxSock.GetField(ref sid, "CHL3");
                    if (chl.Trim().Length < 1)
                        chl = MvxSock.GetField(ref sid, "CHL2");
                    if (chl.Trim().Length < 1)
                        chl = MvxSock.GetField(ref sid, "CHL1");
                    Console.WriteLine("cuno: " + cuno + " chl1: " + chl);
                    if (returnStrings.ContainsKey(chl))
                    {
                        Console.WriteLine("kedja: " + chl + " existerar i returnStrings");
                        returnStrings[chl].Add(cuno);
                    }
                    else
                    {
                        Console.WriteLine("kedja: " + chl + " saknas i returnStrings");
                        List<string> cunoTemp = new List<string>();
                        cunoTemp.Add(cuno);
                        returnStrings.Add(chl, cunoTemp);
                    }
                    //Mooves to next row
                    MvxSock.Access(ref sid, null);
                }

                MvxSock.Close(ref sid);

                Console.WriteLine("M3 communication: SUCCESS!!");

                ClassStaticVaribles.SetKedjaToCunoListM3(returnStrings);
                Console.WriteLine("Antal Kedjor: " + returnStrings.Count);
                Console.WriteLine("Time KedjaToCuno: " + stopwatch2.ElapsedMilliseconds);
                //return returnStrings;
            }
        }



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
            if(ClassStaticVaribles.Production)
            { 
                ipNummer = "172.31.157.14";
                portNumber = 16105;
            }
            
            uint rc = 0;
            try
            {
                rc = MvxSock.Connect(ref sid, ipNummer, portNumber, userName, userPsw, m3Interface, null);
                //Console.WriteLine("After Connect to M3 Interface: " + M3Interface);
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
            Console.WriteLine("SetMaxList: " + maxInt);
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

                    //Lägg alltid in nummer från gamla navision om det existerar för att hålla Nav databasen samma
                    if (ClassStaticVaribles.NewNumberDictM3Key.ContainsKey(itemNBR))
                    {
                        itemNBR = ClassStaticVaribles.NewNumberDictM3Key[itemNBR];
                    }

                    string cutting = MvxSock.GetField(ref sid, "PUIT");// + "\t\t";
                    if (cutting.Contains("1") && !ClassStaticVaribles.AllCuttingProducts.Contains(itemNBR))
                    {
                        ClassStaticVaribles.AllCuttingProducts.Add(itemNBR);
                    }

                    string itemName = MvxSock.GetField(ref sid, "ITDS");// + "\t\t";


                    string itemSupplier = MvxSock.GetField(ref sid, "SUNO");// + "\t\t";
                    if (itemSupplier.Length > 0 && Convert.ToInt32(itemSupplier) > 99)
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
                    }
                    //Mooves to next row
                    MvxSock.Access(ref sid, null);

                }

                MvxSock.Close(ref sid);
                ClassStaticVaribles.SetAllProductsM3Dict(dictItems);
                return dictItems;
            }
        }

        public Dictionary<string, List<string>> GetSupplierWithItemsDict()
        {
            ClassStaticVaribles.SetAllSuppliersM3(dictSupplier);
            Dictionary<string, string> AllSuppliersNameDict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, List<string>> item in dictSupplier)
            {
                AllSuppliersNameDict.Add(item.Key, this.GetSupplierNameByNumber(item.Key));
            }
            ClassStaticVaribles.SetAllSuppliersNameDict(AllSuppliersNameDict);

            return dictSupplier;
        }



        public List<string> GetStockInfo(string m3ProdNumber)
        {
            //string returnString = "";
            List<string> returnStrings = new List<string>();
            Dictionary<string, int> dict = new Dictionary<string, int>();
            if (m3ProdNumber.Length > 0)
            {
                SERVER_ID sid = new SERVER_ID();
                uint rc;
                rc = ConnectToM3Interface(ref sid, "MMS060MI");
                if (rc != 0)
                {
                    return null;
                }
                ////Set the field without need to know position Start from this customer 00752
                MvxSock.SetField(ref sid, "ITNO", m3ProdNumber);
                MvxSock.SetField(ref sid, "CONO", "001");

                MvxSock.SetField(ref sid, "WHLO", "LSK");
                rc = MvxSock.Access(ref sid, "List");

                if (rc != 0)
                {
                    //MvxSock.ShowLastError(ref sid, "Error in get Stock info by productsNbr: " + rc + "\n");
                    Console.WriteLine("Error in get Stock info by productsNbr: " + rc + "\n");
                    //returnStrings.Add("In Stock: 0  Nothing in stock!");
                    MvxSock.Close(ref sid);
                    return null;
                }

                while (MvxSock.More(ref sid))
                {
                    string datum = MvxSock.GetField(ref sid, "PRDT");
                    string antal = MvxSock.GetField(ref sid, "STQT");
                    int allokerbar = Convert.ToInt16(MvxSock.GetField(ref sid, "ALOC"));
                    string antalAllokerade = MvxSock.GetField(ref sid, "ALQT");
                    string allokplats = MvxSock.GetField(ref sid, "WHSL");

                    Console.WriteLine(allokplats);
                    if (!(allokplats == "IRMA") && (allokerbar > 0))
                    {
                        if (dict.ContainsKey(datum))
                        {
                            dict[datum] = Convert.ToInt32(antal) + dict[datum] - Convert.ToInt32(antalAllokerade);
                        }
                        else
                        {
                            dict.Add(datum, Convert.ToInt32(antal) - Convert.ToInt32(antalAllokerade));
                        }
                        string temp = "In Stock: " + antal + "   Date: " + datum + "  NBR Allocated: " + antalAllokerade;

                        temp = temp + "\n";

                        //returnStrings.Add(temp);
                    }

                    //Mooves to next row
                    MvxSock.Access(ref sid, null);

                }

                int total = 0;
                foreach (KeyValuePair<string, int> item in dict)
                {
                    DateTime dt =
                   DateTime.ParseExact(item.Key, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    string temp = "In Stock: " + item.Value + "   Date: " + dt.Day + "-" + dt.Month + "-" + dt.Year;// +"  NBR Allocated: " + antalAllokerbara;
                    temp = temp + "\n";
                    total = item.Value + total;
                    returnStrings.Add(temp);
                }
                returnStrings.Add("Total: " + total);
            }

            else
            {
                returnStrings.Add("In Stock: 0  Empty in stock");
            }

            return returnStrings;
        }





        internal string GetKampagnWeekInfo(int latestWeek, string itno, int year)
        {
            Console.WriteLine("M3 Campain TRY GET ROWS");
            SERVER_ID sid = new SERVER_ID();
            Dictionary<int, int> kampagn_TY = new Dictionary<int, int>();

            string tempInfo = "";

            uint rc = 0;
            rc = ConnectToM3Interface(ref sid, "OIS840MI");
            if (rc != 0)
            {
                return null;
            }
            SetMaxList(sid, 2000);
            Console.WriteLine("Get Item campaign Info: " + itno);
            //Set the field without need to know position Start from this customer 00752
            MvxSock.SetField(ref sid, "YEAR", "2015");
            MvxSock.SetField(ref sid, "CONO", "001");
            MvxSock.SetField(ref sid, "DIVI", "710");
            MvxSock.SetField(ref sid, "ITNO", itno);
            rc = MvxSock.Access(ref sid, "LstPromItem");
            if (rc != 0)
            {
                MvxSock.ShowLastError(ref sid, "Error in get products no " + rc + "\n");
                MvxSock.Close(ref sid);
                return null;
            }

            for (int i = 0; i < 54; i++)
            {
                kampagn_TY.Add(i, 0);
            }

            while (MvxSock.More(ref sid))
            {
                string tempstartDate = MvxSock.GetField(ref sid, "FVDT");
                string tempEndDate = MvxSock.GetField(ref sid, "LVDT");
                string tempAntal = MvxSock.GetField(ref sid, "BUQT");
                string[] temp = tempAntal.Split('.');
                int Antal = Convert.ToInt32(temp[0]);
                string tempCustNBR = MvxSock.GetField(ref sid, "CUSE");
                Console.WriteLine("Kedja: " + tempCustNBR + " antal: " + tempAntal);
                //Mooves to next row
                MvxSock.Access(ref sid, null);


                DateTime tempDate = DateTime.ParseExact(tempstartDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                tempDate = tempDate.AddMinutes(10);
                double weekStart = (tempDate - ClassStaticVaribles.StartDate[year]).TotalDays;
                double weekStartNBR = weekStart / 7.0;
                int weekStartInt = (int)Math.Floor(weekStartNBR);

                int weekInt = weekStartInt;
                int thisWeekday = (int)tempDate.DayOfWeek;
                int weekDay;
                // this code is (monday = 1, teuseday = 2,.... sunday=7
                // answer from day of week code is (sunday=7 = 0, monday = 1, teuseday = 2,.... 
                if (thisWeekday > 0) //if not sunday
                {
                    weekDay = thisWeekday;
                }
                else
                {
                    weekDay = 7; //sunday
                }
                if (weekDay > 5)
                {
                    weekInt = weekInt + 1;
                }

                if (weekInt == latestWeek)
                {
                    if (ClassStaticVaribles.AssortmentDictionaryM3.ContainsKey(tempCustNBR))
                    {
                        tempInfo = tempInfo + "\n  " + tempCustNBR + "  " + ClassStaticVaribles.AssortmentDictionaryM3[tempCustNBR] + "  " + tempAntal;
                    }
                    else
                    {
                        tempInfo = tempInfo + "\n  " + tempCustNBR + "  " + tempAntal;
                    }
                }
            }

            MvxSock.Close(ref sid);

            Console.WriteLine("M3 Campain communication: SUCCESS!!");
            return tempInfo;
        }

        internal void GetKampainForAllCustomerAsList()
        {

        }

        internal List<KampaignInfo> GetKampainForAllCustomerAsListv2(string itno, int year)
        {
            List<KampaignInfo> answerList = new List<KampaignInfo>();
            SERVER_ID sid = new SERVER_ID();
            Dictionary<int, int> kampagn_TY = new Dictionary<int, int>();

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
            MvxSock.SetField(ref sid, "DIVI", "710");
            MvxSock.SetField(ref sid, "ITNO", itno);
            rc = MvxSock.Access(ref sid, "LstPromItem");
            if (rc != 0)
            {
                MvxSock.ShowLastError(ref sid, "Error in get products no " + rc + "\n");
                MvxSock.Close(ref sid);
                return null;
            }

            for (int i = 0; i < 54; i++)
            {
                kampagn_TY.Add(i, 0);
            }

            while (MvxSock.More(ref sid))
            {
                string tempstartDate = MvxSock.GetField(ref sid, "FVDT");
                string tempEndDate = MvxSock.GetField(ref sid, "LVDT");
                string tempAntal = MvxSock.GetField(ref sid, "BUQT");
                string[] temp = tempAntal.Split('.');
                int Antal = Convert.ToInt32(temp[0]);
                string tempCampCuse = MvxSock.GetField(ref sid, "CUSE");
                Console.WriteLine("Kedja: " + tempCampCuse + " start: " + tempstartDate + " end: " + tempEndDate + " antal: " + tempAntal);
                //Mooves to next row
                MvxSock.Access(ref sid, null);


                DateTime tempDate = DateTime.ParseExact(tempstartDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                tempDate = tempDate.AddMinutes(10);
                double weekStart = (tempDate - ClassStaticVaribles.StartDate[year]).TotalDays;
                double weekStartNBR = weekStart / 7.0;
                int weekStartInt = (int)Math.Floor(weekStartNBR);

                int weekInt = weekStartInt;
                int thisWeekday = (int)tempDate.DayOfWeek;
                int weekDay;
                // this code is (monday = 1, teuseday = 2,.... sunday=7
                // answer from day of week code is (sunday=7 = 0, monday = 1, teuseday = 2,.... 
                if (thisWeekday > 0) //if not sunday
                {
                    weekDay = thisWeekday;
                }
                else
                {
                    weekDay = 7; //sunday
                }
                if (weekDay > 5)
                {
                    weekInt = weekInt + 1;
                }

                if (weekInt > 0 && weekInt < 54)
                {
                    KampaignInfo tempKampagnInfo = new KampaignInfo(tempCampCuse, Antal, weekInt, itno);
                    answerList.Add(tempKampagnInfo);
                }
            }

            MvxSock.Close(ref sid);

            Console.WriteLine("M3 communication: SUCCESS!!");
            return answerList;
        }




        internal Dictionary<int, int> GetKopsorderFromM3(string Warehouse, string itemNumber, int year, Dictionary<string, int> unitToNBR)
        {
            Stopwatch stopwatch2 = Stopwatch.StartNew();
            if (itemNumber == "1095")
            {
                Console.WriteLine(itemNumber);
            }
            Dictionary<int, int> answerDict = new Dictionary<int, int>();
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
            //Set the field without need to know position Start from this customer 00752
            //MvxSock.SetField(ref sid, "YEAR", year.ToString());
            //MvxSock.SetField(ref sid, "CONO", "001");
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

            for (int i = 0; i < 54; i++)
            {
                answerDict.Add(i, 0);
            }
            Console.WriteLine("Time5ccc: " + stopwatch2.ElapsedMilliseconds);
            while (MvxSock.More(ref sid))
            {
                string orderDate = MvxSock.GetField(ref sid, "RCDT");

                string tempAntal = MvxSock.GetField(ref sid, "RVQA");
                string purchaseUnit = MvxSock.GetField(ref sid, "PUUN");
                if (orderDate.Length > 3)
                {
                    int nbrPerUnit = 1; 
                    if (unitToNBR.ContainsKey(purchaseUnit))
                    { 
                        nbrPerUnit = unitToNBR[purchaseUnit];
                    }
                    string[] temp = tempAntal.Split('.');
                    int Antal = Convert.ToInt32(temp[0]);
                    Console.WriteLine("Datum: " + orderDate + " antal: " + tempAntal);
                    //Mooves to next row



                    DateTime tempDate = DateTime.ParseExact(orderDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    tempDate = tempDate.AddMinutes(10);
                    double weekStart = (tempDate - ClassStaticVaribles.StartDate[year]).TotalDays;
                    double weekStartNBR = weekStart / 7.0;
                    int weekStartInt = (int)Math.Floor(weekStartNBR);

                    int weekInt = weekStartInt + 1;

                    // this code is (monday = 1, teuseday = 2,.... sunday=7


                    if (weekInt > 0 && weekInt < 54)
                    {
                        //mpaignInfo tempKampagnInfo = new KampaignInfo(tempCampCuse, Antal, weekInt, itno);
                        answerDict[weekInt] = answerDict[weekInt] + Antal * nbrPerUnit;
                    }
                }

                MvxSock.Access(ref sid, null);
            }
            Console.WriteLine("Time5ddd: " + stopwatch2.ElapsedMilliseconds);
            MvxSock.Close(ref sid);

            Console.WriteLine("M3 communication: SUCCESS!!");
            return answerDict;
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
