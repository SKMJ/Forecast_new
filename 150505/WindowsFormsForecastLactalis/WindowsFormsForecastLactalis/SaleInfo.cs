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
            //System.Windows.Forms.TreeNodeCollection nodeCollection = new System.Windows.Forms.TreeNodeCollection()
            bool first = true;
            //treeView1.Nodes.Add(treeNode1);
            foreach (ISalesRow row in salesRows)
            {
                string date = "";
                string orderNumber = "";
                int quantity = 0;
                string customer = "";
                string bbd = "";
                if (row is SalesRow)
                {
                    SalesRow m3row = (SalesRow)row;
                    date = m3row.Date.ToShortDateString();
                    orderNumber = m3row.OrderNumber;
                    quantity = m3row.Quantity;
                    customer = m3row.Customer;
                    bbd = "" + m3row.BestBeforeDate;
                }
                else
                {
                    date = row.Date.ToShortDateString();
                    quantity = row.Quantity;
                }
                /*if(first)
                {
                    first = false;
                    treeNode1 = new TreeNode(String.Format("{0}", customer));
                }*/
                System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode(String.Format("{0,-30}\t{1,-30}\t{2,-30}\t{3,-30}\t{4,-30}",
                                                                                                            date,
                                                                                                            customer,
                                                                                                            orderNumber,
                                                                                                            quantity,
                                                                                                            bbd));
                if (!currentCustomer.Equals(row.CustomerName))
                {
                    treeNode1.Text = treeNode1.Text + " - " + totalQuantity + " st";
                    totalQuantity = 0;
                    treeNode1 = new System.Windows.Forms.TreeNode(String.Format("{0}", row.CustomerName));
                    treeView1.Nodes.Add(treeNode1);
                    treeNode1.Nodes.Add(new System.Windows.Forms.TreeNode(String.Format("{0,-30}\t{1,-30}\t{2,-30}\t{3,-30}\t{4,-30}",
                                                                                            "Dato" + "                                   ",
                                                                                            "Kund" + "                               ",
                                                                                            "Order" + "                              ",
                                                                                            "Antal" + "                              ",
                                                                                            "BBD")));
                    currentCustomer = row.CustomerName;
                }
                //System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Order     BBD       Kvantitet");
                treeNode1.Nodes.Add(treeNode2);
                totalQuantity = totalQuantity + quantity;
                /*
                 System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Order     BBD       Kvantitet");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("123       160431    10");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("456       160416    16");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("65123", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("COOP SE - 26 st", new System.Windows.Forms.TreeNode[] {
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Order     BBD       Kvantitet");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("113       160431    11");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("173       160430    7");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Arla - 18 st", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8});
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
                */
                /*message = message + String.Format("{0, -20}\t{1, -20}\t{1, -20}\t{2, 8:N0}{3}",
                    date,
                    orderNumber,
                    quantity,
                    Environment.NewLine);
                    */
            }
            treeNode1.Text = treeNode1.Text + " - " + totalQuantity + " st";

        }
    }
}
