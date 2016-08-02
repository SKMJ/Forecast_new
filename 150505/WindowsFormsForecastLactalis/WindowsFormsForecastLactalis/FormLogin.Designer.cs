namespace WindowsFormsForecastLactalis
{
    partial class FormLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.buttonLogIN = new System.Windows.Forms.Button();
            this.labelUserName = new System.Windows.Forms.Label();
            this.labelPwd = new System.Windows.Forms.Label();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelMessageText = new System.Windows.Forms.Label();
            this.comboBoxProdOrTest = new System.Windows.Forms.ComboBox();
            this.labelM3Version = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonLogIN
            // 
            this.buttonLogIN.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonLogIN.Location = new System.Drawing.Point(348, 324);
            this.buttonLogIN.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLogIN.Name = "buttonLogIN";
            this.buttonLogIN.Size = new System.Drawing.Size(106, 37);
            this.buttonLogIN.TabIndex = 4;
            this.buttonLogIN.Text = "Log In";
            this.buttonLogIN.UseVisualStyleBackColor = false;
            this.buttonLogIN.Click += new System.EventHandler(this.buttonLogIN_Click);
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(102, 114);
            this.labelUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(98, 18);
            this.labelUserName.TabIndex = 1;
            this.labelUserName.Text = "User Name:";
            // 
            // labelPwd
            // 
            this.labelPwd.AutoSize = true;
            this.labelPwd.Location = new System.Drawing.Point(102, 223);
            this.labelPwd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPwd.Name = "labelPwd";
            this.labelPwd.Size = new System.Drawing.Size(88, 18);
            this.labelPwd.TabIndex = 2;
            this.labelPwd.Text = "Password:";
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(106, 156);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(240, 24);
            this.textBoxUserName.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(106, 267);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(240, 24);
            this.textBox1.TabIndex = 1;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WindowsFormsForecastLactalis.Properties.Resources.lactalis;
            this.pictureBox1.Location = new System.Drawing.Point(23, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(270, 76);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // labelMessageText
            // 
            this.labelMessageText.AutoSize = true;
            this.labelMessageText.ForeColor = System.Drawing.Color.Red;
            this.labelMessageText.Location = new System.Drawing.Point(19, 324);
            this.labelMessageText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMessageText.Name = "labelMessageText";
            this.labelMessageText.Size = new System.Drawing.Size(18, 18);
            this.labelMessageText.TabIndex = 6;
            this.labelMessageText.Text = "A";
            // 
            // comboBoxProdOrTest
            // 
            this.comboBoxProdOrTest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProdOrTest.FormattingEnabled = true;
            this.comboBoxProdOrTest.Location = new System.Drawing.Point(308, 69);
            this.comboBoxProdOrTest.Name = "comboBoxProdOrTest";
            this.comboBoxProdOrTest.Size = new System.Drawing.Size(155, 25);
            this.comboBoxProdOrTest.TabIndex = 3;
            this.comboBoxProdOrTest.SelectedIndexChanged += new System.EventHandler(this.comboBoxProdOrTest_SelectedIndexChanged);
            // 
            // labelM3Version
            // 
            this.labelM3Version.AutoSize = true;
            this.labelM3Version.Location = new System.Drawing.Point(308, 46);
            this.labelM3Version.Name = "labelM3Version";
            this.labelM3Version.Size = new System.Drawing.Size(93, 18);
            this.labelM3Version.TabIndex = 8;
            this.labelM3Version.Text = "M3 Version";
            this.labelM3Version.Click += new System.EventHandler(this.labelM3Version_Click);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(475, 374);
            this.Controls.Add(this.labelM3Version);
            this.Controls.Add(this.comboBoxProdOrTest);
            this.Controls.Add(this.labelMessageText);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.labelPwd);
            this.Controls.Add(this.labelUserName);
            this.Controls.Add(this.buttonLogIN);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormLogin";
            this.Text = "Login Forecast";
            this.Shown += new System.EventHandler(this.FormLogin_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLogIN;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.Label labelPwd;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelMessageText;
        private System.Windows.Forms.ComboBox comboBoxProdOrTest;
        private System.Windows.Forms.Label labelM3Version;
    }
}