﻿using System;
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
    public partial class Main : Form
    {
        public Main()
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
                String taffFilename= filedialog.FileName;

                FileReader fileReader = new FileReader();
                TaskAllocation taskAllocation = new TaskAllocation();
                Configuration configuration = new Configuration();

                //taskAllocation.cffFilename;

                if(taskAllocation.getCffFilename(taffFilename))
                {


                    allocationsToolStripMenuItem.Enabled = true;

                    MessageBox.Show(taskAllocation.cffFilename);
                   
                    if (taskAllocation.Validate(taffFilename) && configuration.Validate(taskAllocation.cffFilename))
                    {

                        MessageBox.Show(configuration.Logfile);
                    }

                }




                String data = fileReader.readData(taffFilename);
               

                webBrowser1.DocumentText = data;

          
            }


        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void errorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Errors errorWindow = new Errors();

            errorWindow.getBrowser.DocumentText = "error";
            
            errorWindow.Show();

            
        }
    }
}