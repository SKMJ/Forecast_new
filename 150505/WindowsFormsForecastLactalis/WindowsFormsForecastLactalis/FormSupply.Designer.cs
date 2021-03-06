﻿namespace WindowsFormsForecastLactalis
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSupply));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dataGridForecastInfo = new System.Windows.Forms.DataGridView();
            this.ColumnVareNR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonSalesView = new System.Windows.Forms.Button();
            this.buttonGetProductsBySupplier = new System.Windows.Forms.Button();
            this.labelSupplier = new System.Windows.Forms.Label();
            this.comboBoxSupplier = new System.Windows.Forms.ComboBox();
            this.buttonTestSupplItems = new System.Windows.Forms.Button();
            this.numericSupplyNBR = new System.Windows.Forms.NumericUpDown();
            this.buttonGetSupplierFromNBR = new System.Windows.Forms.Button();
            this.labelSupplierFreeText = new System.Windows.Forms.Label();
            this.labelYear = new System.Windows.Forms.Label();
            this.comboBoxYear = new System.Windows.Forms.ComboBox();
            this.buttonGetProductByNumber = new System.Windows.Forms.Button();
            this.buttonCreateM3LactalisOrders = new System.Windows.Forms.Button();
            this.checkBoxLastYear = new System.Windows.Forms.CheckBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonCreateLactalisFile = new System.Windows.Forms.Button();
            this.textBoxProdNBR = new System.Windows.Forms.TextBox();
            this.labelCalculator = new System.Windows.Forms.Label();
            this.textBoxCalculator = new System.Windows.Forms.TextBox();
            this.checkBoxKampgn = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridForecastInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSupplyNBR)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = global::WindowsFormsForecastLactalis.Properties.Resources.RESUND_1;
            this.pictureBox1.Location = new System.Drawing.Point(13, 14);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(289, 103);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
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
            this.dataGridForecastInfo.Size = new System.Drawing.Size(1468, 640);
            this.dataGridForecastInfo.TabIndex = 3;
            this.dataGridForecastInfo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridForecastInfo_CellClick);
            this.dataGridForecastInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridForecastInfo_CellContentClick);
            this.dataGridForecastInfo.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridForecastInfo_CellValidating_1);
            this.dataGridForecastInfo.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridForecastInfo_ColumnWidthChanged);
            this.dataGridForecastInfo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataGridForecastInfo_KeyUp);
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
            // buttonSalesView
            // 
            this.buttonSalesView.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonSalesView.Location = new System.Drawing.Point(469, 39);
            this.buttonSalesView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonSalesView.Name = "buttonSalesView";
            this.buttonSalesView.Size = new System.Drawing.Size(200, 34);
            this.buttonSalesView.TabIndex = 4;
            this.buttonSalesView.Text = "Change to Sales View";
            this.buttonSalesView.UseVisualStyleBackColor = false;
            this.buttonSalesView.Click += new System.EventHandler(this.buttonSalesView_Click);
            // 
            // buttonGetProductsBySupplier
            // 
            this.buttonGetProductsBySupplier.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonGetProductsBySupplier.Location = new System.Drawing.Point(328, 126);
            this.buttonGetProductsBySupplier.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonGetProductsBySupplier.Name = "buttonGetProductsBySupplier";
            this.buttonGetProductsBySupplier.Size = new System.Drawing.Size(101, 27);
            this.buttonGetProductsBySupplier.TabIndex = 9;
            this.buttonGetProductsBySupplier.Text = "Get Products";
            this.buttonGetProductsBySupplier.UseVisualStyleBackColor = false;
            this.buttonGetProductsBySupplier.Click += new System.EventHandler(this.buttonGetProductsBySupplier_Click);
            this.buttonGetProductsBySupplier.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.buttonGetProductsBySupplier_KeyPress);
            // 
            // labelSupplier
            // 
            this.labelSupplier.AutoSize = true;
            this.labelSupplier.Location = new System.Drawing.Point(328, 79);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(60, 17);
            this.labelSupplier.TabIndex = 8;
            this.labelSupplier.Text = "Supplier";
            // 
            // comboBoxSupplier
            // 
            this.comboBoxSupplier.FormattingEnabled = true;
            this.comboBoxSupplier.Location = new System.Drawing.Point(328, 98);
            this.comboBoxSupplier.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxSupplier.Name = "comboBoxSupplier";
            this.comboBoxSupplier.Size = new System.Drawing.Size(263, 24);
            this.comboBoxSupplier.TabIndex = 7;
            this.comboBoxSupplier.SelectedIndexChanged += new System.EventHandler(this.comboBoxSupplier_SelectedIndexChanged);
            // 
            // buttonTestSupplItems
            // 
            this.buttonTestSupplItems.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonTestSupplItems.Location = new System.Drawing.Point(883, 49);
            this.buttonTestSupplItems.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonTestSupplItems.Name = "buttonTestSupplItems";
            this.buttonTestSupplItems.Size = new System.Drawing.Size(112, 23);
            this.buttonTestSupplItems.TabIndex = 10;
            this.buttonTestSupplItems.Text = "buttonTestSupplItems";
            this.buttonTestSupplItems.UseVisualStyleBackColor = false;
            this.buttonTestSupplItems.Visible = false;
            this.buttonTestSupplItems.Click += new System.EventHandler(this.buttonTestSupplItems_Click);
            // 
            // numericSupplyNBR
            // 
            this.numericSupplyNBR.Location = new System.Drawing.Point(605, 123);
            this.numericSupplyNBR.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.buttonGetSupplierFromNBR.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonGetSupplierFromNBR.Location = new System.Drawing.Point(605, 146);
            this.buttonGetSupplierFromNBR.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonGetSupplierFromNBR.Name = "buttonGetSupplierFromNBR";
            this.buttonGetSupplierFromNBR.Size = new System.Drawing.Size(101, 27);
            this.buttonGetSupplierFromNBR.TabIndex = 12;
            this.buttonGetSupplierFromNBR.Text = "Get Products";
            this.buttonGetSupplierFromNBR.UseVisualStyleBackColor = false;
            this.buttonGetSupplierFromNBR.Click += new System.EventHandler(this.buttonGetSupplierFromNBR_Click);
            // 
            // labelSupplierFreeText
            // 
            this.labelSupplierFreeText.AutoSize = true;
            this.labelSupplierFreeText.Location = new System.Drawing.Point(603, 98);
            this.labelSupplierFreeText.Name = "labelSupplierFreeText";
            this.labelSupplierFreeText.Size = new System.Drawing.Size(114, 17);
            this.labelSupplierFreeText.TabIndex = 13;
            this.labelSupplierFreeText.Text = "Supplier Number";
            // 
            // labelYear
            // 
            this.labelYear.AutoSize = true;
            this.labelYear.Location = new System.Drawing.Point(805, 98);
            this.labelYear.Name = "labelYear";
            this.labelYear.Size = new System.Drawing.Size(38, 17);
            this.labelYear.TabIndex = 15;
            this.labelYear.Text = "Year";
            // 
            // comboBoxYear
            // 
            this.comboBoxYear.FormattingEnabled = true;
            this.comboBoxYear.Location = new System.Drawing.Point(805, 123);
            this.comboBoxYear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxYear.Name = "comboBoxYear";
            this.comboBoxYear.Size = new System.Drawing.Size(79, 24);
            this.comboBoxYear.TabIndex = 14;
            this.comboBoxYear.SelectedIndexChanged += new System.EventHandler(this.comboBoxYear_SelectedIndexChanged);
            // 
            // buttonGetProductByNumber
            // 
            this.buttonGetProductByNumber.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonGetProductByNumber.Location = new System.Drawing.Point(13, 145);
            this.buttonGetProductByNumber.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonGetProductByNumber.Name = "buttonGetProductByNumber";
            this.buttonGetProductByNumber.Size = new System.Drawing.Size(101, 27);
            this.buttonGetProductByNumber.TabIndex = 17;
            this.buttonGetProductByNumber.Text = "Get Product";
            this.buttonGetProductByNumber.UseVisualStyleBackColor = false;
            this.buttonGetProductByNumber.Click += new System.EventHandler(this.buttonGetProductByNumber_Click);
            // 
            // buttonCreateM3LactalisOrders
            // 
            this.buttonCreateM3LactalisOrders.Location = new System.Drawing.Point(1000, 121);
            this.buttonCreateM3LactalisOrders.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCreateM3LactalisOrders.Name = "buttonCreateM3LactalisOrders";
            this.buttonCreateM3LactalisOrders.Size = new System.Drawing.Size(112, 23);
            this.buttonCreateM3LactalisOrders.TabIndex = 18;
            this.buttonCreateM3LactalisOrders.Text = "create orders";
            this.buttonCreateM3LactalisOrders.UseVisualStyleBackColor = true;
            this.buttonCreateM3LactalisOrders.Visible = false;
            this.buttonCreateM3LactalisOrders.Click += new System.EventHandler(this.buttonCreateM3LactalisOrders_Click);
            // 
            // checkBoxLastYear
            // 
            this.checkBoxLastYear.AutoSize = true;
            this.checkBoxLastYear.Location = new System.Drawing.Point(436, 150);
            this.checkBoxLastYear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxLastYear.Name = "checkBoxLastYear";
            this.checkBoxLastYear.Size = new System.Drawing.Size(123, 21);
            this.checkBoxLastYear.TabIndex = 19;
            this.checkBoxLastYear.Text = "Load LastYear";
            this.checkBoxLastYear.UseVisualStyleBackColor = true;
            this.checkBoxLastYear.CheckedChanged += new System.EventHandler(this.checkBoxLastYear_CheckedChanged);
            this.checkBoxLastYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.buttonGetProductsBySupplier_KeyPress);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(804, 151);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(181, 25);
            this.labelStatus.TabIndex = 21;
            this.labelStatus.Text = "Loading Numbers";
            this.labelStatus.Visible = false;
            // 
            // buttonCreateLactalisFile
            // 
            this.buttonCreateLactalisFile.Location = new System.Drawing.Point(1000, 75);
            this.buttonCreateLactalisFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCreateLactalisFile.Name = "buttonCreateLactalisFile";
            this.buttonCreateLactalisFile.Size = new System.Drawing.Size(139, 23);
            this.buttonCreateLactalisFile.TabIndex = 22;
            this.buttonCreateLactalisFile.Text = "create Lacatlis File";
            this.buttonCreateLactalisFile.UseVisualStyleBackColor = true;
            this.buttonCreateLactalisFile.Visible = false;
            this.buttonCreateLactalisFile.Click += new System.EventHandler(this.buttonCreateLactalisFile_Click);
            // 
            // textBoxProdNBR
            // 
            this.textBoxProdNBR.Location = new System.Drawing.Point(13, 122);
            this.textBoxProdNBR.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxProdNBR.Name = "textBoxProdNBR";
            this.textBoxProdNBR.Size = new System.Drawing.Size(100, 22);
            this.textBoxProdNBR.TabIndex = 23;
            this.textBoxProdNBR.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxProdNBR_KeyDown);
            // 
            // labelCalculator
            // 
            this.labelCalculator.AutoSize = true;
            this.labelCalculator.Location = new System.Drawing.Point(12, 829);
            this.labelCalculator.Name = "labelCalculator";
            this.labelCalculator.Size = new System.Drawing.Size(102, 17);
            this.labelCalculator.TabIndex = 25;
            this.labelCalculator.Text = "Regneformler: ";
            // 
            // textBoxCalculator
            // 
            this.textBoxCalculator.Location = new System.Drawing.Point(140, 826);
            this.textBoxCalculator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxCalculator.Name = "textBoxCalculator";
            this.textBoxCalculator.Size = new System.Drawing.Size(226, 22);
            this.textBoxCalculator.TabIndex = 24;
            this.textBoxCalculator.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCalculator_KeyPress);
            // 
            // checkBoxKampgn
            // 
            this.checkBoxKampgn.AutoSize = true;
            this.checkBoxKampgn.Location = new System.Drawing.Point(436, 125);
            this.checkBoxKampgn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxKampgn.Name = "checkBoxKampgn";
            this.checkBoxKampgn.Size = new System.Drawing.Size(137, 21);
            this.checkBoxKampgn.TabIndex = 26;
            this.checkBoxKampgn.Text = "Load Promotions";
            this.checkBoxKampgn.UseVisualStyleBackColor = true;
            // 
            // FormSupply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaGreen;
            this.ClientSize = new System.Drawing.Size(1483, 855);
            this.Controls.Add(this.checkBoxKampgn);
            this.Controls.Add(this.labelCalculator);
            this.Controls.Add(this.textBoxCalculator);
            this.Controls.Add(this.textBoxProdNBR);
            this.Controls.Add(this.buttonCreateLactalisFile);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.checkBoxLastYear);
            this.Controls.Add(this.buttonCreateM3LactalisOrders);
            this.Controls.Add(this.buttonGetProductByNumber);
            this.Controls.Add(this.labelYear);
            this.Controls.Add(this.comboBoxYear);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormSupply";
            this.Text = "Forecast Supply ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormSupply_FormClosed);
            this.SizeChanged += new System.EventHandler(this.FormSupply_SizeChanged);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.buttonGetProductsBySupplier_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridForecastInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSupplyNBR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView dataGridForecastInfo;
        private System.Windows.Forms.Button buttonSalesView;
        private System.Windows.Forms.Button buttonGetProductsBySupplier;
        private System.Windows.Forms.Label labelSupplier;
        private System.Windows.Forms.ComboBox comboBoxSupplier;
        private System.Windows.Forms.Button buttonTestSupplItems;
        private System.Windows.Forms.NumericUpDown numericSupplyNBR;
        private System.Windows.Forms.Button buttonGetSupplierFromNBR;
        private System.Windows.Forms.Label labelSupplierFreeText;
        private System.Windows.Forms.Label labelYear;
        private System.Windows.Forms.ComboBox comboBoxYear;
        private System.Windows.Forms.Button buttonGetProductByNumber;
        private System.Windows.Forms.Button buttonCreateM3LactalisOrders;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVareNR;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnType;
        private System.Windows.Forms.CheckBox checkBoxLastYear;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button buttonCreateLactalisFile;
        private System.Windows.Forms.TextBox textBoxProdNBR;
        private System.Windows.Forms.Label labelCalculator;
        private System.Windows.Forms.TextBox textBoxCalculator;
        private System.Windows.Forms.CheckBox checkBoxKampgn;
    }
}