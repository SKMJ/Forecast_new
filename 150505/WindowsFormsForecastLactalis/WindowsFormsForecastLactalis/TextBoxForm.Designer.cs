namespace WindowsFormsForecastLactalis
{
    partial class TextBoxForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBoxInfo = new System.Windows.Forms.RichTextBox();
            this.buttonSaveInfo = new System.Windows.Forms.Button();
            this.buttonCloseInfo = new System.Windows.Forms.Button();
            this.richTextBoxOldInfo = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveImmediateBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBoxInfo
            // 
            this.richTextBoxInfo.Location = new System.Drawing.Point(0, 103);
            this.richTextBoxInfo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.richTextBoxInfo.Name = "richTextBoxInfo";
            this.richTextBoxInfo.Size = new System.Drawing.Size(389, 70);
            this.richTextBoxInfo.TabIndex = 1;
            this.richTextBoxInfo.Text = "";
            this.richTextBoxInfo.TextChanged += new System.EventHandler(this.richTextBoxInfo_TextChanged);
            // 
            // buttonSaveInfo
            // 
            this.buttonSaveInfo.Location = new System.Drawing.Point(225, 177);
            this.buttonSaveInfo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonSaveInfo.Name = "buttonSaveInfo";
            this.buttonSaveInfo.Size = new System.Drawing.Size(75, 32);
            this.buttonSaveInfo.TabIndex = 2;
            this.buttonSaveInfo.Text = "Gem";
            this.buttonSaveInfo.UseVisualStyleBackColor = true;
            this.buttonSaveInfo.Click += new System.EventHandler(this.buttonSaveInfo_Click);
            // 
            // buttonCloseInfo
            // 
            this.buttonCloseInfo.Location = new System.Drawing.Point(305, 177);
            this.buttonCloseInfo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonCloseInfo.Name = "buttonCloseInfo";
            this.buttonCloseInfo.Size = new System.Drawing.Size(75, 32);
            this.buttonCloseInfo.TabIndex = 3;
            this.buttonCloseInfo.Text = "Luk";
            this.buttonCloseInfo.UseVisualStyleBackColor = true;
            this.buttonCloseInfo.Click += new System.EventHandler(this.buttonCloseInfo_Click);
            this.buttonCloseInfo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.buttonCloseInfo_KeyDown);
            // 
            // richTextBoxOldInfo
            // 
            this.richTextBoxOldInfo.Location = new System.Drawing.Point(0, 1);
            this.richTextBoxOldInfo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.richTextBoxOldInfo.Name = "richTextBoxOldInfo";
            this.richTextBoxOldInfo.Size = new System.Drawing.Size(389, 70);
            this.richTextBoxOldInfo.TabIndex = 4;
            this.richTextBoxOldInfo.Text = "";
            this.richTextBoxOldInfo.TextChanged += new System.EventHandler(this.richTextBoxOldInfo_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 87);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Add New Comment Below:";
            // 
            // saveImmediateBtn
            // 
            this.saveImmediateBtn.Location = new System.Drawing.Point(146, 177);
            this.saveImmediateBtn.Margin = new System.Windows.Forms.Padding(2);
            this.saveImmediateBtn.Name = "saveImmediateBtn";
            this.saveImmediateBtn.Size = new System.Drawing.Size(75, 32);
            this.saveImmediateBtn.TabIndex = 6;
            this.saveImmediateBtn.Text = "Spara direkt";
            this.saveImmediateBtn.UseVisualStyleBackColor = true;
            this.saveImmediateBtn.Click += new System.EventHandler(this.saveImmediateBtn_Click);
            // 
            // TextBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 232);
            this.Controls.Add(this.saveImmediateBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBoxOldInfo);
            this.Controls.Add(this.buttonCloseInfo);
            this.Controls.Add(this.buttonSaveInfo);
            this.Controls.Add(this.richTextBoxInfo);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "TextBoxForm";
            this.Text = "Information";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TextBoxForm_FormClosing);
            this.Load += new System.EventHandler(this.TextBoxForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxInfo;
        private System.Windows.Forms.Button buttonSaveInfo;
        private System.Windows.Forms.Button buttonCloseInfo;
        private System.Windows.Forms.RichTextBox richTextBoxOldInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button saveImmediateBtn;
    }
}