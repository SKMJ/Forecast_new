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
    public partial class ScrollToProdNumber : Form
    {
        private FormSales formSalesInstanceCaller;
        private bool callerSales = false;
        public ScrollToProdNumber(FormSales formSalesInstance, bool calledFromSales)
        {
            callerSales = calledFromSales;
            InitializeComponent();
            textBoxNumberSearch.Focus();
            this.Focus();
            formSalesInstanceCaller = formSalesInstance;
        }

        private void textBoxNumberSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                string temp = sender.GetType().ToString();
                if(temp.Contains("TextBox"))
                {
                    dynamic tempVar = sender;
                    string inputString = tempVar.Text;
                    Console.WriteLine(sender.ToString() + sender.GetType());
                    ScrollToRowWithNumber(inputString);
                    this.Close();
                }
                //if (e.GetType() == System.Windows.Forms.TextBox)
                    
            }
            Console.WriteLine(e.GetType());
        }

        private void ScrollToRowWithNumber(string inputString)
        {
            if(callerSales)
            {
                formSalesInstanceCaller.ScrollToNumber(inputString);
            }
            else
            {
                formSalesInstanceCaller.supplyViewInstance.ScrollToNumber(inputString);
            }            
        }

        private void textBoxNumberSearch_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
