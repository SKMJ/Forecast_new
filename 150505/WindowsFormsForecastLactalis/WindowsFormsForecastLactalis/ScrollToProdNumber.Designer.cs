namespace WindowsFormsForecastLactalis
{
    partial class ScrollToProdNumber
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
            this.textBoxNumberSearch = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxNumberSearch
            // 
            this.textBoxNumberSearch.Location = new System.Drawing.Point(1, 12);
            this.textBoxNumberSearch.Name = "textBoxNumberSearch";
            this.textBoxNumberSearch.Size = new System.Drawing.Size(250, 22);
            this.textBoxNumberSearch.TabIndex = 0;
            this.textBoxNumberSearch.TextChanged += new System.EventHandler(this.textBoxNumberSearch_TextChanged);
            this.textBoxNumberSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumberSearch_KeyPress);
            // 
            // ScrollToProdNumber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 50);
            this.Controls.Add(this.textBoxNumberSearch);
            this.Name = "ScrollToProdNumber";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Prod Number";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxNumberSearch;
    }
}