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
        //Object Instance;
        FormSales FormSalesInstance;
        FormSupply FormSupplyInstance;
        public int desiredStartLocationX;
        public int desiredStartLocationY;
        private string unchangedText;
        private bool isSupplyComment;

        public TextBoxForm()
        {
            desiredStartLocationX = 0;
            desiredStartLocationY = 0;

            InitializeComponent();
            MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            //if (formInstance.GetType() == typeof(FormSales))
            //{
            //    isSupplyComment = false;
            //    saveImmediateBtn.Visible = true;
            //    buttonSaveInfo.Visible = false;
            //    FormSalesInstance = (FormSales)formInstance;
            //}
            //else
            //{
            //    saveImmediateBtn.Visible = false;
            //    buttonSaveInfo.Visible = true;
            //    isSupplyComment = true;
            //    FormSupplyInstance = (FormSupply)formInstance;
            //}
        }

        private void Form2_Load(object sender, System.EventArgs e)
        {
            this.SetDesktopLocation(desiredStartLocationX, desiredStartLocationY);
        }

        private void TextBoxForm_Load(object sender, EventArgs e)
        {

        }

        public void SetInfoText(Object formInstance, string comment, string header)
        {

            richTextBoxInfo.Text = comment;
            if(formInstance.GetType() == typeof(FormSales))
            {
                isSupplyComment = false;
                saveImmediateBtn.Visible = true;
                buttonSaveInfo.Visible = false;
                FormSalesInstance = (FormSales)formInstance;
            }
            else
            {
                saveImmediateBtn.Visible = false;
                buttonSaveInfo.Visible = true;
                isSupplyComment = true;
                FormSupplyInstance = (FormSupply)formInstance;
            }
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
                if (isSupplyComment)
                {

                    FormSupplyInstance.SetProductRegComment(richTextBoxInfo.Text);                    
                }
                else
                {
                    FormSalesInstance.SetProductComment(richTextBoxInfo.Text);
                }

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

        public void FocusTextBox(Object formInstance)
        {
            if (formInstance.GetType() == typeof(FormSales))
            {
                isSupplyComment = false;
                saveImmediateBtn.Visible = true;
                buttonSaveInfo.Visible = false;
                FormSalesInstance = (FormSales)formInstance;
            }
            else
            {
                saveImmediateBtn.Visible = false;
                buttonSaveInfo.Visible = true;
                isSupplyComment = true;
                FormSupplyInstance = (FormSupply)formInstance;
            }
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

        public void SaveButtonVisible(bool visible)
        {
            buttonSaveInfo.Visible = visible;
        }

        private void richTextBoxOldInfo_TextChanged(object sender, EventArgs e)
        {

        }

        public void SetOldInfo(string text)
        {
            richTextBoxOldInfo.ReadOnly = true;
            richTextBoxOldInfo.Text = text;

        }

        /// <summary>
        /// Determine if save button should be disabled. It will be disabled if the week is locked.
        /// The save immediately button will be enabled.
        /// </summary>
        /// <param name="readOnly">If true save button will be disabled</param>
        public void IsReadOnly(bool readOnly)
        {
            buttonSaveInfo.Enabled = !readOnly;
        }

        private void saveImmediateBtn_Click(object sender, EventArgs e)
        {
            if (richTextBoxInfo.Text.Trim().Length > 0)
            {
                if (!isSupplyComment)
                {
                    FormSalesInstance.SetProductCommentImmediately(richTextBoxInfo.Text);
                }

            }
            this.Hide();
        }

        internal void LastYearNoSave(bool p)
        {
            if (p)
            {
                saveImmediateBtn.Visible = false;
                buttonSaveInfo.Visible = false;
            }
            
        }
    }
}
