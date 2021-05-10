using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using DataValidation.ConfigurationClasses;


namespace DataValidation
{
    class Configuration
    {

        public string cffFilename;

        public string Logfile { get; set; }
        
        public List<string> Errors = new List<string>(); 

        public List<Processor> Processors { get; set; }

        public List<Task> Tasks { get; set; }
        
       
        public List<ProcessorType> processorTypes;

        public  double[,] LocalCommunication {get;set;}


        public double[,] RemoteCommunication { get; set; }

        IDictionary<String, Double> Limits { get; set; }

        public IDictionary<String, Double> Program { get; set; }



        
        
        public Configuration(String cffFilename)
        {
            this.cffFilename = cffFilename;
        }

        



        /// <summary>
        /// Get the log file name from the cff file
        /// </summary>
        /// <returns>true if the log file found</returns>


        public Boolean Validate()
        {

            
            bool found = false;
            StreamReader sr = new StreamReader(cffFilename);
            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                line = line.Trim();
               
                if (line.Length == 0)
                {

                }
                else if (line.StartsWith("//"))
                {

                }
                else if (line.StartsWith(Keywords._default))
                {
                    String[] data = line.Split('=');

                    if (data.Length == 2)
                    {
                        if (data[1].StartsWith("\"") && data[1].EndsWith("\""))
                        {
                            line = data[1].Trim('"');

                            if (line.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
                            {
                                String path = Path.GetDirectoryName(cffFilename);
                                Logfile = path + Path.DirectorySeparatorChar + line;

                                found = true;
                            }
                          
                        }
                        
                    }
                   
                    
                }
            
            }
            sr.Close();

            return found;


            

        }


        /// <summary>
        /// Get all the configuration objects
        /// </summary>

        public void GetConfigurations()
        {
            GetLimit();
            GetProgram();
            GetTasks();
            GetProcessors();
            GetLocalCommunication();
            GetRemoteCommunication();
                
        }
      

        public void GetTasks()
        {


            StreamReader sr = new StreamReader(cffFilename);


            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                line = line.Trim();
                
                if (line.StartsWith(Keywords.tasks))
                {
                     Tasks = new List<Task>();

                    while (!line.StartsWith(Keywords.tasks_end))
                    {
                        line = sr.ReadLine();
                        line = line.Trim();

                        if (line.StartsWith(Keywords.task))
                        {
                            Task task = new Task();

                            while (!line.StartsWith(Keywords.task_end))
                            {
                                line = sr.ReadLine();
                                line = line.Trim();

                                if (line.StartsWith(Keywords.id))
                                {
                                    String[] data = line.Split('=');

                                    task.ID = int.Parse(data[1]);

                                }

                                if (line.StartsWith(Keywords.runtime))
                                {

                                    String[] data = line.Split('=');

                                    task.Runtime = double.Parse(data[1]);
                                    
                                    
                                   

                                }

                                if (line.StartsWith(Keywords.reference_frequency))
                                {
                                    String[] data = line.Split('=');

                                    task.ReferenceFrequency = double.Parse(data[1]);




                                }

                                if (line.StartsWith(Keywords.ram))
                                {


                                    String[] data = line.Split('=');
                                    double minimumRam;
                                    double maximumRam;

                                    task.Ram = int.Parse(data[1]);

                                    bool minimum = Limits.TryGetValue(Keywords.minimum_ram , out minimumRam);

                                    bool maximum = Limits.TryGetValue(Keywords.maximum_ram, out maximumRam);

                                    if (minimum && maximum)
                                    {
                                        if (double.Parse(data[1]) < minimumRam || double.Parse(data[1]) > maximumRam)
                                        {
                                            Errors.Add("RAM OF TASK " + "ID : " + task.ID+ "IS NOT IN RANGE ");
                                        }
                                    }


                                }

                                if (line.StartsWith(Keywords.download))
                                {
                                  

                                   

                                    String[] data = line.Split('=');
                                    double minimumDownload;
                                    double maximumDownload;
                                    
                                    task.Download = int.Parse(data[1]);

                                    bool minimum = Limits.TryGetValue(Keywords.minimum_download , out minimumDownload);

                                    bool maximum = Limits.TryGetValue(Keywords.maximum_download, out maximumDownload);

                                    if (minimum && maximum)
                                    {
                                        if (double.Parse(data[1]) < minimumDownload || double.Parse(data[1]) > maximumDownload)
                                        {
                                            Errors.Add("DOWNLOAD SPEED OF TASK " + "ID : " + task.ID + " IS NOT IN RANGE ");
                                        }
                                    }



                                }

                                if (line.StartsWith(Keywords.upload))
                                {
                                   
                                    String[] data = line.Split('=');
                                    double minimumUpload;
                                    double maximumUpload;

                                    task.Upload = int.Parse(data[1]);

                                    bool minimum = Limits.TryGetValue(Keywords.minimum_upload, out minimumUpload);

                                    bool maximum = Limits.TryGetValue(Keywords.maximum_upload, out maximumUpload);

                                    if (minimum && maximum)
                                    {
                                        if (double.Parse(data[1]) < minimumUpload || double.Parse(data[1]) > maximumUpload)
                                        {
                                            Errors.Add("UPLOAD SPEED OF TASK " + "ID : " + task.ID + " IS NOT IN RANGE ");
                                        }
                                    }



                                }

                             

                            }

                            Tasks.Add(task);
                        }


                    }

                }

            }
            
        }


        public void GetProcessors()
        {
            StreamReader sr = new StreamReader(cffFilename);

            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                line = line.Trim();

                if (line.StartsWith(Keywords.processors))
                {
                    Processors = new List<Processor>();
                    
                    while (!line.StartsWith(Keywords.processors_end))
                    {
                        line = sr.ReadLine();
                        line = line.Trim();

                        if (line.StartsWith(Keywords.processor))
                        {

                            Processor processor = new Processor();

                            while (!line.StartsWith(Keywords.processor_end))
                            {
                                line = sr.ReadLine();
                                line = line.Trim();

                                if (line.StartsWith(Keywords.id))
                                {
                                    String[] data = line.Split('=');

                                    processor.ID = int.Parse(data[1]);
                                }


                                if (line.StartsWith(Keywords.type))
                                {
                                    if (processorTypes == null)
                                    {
                                        GetProcessorType();
                                    }
                                  

                                    String[] data = line.Split('=');
                                    foreach(ProcessorType processorType in processorTypes)
                                    {
                                        if (data[1].Trim().Trim('"').Equals(processorType.Name))
                                        {
                                            processor.Type = processorType;

                                        }
                                        
                                    }
                                        
                                }


                                if (line.StartsWith(Keywords.frequency))
                                {
                                    String[] data = line.Split('=');

                                    processor.Frequency = double.Parse(data[1]);
                                }


                                if (line.StartsWith(Keywords.ram))
                                {

                                    String[] data = line.Split('=');
                                    double minimumRam;
                                    double maximumRam;

                                    processor.Ram = int.Parse(data[1]);

                                    bool minimum = Limits.TryGetValue(Keywords.minimum_ram, out minimumRam);

                                    bool maximum = Limits.TryGetValue(Keywords.maximum_ram, out maximumRam);

                                    if (minimum && maximum)
                                    {
                                        if (double.Parse(data[1]) < minimumRam || double.Parse(data[1]) > maximumRam)
                                        {
                                            Errors.Add("RAM OF PROCESSOR " + "ID : " + processor.ID + "IS NOT IN RANGE ");
                                        }
                                    }


                                    
                                }

                                if (line.StartsWith(Keywords.download))
                                {
                                    String[] data = line.Split('=');
                                    double minimumDownload;
                                    double maximumDownload;

                                    processor.Download = int.Parse(data[1]);

                                    bool minimum = Limits.TryGetValue(Keywords.minimum_download, out minimumDownload);

                                    bool maximum = Limits.TryGetValue(Keywords.maximum_download, out maximumDownload);

                                    if (minimum && maximum)
                                    {
                                        if (double.Parse(data[1]) < minimumDownload || double.Parse(data[1]) > maximumDownload)
                                        {
                                            Errors.Add("DOWNLOAD SPEED OF PROCESSOR " + "ID : " + processor.ID + "IS NOT IN RANGE ");
                                        }
                                    }

                                    
                                }

                                if (line.StartsWith(Keywords.upload))
                                {
                                    String[] data = line.Split('=');
                                    double minimumUpload;
                                    double maximumUpload;

                                    processor.Upload = int.Parse(data[1]);

                                    bool minimum = Limits.TryGetValue(Keywords.minimum_upload, out minimumUpload);

                                    bool maximum = Limits.TryGetValue(Keywords.maximum_upload, out maximumUpload);

                                    if (minimum && maximum)
                                    {
                                        if (double.Parse(data[1]) < minimumUpload || double.Parse(data[1]) > maximumUpload)
                                        {
                                            Errors.Add("UPLOAD SPEED OF PROCESSOR " + "ID : " + processor.ID + "IS NOT IN RANGE ");
                                        }
                                    }

                                   
                                }




                            }

                            Processors.Add(processor);
                        }

                    }


                }
            }
            
          

        }

        public void GetProcessorType()
        {
            StreamReader sr = new StreamReader(cffFilename);
            
            while(!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                line = line.Trim();

                if (line.StartsWith(Keywords.processor_types))
                {
                    processorTypes = new List<ProcessorType>();
                    
                    while (!line.StartsWith(Keywords.processortypes_end))
                    {
                        line = sr.ReadLine();
                        line = line.Trim();

                        if (line.StartsWith(Keywords.processor_type))
                        {
                            ProcessorType processorType = new ProcessorType();

                            while (!line.StartsWith(Keywords.processortype_end))
                            {
                                line = sr.ReadLine();
                                line = line.Trim();

                                if (line.StartsWith(Keywords.name))
                                {
                                    String[] data = line.Split('=');
                                    String name = data[1].Trim().Trim('"');
                                    processorType.Name = name;
                                 
                                }

                                if (line.StartsWith(Keywords.c2))
                                {
                                    String[] data = line.Split('=');
                                    processorType.C2 = double.Parse(data[1]);
                                }
                                if (line.StartsWith(Keywords.c1))
                                {
                                    String[] data = line.Split('=');
                                    processorType.C1 = double.Parse(data[1]);
                                }
                                if (line.StartsWith(Keywords.c0))
                                {
                                    String[] data = line.Split('=');
                                    processorType.C0 = double.Parse(data[1]);
                                }
                            

                            }

                            processorTypes.Add(processorType);

                        }
                    }

                }

            }
            

        }

        public void GetLocalCommunication()
        {
            StreamReader sr = new StreamReader(cffFilename);

            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                line = line.Trim();

                if (line.StartsWith(Keywords.localCommunication))
                {
                    while(!line.StartsWith(Keywords.localCommunication_end))
                    {
                        line = sr.ReadLine();
                        line = line.Trim();

                        if (line.StartsWith(Keywords.map))
                        {
                            String[] data = line.Split('=');

                            String[] taskRow = data[1].Split(';');

                            String[] taskColumn = taskRow[0].Split(',');

                            LocalCommunication = new double[taskRow.Length, taskColumn.Length];

                            for (int i = 0; i < LocalCommunication.GetLength(0); i++)
                            {
                                String[] number = taskRow[i].Split(',');

                                for (int j = 0; j < LocalCommunication.GetLength(1); j++)
                                {
                                    if (i.Equals(j))
                                    {
                                        if (!double.Parse(number[j]).Equals(0))
                                        {
                                            Errors.Add("A TASK CANNOT COMMUNICATE WITH ITSELF LOCALLY");
                                        }

                                    }

                                    LocalCommunication[i, j] = double.Parse(number[j]);
                                }

                            }

                            if (LocalCommunication.GetLength(0) != LocalCommunication.GetLength(1))
                            {
                                Errors.Add("ERROR IN LOCAL COMMUNICATION MAPPING");
                            }

                        }
                    }

                }

            }
            
        }

        public void GetRemoteCommunication()
        {
            StreamReader sr = new StreamReader(cffFilename);

            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                line = line.Trim();

                if (line.StartsWith(Keywords.remoteCommunication))
                {
                    while (!line.StartsWith(Keywords.remoteCommunication_end))
                    {
                        line = sr.ReadLine();
                        line = line.Trim();

                        if (line.StartsWith(Keywords.map))
                        {
                            String[] data = line.Split('=');

                            String[] taskRow = data[1].Split(';');

                            String[] taskColumn = taskRow[0].Split(',');

                            RemoteCommunication = new double[taskRow.Length, taskColumn.Length];

                            for (int i = 0; i < RemoteCommunication.GetLength(0); i++)
                            {
                                String[] number = taskRow[i].Split(',');

                                for (int j = 0; j < RemoteCommunication.GetLength(1); j++)
                                {
                                    if (i.Equals(j))
                                    {
                                        if (!double.Parse(number[j]).Equals(0))
                                        {
                                            Errors.Add("A TASK CANNOT COMMUNICATE WITH ITSELF REMOTELY");
                                        }
                                       
                                    }

                                    RemoteCommunication[i, j] = double.Parse(number[j]);
                                }

                            }

                            if (RemoteCommunication.GetLength(0) != RemoteCommunication.GetLength(1))
                            {
                                Errors.Add("ERROR IN REMOTE COMMUNICATION MAPPING");
                            }

                        }
                    }

                }
                
            }
        }

        public void GetLimit()
        {
            StreamReader sr = new StreamReader(cffFilename);
            

            while(!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                line = line.Trim();
                if (line.StartsWith(Keywords.limits))
                {
                    Limits = new Dictionary<String, Double>();
                    while(!line.StartsWith(Keywords.limits_end))
                    {
                        line = sr.ReadLine();
                        line = line.Trim();

                        if (line.StartsWith(Keywords.minimum_tasks))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith(Keywords.maximum_tasks))
                        {
                            String[] data = line.Split('=');
                            double value;
                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));
                            
                            bool rangeCheck= Limits.TryGetValue(Keywords.minimum_tasks ,out value);
                            
                            if(rangeCheck)
                            {
                                if (value > double.Parse(data[1]))
                                {
                                    Errors.Add(Keywords.maximum_tasks + " is less than " + " MINIMUM TASKS");
                                }
                            }
                            else
                            {
                                Errors.Add(Keywords.minimum_tasks + " NOT PROVIDED");
                            }
                        
                        }

                        if (line.StartsWith(Keywords.minimum_processors))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith(Keywords.maximum_processors))
                        {
                            String[] data = line.Split('=');
                            double value;
                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));       
                           
                           

                            bool rangeCheck = Limits.TryGetValue(Keywords.minimum_processors, out value);

                            if (rangeCheck)
                            {
                                if (value > double.Parse(data[1]))
                                {
                                    Errors.Add(Keywords.maximum_processors + " is less than " + "MINIMUM-PROCESSORS");
                                }
                            }
                            else
                            {
                                Errors.Add(Keywords.minimum_processors + " NOT PROVIDED");
                            }
                        }


                        if (line.StartsWith(Keywords.minimum_procfrequencies))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith(Keywords.maximum_procfrequencies))
                        {
                            String[] data = line.Split('=');
                            double value;
                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));



                            bool rangeCheck = Limits.TryGetValue(Keywords.minimum_procfrequencies, out value);

                            if (rangeCheck)
                            {
                                if (value > double.Parse(data[1]))
                                {
                                    Errors.Add(Keywords.minimum_procfrequencies + " is less than" + " MAXIMUM-PROCESSOR-FREQUENCIES");
                                }
                            }
                            else
                            {
                                Errors.Add(Keywords.minimum_procfrequencies + " NOT PROVIDED");
                            }

                        }

                    

                        if (line.StartsWith(Keywords.minimum_ram))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith(Keywords.maximum_ram))
                        {
                            String[] data = line.Split('=');
                            double value;
                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));



                            bool rangeCheck = Limits.TryGetValue(Keywords.minimum_ram, out value);

                            if (rangeCheck)
                            {
                                if (value > double.Parse(data[1]))
                                {
                                    Errors.Add(Keywords.maximum_ram + " is less than" + " MINIMUM-RAM");
                                }
                            }
                            else
                            {
                                Errors.Add(Keywords.minimum_ram + " NOT PROVIDED");
                            }

                        }

                        if (line.StartsWith(Keywords.minimum_download))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith(Keywords.maximum_download))
                        {
                            String[] data = line.Split('=');
                            double value;
                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));



                            bool rangeCheck = Limits.TryGetValue(Keywords.minimum_download, out value);

                            if (rangeCheck)
                            {
                                if (value > double.Parse(data[1]))
                                {
                                    Errors.Add(Keywords.maximum_download + " is less than" + " MINIMUM-DOWNLOAD");
                                }
                            }
                            else
                            {
                                Errors.Add(Keywords.minimum_download + " NOT PROVIDED");
                            }

                        }

                        if (line.StartsWith(Keywords.minimum_upload))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith(Keywords.maximum_upload))
                        {
                            String[] data = line.Split('=');
                            double value;
                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));



                            bool rangeCheck = Limits.TryGetValue(Keywords.minimum_upload, out value);

                            if (rangeCheck)
                            {
                                if (value > double.Parse(data[1]))
                                {
                                    Errors.Add(Keywords.maximum_upload + " is less than " + " MINIMUM-UPLOAD");
                                }
                            }
                            else
                            {
                                Errors.Add(Keywords.minimum_upload + " NOT PROVIDED");
                            }

                        }

                   
                    }
                }
            }
            sr.Close();
        }

        public void GetProgram()
        {
            StreamReader sr = new StreamReader(cffFilename);

            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                line = line.Trim();
                if (line.StartsWith(Keywords.program))
                {
                    Program = new Dictionary<String, Double>();
                   
                    while (!line.StartsWith(Keywords.program_end))
                    {
                        line = sr.ReadLine();
                        line = line.Trim();

                        if (line.StartsWith(Keywords.duration))
                        {
                            String[] data = line.Split('=');

                            Program.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith(Keywords.tasks))
                        {
                            String[] data = line.Split('=');
                            double minimumTask;
                            double maximumTask;

                            Program.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                            bool minimum= Limits.TryGetValue(Keywords.minimum_tasks, out minimumTask);

                            bool maximum = Limits.TryGetValue(Keywords.maximum_tasks, out maximumTask);

                            if (minimum && maximum)
                            {
                                if (double.Parse(data[1]) < minimumTask || double.Parse(data[1]) > maximumTask)
                                {
                                    Errors.Add("PROGRAM TASKS is not in Range");
                                }
                            }


                        }

                        if (line.StartsWith(Keywords.processors))
                        {
                            String[] data = line.Split('=');
                            double minimumProcessor;
                            double maximumProcessor;

                            Program.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                            bool minimum = Limits.TryGetValue(Keywords.minimum_processors, out minimumProcessor);

                            bool maximum = Limits.TryGetValue(Keywords.maximum_processors, out maximumProcessor);

                            if (minimum && maximum)
                            {
                                if (double.Parse(data[1]) < minimumProcessor || double.Parse(data[1]) > maximumProcessor)
                                {
                                    Errors.Add("PROGRAM PROCESSORS is not in Range");
                                }
                            }

                        }
                    }
                }
            }
            sr.Close();
        }


       


    }
}
