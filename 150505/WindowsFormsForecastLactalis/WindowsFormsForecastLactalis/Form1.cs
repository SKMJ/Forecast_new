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
    public partial class Form1 : Form
    {
        public static List<PrognosInfo> Products = new List<PrognosInfo>();
        public FormSupply SupplyViewInstance;
        private List<string> Assortments =new List<string>();
        GetFromM3 m3_info = new GetFromM3();
        //Get_FromSimulatedM3 m3_info = new Get_FromSimulatedM3();
        public Form1()
        {
            InitializeComponent();
            Assortments.Add("COOP");
            Assortments.Add("HITTEPÅ");
            comboBoxAssortment.DataSource = Assortments;

        }


        private void dataGridForecastInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string tempSender = sender.ToString();
            string TempE = e.ToString();
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            Console.WriteLine("Value clicked... Column index: " + columnIndex + "  rowIndex: " + rowIndex);
        }
        private void FillInfo()
        {
            foreach (PrognosInfo item in Products)
            {

                dataGridForecastInfo.Rows.Add(item.ProductNumber, item.ProductName, "RealiseretKampagn_LastYear", item.RealiseretKampagn_LastYear[1], item.RealiseretKampagn_LastYear[2], item.RealiseretKampagn_LastYear[3], item.RealiseretKampagn_LastYear[4], item.RealiseretKampagn_LastYear[5], item.RealiseretKampagn_LastYear[6], 0);
                dataGridForecastInfo.Rows.Add("", "", "RealiseretSalgsbudget_LastYear", item.RealiseretSalgsbudget_LastYear[1], item.RealiseretSalgsbudget_LastYear[2], item.RealiseretSalgsbudget_LastYear[3], item.RealiseretSalgsbudget_LastYear[4], item.RealiseretSalgsbudget_LastYear[5], item.RealiseretSalgsbudget_LastYear[6]);

                dataGridForecastInfo.Rows.Add("", "", "Realiserat_ThisYear", item.Realiserat_ThisYear[1], item.Realiserat_ThisYear[2], item.Realiserat_ThisYear[3], item.Realiserat_ThisYear[4], item.Realiserat_ThisYear[5], item.Realiserat_ThisYear[6]);
                dataGridForecastInfo.Rows.Add("", "", "Kampagn_ThisYear", item.Kampagn_ThisYear[1], item.Kampagn_ThisYear[2], item.Kampagn_ThisYear[3], item.Kampagn_ThisYear[4], item.Kampagn_ThisYear[5], item.Kampagn_ThisYear[6]);
                dataGridForecastInfo.Rows.Add("", "", "Salgsbudget_ThisYear", item.Salgsbudget_ThisYear[1], item.Salgsbudget_ThisYear[2], item.Salgsbudget_ThisYear[3], item.Salgsbudget_ThisYear[4], item.Salgsbudget_ThisYear[5], item.Salgsbudget_ThisYear[6]);
            }
            //Thread.Sleep(2000);
            foreach (DataGridViewRow row in dataGridForecastInfo.Rows)
            {
                row.Height = 35;
                if (Convert.ToString(row.Cells[2].Value) == "RealiseretKampagn_LastYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Blue;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "RealiseretSalgsbudget_LastYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Blue;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Kampagn_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Salgsbudget_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);


                }
            }
            int colNBR = 0;
            foreach (DataGridViewColumn col in dataGridForecastInfo.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                if (colNBR < 2)
                {
                    col.DefaultCellStyle.ForeColor = Color.Black;
                    col.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
                    col.DefaultCellStyle.ForeColor = Color.Black;

                }
                colNBR++;
            }
        }

        private void CreateProducts()
        {
            //GetFromM3 m3_info = new GetFromM3();
            //List<int> productList = m3_info.GetListOfProductsNbrByAssortment("COOP");


            PrognosInfo product1 = new PrognosInfo("Brie 400gr", 1);
            PrognosInfo product2 = new PrognosInfo("Mozarellabollar", 2);
            PrognosInfo product3 = new PrognosInfo("Prästost", 3);
            PrognosInfo product4 = new PrognosInfo("MögelFranskost", 4);
            PrognosInfo product5 = new PrognosInfo("Limhamns Specialost", 5);
            product1.FillNumbers();
            product2.FillNumbers();
            product3.FillNumbers();
            product4.FillNumbers();
            product5.FillNumbers();
            Products.Add(product1);
            Products.Add(product2);
            Products.Add(product3);
            Products.Add(product4);
            Products.Add(product5);
            //Products[0] = product1;
            //Products[1] = product2;
            //Products[2] = product3;
            //Products[3] = product4;
            //Products[4] = product5;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            sender.ToString();

            e.ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void buttonSupplyView_Click(object sender, EventArgs e)
        {
            Console.WriteLine("User press Supply View");
            SupplyViewInstance = new FormSupply();
            Point tempLocation = this.Location;
            SupplyViewInstance.Location = this.Location;

            this.Visible = false;


            SupplyViewInstance.SetForm1Instanse(this);
            SupplyViewInstance.BringToFront();

            SupplyViewInstance.Show();
            SupplyViewInstance.Location = tempLocation;

            

            
        }

        private void comboBoxAssortment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.dataGridForecastInfo.DataSource = null;

            this.dataGridForecastInfo.Rows.Clear();

            

            if (comboBoxAssortment.SelectedItem.ToString() != "COOP")
            {
                Console.WriteLine("Hittepåkund vald");
                Products = new List<PrognosInfo>();
                CreateProducts();

                FillInfo();
            }
            else
            {
                Console.WriteLine("COOP vald");
                Products = new List<PrognosInfo>();
                
                List<int> productList = m3_info.GetListOfProductsNbrByAssortment("COOP");

                foreach (int item in productList)
                {
                    string temp = m3_info.GetNameByItemNumber(item);
                    PrognosInfo product1 = new PrognosInfo(temp, item);
                    product1.FillNumbers();
                    Products.Add(product1);

                    

                }
                FillInfo();

                


            }
        }

       
    }
}
