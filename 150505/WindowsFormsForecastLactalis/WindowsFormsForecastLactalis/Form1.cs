﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsForecastLactalis
{
    public partial class Form1 : Form
    {
        private static List<PrognosInfoSales> Products = new List<PrognosInfoSales>();
        public FormSupply supplyViewInstance;
        private List<string> assortments = new List<string>();
        GetFromM3 m3Info = new GetFromM3();
        TextBoxForm infoboxSales = new TextBoxForm();
        private string latestProductNumber;
        private int latestWeek;
        Point latestMouseClick;

        //Get_FromSimulatedM3 m3_info = new Get_FromSimulatedM3();
        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("Start Form1!");
            //test with Coop and test customer
            FixCustomerChoices();

            m3Info.TestM3Connection();
            SetupColumns();

        }

        //Populate the drop downlist wit customers
        private void FixCustomerChoices()
        {
            assortments.Add("COOP");
            assortments.Add("TEST CUSTOMER");
            comboBoxAssortment.DataSource = assortments;
            comboBoxAssortment.DropDownStyle = ComboBoxStyle.DropDownList;
        }


        private void dataGridForecastInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //This takes a list of strings and add a row column by column
        private void AddRowFromList(List<object> stringList)
        {
            object[] array = stringList.ToArray();
            dataGridForecastInfo.Rows.Add(array);

        }

        //Name the columns in the grid
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

        //Fill the grid with info from the Products
        public void FillInfo()
        {
            this.dataGridForecastInfo.DataSource = null;

            this.dataGridForecastInfo.Rows.Clear();

            this.dataGridForecastInfo.AllowUserToAddRows = false;
            this.dataGridForecastInfo.AllowUserToDeleteRows = false;
            this.dataGridForecastInfo.AllowUserToOrderColumns = false;

            foreach (PrognosInfoSales item in Products)
            {
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
                tempList.Add("Salgsbudget_LastYear");
                for (int i = 1; i < 53; i++)
                {
                    tempList.Add(item.Salgsbudget_LastYear[i]);
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

                tempList = new List<object>();
                tempList.Add("");
                tempList.Add("");
                tempList.Add("Salgsbudget_ChangeHistory");
                for (int i = 1; i < 53; i++)
                {
                    tempList.Add(item.Salgsbudget_ChangeHistory[i]);
                }
                AddRowFromList(tempList);
            }
            //Thread.Sleep(2000);

            //After all is filled set colors and reaqdonly properties
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
                else if (Convert.ToString(row.Cells[2].Value) == "Salgsbudget_LastYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.Blue;
                    row.ReadOnly = true;
                }
                else if (Convert.ToString(row.Cells[2].Value) == "Kampagn_ThisYear")
                {
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
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
                    col.DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);
                    col.DefaultCellStyle.ForeColor = Color.Black;
                }
                if (colNBR < 3)
                {
                    col.ReadOnly = true;
                }
                colNBR++;
            }
        }

        //Add testinfo Products
        private void CreateProducts()
        {
            PrognosInfoSales product1 = new PrognosInfoSales("GALBANI MOZZARELLA MAXI 200 G", 2432, 1);
            PrognosInfoSales product2 = new PrognosInfoSales("RONDELE M./VALNØD 125 G", 1442, 1);
            PrognosInfoSales product3 = new PrognosInfoSales("RONDELE M./HAVSALT 125 g", 1443, 1);
            PrognosInfoSales product4 = new PrognosInfoSales("RONDELE BLEU 125 G", 1447, 1);
            PrognosInfoSales product5 = new PrognosInfoSales("IGOR BLUE PORTION, 200 G", 2239, 1);

            product1.FillNumbers(2432);
            product2.FillNumbers(1442);
            product3.FillNumbers(1443);
            product4.FillNumbers(1447);
            product5.FillNumbers(2239);

            Products.Add(product1);
            Products.Add(product2);
            Products.Add(product3);
            Products.Add(product4);
            Products.Add(product5);
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //sender.ToString();

            //e.ToString();
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        //Change to supply view
        private void buttonSupplyView_Click(object sender, EventArgs e)
        {
            Console.WriteLine("User press Supply View");
            if (!infoboxSales.Visible)
            {
                //if (Products.Count < 1)
                //{
                //    comboBoxAssortment.SelectedItem = "TEST CUSTOMER";
                //    Console.WriteLine("Fill with Fake customer before switch to supply");
                //    Products = new List<PrognosInfo>();
                //    CreateProducts();

                //    FillInfo();
                //}
                //SupplyViewInstance = new FormSupply();
                Point tempLocation = this.Location;
                supplyViewInstance = new FormSupply(tempLocation);
                supplyViewInstance.Location = this.Location;

                this.Visible = false;
                supplyViewInstance.SetForm1Instanse(this);
                supplyViewInstance.BringToFront();
                supplyViewInstance.Show();
            }
            else
            {
                MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before changing view!");
            }
        }


        private void comboBoxAssortment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Load products by customer
        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            
            if (!infoboxSales.Visible)
            {
                if (comboBoxAssortment.SelectedItem.ToString() != "COOP")
                {
                    Console.WriteLine("Hittepåkund vald");
                    Products = new List<PrognosInfoSales>();
                    CreateProducts();

                    FillInfo();
                }
                else
                {

                    Console.WriteLine("COOP vald");
                    Products = new List<PrognosInfoSales>();
                    List<int> productList = m3Info.GetListOfProductsNbrByAssortment("COOP");
                    if (productList != null)
                    {
                        foreach (int item in productList)
                        {
                            string temp = m3Info.GetItemNameByItemNumber(item.ToString());
                            PrognosInfoSales product1 = new PrognosInfoSales(temp, item, 1);
                            product1.FillNumbers(2432);
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
            else
            {
                MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before loading new info!");
            }

            stopwatch.Stop();
            double timeConnectSeconds = stopwatch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Load all Customers Sales! Time (s): " + timeConnectSeconds);
        }

        //Set comment from outside this form (textbox)
        public void SetProductComment(string comment)
        {
            foreach (PrognosInfoSales item in Products)
            {
                if (item.ProductNumber.ToString() == latestProductNumber)
                {
                    item.Salgsbudget_Comment[latestWeek] = comment;
                    //TODO: Write to database
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


        //public PrognosInfoSales GetProductInfoByNumber(int productNbr)
        //{
        //    foreach (PrognosInfoSales item in Products)
        //    {
        //        if (item.ProductNumber == productNbr)
        //        {
        //            return item;
        //        }
        //    }
        //    return null;
        //}


        private void dataGridForecastInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            //Take care of History value and fill value when value changed
            if (rowIndex >= 0 && GetValueFromGridAsString(rowIndex, 2) == "Salgsbudget_ThisYear" && columnIndex > 2)//(rowIndex % 7 == 5 && columnIndex > 2) // 1 should be your column index
            {
                int i;
                string productNumber = GetProductNumberFromRow(rowIndex);//GetValueFromGridAsString(rowIndex - 5, 0);
                int week = columnIndex - 2;
                if (int.TryParse(GetValueFromGridAsString(rowIndex, columnIndex), out i))
                {
                    foreach (PrognosInfoSales item in Products)
                    {
                        if (item.ProductNumber.ToString() == productNumber)
                        {
                            //Check so value really is changed to new value
                            if (GetValueFromGridAsString(rowIndex, columnIndex) != GetLastNumber(item.Salgsbudget_ChangeHistory[week]))
                            {
                                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                                item.Salgsbudget_ChangeHistory[week] = item.Salgsbudget_ChangeHistory[week] + "  Changed by " + userName + " Date: " + DateTime.Now.ToShortDateString() + " To Value: " + GetValueFromGridAsString(rowIndex, columnIndex);
                            }
                            //TODO: Write to database
                        }
                    }
                }
                FillInfo();
            }
        }

        //Velidate that input is valid value
        private void dataGridForecastInfo_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            if (GetValueFromGridAsString(rowIndex, 2) == "Salgsbudget_ThisYear" && columnIndex > 2)//(rowIndex % 7 == 5 && columnIndex > 2) // 1 should be your column index
            {
                int i;
                string productNumber = GetProductNumberFromRow(rowIndex);//GetValueFromGridAsString(rowIndex - 5, 0);
                int week = columnIndex - 2;
                if (!int.TryParse(Convert.ToString(e.FormattedValue), out i))
                {
                    e.Cancel = true;
                    MessageBox.Show(new Form() { TopMost = true }, "Please enter numeric value. Product: " + productNumber + " Week: " + week);
                }
                else
                {
                    // the input is numeric 
                    int value = Convert.ToInt32(e.FormattedValue);
                    if (value < 0 || value > 1000000)
                    {
                        e.Cancel = true;
                        MessageBox.Show(new Form() { TopMost = true }, "Please enter a positive possible value. Product: " + productNumber + " Week: " + week);
                    }
                    else
                    {
                        foreach (PrognosInfoSales item in Products)
                        {
                            if (item.ProductNumber.ToString() == productNumber)
                            {
                                item.Salgsbudget_ThisYear[week] = Convert.ToInt32(e.FormattedValue);
                                //if (e.FormattedValue.ToString() != getLastNumber(item.Salgsbudget_ChangeHistory[week]))
                                //{
                                //    item.Salgsbudget_ChangeHistory[week] = item.Salgsbudget_ChangeHistory[week] + "  " + "Changed " + DateTime.Now.ToShortDateString() + " To: " + e.FormattedValue;
                                //}
                                //TODO: Write to database
                            }
                        }
                    }
                }
            }
        }

        //This function returns last number in a string
        private string GetLastNumber(string testString)
        {
            var matches = Regex.Matches(testString, @"\d+");
            if(matches.Count < 1)
            {
                return "No Number";
            }
            else
            {
                var lastMatch = matches[matches.Count - 1];
                var number = Int32.Parse(lastMatch.Value);
                //return testString.Remove(lastMatch.Index, lastMatch.Length).Insert(lastMatch.Index, number.ToString());
                return number.ToString();
            }
        }

        //returns the prooduct that the row is connected to
        private string GetProductNumberFromRow(int rowIndex)
        {
            string returnString = "";

            int row = rowIndex;
            while ((GetValueFromGridAsString(row, 0).Length < 1) && row >= 0)
            {
                row--;
            }
            returnString = GetValueFromGridAsString(row, 0);
            return returnString;
        }


        //Handles when user click in the grid. Different actions for different Cells. 
        private void dataGridForecastInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string tempSender = sender.ToString();
            string TempE = e.ToString();
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            latestMouseClick = System.Windows.Forms.Cursor.Position;
            Console.WriteLine("Value clicked... Column index: " + columnIndex + "  rowIndex: " + rowIndex);
            if (columnIndex > 2)
            {
                if (GetValueFromGridAsString(rowIndex, 2).Contains("Comment"))
                {
                    if (!infoboxSales.Visible)
                    {
                        string temp = GetValueFromGridAsString(rowIndex, columnIndex);
                        //string temp2 = GetValueFromGridAsString(rowIndex - 6, 0);


                        latestWeek = columnIndex - 2;
                        latestProductNumber = GetProductNumberFromRow(rowIndex);
                        infoboxSales.SetInfoText(this, temp, " Product: " + latestProductNumber + " Week: " + latestWeek);
                        infoboxSales.TopMost = true;

                        //First time it is showed needs special handling
                        if (infoboxSales.desiredStartLocationX == 0 && infoboxSales.desiredStartLocationY == 0)
                        {
                            infoboxSales.SetNextLocation(latestMouseClick);
                        }
                        else
                        {
                            infoboxSales.Location = latestMouseClick;
                        }
                        infoboxSales.Show();
                        infoboxSales.SaveButtonVisible(true);
                        infoboxSales.FocusTextBox();
                    }
                    else
                    {
                        MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before open a new one!");
                    }
                }
                else if (GetValueFromGridAsString(rowIndex, 2) == "Kampagn_ThisYear")
                {
                    if (!infoboxSales.Visible)
                    {
                        MessageBox.Show(GetProductNumberFromRow(rowIndex) + "\n\nKampagnen innefattar kund 1 och kund 2.");
                    }
                }
                else if (GetValueFromGridAsString(rowIndex, 2).Contains("History"))
                {
                    if (!infoboxSales.Visible)
                    {
                        string temp = GetValueFromGridAsString(rowIndex, columnIndex).Replace("Changed" , "\nChanged");
                        //string temp2 = GetValueFromGridAsString(rowIndex - 6, 0);


                        latestWeek = columnIndex - 2;
                        latestProductNumber = GetProductNumberFromRow(rowIndex);
                        infoboxSales.SetInfoText(this, temp, "Salgsbudget History Product: " + latestProductNumber + " Week: " + latestWeek);
                        infoboxSales.TopMost = true;
                        //First time it is showed needs special handling
                        if (infoboxSales.desiredStartLocationX == 0 && infoboxSales.desiredStartLocationY == 0)
                        {
                            infoboxSales.SetNextLocation(latestMouseClick);
                        }
                        else
                        {
                            infoboxSales.Location = latestMouseClick;
                        }
                        infoboxSales.Show();
                        infoboxSales.SaveButtonVisible(false);
                        infoboxSales.FocusTextBox();
                    }
                    else
                    {
                        MessageBox.Show(new Form() { TopMost = true }, "Close the open comment window before open a new one!");
                    }
                }
            }
        }

        //Return the value of a grid cell as string
        private string GetValueFromGridAsString(int row, int col)
        {
            string returnValue = "";

            if (row >= 0 && row < dataGridForecastInfo.RowCount && col >= 0 && col < dataGridForecastInfo.ColumnCount)
            {
                if (dataGridForecastInfo.Rows[row].Cells[col].Value != null)
                {
                    returnValue = dataGridForecastInfo.Rows[row].Cells[col].Value.ToString();
                }
            }
            else
            {
                Console.WriteLine("Sales, Value from grid out of bounds Row: " + row + " Col: " + col);
            }
            return returnValue;
        }



        public void SetKöpsbudget(int week, string productNumber, int value)
        {
            foreach (PrognosInfoSales item in Products)
            {
                if (item.ProductNumber.ToString() == productNumber)
                {
                    item.Salgsbudget_ThisYear[week] = Convert.ToInt32(value);
                }
            }
        }


    }
}
