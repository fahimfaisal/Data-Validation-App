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
        String taffFilename;
        Configuration configuration;
        TaskAllocation taskAllocation;
        
        Errors errorWindow = new Errors();
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
            FileReader fileReader = new FileReader();
            
            string logFile;
            

            List<string> errorsTaff = new List<string>() ;
            List<string> errorsCff = new List<string>() ;
            List<string> ErrorList = new List<string>();


            DialogResult result = filedialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                taffFilename = filedialog.FileName;

                MessageBox.Show(taffFilename);

                String taffData = fileReader.readTaff(taffFilename, out errorsTaff);

                taskAllocation = new TaskAllocation();

                webBrowser1.DocumentText = taffData;


                if (taskAllocation.getCffFilename(taffFilename))
                {
                    cffFilename = taskAllocation.cffFilename;

                    MessageBox.Show(taskAllocation.cffFilename);

                    configuration = new Configuration(cffFilename);
                    


                    String CffData = fileReader.Readcff(cffFilename, out errorsCff);

                    cff.CffBrowser.DocumentText = CffData;
                    cff.Show();
                    

                    ErrorList = errorsTaff.Concat(errorsCff).ToList();
                    
                    if (configuration.Validate())
                    {

                        logFile = configuration.Logfile;
                        string line = "";


                        if (ErrorList.Count == 0)
                        {
                            configuration.Validate();                       
                            configuration.GetConfigurations();
                            taskAllocation.GetAllocations(taffFilename);

                          
                            if (configuration.Errors.Count.Equals(0))
                            {
                                allocationsToolStripMenuItem.Enabled = true;
                            }
                            else
                            {
                                if (File.Exists(logFile))
                                {
                                    File.Delete(logFile);
                                }

                                using (StreamWriter sw = File.CreateText(logFile))
                                {
                                    foreach (string error in configuration.Errors)
                                    {
                                        line += "<p>" + error + "</p>";
                                        sw.WriteLine(error);
                                    }
                                }

                                errorWindow.getBrowser.DocumentText = line;
                                errorWindow.Show();

                            }
                           

                           
                        }
                        else
                        {


                            if (File.Exists(logFile))
                            {
                                File.Delete(logFile);
                            }

                            using (StreamWriter sw = File.CreateText(logFile))
                            {
                                foreach (string error in ErrorList)
                                {
                                    line += "<p>" + error + "</p>";
                                    sw.WriteLine(error);
                                }
                            }

                            errorWindow.getBrowser.DocumentText = line;
                            

                        }

                    }
                    else
                    {
                        string line = "<p>INVALID LOG FILE NAME</p>";
                        foreach (string error in ErrorList)
                        {
                            line += "<p>" + error + "</p>";
                        }


                        errorWindow.getBrowser.DocumentText = line;
                        errorWindow.Show();
                    }




                }
                else
                {
                    string line = "<p>NO CONFIGURATION DATA FOUND </p>";
                    foreach (string error in errorsTaff)
                    {
                        line += "<p>" + error + "</p>";
                    }


                    errorWindow.getBrowser.DocumentText = line;
                    errorWindow.Show();
                }
            }  
            
            

            


        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void errorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            errorWindow.Show();


        }

        private void allocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllocationValidation allocationWindow = new AllocationValidation();

           
            
            
        



            Validator validator = new Validator();

            string line = validator.ValidationAllocations(taskAllocation, configuration);
           
           


            allocationWindow.validationBrowser.DocumentText = line;

            allocationWindow.Show();

        }
    }
}
