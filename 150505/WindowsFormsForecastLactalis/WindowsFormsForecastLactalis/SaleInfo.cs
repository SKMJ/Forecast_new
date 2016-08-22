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
        public SaleInfo()
        {
            InitializeComponent();
        }

        public void LoadSaleInfo(List<ISalesRow> salesRowList, int week)
        {
            string currentCustomer = "-";
            int totalQuantity = 0;
            var salesRows = from row in salesRowList
                            where row.Week == week
                            orderby row.CustomerName
                            select row;
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("");

            foreach (ISalesRow row in salesRows)
            {
                string date = "";
                string orderNumber = "";
                int quantity = 0;
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
                    customer = m3row.Customer;
                    bbd = "" + StaticVariables.ReturnDanishFormat(m3row.BestBeforeDate.ToString());
                    wantedBbd = "" + StaticVariables.ReturnDanishFormat(m3row.WantedBestBeforeDate.ToString());
                }
                else
                {
                    date = StaticVariables.ReturnDanishFormat(row.Date.ToShortDateString());
                    quantity = row.Quantity;
                }
                System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode(String.Format("{0,-15}\t{1,-10}\t{2,-13}\t{3,-9}\t{5,-14}\t{4,-10}",
                                                                                                            date,
                                                                                                            customer,
                                                                                                            orderNumber,
                                                                                                            quantity,
                                                                                                            bbd,
                                                                                                            wantedBbd));
                if (!currentCustomer.Equals(row.CustomerName))
                {
                    treeNode1.Text = treeNode1.Text + " - " + totalQuantity + " st";
                    totalQuantity = 0;
                    treeNode1 = new System.Windows.Forms.TreeNode(String.Format("{0}", row.CustomerName));
                    treeView1.Nodes.Add(treeNode1);
                    treeNode1.Nodes.Add(new System.Windows.Forms.TreeNode(String.Format("{0,-15}\t{1,-10}\t{2,-13}\t{3,-9}\t{5,-14}\t{4,-10}",
                                                                                            "Dato",
                                                                                            "Kund",
                                                                                            "Order",
                                                                                            "Antal",
                                                                                            "BBD",
                                                                                            "Ønsket BBD")));
                    currentCustomer = row.CustomerName;
                }
                treeNode1.Nodes.Add(treeNode2);
                totalQuantity = totalQuantity + quantity;
            }
            treeNode1.Text = treeNode1.Text + " - " + totalQuantity + " st";

        }
    }
}
