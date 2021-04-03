using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DataValidation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // OPEN A TASK ALLOCATION FILE 

            DialogResult result = filedialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                String path = filedialog.FileName;

                

                FileReader fileReader = new FileReader();
               

                String data = fileReader.readData(path);
               

                webBrowser1.DocumentText = data;

          
            }


        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.ShowDialog();
        }

        private void errorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Errors errorWindow = new Errors();

            errorWindow.getBrowser.DocumentText = "new test";
              errorWindow.Show();

            
        }
    }
}
