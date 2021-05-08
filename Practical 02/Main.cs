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
            

            List<string> errorsTaff = new List<string>();
            List<string> errorsCff = new List<string>(); ;
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

                    String CffData = fileReader.Readcff(cffFilename, out errorsCff);
                    cff.CffBrowser.DocumentText = CffData;
                    cff.Show();

                    
                            
                    configuration = new Configuration(cffFilename);

                    if (configuration.Validate())
                    {
                         logFile = configuration.Logfile;

                        if (File.Exists(logFile))
                        {
                            File.Delete(logFile);
                        }

                        ErrorList = errorsTaff.Concat(errorsCff).ToList();



                        using (StreamWriter sw = File.CreateText(logFile))
                        {
                            foreach (string error in ErrorList)
                            {
                                sw.WriteLine(error);
                            }
                        }

                        if (ErrorList.Count == 0)
                        {

                        }

                    }
                    else
                    {
                        errorsCff.Add("NO LOG FILE FOUND");
                        string line = "";
                        
                        foreach (string element in ErrorList)
                        {
                            line += "<p>" + element + "</p>";
                        }

                        errorWindow.getBrowser.DocumentText = line;
                        errorWindow.Show();

                    }
                        


                    if (errorsCff.Count == 0 && errorsTaff.Count == 0)
                    {
                        allocationsToolStripMenuItem.Enabled = true;
                    }

                }
                else
                {
                    errorsTaff.Add("NO CONFIGURATION DATA FOUND ");
                    string line = "";
                    foreach(string element in errorsTaff)
                    {
                       line += "<p>" + element + "</p>";
                    }

                    errorWindow.getBrowser.DocumentText = line;
                    errorWindow.Show();
                }

               





                //CREATE CONFIGURATION OBJECT

                //configuration = new Configuration(taskAllocation.cffFilename);














                //if (taskAllocation.Validate(taffFilename))
                //{
                //    String line = fileReader.Readcff(cffFilename, out errorsCff);
                //   
                //    cff.Show();

                //    if (errors.Count == 0)
                //    {
                //        allocationsToolStripMenuItem.Enabled = true;
                //    }

                //   List<string> errorsConcat = err.Concat(errors).ToList();

                //   configuration.Validate();
                //   string fileName = configuration.Logfile;

                //    if (File.Exists(fileName))
                //    {
                //        File.Delete(fileName);
                //    }


                //    using (StreamWriter sw = File.CreateText(fileName))
                //    {
                //        foreach (string error in errorsConcat)
                //        {
                //            sw.WriteLine(errorsConcat);
                //        }
                //    }





                //}








                //string path = @"D:\test.txt";
                //if (!File.Exists(path))
                //{
                //    // Create a file to write to.
                //    using (StreamWriter sw = File.CreateText(path))
                //    {
                //        foreach  (string error in errors)
                //        {
                //            sw.WriteLine(error);
                //        }
                //    }
                //}

                //errorWindow.getBrowser.DocumentText = "";







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

            configuration.GetRemoteCommunication();
            configuration.GetLocalCommunication();
            taskAllocation.GetAllocations(taffFilename);

            configuration.GetProcessors();
            configuration.GetTasks();
            


            Validator validator = new Validator();

            IDictionary<Allocation, double[]> timeEnergy = validator.valid(taskAllocation, configuration);
            string Time = "";
            String id = "";
            double Energy =0d;
            
            string test = validator.ValidationAllocations(taskAllocation, configuration);

            foreach (var time in timeEnergy)
            {
                id = time.Key.ID.ToString() + " "+',';
                Time = time.Value[int.Parse(Keywords.TOTALTIME)].ToString()+ " " + ',';
                Energy = time.Value[int.Parse(Keywords.TOTALENERGY)];

                break;
            }


            //allocationWindow.validationBrowser.DocumentText = "ENERGY " + String.Format("{0:0.##}", Energy)+ "Times " + Time+ "ID " + id;
            allocationWindow.validationBrowser.DocumentText = test;

            allocationWindow.Show();

        }
    }
}
