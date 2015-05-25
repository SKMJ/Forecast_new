﻿using System;
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

        /// <summary>
        /// This function is just to test M3 Connection
        /// </summary>
        /// <returns></returns>
        public bool TestM3Connection()
        {
            //List<int> productList = new List<int>();

            SERVER_ID sid = new SERVER_ID();

            uint rc = 0;
            var watch = Stopwatch.StartNew();       
            try
            {
                rc = MvxSock.Connect(ref sid, ipNummer, portNumber, userName, userPsw, "CRS105MI", null);
                //Console.WriteLine("After Connect to M3 Interface: " + M3Interface);
            }
            catch (Exception ex)
            {
                MessageBox.Show("fail M3 communication Connect!");
                Console.WriteLine("Eception in M3: " + ex.Message);
                return false;
            }
            if (rc != 0)
            {
                MvxSock.ShowLastError(ref sid, "Error in M3 communication: " + rc + "\n");
                MessageBox.Show("fail M3 communication Connect!");
                return false;
            }            
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("time connect: " + elapsedMs);
            if (rc != 0)
            {
                Console.WriteLine("M3 communication: FAIL!!");
                return false;
            }

            //Set the field without need to know position Start from this customer 00752
            MvxSock.SetField(ref sid, "ASCD", "COOP");
            MvxSock.SetField(ref sid, "CONO", "001");
            rc = MvxSock.Access(ref sid, "LstAssmItem");
            if (rc != 0)
            {
                MessageBox.Show("M3 Communication Fail! ");
                //MvxSock.ShowLastError(ref sid, "M3 Communication Fail! " + "\n");
                MvxSock.Close(ref sid);
                Console.WriteLine("M3 communication: FAIL!!");
                return false;
            }

            bool once = true;
            while (MvxSock.More(ref sid) && once)
            {
                string tempItemNbr = MvxSock.GetField(ref sid, "ITNO") + "\t\t";
                Console.Write("Item nr: " + tempItemNbr);
                Console.WriteLine("Kedja: " + MvxSock.GetField(ref sid, "ASCD"));
                //Mooves to next row
                MvxSock.Access(ref sid, null);
                once = false;
            }
            MvxSock.Close(ref sid);
            Console.WriteLine("M3 communication: SUCCESS!!");
            return true;
        }


        public List<int> GetListOfProductsNbrByAssortment(string assortment)
        {
            List<int> productList = new List<int>();
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

                    productList.Add(Convert.ToInt32(tempItemNbr));
                
                }
                
                MvxSock.Close(ref sid);

                Console.WriteLine("M3 communication: SUCCESS!!");
                return productList;
            }
        }


        public string GetItemNameByItemNumber(string itemNbr)
        {
            string returnString = "";
            {
                SERVER_ID sid = new SERVER_ID();
                uint rc;
                rc = ConnectToM3Interface(ref sid, "MMS200MI");
                if (rc != 0)
                {
                    return returnString;
                }
                //Set the field without need to know position Start from this customer 00752
                MvxSock.SetField(ref sid, "ITNO", itemNbr);
                MvxSock.SetField(ref sid, "CONO", "001");
                rc = MvxSock.Access(ref sid, "GetItmBasic");
                if (rc != 0)
                {
                    //MvxSock.ShowLastError(ref sid, "Error in get Name by productsNbr: " + rc + "\n");
                    MvxSock.Close(ref sid);
                    return returnString;
                }
                returnString = MvxSock.GetField(ref sid, "ITDS");
                Console.WriteLine("ProductNBR: " + itemNbr + " Name: " + returnString);
                MvxSock.Close(ref sid);
                return returnString;
            }
        }


        public List<int> GetListOfSuppliers()
        {
            List<int> supplierList = new List<int>();
            {
                SERVER_ID sid = new SERVER_ID();
                uint rc;
                rc = ConnectToM3Interface(ref sid, "CRS111MI");
                //rc = MvxSock.Connect(ref sid, ipNummer, portNumber, userName, userPsw, "CRS111MI", null);
                if (rc != 0)
                {
                    //MvxSock.ShowLastError(ref sid, "Error no " + rc + "\n");
                    return null;
                }
                SetMaxList(sid, 99);
                //Set the field without need to know position Start from this customer 00752
                //MvxSock.SetField(ref sid, "ASCD", Assortment);
                MvxSock.SetField(ref sid, "CONO", "001");
                rc = MvxSock.Access(ref sid, "LstSuppliers");
                if (rc != 0)
                {
                    MvxSock.ShowLastError(ref sid, "Error in get suppliers no " + rc + "\n");
                    MvxSock.Close(ref sid);
                    return null;
                }

                while (MvxSock.More(ref sid))
                {
                    string tempSupplierNbr = MvxSock.GetField(ref sid, "SUNO") + "\t\t";
                    if (Convert.ToInt32(tempSupplierNbr) > 99)
                    {
                        //Console.Write("Supplier nr: " + tempSupplierNBR);
                        //Console.WriteLine("Kedja: " + MvxSock.GetField(ref sid, "ASCD"));
                        supplierList.Add(Convert.ToInt32(tempSupplierNbr));
                    }
                    //Mooves to next row
                    MvxSock.Access(ref sid, null);
                }

                MvxSock.Close(ref sid);
                return supplierList;
            }
        }


        public List<string> GetListOfProductsBySupplier(string supplNbr)
        {
            List<string> itemList = new List<string>();
            //Dictionary<string, string> itemDict = new Dictionary<string, string>();
            {

                SERVER_ID sid = new SERVER_ID();

                uint rc;
                rc = ConnectToM3Interface(ref sid, "MDBREADMI");
                if (rc != 0)
                {
                    //MvxSock.ShowLastError(ref sid, "Error no " + rc + "\n");
                    return null;
                }

                SetMaxList(sid, 99);

                //Set the field without need to know position Start from this customer 00752
                MvxSock.SetField(ref sid, "SUNO", supplNbr);
                rc = MvxSock.Access(ref sid, "LstMITVEN10");
                if (rc != 0)
                {
                    MvxSock.ShowLastError(ref sid, "Error in get suppliers items " + rc + "\n");
                    MvxSock.Close(ref sid);
                    return null;
                }

                while (MvxSock.More(ref sid))
                {
                    string tempItemNbr = MvxSock.GetField(ref sid, "ITNO") ;
                    string tempSuno = MvxSock.GetField(ref sid, "SUNO") ;
                    //Console.WriteLine("XX Supplyer item Nbr: " + tempItemNbr + " YY Supplyer item Nbr: " + tempSuno + "Suppl NBR: " + supplNbr);
                    if (tempItemNbr.Length > 1 && tempSuno.Equals(supplNbr))
                    {
                        //Console.Write("Supplier nr: " + tempSupplierNBR);
                        //Console.WriteLine("Kedja: " + MvxSock.GetField(ref sid, "ASCD"));
                        Console.WriteLine("Listed Supplyer item Nbr: " + tempItemNbr + " Suppl NBR: " + supplNbr);
                        itemList.Add(tempItemNbr);
                    }
                    //Mooves to next row
                    MvxSock.Access(ref sid, null);
                }

                MvxSock.Close(ref sid);
                return itemList;
            }
        }


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
    }
}
