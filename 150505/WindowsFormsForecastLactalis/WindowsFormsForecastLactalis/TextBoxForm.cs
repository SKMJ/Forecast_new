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
    //This form is used to show and edit comments
    public partial class TextBoxForm : Form
    {
        Form1 Form1Instance;
        public int desiredStartLocationX;
        public int desiredStartLocationY;
        private string unchangedText;
        public TextBoxForm()
        {
            desiredStartLocationX = 0;
            desiredStartLocationY = 0;

            InitializeComponent();
            MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void Form2_Load(object sender, System.EventArgs e)
        {
            this.SetDesktopLocation(desiredStartLocationX, desiredStartLocationY);
        }

        private void TextBoxForm_Load(object sender, EventArgs e)
        {

        }

        public void SetInfoText(Form1 formInstance, string comment, string header)
        {
            richTextBoxInfo.Text = comment;
            Form1Instance = formInstance;
            this.Text = "Info " + header;
            unchangedText = comment;
        }

        private void richTextBoxInfo_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonCloseInfo_Click(object sender, EventArgs e)
        {
            CloseCommentWindow();
        }

        private void buttonSaveInfo_Click(object sender, EventArgs e)
        {
            if (unchangedText != richTextBoxInfo.Text)
            {
                Form1Instance.SetProductComment(richTextBoxInfo.Text);
            }
            this.Hide();
        }

        private void TextBoxForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseCommentWindow();
            e.Cancel = true;

        }


        public void SetNextLocation(Point pos)
        {
            // here store the value for x & y into instance variables
            this.desiredStartLocationX = pos.X;
            this.desiredStartLocationY = pos.Y;
            Load += new EventHandler(Form2_Load);
        }

        internal void FocusTextBox()
        {
            richTextBoxInfo.Focus();
            if (richTextBoxInfo.Text == "Comment")
            {
                richTextBoxInfo.Select(0, richTextBoxInfo.Text.Length);
            }
            else
            {
                richTextBoxInfo.Select(richTextBoxInfo.Text.Length, 0);
            }
        }

        private void buttonCloseInfo_KeyDown(object sender, KeyEventArgs e)
        {

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                CloseCommentWindow();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CloseCommentWindow()
        {
            if (unchangedText == richTextBoxInfo.Text)
            {
                //Text not changed close
                this.Hide();
            }
            else
            {
                //Text changed. Ask before closing
                DialogResult dialogResult = MessageBox.Show("Do you really want to close without saving?", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //do something
                    this.Hide();
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
        }
    }
}
