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
    class GetFromM3
    {
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
                    string tempItemNbr = MvxSock.GetField(ref sid, "ITNO") + "\t\t";
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


        public Dictionary<string, string> GetItemInfoByItemNumber(string itemNbr)
        {
            //string returnString = "";
            Dictionary<string, string> returnStrings = new Dictionary<string, string>();
            {
                //SERVER_ID sid = new SERVER_ID();
                //uint rc;
                //rc = ConnectToM3Interface(ref sid, "MMS200MI");
                //if (rc != 0)
                //{
                //    return returnStrings;
                //}
                ////Set the field without need to know position Start from this customer 00752
                //MvxSock.SetField(ref sid, "ITNO", itemNbr);
                //MvxSock.SetField(ref sid, "CONO", "001");
                //rc = MvxSock.Access(ref sid, "GetItmBasic");
                //if (rc != 0)
                //{
                //    //MvxSock.ShowLastError(ref sid, "Error in get Name by productsNbr: " + rc + "\n");
                //    MvxSock.Close(ref sid);
                //    return returnStrings;
                //}
                ////Division, Prep location och Whs location
                //string div  = MvxSock.GetField(ref sid, "Division");
                //string prepLocation = MvxSock.GetField(ref sid, "Prep Location");
                //string whsLocation = MvxSock.GetField(ref sid, "Whs Location");

                string div  = "hittepåDiv";
                string prepLocation = "hittepåPrep";
                string whsLocation = "hittepåWHS";

                returnStrings.Add("div", div);
                returnStrings.Add("prepLocation", prepLocation);
                returnStrings.Add("whsLocation", whsLocation);

 
                //Console.WriteLine("ProductNBR: " + itemNbr + " Name: " + returnString);
                //MvxSock.Close(ref sid);
                return returnStrings;
            }
        }


        //public string GetItemNameByItemNumber(string itemNbr)
        //{
        //    string returnString = "";
        //    {
        //        SERVER_ID sid = new SERVER_ID();
        //        uint rc;
        //        rc = ConnectToM3Interface(ref sid, "MMS200MI");
        //        if (rc != 0)
        //        {
        //            return returnString;
        //        }
        //        //Set the field without need to know position Start from this customer 00752
        //        MvxSock.SetField(ref sid, "ITNO", itemNbr);
        //        MvxSock.SetField(ref sid, "CONO", "001");
        //        rc = MvxSock.Access(ref sid, "GetItmBasic");
        //        if (rc != 0)
        //        {
        //            //MvxSock.ShowLastError(ref sid, "Error in get Name by productsNbr: " + rc + "\n");
        //            MvxSock.Close(ref sid);
        //            return returnString;
        //        }
        //        returnString = MvxSock.GetField(ref sid, "ITDS");
        //        Console.WriteLine("ProductNBR: " + itemNbr + " Name: " + returnString);
        //        MvxSock.Close(ref sid);
        //        return returnString;
        //    }
        //}


        //public List<int> GetListOfSuppliers()
        //{
        //    List<int> supplierList = new List<int>();
        //    {
        //        SERVER_ID sid = new SERVER_ID();
        //        uint rc;
        //        rc = ConnectToM3Interface(ref sid, "CRS111MI");
        //        //rc = MvxSock.Connect(ref sid, ipNummer, portNumber, userName, userPsw, "CRS111MI", null);
        //        if (rc != 0)
        //        {
        //            //MvxSock.ShowLastError(ref sid, "Error no " + rc + "\n");
        //            return null;
        //        }
        //        SetMaxList(sid, 299);
        //        //Set the field without need to know position Start from this customer 00752
        //        //MvxSock.SetField(ref sid, "ASCD", Assortment);
        //        MvxSock.SetField(ref sid, "CONO", "001");
        //        rc = MvxSock.Access(ref sid, "LstSuppliers");
        //        if (rc != 0)
        //        {
        //            MvxSock.ShowLastError(ref sid, "Error in get suppliers no " + rc + "\n");
        //            MvxSock.Close(ref sid);
        //            return null;
        //        }

        //        while (MvxSock.More(ref sid))
        //        {
        //            string tempSupplierNbr = MvxSock.GetField(ref sid, "SUNO") + "\t\t";
        //            if (Convert.ToInt32(tempSupplierNbr) > 99)
        //            {
        //                //Console.Write("Supplier nr: " + tempSupplierNBR);
        //                //Console.WriteLine("Kedja: " + MvxSock.GetField(ref sid, "ASCD"));
        //                supplierList.Add(Convert.ToInt32(tempSupplierNbr));
        //            }
        //            //Mooves to next row
        //            MvxSock.Access(ref sid, null);
        //        }

        //        MvxSock.Close(ref sid);
        //        return supplierList;
        //    }
        //}


        //public List<string> GetListOfProductsBySupplier(string supplNbr)
        //{
        //    List<string> itemList = new List<string>();
        //    //Dictionary<string, string> itemDict = new Dictionary<string, string>();
        //    {

        //        SERVER_ID sid = new SERVER_ID();

        //        uint rc;
        //        rc = ConnectToM3Interface(ref sid, "MDBREADMI");
        //        if (rc != 0)
        //        {
        //            //MvxSock.ShowLastError(ref sid, "Error no " + rc + "\n");
        //            return null;
        //        }

        //        SetMaxList(sid, 299);

        //        //Set the field without need to know position Start from this customer 00752
        //        MvxSock.SetField(ref sid, "SUNO", supplNbr);
        //        rc = MvxSock.Access(ref sid, "LstMITVEN10");
        //        if (rc != 0)
        //        {
        //            MvxSock.ShowLastError(ref sid, "Error in get suppliers items " + rc + "\n");
        //            MvxSock.Close(ref sid);
        //            return null;
        //        }

        //        while (MvxSock.More(ref sid))
        //        {
        //            string tempItemNbr = MvxSock.GetField(ref sid, "ITNO") ;
        //            string tempSuno = MvxSock.GetField(ref sid, "SUNO") ;
        //            //Console.WriteLine("XX Supplyer item Nbr: " + tempItemNbr + " YY Supplyer item Nbr: " + tempSuno + "Suppl NBR: " + supplNbr);
        //            if (tempItemNbr.Length > 1 && tempSuno.Equals(supplNbr))
        //            {
        //                //Console.Write("Supplier nr: " + tempSupplierNBR);
        //                //Console.WriteLine("Kedja: " + MvxSock.GetField(ref sid, "ASCD"));
        //                Console.WriteLine("M3 Listed Supplyer item Nbr: " + tempItemNbr + " Suppl NBR: " + supplNbr);
        //                itemList.Add(tempItemNbr);
        //            }
        //            //Mooves to next row
        //            MvxSock.Access(ref sid, null);
        //        }

        //        MvxSock.Close(ref sid);
        //        return itemList;
        //    }
        //}


        public string GetSupplierNameByNumber(int supplierNbr)
        {
            string returnString = "";
            {
                SERVER_ID sid = new SERVER_ID();

                uint rc;
                rc = ConnectToM3Interface(ref sid, "CRS620MI");
                //rc = MvxSock.Connect(ref sid, ipNummer, portNumber, userName, userPsw, "CRS620MI", null);
                if (rc != 0)
                {
                    //MvxSock.ShowLastError(ref sid, "Error no " + rc + "\n");
                    return returnString;
                }

                //Set the field without need to know position Start from this customer 00752
                MvxSock.SetField(ref sid, "SUNO", supplierNbr.ToString());
                MvxSock.SetField(ref sid, "CONO", "001");
                rc = MvxSock.Access(ref sid, "GetBasicData");
                if (rc != 0)
                {
                    MvxSock.ShowLastError(ref sid, "Error in get Supplier by number " + rc + "\n");
                    MvxSock.Close(ref sid);
                    return returnString;
                }

                //string tempItemNBR = MvxSock.GetField(ref sid, "ITNO") + "\t\t";
                returnString = MvxSock.GetField(ref sid, "SUNM");
                Console.WriteLine("SupplierNBR: " + supplierNbr + " Name: " + returnString);
                MvxSock.Close(ref sid);
                return returnString;

            }
        }


        private uint ConnectToM3Interface(ref SERVER_ID sid, string m3Interface)
        {
            //Console.WriteLine("Before Connect to M3 Interface: " + M3Interface);
            uint rc = 0;
            try
            {
                rc = MvxSock.Connect(ref sid, ipNummer, portNumber, userName, userPsw, m3Interface, null);
                //Console.WriteLine("After Connect to M3 Interface: " + M3Interface);
            }
            catch (Exception ex)
            {
                MessageBox.Show("fail M3 communication Connect! 2: MI: " + m3Interface);
                Console.WriteLine("Exception in M3 connection: " + ex.Message);
                return rc;
            }
            if (rc != 0)
            {
                MessageBox.Show("Not Connected to M3!! communication Fail! Application will not work!");
                MvxSock.ShowLastError(ref sid, "Error no " + rc + "\n");
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
        internal string GetNewOrderCode()
        {
            //Catrin helps to get this new code
            string returnString = "";
            uint rc;
            SERVER_ID sid = new SERVER_ID();

            rc = ConnectToM3Interface(ref sid, "PPS370MI");
            //rc = MvxSock.Connect(ref sid, ipNummer, portNumber, userName, userPsw, "CRS620MI", null);
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
            Console.WriteLine("Message Number: " + returnString );
            MvxSock.Close(ref sid);
            return returnString;
        }


        //Creates order purposal line.
        //Added line to ol order if same MSGN
        internal string CreateNewOrderProposal(string MSGN, string ITNO, int ordQuant, string SUNO, string Date)
        {
            string returnString = "";
            uint rc;
            SERVER_ID sid = new SERVER_ID();

            rc = ConnectToM3Interface(ref sid, "PPS370MI");
            //rc = MvxSock.Connect(ref sid, ipNummer, portNumber, userName, userPsw, "CRS620MI", null);
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

        internal Dictionary<string, string> GetAllSkaevingeProductsDict()
        {
            //Catrin helps to get this new code
            Dictionary<string, string> dictItems = new Dictionary<string, string>();
            dictSupplier = new Dictionary<string, List<string>>();
            {
                SERVER_ID sid = new SERVER_ID();
                uint rc;
                rc = ConnectToM3Interface(ref sid, "MMS200MI");
                //rc = MvxSock.Connect(ref sid, ipNummer, portNumber, userName, userPsw, "CRS111MI", null);
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
                    if(ClassStaticVaribles.NewNumberDictM3Key.ContainsKey(itemNBR))
                    {
                        itemNBR = ClassStaticVaribles.NewNumberDictM3Key[itemNBR];
                    }

                    string itemName = MvxSock.GetField(ref sid, "ITDS");// + "\t\t";
                    string itemSupplier = MvxSock.GetField(ref sid, "SUNO");// + "\t\t";
                    if (itemSupplier.Length > 0 && Convert.ToInt32(itemSupplier) > 99)
                    {
                        //Console.Write("Supplier nr: " + tempSupplierNBR);
                        //Console.WriteLine("Kedja: " + MvxSock.GetField(ref sid, "ASCD"));
                        //supplierList.Add(Convert.ToInt32(tempSupplierNbr));
                        dictItems.Add(itemNBR, itemName);
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

        internal Dictionary <string, List<string>> GetSupplierWithItemsDict()
        {
            ClassStaticVaribles.SetAllSuppliersM3(dictSupplier);
            return dictSupplier;
        }
    }
}
