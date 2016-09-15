namespace WindowsFormsForecastLactalis
{
    partial class FormSales
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSales));
            this.dataGridForecastInfo = new System.Windows.Forms.DataGridView();
            this.ColumnVareNR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonSupplyView = new System.Windows.Forms.Button();
            this.comboBoxAssortment = new System.Windows.Forms.ComboBox();
            this.labelAssortment = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.labelYear = new System.Windows.Forms.Label();
            this.comboBoxYear = new System.Windows.Forms.ComboBox();
            this.buttonGetProductByNumber = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.textBoxProdNBR = new System.Windows.Forms.TextBox();
            this.textBoxCalculator = new System.Windows.Forms.TextBox();
            this.labelCalculator = new System.Windows.Forms.Label();
            this.checkBoxLastYear = new System.Windows.Forms.CheckBox();
            this.checkBoxKampgn = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridForecastInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridForecastInfo
            // 
            this.dataGridForecastInfo.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dataGridForecastInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridForecastInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnVareNR,
            this.ColumnProductName,
            this.ColumnType});
            this.dataGridForecastInfo.Location = new System.Drawing.Point(13, 178);
            this.dataGridForecastInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridForecastInfo.Name = "dataGridForecastInfo";
            this.dataGridForecastInfo.RowTemplate.Height = 24;
            this.dataGridForecastInfo.Size = new System.Drawing.Size(1468, 637);
            this.dataGridForecastInfo.TabIndex = 0;
            this.dataGridForecastInfo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridForecastInfo_CellClick);
            this.dataGridForecastInfo.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridForecastInfo_CellValidating);
            this.dataGridForecastInfo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridForecastInfo_CellValueChanged);
            this.dataGridForecastInfo.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridForecastInfo_ColumnWidthChanged);
            this.dataGridForecastInfo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataGridForecastInfo_KeyUp);
            this.dataGridForecastInfo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridForecastInfo_MouseClick);
            // 
            // ColumnVareNR
            // 
            this.ColumnVareNR.Frozen = true;
            this.ColumnVareNR.HeaderText = "VareNr";
            this.ColumnVareNR.Name = "ColumnVareNR";
            this.ColumnVareNR.Width = 70;
            // 
            // ColumnProductName
            // 
            this.ColumnProductName.Frozen = true;
            this.ColumnProductName.HeaderText = "Beskrivelse";
            this.ColumnProductName.Name = "ColumnProductName";
            this.ColumnProductName.Width = 270;
            // 
            // ColumnType
            // 
            this.ColumnType.Frozen = true;
            this.ColumnType.HeaderText = "Type";
            this.ColumnType.Name = "ColumnType";
            this.ColumnType.Width = 250;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = global::WindowsFormsForecastLactalis.Properties.Resources.lactalis;
            this.pictureBox1.Location = new System.Drawing.Point(13, 14);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(289, 103);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // buttonSupplyView
            // 
            this.buttonSupplyView.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonSupplyView.Location = new System.Drawing.Point(469, 39);
            this.buttonSupplyView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonSupplyView.Name = "buttonSupplyView";
            this.buttonSupplyView.Size = new System.Drawing.Size(200, 34);
            this.buttonSupplyView.TabIndex = 2;
            this.buttonSupplyView.Text = "Change to Supply View";
            this.buttonSupplyView.UseVisualStyleBackColor = false;
            this.buttonSupplyView.Click += new System.EventHandler(this.buttonSupplyView_Click);
            this.buttonSupplyView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonSupplyView_MouseClick);
            // 
            // comboBoxAssortment
            // 
            this.comboBoxAssortment.FormattingEnabled = true;
            this.comboBoxAssortment.Location = new System.Drawing.Point(326, 92);
            this.comboBoxAssortment.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxAssortment.Name = "comboBoxAssortment";
            this.comboBoxAssortment.Size = new System.Drawing.Size(191, 24);
            this.comboBoxAssortment.TabIndex = 3;
            this.comboBoxAssortment.SelectedIndexChanged += new System.EventHandler(this.comboBoxAssortment_SelectedIndexChanged);
            // 
            // labelAssortment
            // 
            this.labelAssortment.AutoSize = true;
            this.labelAssortment.Location = new System.Drawing.Point(326, 68);
            this.labelAssortment.Name = "labelAssortment";
            this.labelAssortment.Size = new System.Drawing.Size(79, 17);
            this.labelAssortment.TabIndex = 4;
            this.labelAssortment.Text = "Assortment";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button1.Location = new System.Drawing.Point(326, 117);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 27);
            this.button1.TabIndex = 5;
            this.button1.Text = "Get Products";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormSales_KeyPress);
            // 
            // labelYear
            // 
            this.labelYear.AutoSize = true;
            this.labelYear.Location = new System.Drawing.Point(579, 97);
            this.labelYear.Name = "labelYear";
            this.labelYear.Size = new System.Drawing.Size(38, 17);
            this.labelYear.TabIndex = 7;
            this.labelYear.Text = "Year";
            // 
            // comboBoxYear
            // 
            this.comboBoxYear.FormattingEnabled = true;
            this.comboBoxYear.Location = new System.Drawing.Point(579, 121);
            this.comboBoxYear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxYear.Name = "comboBoxYear";
            this.comboBoxYear.Size = new System.Drawing.Size(79, 24);
            this.comboBoxYear.TabIndex = 6;
            this.comboBoxYear.SelectedIndexChanged += new System.EventHandler(this.comboBoxYear_SelectedIndexChanged);
            // 
            // buttonGetProductByNumber
            // 
            this.buttonGetProductByNumber.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonGetProductByNumber.Location = new System.Drawing.Point(13, 145);
            this.buttonGetProductByNumber.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonGetProductByNumber.Name = "buttonGetProductByNumber";
            this.buttonGetProductByNumber.Size = new System.Drawing.Size(101, 27);
            this.buttonGetProductByNumber.TabIndex = 19;
            this.buttonGetProductByNumber.Text = "Get Product";
            this.buttonGetProductByNumber.UseVisualStyleBackColor = false;
            this.buttonGetProductByNumber.Click += new System.EventHandler(this.buttonGetProductByNumber_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(704, 128);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(179, 25);
            this.labelStatus.TabIndex = 20;
            this.labelStatus.Text = "Loading Forecast";
            this.labelStatus.Visible = false;
            // 
            // textBoxProdNBR
            // 
            this.textBoxProdNBR.Location = new System.Drawing.Point(13, 122);
            this.textBoxProdNBR.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxProdNBR.Name = "textBoxProdNBR";
            this.textBoxProdNBR.Size = new System.Drawing.Size(100, 22);
            this.textBoxProdNBR.TabIndex = 21;
            this.textBoxProdNBR.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxProdNBR_KeyDown);
            // 
            // textBoxCalculator
            // 
            this.textBoxCalculator.Location = new System.Drawing.Point(143, 822);
            this.textBoxCalculator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxCalculator.Name = "textBoxCalculator";
            this.textBoxCalculator.Size = new System.Drawing.Size(226, 22);
            this.textBoxCalculator.TabIndex = 22;
            this.textBoxCalculator.Enter += new System.EventHandler(this.textBoxCalculator_Enter);
            this.textBoxCalculator.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCalculator_KeyPress);
            // 
            // labelCalculator
            // 
            this.labelCalculator.AutoSize = true;
            this.labelCalculator.Location = new System.Drawing.Point(15, 825);
            this.labelCalculator.Name = "labelCalculator";
            this.labelCalculator.Size = new System.Drawing.Size(102, 17);
            this.labelCalculator.TabIndex = 23;
            this.labelCalculator.Text = "Regneformler: ";
            // 
            // checkBoxLastYear
            // 
            this.checkBoxLastYear.AutoSize = true;
            this.checkBoxLastYear.Location = new System.Drawing.Point(435, 151);
            this.checkBoxLastYear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxLastYear.Name = "checkBoxLastYear";
            this.checkBoxLastYear.Size = new System.Drawing.Size(123, 21);
            this.checkBoxLastYear.TabIndex = 24;
            this.checkBoxLastYear.Text = "Load LastYear";
            this.checkBoxLastYear.UseVisualStyleBackColor = true;
            // 
            // checkBoxKampgn
            // 
            this.checkBoxKampgn.AutoSize = true;
            this.checkBoxKampgn.Location = new System.Drawing.Point(435, 122);
            this.checkBoxKampgn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxKampgn.Name = "checkBoxKampgn";
            this.checkBoxKampgn.Size = new System.Drawing.Size(137, 21);
            this.checkBoxKampgn.TabIndex = 27;
            this.checkBoxKampgn.Text = "Load Promotions";
            this.checkBoxKampgn.UseVisualStyleBackColor = true;
            // 
            // FormSales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.ClientSize = new System.Drawing.Size(1483, 855);
            this.Controls.Add(this.checkBoxKampgn);
            this.Controls.Add(this.checkBoxLastYear);
            this.Controls.Add(this.labelCalculator);
            this.Controls.Add(this.textBoxCalculator);
            this.Controls.Add(this.textBoxProdNBR);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonGetProductByNumber);
            this.Controls.Add(this.labelYear);
            this.Controls.Add(this.comboBoxYear);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelAssortment);
            this.Controls.Add(this.comboBoxAssortment);
            this.Controls.Add(this.buttonSupplyView);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.dataGridForecastInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormSales";
            this.Text = "Forecast Sales";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormSales_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridForecastInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridForecastInfo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonSupplyView;
        private System.Windows.Forms.ComboBox comboBoxAssortment;
        private System.Windows.Forms.Label labelAssortment;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelYear;
        private System.Windows.Forms.ComboBox comboBoxYear;
        private System.Windows.Forms.Button buttonGetProductByNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVareNR;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnType;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.TextBox textBoxProdNBR;
        private System.Windows.Forms.TextBox textBoxCalculator;
        private System.Windows.Forms.Label labelCalculator;
        private System.Windows.Forms.CheckBox checkBoxLastYear;
        private System.Windows.Forms.CheckBox checkBoxKampgn;
    }
}

