namespace WindowsFormsForecastLactalis
{
    partial class Form1
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
            this.dataGridForecastInfo = new System.Windows.Forms.DataGridView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonSupplyView = new System.Windows.Forms.Button();
            this.comboBoxAssortment = new System.Windows.Forms.ComboBox();
            this.labelAssortment = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.labelYear = new System.Windows.Forms.Label();
            this.comboBoxYear = new System.Windows.Forms.ComboBox();
            this.buttonGetProductByNumber = new System.Windows.Forms.Button();
            this.numericUpDownPRoductNumber = new System.Windows.Forms.NumericUpDown();
            this.ColumnVareNR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridForecastInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPRoductNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridForecastInfo
            // 
            this.dataGridForecastInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridForecastInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnVareNR,
            this.ColumnProductName,
            this.ColumnType});
            this.dataGridForecastInfo.Location = new System.Drawing.Point(13, 179);
            this.dataGridForecastInfo.Name = "dataGridForecastInfo";
            this.dataGridForecastInfo.RowTemplate.Height = 24;
            this.dataGridForecastInfo.Size = new System.Drawing.Size(1468, 661);
            this.dataGridForecastInfo.TabIndex = 0;
            this.dataGridForecastInfo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridForecastInfo_CellClick);
            this.dataGridForecastInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridForecastInfo_CellContentClick);
            this.dataGridForecastInfo.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridForecastInfo_CellMouseClick);
            this.dataGridForecastInfo.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridForecastInfo_CellValidating);
            this.dataGridForecastInfo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridForecastInfo_CellValueChanged);
            this.dataGridForecastInfo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridForecastInfo_MouseClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = global::WindowsFormsForecastLactalis.Properties.Resources.lactalis;
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(289, 103);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // buttonSupplyView
            // 
            this.buttonSupplyView.Location = new System.Drawing.Point(469, 39);
            this.buttonSupplyView.Name = "buttonSupplyView";
            this.buttonSupplyView.Size = new System.Drawing.Size(200, 34);
            this.buttonSupplyView.TabIndex = 2;
            this.buttonSupplyView.Text = "Change to Supply View";
            this.buttonSupplyView.UseVisualStyleBackColor = true;
            this.buttonSupplyView.Click += new System.EventHandler(this.buttonSupplyView_Click);
            this.buttonSupplyView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonSupplyView_MouseClick);
            // 
            // comboBoxAssortment
            // 
            this.comboBoxAssortment.FormattingEnabled = true;
            this.comboBoxAssortment.Location = new System.Drawing.Point(328, 121);
            this.comboBoxAssortment.Name = "comboBoxAssortment";
            this.comboBoxAssortment.Size = new System.Drawing.Size(190, 24);
            this.comboBoxAssortment.TabIndex = 3;
            this.comboBoxAssortment.SelectedIndexChanged += new System.EventHandler(this.comboBoxAssortment_SelectedIndexChanged);
            // 
            // labelAssortment
            // 
            this.labelAssortment.AutoSize = true;
            this.labelAssortment.Location = new System.Drawing.Point(328, 97);
            this.labelAssortment.Name = "labelAssortment";
            this.labelAssortment.Size = new System.Drawing.Size(79, 17);
            this.labelAssortment.TabIndex = 4;
            this.labelAssortment.Text = "Assortment";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(328, 146);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 27);
            this.button1.TabIndex = 5;
            this.button1.Text = "Get Products";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelYear
            // 
            this.labelYear.AutoSize = true;
            this.labelYear.Location = new System.Drawing.Point(578, 97);
            this.labelYear.Name = "labelYear";
            this.labelYear.Size = new System.Drawing.Size(38, 17);
            this.labelYear.TabIndex = 7;
            this.labelYear.Text = "Year";
            // 
            // comboBoxYear
            // 
            this.comboBoxYear.FormattingEnabled = true;
            this.comboBoxYear.Location = new System.Drawing.Point(578, 121);
            this.comboBoxYear.Name = "comboBoxYear";
            this.comboBoxYear.Size = new System.Drawing.Size(79, 24);
            this.comboBoxYear.TabIndex = 6;
            this.comboBoxYear.SelectedIndexChanged += new System.EventHandler(this.comboBoxYear_SelectedIndexChanged);
            // 
            // buttonGetProductByNumber
            // 
            this.buttonGetProductByNumber.Location = new System.Drawing.Point(51, 146);
            this.buttonGetProductByNumber.Name = "buttonGetProductByNumber";
            this.buttonGetProductByNumber.Size = new System.Drawing.Size(102, 27);
            this.buttonGetProductByNumber.TabIndex = 19;
            this.buttonGetProductByNumber.Text = "Get Product";
            this.buttonGetProductByNumber.UseVisualStyleBackColor = true;
            this.buttonGetProductByNumber.Click += new System.EventHandler(this.buttonGetProductByNumber_Click);
            // 
            // numericUpDownPRoductNumber
            // 
            this.numericUpDownPRoductNumber.Location = new System.Drawing.Point(51, 123);
            this.numericUpDownPRoductNumber.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericUpDownPRoductNumber.Name = "numericUpDownPRoductNumber";
            this.numericUpDownPRoductNumber.Size = new System.Drawing.Size(102, 22);
            this.numericUpDownPRoductNumber.TabIndex = 18;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.ClientSize = new System.Drawing.Size(1482, 855);
            this.Controls.Add(this.buttonGetProductByNumber);
            this.Controls.Add(this.numericUpDownPRoductNumber);
            this.Controls.Add(this.labelYear);
            this.Controls.Add(this.comboBoxYear);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelAssortment);
            this.Controls.Add(this.comboBoxAssortment);
            this.Controls.Add(this.buttonSupplyView);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.dataGridForecastInfo);
            this.Name = "Form1";
            this.Text = "Forecast Sales";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridForecastInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPRoductNumber)).EndInit();
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
        private System.Windows.Forms.NumericUpDown numericUpDownPRoductNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVareNR;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnType;
    }
}

