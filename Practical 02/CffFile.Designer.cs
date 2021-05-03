
namespace DataValidation
{
    partial class CffFile
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
            this.CffBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // CffBrowser
            // 
            this.CffBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CffBrowser.Location = new System.Drawing.Point(0, 0);
            this.CffBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.CffBrowser.Name = "CffBrowser";
            this.CffBrowser.Size = new System.Drawing.Size(800, 450);
            this.CffBrowser.TabIndex = 0;
            // 
            // CffFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CffBrowser);
            this.Name = "CffFile";
            this.Text = "CFF File";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.WebBrowser CffBrowser;
    }
}