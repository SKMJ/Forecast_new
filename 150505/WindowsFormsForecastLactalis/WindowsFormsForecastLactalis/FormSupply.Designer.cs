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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridForecastInfo)).BeginInit();
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
            this.dataGridForecastInfo.Size = new System.Drawing.Size(1051, 661);
            this.dataGridForecastInfo.TabIndex = 3;
            this.dataGridForecastInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridForecastInfo_CellContentClick);
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
            this.buttonSalesView.Location = new System.Drawing.Point(539, 52);
            this.buttonSalesView.Name = "buttonSalesView";
            this.buttonSalesView.Size = new System.Drawing.Size(179, 34);
            this.buttonSalesView.TabIndex = 4;
            this.buttonSalesView.Text = "Change to Sales View";
            this.buttonSalesView.UseVisualStyleBackColor = true;
            this.buttonSalesView.Click += new System.EventHandler(this.buttonSalesView_Click);
            // 
            // FormSupply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.ClientSize = new System.Drawing.Size(1082, 855);
            this.Controls.Add(this.buttonSalesView);
            this.Controls.Add(this.dataGridForecastInfo);
            this.Controls.Add(this.pictureBox1);
            this.Name = "FormSupply";
            this.Text = "Forecast Supply ";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridForecastInfo)).EndInit();
            this.ResumeLayout(false);

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
    }
}