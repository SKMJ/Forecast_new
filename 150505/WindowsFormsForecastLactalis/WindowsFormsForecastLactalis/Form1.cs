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
        private List<string> Assortments = new List<string>();
        GetFromM3 m3_info = new GetFromM3();
        TextBoxForm infobox = new TextBoxForm();
        private string latestProductNumber;
        private int latestWeek;
        Point latestMouseClick;

        //Get_FromSimulatedM3 m3_info = new Get_FromSimulatedM3();
        public Form1()
        {
            InitializeComponent();
            //test with Coop and test customer
            Assortments.Add("COOP");
            Assortments.Add("TEST CUSTOMER");
            comboBoxAssortment.DataSource = Assortments;
            m3_info.TestM3Connection();
            SetupColumns();

        }


        private void dataGridForecastInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string tempSender = sender.ToString();
            string TempE = e.ToString();
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            latestMouseClick = System.Windows.Forms.Cursor.Position;
            Console.WriteLine("Value clicked... Column index: " + columnIndex + "  rowIndex: " + rowIndex);
            if (columnIndex > 2)
            {
                if (rowIndex % 6 == 5 && columnIndex > 2)
                {
                    if (!infobox.Visible)
                    {
                        string temp = dataGridForecastInfo.Rows[rowIndex].Cells[columnIndex].Value.ToString();
                        string temp2 = dataGridForecastInfo.Rows[rowIndex - 5].Cells[0].Value.ToString();


                        latestWeek = columnIndex - 2;
                        latestProductNumber = temp2;
                        infobox.SetInfoText(this, temp, "Product: " + temp2 + " Week: " + latestWeek);
                        infobox.TopMost = true;

                        if (infobox.desiredStartLocationX == 0 && infobox.desiredStartLocationY == 0)
                        {
                            infobox.SetNextLocation(latestMouseClick);
                        }
                        else
                        {
                            infobox.Location = latestMouseClick;
                        }

                        infobox.Show();
                        infobox.FocusTextBox();
                    }
                    else
                    {
                        MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before open a new one!");


                    }
                    //MessageBox.Show(temp);
                }
                else if (rowIndex % 6 == 3)
                {
                    MessageBox.Show("Kampagnen innefattar kund 1 och kund 2.");
                }
            }
        }


        private void AddRowFromList(List<object> stringList)
        {
            object[] array = stringList.ToArray();
            dataGridForecastInfo.Rows.Add(array);

        }

        public void SetupColumns()
        {
            dataGridForecastInfo.ColumnCount = 55;
            dataGridForecastInfo.Columns[0].Name = "VareNr";
            dataGridForecastInfo.Columns[1].Name = "Beskrivelse";
            dataGridForecastInfo.Columns[2].Name = "Type";

            for (int i = 1; i < 53; i++)
            {
                string temp = i + ".2015";
                dataGridForecastInfo.Columns[i + 2].Name = temp;
            }
        }

        public void FillInfo()
        {
            this.dataGridForecastInfo.DataSource = null;

            this.dataGridForecastInfo.Rows.Clear();

            this.dataGridForecastInfo.AllowUserToAddRows = false;
            this.dataGridForecastInfo.AllowUserToDeleteRows = false;
            this.dataGridForecastInfo.AllowUserToOrderColumns = false;

            foreach (PrognosInfo item in Products)
            {
                //dataGridForecastInfo.Rows.Add( item.RealiseretKampagn_LastYear.Values.ToArray());
                //dataGridForecastInfo.Rows.Add( item.RealiseretSalgsbudget_LastYear.Values.ToArray());
                List<object> tempList = new List<object>();
                tempList.Add(item.ProductNumber.ToString());
                tempList.Add(item.ProductName);
                tempList.Add("RealiseretKampagn_LastYear");
                for (int i = 1; i < 53; i++)
                {
                    tempList.Add(item.RealiseretKampagn_LastYear[i]);
                }
                AddRowFromList(tempList);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("RealiseretSalgsbudget_LastYear");
                for (int i = 1; i < 53; i++)
                {
                    tempList.Add(item.RealiseretSalgsbudget_LastYear[i]);
                }
                AddRowFromList(tempList);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("Realiserat_ThisYear");
                for (int i = 1; i < 53; i++)
                {
                    tempList.Add(item.Realiserat_ThisYear[i]);
                }
                AddRowFromList(tempList);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("Kampagn_ThisYear");
                for (int i = 1; i < 53; i++)
                {
                    tempList.Add(item.Kampagn_ThisYear[i]);
                }
                AddRowFromList(tempList);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("Salgsbudget_ThisYear");
                for (int i = 1; i < 53; i++)
                {
                    tempList.Add(item.Salgsbudget_ThisYear[i]);
                }
                AddRowFromList(tempList);

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("Salgsbudget_Comment");
                for (int i = 1; i < 53; i++)
                {
                    tempList.Add(item.Salgsbudget_Comment[i]);
                }
                AddRowFromList(tempList);


                //dataGridForecastInfo.Rows.Add(item.ProductNumber, item.ProductName, "RealiseretKampagn_LastYear", item.RealiseretKampagn_LastYear[1], item.RealiseretKampagn_LastYear[2], item.RealiseretKampagn_LastYear[3], item.RealiseretKampagn_LastYear[4], item.RealiseretKampagn_LastYear[5], item.RealiseretKampagn_LastYear[6]);
                //dataGridForecastInfo.Rows.Add("", "", "RealiseretSalgsbudget_LastYear", item.RealiseretSalgsbudget_LastYear[1], item.RealiseretSalgsbudget_LastYear[2], item.RealiseretSalgsbudget_LastYear[3], item.RealiseretSalgsbudget_LastYear[4], item.RealiseretSalgsbudget_LastYear[5], item.RealiseretSalgsbudget_LastYear[6]);

                //dataGridForecastInfo.Rows.Add("", "", "Realiserat_ThisYear", item.Realiserat_ThisYear[1], item.Realiserat_ThisYear[2], item.Realiserat_ThisYear[3], item.Realiserat_ThisYear[4], item.Realiserat_ThisYear[5], item.Realiserat_ThisYear[6]);
                //dataGridForecastInfo.Rows.Add("", "", "Kampagn_ThisYear", item.Kampagn_ThisYear[1], item.Kampagn_ThisYear[2], item.Kampagn_ThisYear[3], item.Kampagn_ThisYear[4], item.Kampagn_ThisYear[5], item.Kampagn_ThisYear[6]);
                //dataGridForecastInfo.Rows.Add("", "", "Salgsbudget_ThisYear", item.Salgsbudget_ThisYear[1], item.Salgsbudget_ThisYear[2], item.Salgsbudget_ThisYear[3], item.Salgsbudget_ThisYear[4], item.Salgsbudget_ThisYear[5], item.Salgsbudget_ThisYear[6]);
                //dataGridForecastInfo.Rows.Add("", "", "Comments ", item.Salgsbudget_Comment[1], item.Salgsbudget_Comment[2], item.Salgsbudget_Comment[3], item.Salgsbudget_Comment[4], item.Salgsbudget_Comment[5], item.Salgsbudget_Comment[6]);

            }
            //Thread.Sleep(2000);
            foreach (DataGridViewRow row in dataGridForecastInfo.Rows)
            {
                row.Height = 35;
                if (Convert.ToString(row.Cells[2].Value) == "RealiseretKampagn_LastYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Blue;
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "RealiseretSalgsbudget_LastYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Blue;
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Kampagn_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Salgsbudget_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    row.ReadOnly = false;
                    row.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);


                }
                else if (Convert.ToString(row.Cells[2].Value).Contains("Commen"))
                {
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Realiserat_ThisYear")
                {
                    row.ReadOnly = true;
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

                if (colNBR < 3)
                {
                    col.ReadOnly = true;
                }
                colNBR++;
            }
        }

        private void CreateProducts()
        {
            //GetFromM3 m3_info = new GetFromM3();
            //List<int> productList = m3_info.GetListOfProductsNbrByAssortment("COOP");


            PrognosInfo product1 = new PrognosInfo("Brie 400gr", 1, 1);
            PrognosInfo product2 = new PrognosInfo("Mozarellabollar", 2, 1);
            PrognosInfo product3 = new PrognosInfo("Prästost", 3, 1);
            PrognosInfo product4 = new PrognosInfo("MögelFranskost", 4, 1);
            PrognosInfo product5 = new PrognosInfo("Limhamns Specialost", 5, 1);
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
            //SupplyViewInstance = new FormSupply();
            Point tempLocation = this.Location;
            SupplyViewInstance = new FormSupply(tempLocation);
            SupplyViewInstance.Location = this.Location;

            this.Visible = false;


            SupplyViewInstance.SetForm1Instanse(this);

            SupplyViewInstance.BringToFront();

            SupplyViewInstance.Show();

            //SupplyViewInstance.Location = tempLocation;





        }

        private void comboBoxAssortment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
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
                if (productList != null)
                {
                    foreach (int item in productList)
                    {
                        string temp = m3_info.GetItemNameByItemNumber(item.ToString());
                        PrognosInfo product1 = new PrognosInfo(temp, item, 2);
                        product1.FillNumbers();
                        Products.Add(product1);
                    }
                    FillInfo();
                }
                else
                {
                    MessageBox.Show("M3 communication fail");
                }
            }
        }

        public void SetProductComment(string comment)
        {
            foreach (PrognosInfo item in Products)
            {
                if (item.ProductNumber.ToString() == latestProductNumber)
                {

                    item.Salgsbudget_Comment[latestWeek] = comment;
                }
            }
            FillInfo();
        }

        private void buttonSupplyView_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void dataGridForecastInfo_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
        }

        private void dataGridForecastInfo_MouseClick(object sender, MouseEventArgs e)
        {
            latestMouseClick = System.Windows.Forms.Cursor.Position;

        }

        public PrognosInfo GetProductInfoByNumber(int productNbr)
        {
            foreach (PrognosInfo item in Products)
            {
                if (item.ProductNumber == productNbr)
                {
                    return item;
                }
            }
            return null;
        }


    }
}
