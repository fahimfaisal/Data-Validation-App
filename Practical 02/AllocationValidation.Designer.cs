
namespace DataValidation
{
    partial class AllocationValidation
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
            this.validationBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // validationBrowser
            // 
            this.validationBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.validationBrowser.Location = new System.Drawing.Point(0, 0);
            this.validationBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.validationBrowser.Name = "validationBrowser";
            this.validationBrowser.Size = new System.Drawing.Size(800, 450);
            this.validationBrowser.TabIndex = 0;
            // 
            // AllocationValidation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.validationBrowser);
            this.Name = "AllocationValidation";
            this.Text = "AllocationValidation";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.WebBrowser validationBrowser;
    }
}