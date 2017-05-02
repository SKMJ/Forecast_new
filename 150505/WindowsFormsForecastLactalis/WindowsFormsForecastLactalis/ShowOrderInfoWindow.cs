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
    public partial class ShowOrderInfoWindow : Form
    {
        public ShowOrderInfoWindow()
        {
            InitializeComponent();
        }

        private void ShowOrderInfoWindow_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void SetText(string text)
        {
            this.richTextBox1.Text = text;
        }
    }
}
