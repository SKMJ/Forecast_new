using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsForecastLactalis
{
    public partial class SaleInfo : Form
    {
        public SaleInfo(string text)
        {
            InitializeComponent();
            this.Text = text;
        }

        public void LoadSaleInfo(List<ISalesRow> salesRowList, int week, int completelyZeroed, bool supplyView)        
        {
            int allaKundersNollade = 0;
            bool orderedInBoxes = false;
            string currentCustomer = "-";
            int totalQuantity = 0;
            int lastNollade = 0;
            var salesRows = from row in salesRowList
                            where row.Week == week
                            orderby row.CustomerName
                            select row;
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("");

            Dictionary<string, int> sumOrdered = new Dictionary<string, int>();
            Dictionary<string, int> sumDelivered = new Dictionary<string, int>();
            bool noRealiserat = true;
            foreach (ISalesRow row in salesRows)
            {
                noRealiserat = false;
                string date = "";
                string orderNumber = "";
                int quantity = 0;
                int quantityOrdered = 0;
                int quantityDiff = 0;
                int quantityLeft = 0;
                string customer = "";
                string bbd = "";
                string wantedBbd = "";
                if (row is SalesRow)
                {
                    SalesRow m3row = (SalesRow)row;
                    //StaticVariables.ReturnDanishFormat()
                    date = StaticVariables.ReturnDanishFormat(m3row.Date.ToShortDateString());
                    orderNumber = m3row.OrderNumber;
                    quantity = m3row.Quantity;
                    quantityOrdered = m3row.QuantityOrdered;
                    quantityDiff = quantityOrdered - quantity;

                    
                    customer = m3row.Customer;
                    bbd = "" + StaticVariables.ReturnDanishFormat(m3row.BestBeforeDate.ToString());
                    wantedBbd = "" + StaticVariables.ReturnDanishFormat(m3row.WantedBestBeforeDate.ToString());
                    if (quantityDiff < 0)
                    {
                        orderedInBoxes = true;
                    }

                    //if new customer clear the old sums
                    if (!currentCustomer.Equals(row.CustomerName))
                    {
                        sumOrdered = new Dictionary<string, int>();
                        sumDelivered = new Dictionary<string, int>();
                    }

                    if (quantityDiff > 0)
                    {
                        if (!sumOrdered.ContainsKey(orderNumber))
                        {
                            sumOrdered.Add(orderNumber, quantityOrdered);
                        }
                        if (!sumDelivered.ContainsKey(orderNumber))
                        {
                            sumDelivered.Add(orderNumber, quantity);
                        }
                        else
                        {
                            sumDelivered[orderNumber] = sumDelivered[orderNumber] + quantity;
                        }
                        Console.WriteLine("q: " + quantity + "zero: " + quantityDiff);
                        quantityLeft = quantityOrdered - sumDelivered[orderNumber];
                    }
                    else
                    {
                        quantityLeft = quantityDiff;
                    }
                }
                else
                {
                    date = StaticVariables.ReturnDanishFormat(row.Date.ToShortDateString());
                    quantity = row.Quantity;
                }
                System.Windows.Forms.TreeNode treeNode2;
                if (!orderedInBoxes)
                {
                    treeNode2 = new System.Windows.Forms.TreeNode(String.Format("{0,-15}\t{1,-10}\t{2,-13}\t{3,-9}\t{5,-14}\t{4,-15}\t{6,-15}",
                                                                                                                date,
                                                                                                                customer,
                                                                                                                orderNumber,
                                                                                                                quantity,
                                                                                                                bbd,
                                                                                                                wantedBbd,
                                                                                                                quantityOrdered));
                }
                else
                {
                    treeNode2 = new System.Windows.Forms.TreeNode(String.Format("{0,-15}\t{1,-10}\t{2,-13}\t{3,-9}\t{5,-14}\t{4,-15}",
                                                                                                                                date,
                                                                                                                                customer,
                                                                                                                                orderNumber,
                                                                                                                                quantity,
                                                                                                                                bbd,
                                                                                                                                wantedBbd));


                }

                //Om vi kommit till en ny kund sätt rubrik-raden för den 
                if (!currentCustomer.Equals(row.CustomerName))
                {
                    treeNode1.Text = treeNode1.Text + " - " + "Realiserat: " + totalQuantity + " st  ";// +" DebugNBR: " + orderNBR;
                    //if (!orderedInBoxes && !supplyView)
                    //{
                    //    treeNode1.Text = treeNode1.Text + "Nollat: " + lastNollade + " st  ";
                    //    allaKundersNollade += lastNollade;
                    //}
                    totalQuantity = 0;
                    treeNode1 = new System.Windows.Forms.TreeNode(String.Format("{0}", row.CustomerName));
                    treeView1.Nodes.Add(treeNode1);
                    treeNode1.Nodes.Add(new System.Windows.Forms.TreeNode(String.Format("{0,-15}\t{1,-10}\t{2,-13}\t{3,-9}\t{5,-14}\t{4,-15}\t{6,-15}",
                                                                                            "Dato",
                                                                                            "Kund",
                                                                                            "Order",
                                                                                            "Antal",
                                                                                            "BBD",
                                                                                            "Ønsket BBD",
                                                                                            "AntalOrdered")));
                    currentCustomer = row.CustomerName;
                }
                treeNode1.Nodes.Add(treeNode2);
                totalQuantity = totalQuantity + quantity;

                int zero = 0;
                foreach (KeyValuePair<string, int> item in sumDelivered)
                {
                    int diffrence = sumOrdered[item.Key] - item.Value;
                    if (diffrence > 0)
                    {
                        zero += diffrence;
                    }

                }

                lastNollade = zero;
            }
            int zeroed = 0;
            foreach (KeyValuePair<string, int> item in sumDelivered)
            {
                int diffrence = sumOrdered[item.Key] - item.Value;
                if (diffrence > 0)
                {
                    zeroed += diffrence;
                }

            }
            if(noRealiserat)
            {
                treeNode1 = new System.Windows.Forms.TreeNode(String.Format("{0}", " "));
                treeView1.Nodes.Add(treeNode1);

            }
            treeNode1.Text = treeNode1.Text + " - " +"Realiserat: " + totalQuantity + " st.  ";
            //if (!orderedInBoxes && !supplyView)
            //{

            //    zeroed += completelyZeroed;

            //    treeNode1.Text = treeNode1.Text + "Nollat: " + zeroed + " st ";
            //}
            //if (!orderedInBoxes && supplyView)
            //{
            //    allaKundersNollade += completelyZeroed;
            //    treeNode1 = new System.Windows.Forms.TreeNode(String.Format("{0}", "Total Nollat (Alla Kunder) : " + allaKundersNollade));
            //    treeView1.Nodes.Add(treeNode1);
            //    treeNode1.Nodes.Add(new System.Windows.Forms.TreeNode(String.Format("Nollat Alla Kunder: " + allaKundersNollade)));
            //}
        }

        private void SaleInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
