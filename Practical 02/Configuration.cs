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
        List<string> Errors { get; set; }

        public List<Processor> Processors { get; set; }

        public List<Task> Tasks { get; set; }
        
       
        public List<ProcessorType> processorTypes;

        public  double[,] LocalCommunication {get;set;}


        public double[,] RemoteCommunication { get; set; }

        IDictionary<String, Double> Limits { get; set; }

        IDictionary<String, Double> Program { get; set; }



        
        
        public Configuration(String cffFilename)
        {
            this.cffFilename = cffFilename;
        }

        
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
                else if (line.StartsWith("DEFAULT"))
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


        public void GetTasks()
        {

          
            
            
            StreamReader sr = new StreamReader(cffFilename);

            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                line = line.Trim();
                
                if (line.StartsWith("TASKS"))
                {
                     Tasks = new List<Task>();

                    while (!line.StartsWith("END-TASKS"))
                    {
                        line = sr.ReadLine();
                        line = line.Trim();

                        if (line.StartsWith("TASK"))
                        {
                            Task task = new Task();

                            while (!line.StartsWith("END-TASK"))
                            {
                                line = sr.ReadLine();
                                line = line.Trim();

                                if (line.StartsWith("ID"))
                                {
                                    String[] data = line.Split('=');

                                    task.ID = int.Parse(data[1]);

                                }

                                if (line.StartsWith("RUNTIME"))
                                {
                                    String[] data = line.Split('=');

                                    task.runtime = double.Parse(data[1]);

                                }

                                if (line.StartsWith("REFERENCE-FREQUENCY"))
                                {
                                    String[] data = line.Split('=');

                                    task.referenceFrequency = double.Parse(data[1]);




                                }

                                if (line.StartsWith("RAM"))
                                {
                                    String[] data = line.Split('=');

                                    task.ram = int.Parse(data[1]);




                                }

                                if (line.StartsWith("DOWNLOAD"))
                                {
                                    String[] data = line.Split('=');

                                    task.download = int.Parse(data[1]);




                                }

                                if (line.StartsWith("UPLOAD"))
                                {
                                    String[] data = line.Split('=');

                                    task.upload = int.Parse(data[1]);




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

                if (line.StartsWith("PROCESSORS"))
                {
                    Processors = new List<Processor>();
                    
                    while (!line.StartsWith("END-PROCESSORS"))
                    {
                        line = sr.ReadLine();
                        line = line.Trim();

                        if (line.StartsWith("PROCESSOR"))
                        {

                            Processor processor = new Processor();

                            while (!line.StartsWith("END-PROCESSOR"))
                            {
                                line = sr.ReadLine();
                                line = line.Trim();

                                if (line.StartsWith("ID"))
                                {
                                    String[] data = line.Split('=');

                                    processor.ID = int.Parse(data[1]);
                                }


                                if (line.StartsWith("TYPE"))
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


                                if (line.StartsWith("FREQUENCY"))
                                {
                                    String[] data = line.Split('=');

                                    processor.Frequency = double.Parse(data[1]);
                                }


                                if (line.StartsWith("RAM"))
                                {
                                    String[] data = line.Split('=');

                                    processor.Ram = int.Parse(data[1]);
                                }

                                if (line.StartsWith("DOWNLOAD"))
                                {
                                    String[] data = line.Split('=');

                                    processor.Download = int.Parse(data[1]);
                                }

                                if (line.StartsWith("UPLOAD"))
                                {
                                    String[] data = line.Split('=');

                                    processor.Upload = int.Parse(data[1]);
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

                if (line.StartsWith("PROCESSOR-TYPES"))
                {
                    processorTypes = new List<ProcessorType>();
                    
                    while (!line.StartsWith("END-PROCESSOR-TYPES"))
                    {
                        line = sr.ReadLine();
                        line = line.Trim();

                        if (line.StartsWith("PROCESSOR-TYPE"))
                        {
                            ProcessorType processorType = new ProcessorType();

                            while (!line.StartsWith("END-PROCESSOR-TYPE"))
                            {
                                line = sr.ReadLine();
                                line = line.Trim();

                                if (line.StartsWith("NAME"))
                                {
                                    String[] data = line.Split('=');
                                    String name = data[1].Trim().Trim('"');
                                    processorType.Name = name;
                                 
                                }

                                if (line.StartsWith("C2"))
                                {
                                    String[] data = line.Split('=');
                                    processorType.C2 = double.Parse(data[1]);
                                }
                                if (line.StartsWith("C1"))
                                {
                                    String[] data = line.Split('=');
                                    processorType.C1 = double.Parse(data[1]);
                                }
                                if (line.StartsWith("C0"))
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

                if (line.StartsWith("LOCAL-COMMUNICATION"))
                {
                    while(!line.StartsWith("END-LOCAL-COMMUNICATION"))
                    {
                        line = sr.ReadLine();
                        line = line.Trim();

                        if (line.StartsWith("MAP"))
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
                                    LocalCommunication[i, j] = double.Parse(number[j]);
                                }

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

                if (line.StartsWith("REMOTE-COMMUNICATION"))
                {
                    while (!line.StartsWith("END-REMOTE-COMMUNICATION"))
                    {
                        line = sr.ReadLine();
                        line = line.Trim();

                        if (line.StartsWith("MAP"))
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
                                    RemoteCommunication[i, j] = double.Parse(number[j]);
                                }

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
                if (line.StartsWith("LIMITS"))
                {
                    Limits = new Dictionary<String, Double>();
                    while(!line.StartsWith("END-LIMITS"))
                    {
                        line = sr.ReadLine();
                        line = line.Trim();

                        if (line.StartsWith("MINIMUM-TASKS"))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith("MAXIMUM-TASKS"))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith("MINIMUM-PROCESSORS"))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith("MAXIMUM-PROCESSORS"))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }


                        if (line.StartsWith("MINIMUM-PROCESSOR-FREQUENCIES"))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith("MAXIMUM-PROCESSOR-FREQUENCIES"))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith("MINIMUM-RAM"))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith("MAXIMUM-RAM"))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith("MINIMUM-DOWNLOAD"))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith("MAXIMUM-DOWNLOAD"))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith("MINIMUM-UPLOAD"))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith("MAXIMUM-UPLOAD"))
                        {
                            String[] data = line.Split('=');

                            Limits.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

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
                if (line.StartsWith("PROGRAM"))
                {
                    Program = new Dictionary<String, Double>();
                   
                    while (!line.StartsWith("END-PROGRAM"))
                    {
                        line = sr.ReadLine();
                        line = line.Trim();

                        if (line.StartsWith("DURATION"))
                        {
                            String[] data = line.Split('=');

                            Program.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith("TASKS"))
                        {
                            String[] data = line.Split('=');

                            Program.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }

                        if (line.StartsWith("PROCESSORS"))
                        {
                            String[] data = line.Split('=');

                            Program.Add(new KeyValuePair<String, Double>(data[0], double.Parse(data[1])));

                        }
                    }
                }
            }
            sr.Close();
        }


       


    }
}
