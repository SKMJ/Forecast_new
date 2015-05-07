using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lawson.M3.MvxSock;
using System.Windows.Forms;

namespace WindowsFormsForecastLactalis
{
    class GetFromM3
    {
        bool firstTime = true;
        Dictionary<int, string> productName = new Dictionary<int, string>();
        public List<int> GetListOfProductsNbrByAssortment(string Assortment)
        {
            List<int> productList = new List<int>();

            {

                SERVER_ID sid = new SERVER_ID();

                uint rc = 0;
                try
                {
                    rc = MvxSock.Connect(ref sid, "172.31.157.25", 16205, "mi310", "MIPGM99", "CRS105MI", null);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("fail M3 communication! 1");
                    Console.WriteLine("fail M3 communication: " + ex);
                    return productList;
                }


                if (rc != 0)
                {
                    MvxSock.ShowLastError(ref sid, "Error no " + rc + "\n");
                    return null;
                }

                //Set the field without need to know position Start from this customer 00752
                MvxSock.SetField(ref sid, "ASCD", Assortment);
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
                    string tempItemNBR = MvxSock.GetField(ref sid, "ITNO") + "\t\t";
                    Console.Write("Item nr: " + tempItemNBR);
                    Console.WriteLine("Kedja: " + MvxSock.GetField(ref sid, "ASCD"));
                    //Mooves to next row
                    MvxSock.Access(ref sid, null);

                    productList.Add(Convert.ToInt32(tempItemNBR));
                }

                MvxSock.Close(ref sid);

                Console.WriteLine("M3 communication: SUCCESS!!");
                return productList;
            }
        }




        public string GetNameByItemNumber(int itemNbr)
        {
            string returnString = "";
            List<int> productList = new List<int>();
            {
                SERVER_ID sid = new SERVER_ID();
                uint rc;
                try
                {
                    rc = MvxSock.Connect(ref sid, "172.31.157.25", 16205, "mi310", "MIPGM99", "MMS200MI", null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("fail M3 communication! 2");
                    return returnString;
                }
                if (rc != 0)
                {
                    MvxSock.ShowLastError(ref sid, "Error no " + rc + "\n");
                    return returnString;
                }

                //Set the field without need to know position Start from this customer 00752
                MvxSock.SetField(ref sid, "ITNO", itemNbr.ToString());
                MvxSock.SetField(ref sid, "CONO", "001");
                rc = MvxSock.Access(ref sid, "GetItmBasic");
                if (rc != 0)
                {
                    MvxSock.ShowLastError(ref sid, "Error in get products no " + rc + "\n");
                    MvxSock.Close(ref sid);
                    return returnString;
                }

                //string tempItemNBR = MvxSock.GetField(ref sid, "ITNO") + "\t\t";
                returnString = MvxSock.GetField(ref sid, "ITDS");
                Console.WriteLine("ProductNBR: " + itemNbr + " Name: " + returnString);
                MvxSock.Close(ref sid);
                return returnString;
            }
        }

    }
}
