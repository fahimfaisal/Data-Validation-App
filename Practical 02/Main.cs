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
using DataValidation.ConfigurationClasses;
using DataValidation.TaskAllocationClasses;

namespace DataValidation
{
    public partial class Main : Form
    {
        String cffFilename;
         public String taffFilename;
        Configuration configuration;
        TaskAllocation taskAllocation;
        
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

            CffFile cff = new CffFile();
            
            DialogResult result = filedialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                taffFilename= filedialog.FileName;
                
                FileReader fileReader = new FileReader();
                taskAllocation = new TaskAllocation();
               

                //taskAllocation.cffFilename;

                if(taskAllocation.getCffFilename(taffFilename))
                {

         
                    cffFilename = taskAllocation.cffFilename;

                    //CREATE CONFIGURATION OBJECT

                    configuration = new Configuration(taskAllocation.cffFilename);
                    

                    allocationsToolStripMenuItem.Enabled = true;

                    MessageBox.Show(taskAllocation.cffFilename);
                   
                    if (taskAllocation.Validate(taffFilename) && configuration.Validate())
                    {

                        MessageBox.Show(configuration.Logfile);
                    }

                }




                String data = fileReader.readData(taffFilename);
                String line = fileReader.readData(cffFilename);

                webBrowser1.DocumentText = data;
                cff.CffBrowser.DocumentText = line;

                cff.Show();
          
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

        private void allocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllocationValidation allocationWindow = new AllocationValidation();


            taskAllocation.GetAllocations(taffFilename);

            configuration.GetProcessors();
            configuration.GetTasks();

            Validator validator = new Validator();

            IDictionary<Allocation, double[]> timeEnergy = validator.TimeValidation(taskAllocation, configuration);
            string Time = "";
            String id = "";
            double Energy =0d;
            foreach (var time in timeEnergy)
            {
                id = time.Key.ID.ToString() + " "+',';
                Time = time.Value[int.Parse(Keywords.TOTALTIME)].ToString()+ " " + ',';
                Energy = time.Value[int.Parse(Keywords.TOTALENERGY)];

                break;
            }


            allocationWindow.validationBrowser.DocumentText = "ENERGY " + String.Format("{0:0.##}", Energy)+ "Times " + Time+ "ID " + id;

            allocationWindow.Show();

        }
    }
}
