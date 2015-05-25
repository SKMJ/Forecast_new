namespace WindowsFormsForecastLactalis
{
    partial class FormSupply
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dataGridForecastInfo = new System.Windows.Forms.DataGridView();
            this.ColumnVareNR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnWeek1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnWeek2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnWeek3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnWeek4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnWeek5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Week6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonSalesView = new System.Windows.Forms.Button();
            this.buttonGetProductsBySupplier = new System.Windows.Forms.Button();
            this.labelSupplier = new System.Windows.Forms.Label();
            this.comboBoxSupplier = new System.Windows.Forms.ComboBox();
            this.buttonTestSupplItems = new System.Windows.Forms.Button();
            this.numericSupplyNBR = new System.Windows.Forms.NumericUpDown();
            this.buttonGetSupplierFromNBR = new System.Windows.Forms.Button();
            this.labelSupplierFreeText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridForecastInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSupplyNBR)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = global::WindowsFormsForecastLactalis.Properties.Resources.lactalis;
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(289, 103);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // dataGridForecastInfo
            // 
            this.dataGridForecastInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridForecastInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnVareNR,
            this.ColumnProductName,
            this.ColumnType,
            this.ColumnWeek1,
            this.ColumnWeek2,
            this.ColumnWeek3,
            this.ColumnWeek4,
            this.ColumnWeek5,
            this.Week6});
            this.dataGridForecastInfo.Location = new System.Drawing.Point(13, 179);
            this.dataGridForecastInfo.Name = "dataGridForecastInfo";
            this.dataGridForecastInfo.RowTemplate.Height = 24;
            this.dataGridForecastInfo.Size = new System.Drawing.Size(1468, 661);
            this.dataGridForecastInfo.TabIndex = 3;
            this.dataGridForecastInfo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridForecastInfo_CellClick);
            this.dataGridForecastInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridForecastInfo_CellContentClick);
            this.dataGridForecastInfo.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridForecastInfo_CellValidating_1);
            // 
            // ColumnVareNR
            // 
            this.ColumnVareNR.Frozen = true;
            this.ColumnVareNR.HeaderText = "VareNr";
            this.ColumnVareNR.Name = "ColumnVareNR";
            // 
            // ColumnProductName
            // 
            this.ColumnProductName.Frozen = true;
            this.ColumnProductName.HeaderText = "Beskrivelse";
            this.ColumnProductName.Name = "ColumnProductName";
            this.ColumnProductName.Width = 200;
            // 
            // ColumnType
            // 
            this.ColumnType.Frozen = true;
            this.ColumnType.HeaderText = "Type";
            this.ColumnType.Name = "ColumnType";
            this.ColumnType.Width = 300;
            // 
            // ColumnWeek1
            // 
            this.ColumnWeek1.HeaderText = "1.2015";
            this.ColumnWeek1.Name = "ColumnWeek1";
            // 
            // ColumnWeek2
            // 
            this.ColumnWeek2.HeaderText = "2.2015";
            this.ColumnWeek2.Name = "ColumnWeek2";
            // 
            // ColumnWeek3
            // 
            this.ColumnWeek3.HeaderText = "3.2015";
            this.ColumnWeek3.Name = "ColumnWeek3";
            // 
            // ColumnWeek4
            // 
            this.ColumnWeek4.HeaderText = "4.2015";
            this.ColumnWeek4.Name = "ColumnWeek4";
            // 
            // ColumnWeek5
            // 
            this.ColumnWeek5.HeaderText = "5.2015";
            this.ColumnWeek5.Name = "ColumnWeek5";
            // 
            // Week6
            // 
            this.Week6.HeaderText = "6.2015";
            this.Week6.Name = "Week6";
            // 
            // buttonSalesView
            // 
            this.buttonSalesView.Location = new System.Drawing.Point(469, 39);
            this.buttonSalesView.Name = "buttonSalesView";
            this.buttonSalesView.Size = new System.Drawing.Size(200, 34);
            this.buttonSalesView.TabIndex = 4;
            this.buttonSalesView.Text = "Change to Sales View";
            this.buttonSalesView.UseVisualStyleBackColor = true;
            this.buttonSalesView.Click += new System.EventHandler(this.buttonSalesView_Click);
            // 
            // buttonGetProductsBySupplier
            // 
            this.buttonGetProductsBySupplier.Location = new System.Drawing.Point(328, 146);
            this.buttonGetProductsBySupplier.Name = "buttonGetProductsBySupplier";
            this.buttonGetProductsBySupplier.Size = new System.Drawing.Size(102, 27);
            this.buttonGetProductsBySupplier.TabIndex = 9;
            this.buttonGetProductsBySupplier.Text = "Get Products";
            this.buttonGetProductsBySupplier.UseVisualStyleBackColor = true;
            this.buttonGetProductsBySupplier.Click += new System.EventHandler(this.buttonGetProductsBySupplier_Click);
            // 
            // labelSupplier
            // 
            this.labelSupplier.AutoSize = true;
            this.labelSupplier.Location = new System.Drawing.Point(328, 97);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(60, 17);
            this.labelSupplier.TabIndex = 8;
            this.labelSupplier.Text = "Supplier";
            // 
            // comboBoxSupplier
            // 
            this.comboBoxSupplier.FormattingEnabled = true;
            this.comboBoxSupplier.Location = new System.Drawing.Point(328, 121);
            this.comboBoxSupplier.Name = "comboBoxSupplier";
            this.comboBoxSupplier.Size = new System.Drawing.Size(190, 24);
            this.comboBoxSupplier.TabIndex = 7;
            // 
            // buttonTestSupplItems
            // 
            this.buttonTestSupplItems.Location = new System.Drawing.Point(882, 49);
            this.buttonTestSupplItems.Name = "buttonTestSupplItems";
            this.buttonTestSupplItems.Size = new System.Drawing.Size(112, 23);
            this.buttonTestSupplItems.TabIndex = 10;
            this.buttonTestSupplItems.Text = "buttonTestSupplItems";
            this.buttonTestSupplItems.UseVisualStyleBackColor = true;
            this.buttonTestSupplItems.Visible = false;
            this.buttonTestSupplItems.Click += new System.EventHandler(this.buttonTestSupplItems_Click);
            // 
            // numericSupplyNBR
            // 
            this.numericSupplyNBR.Location = new System.Drawing.Point(605, 123);
            this.numericSupplyNBR.Maximum = new decimal(new int[] {
            1316134912,
            2328,
            0,
            0});
            this.numericSupplyNBR.Name = "numericSupplyNBR";
            this.numericSupplyNBR.Size = new System.Drawing.Size(156, 22);
            this.numericSupplyNBR.TabIndex = 11;
            // 
            // buttonGetSupplierFromNBR
            // 
            this.buttonGetSupplierFromNBR.Location = new System.Drawing.Point(605, 146);
            this.buttonGetSupplierFromNBR.Name = "buttonGetSupplierFromNBR";
            this.buttonGetSupplierFromNBR.Size = new System.Drawing.Size(102, 27);
            this.buttonGetSupplierFromNBR.TabIndex = 12;
            this.buttonGetSupplierFromNBR.Text = "Get Products";
            this.buttonGetSupplierFromNBR.UseVisualStyleBackColor = true;
            this.buttonGetSupplierFromNBR.Click += new System.EventHandler(this.buttonGetSupplierFromNBR_Click);
            // 
            // labelSupplierFreeText
            // 
            this.labelSupplierFreeText.AutoSize = true;
            this.labelSupplierFreeText.Location = new System.Drawing.Point(602, 99);
            this.labelSupplierFreeText.Name = "labelSupplierFreeText";
            this.labelSupplierFreeText.Size = new System.Drawing.Size(114, 17);
            this.labelSupplierFreeText.TabIndex = 13;
            this.labelSupplierFreeText.Text = "Supplier Number";
            // 
            // FormSupply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaGreen;
            this.ClientSize = new System.Drawing.Size(1482, 855);
            this.Controls.Add(this.labelSupplierFreeText);
            this.Controls.Add(this.buttonGetSupplierFromNBR);
            this.Controls.Add(this.numericSupplyNBR);
            this.Controls.Add(this.buttonTestSupplItems);
            this.Controls.Add(this.buttonGetProductsBySupplier);
            this.Controls.Add(this.labelSupplier);
            this.Controls.Add(this.comboBoxSupplier);
            this.Controls.Add(this.buttonSalesView);
            this.Controls.Add(this.dataGridForecastInfo);
            this.Controls.Add(this.pictureBox1);
            this.Name = "FormSupply";
            this.Text = "Forecast Supply ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormSupply_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridForecastInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSupplyNBR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView dataGridForecastInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVareNR;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnWeek1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnWeek2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnWeek3;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnWeek4;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnWeek5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Week6;
        private System.Windows.Forms.Button buttonSalesView;
        private System.Windows.Forms.Button buttonGetProductsBySupplier;
        private System.Windows.Forms.Label labelSupplier;
        private System.Windows.Forms.ComboBox comboBoxSupplier;
        private System.Windows.Forms.Button buttonTestSupplItems;
        private System.Windows.Forms.NumericUpDown numericSupplyNBR;
        private System.Windows.Forms.Button buttonGetSupplierFromNBR;
        private System.Windows.Forms.Label labelSupplierFreeText;
    }
}