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
            this.dataGridForecastInfo.Location = new System.Drawing.Point(10, 145);
            this.dataGridForecastInfo.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridForecastInfo.Name = "dataGridForecastInfo";
            this.dataGridForecastInfo.RowTemplate.Height = 24;
            this.dataGridForecastInfo.Size = new System.Drawing.Size(1101, 537);
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
            this.pictureBox1.Location = new System.Drawing.Point(10, 11);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(217, 84);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // buttonSupplyView
            // 
            this.buttonSupplyView.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonSupplyView.Location = new System.Drawing.Point(352, 32);
            this.buttonSupplyView.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSupplyView.Name = "buttonSupplyView";
            this.buttonSupplyView.Size = new System.Drawing.Size(150, 28);
            this.buttonSupplyView.TabIndex = 2;
            this.buttonSupplyView.Text = "Change to Supply View";
            this.buttonSupplyView.UseVisualStyleBackColor = false;
            this.buttonSupplyView.Click += new System.EventHandler(this.buttonSupplyView_Click);
            this.buttonSupplyView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonSupplyView_MouseClick);
            // 
            // comboBoxAssortment
            // 
            this.comboBoxAssortment.FormattingEnabled = true;
            this.comboBoxAssortment.Location = new System.Drawing.Point(246, 98);
            this.comboBoxAssortment.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxAssortment.Name = "comboBoxAssortment";
            this.comboBoxAssortment.Size = new System.Drawing.Size(144, 21);
            this.comboBoxAssortment.TabIndex = 3;
            // 
            // labelAssortment
            // 
            this.labelAssortment.AutoSize = true;
            this.labelAssortment.Location = new System.Drawing.Point(246, 79);
            this.labelAssortment.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAssortment.Name = "labelAssortment";
            this.labelAssortment.Size = new System.Drawing.Size(59, 13);
            this.labelAssortment.TabIndex = 4;
            this.labelAssortment.Text = "Assortment";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button1.Location = new System.Drawing.Point(246, 119);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 22);
            this.button1.TabIndex = 5;
            this.button1.Text = "Get Products";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormSales_KeyPress);
            // 
            // labelYear
            // 
            this.labelYear.AutoSize = true;
            this.labelYear.Location = new System.Drawing.Point(434, 79);
            this.labelYear.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelYear.Name = "labelYear";
            this.labelYear.Size = new System.Drawing.Size(29, 13);
            this.labelYear.TabIndex = 7;
            this.labelYear.Text = "Year";
            // 
            // comboBoxYear
            // 
            this.comboBoxYear.FormattingEnabled = true;
            this.comboBoxYear.Location = new System.Drawing.Point(434, 98);
            this.comboBoxYear.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxYear.Name = "comboBoxYear";
            this.comboBoxYear.Size = new System.Drawing.Size(60, 21);
            this.comboBoxYear.TabIndex = 6;
            this.comboBoxYear.SelectedIndexChanged += new System.EventHandler(this.comboBoxYear_SelectedIndexChanged);
            // 
            // buttonGetProductByNumber
            // 
            this.buttonGetProductByNumber.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonGetProductByNumber.Location = new System.Drawing.Point(10, 118);
            this.buttonGetProductByNumber.Margin = new System.Windows.Forms.Padding(2);
            this.buttonGetProductByNumber.Name = "buttonGetProductByNumber";
            this.buttonGetProductByNumber.Size = new System.Drawing.Size(76, 22);
            this.buttonGetProductByNumber.TabIndex = 19;
            this.buttonGetProductByNumber.Text = "Get Product";
            this.buttonGetProductByNumber.UseVisualStyleBackColor = false;
            this.buttonGetProductByNumber.Click += new System.EventHandler(this.buttonGetProductByNumber_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(528, 104);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(149, 20);
            this.labelStatus.TabIndex = 20;
            this.labelStatus.Text = "Loading Forecast";
            this.labelStatus.Visible = false;
            // 
            // textBoxProdNBR
            // 
            this.textBoxProdNBR.Location = new System.Drawing.Point(10, 99);
            this.textBoxProdNBR.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxProdNBR.Name = "textBoxProdNBR";
            this.textBoxProdNBR.Size = new System.Drawing.Size(76, 20);
            this.textBoxProdNBR.TabIndex = 21;
            this.textBoxProdNBR.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxProdNBR_KeyDown);
            // 
            // FormSales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.ClientSize = new System.Drawing.Size(1112, 695);
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
            this.Margin = new System.Windows.Forms.Padding(2);
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
    }
}

