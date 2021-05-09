
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using DataValidation.ConfigurationClasses;
using DataValidation.TaskAllocationClasses;


namespace DataValidation
{
    class Validator
    {
        //TaskAllocation TaffFile = new TaskAllocation();

        //Configuration CffFile;
        
        

        public Validator()
        {

        }

        //public void SetTaff()
        //{
        //    CffFile = new Configuration(TaffFile.cffFilename);
        //}



        public bool ValidateTaffFormat(String TaffFile, out List<String> Errors, out String line)
        {
            Errors = new List<string>();
            StreamReader sr = new StreamReader(TaffFile);
            line = sr.ReadLine();
            line = line.Trim();

            while (!sr.EndOfStream)
            {
                if (line.StartsWith("//"))
                {

                }
                else if (line.StartsWith("CONFIGURATION-DATA"))
                {
                    String[] data = line.Split('=');

              
                }

                

            }
            return true;


        }



        public string ValidationAllocations(TaskAllocation TaffFile, Configuration CffFile)
        {

            
            double progRuntime = 0;
           
            CffFile.Program.TryGetValue("DURATION", out progRuntime);
            string runTime;
            string[] limits;

            IDictionary<Allocation, double[]> allocations = new Dictionary<Allocation, double[]>();

            String print = "";

            foreach (Allocation allocation in TaffFile.allocations.allocations)
            {
                List<string> errors;
                string line = "";

                if (allocation.map == null)
                {
                    print += "<p style='color:red'>"+"INVALID MAPPING"+"</p>";
                }
                else if(allocation.map.Length == 1)
                {
                    
                    print += "<p style='color:red'>" + "SAME TASK HAS BEEN ASSIGNED TO MULTIPLE PROCESSOR FOR ALLOCATION ID: " + allocation.ID +"</p>";
                }
                else
                {
                    KeyValuePair<Allocation, double[]> allocationValue = validateAllocation(allocation, CffFile, out limits, out errors);



                    if (allocationValue.Value[int.Parse(Keywords.TOTALTIME)] > progRuntime)
                    {
                        runTime = "<span style='color:red'>" + String.Format("{0:0.##}", allocationValue.Value[int.Parse(Keywords.TOTALTIME)]) + "</span>";
                        errors.Add("THE ALLOCATION ID: " + allocation.ID + " HAS GREATER RUNTIME THAN PROGRAM RUNTIME");
                    }
                    else
                    {
                        runTime = String.Format("{0:0.##}", allocationValue.Value[int.Parse(Keywords.TOTALTIME)]);
                    }

                    if (errors.Count > 0)
                    {
                       
                        foreach (string error in errors)
                        {
                            line += "<p style='color:red'>" + error + "</p>";
                        }
                    }



                    print += "ALLOCATION ID" + "=" + allocationValue.Key.ID.ToString() + "," + "&nbsp" + "Runtime" + "=" + runTime + "," + "&nbsp" + "Energy" + "=" + String.Format("{0:0.##}", allocationValue.Value[int.Parse(Keywords.TOTALENERGY)]) + "<br>" + PrintAllocationUnits(printAllocationMap(allocation), limits) + "<br><br>" + line + "<br><br>";
                }

             
            }

            return print;


        }

        public KeyValuePair<Allocation, double[]> validateAllocation(Allocation allocation, Configuration config, out String[] limit ,out List<string> errors)
        {

           


            errors = new List<string>();
            Processor[] sortedProcessor = config.Processors.OrderBy(x => x.ID).ToArray();

            Task[] sortedTasks = config.Tasks.OrderBy(x => x.ID).ToArray();

            List<int[]> activeTasks = new List<int[]>();
            double m = 0d;
            double[] times = new double[allocation.map.GetLength(0)];
            double[] energies = new double[allocation.map.GetLength(0)];
            limit = new string[allocation.map.GetLength(0)];
           
            int[] active;

            for (int i = 0; i < allocation.map.GetLength(0); i++)
            {
                double time = 0d;
                double energy = 0d;


                int ram = 0;
                int upload = 0;
                int dowload = 0;

                for (int j = 0; j < allocation.map.GetLength(1); j++)
                {
                    


                    if (allocation.map[i, j].Equals(1))
                    {
                        active = new int[] { i, j };

                     
                        
                        ram = ram > sortedTasks[j].ram ? ram : sortedTasks[j].ram;


                        

                      
                        
                        dowload = dowload > sortedTasks[j].download ? dowload : sortedTasks[j].download;

                        

                      
                        
                        upload = upload > sortedTasks[j].upload ? upload : sortedTasks[j].upload;


                        if (!sortedProcessor[i].CheckRam(sortedTasks[j]))
                        {
                            errors.Add("THE PROCESSOR ID: " + sortedProcessor[i].ID + " OF ALLOCATION ID: " +allocation.ID+ " HAS " + sortedProcessor[i].Ram +" GB RAM BUT REQUIRES " + sortedTasks[j].ram+" GB RAM");
                        }

                        if (!sortedProcessor[i].CheckDownloadSpeed(sortedTasks[j]))
                        {
                            errors.Add("THE PROCESSOR ID " + sortedProcessor[i].ID + " OF ALLOCATION ID: " + allocation.ID + " HAS " + sortedProcessor[i].Download + " Gbps DOWNLOAD SPEED BUT REQUIRES " + sortedTasks[j].download + " Gbps Download speed");
                        }

                        if (!sortedProcessor[i].CheckUploadSpeed(sortedTasks[j]))
                        {
                            errors.Add("THE PROCESSOR ID: " + sortedProcessor[i].ID + " OF ALLOCATION ID: " + allocation.ID + " HAS " + sortedProcessor[i].Upload + " Gbps UPLOAD SPEED BUT REQUIRES " + sortedTasks[j].upload+ " Gbps UPLOAD speed");
                        }

                        time += sortedProcessor[i].CalcTime(sortedTasks[j]);

                        energy += sortedProcessor[i].CalcEnergy(sortedTasks[j]);


                        activeTasks.Add(active);
                    }


                }

             
                
                times[i] = time;
                energies[i] = energy;


                String Ram = (ram > sortedProcessor[i].Ram ? "<span style='color:red'>"+ ram +"</span>": ram.ToString() ) + "/" + sortedProcessor[i].Ram + "&nbsp GB";
                String Upload = (upload > sortedProcessor[i].Upload ? "<span style='color:red'>" + upload + "</span>" : upload.ToString()) + "/" + sortedProcessor[i].Upload + "&nbsp Gbps";
                String Download = (dowload > sortedProcessor[i].Download ? "<span style='color:red'>" + dowload + "</span>" : dowload.ToString()) + "/" + sortedProcessor[i].Download  + "&nbsp Gbps";

                limit[i] = Ram + " &nbsp&nbsp" + Upload + "&nbsp&nbsp" + Download;


            }


            var activeTasksArray = activeTasks.ToArray();

            foreach (int[] element in activeTasksArray)
            {
                for (int i = 0; i < activeTasksArray.Length; i++)
                {
                    if (element[0] == activeTasksArray[i][0])
                    {
                       m += config.LocalCommunication[element[1], activeTasksArray[i][1]];
                        
                    }
                    else
                    {
                        m += config.RemoteCommunication[element[1], activeTasksArray[i][1]];
                    }
                }

            }



            Array.Sort(times);

            double maxTime = times[times.Length - 1];
            double totalEnergy = energies.Sum() + m;

            double[] val = new double[] { maxTime, totalEnergy };

            return new KeyValuePair<Allocation, double[]>(allocation, val);
        }

        public IDictionary<Allocation, double[]> valid(TaskAllocation TaffFile, Configuration CffFile)
        {

            double energy;
            double[] times;
            double time;
            double[] energies;

            Processor[] sortedProcessor = CffFile.Processors.OrderBy(x => x.ID).ToArray();

            Task[] sortedtasks = CffFile.Tasks.OrderBy(x => x.ID).ToArray();



            IDictionary<Allocation, double[]> allocations = new Dictionary<Allocation, double[]>();



            foreach (Allocation allocation in TaffFile.allocations.allocations)
            {

                times = new double[allocation.map.GetLength(0)];
                energies = new double[allocation.map.GetLength(0)];

                for (int i = 0; i < allocation.map.GetLength(0); i++)
                {
                    time = 0d;

                    energy = 0d;

                    for (int j = 0; j < allocation.map.GetLength(1); j++)
                    {
                        if (allocation.map[i, j].Equals(1))
                        {


                            time += sortedProcessor[i].CalcTime(sortedtasks[j]);

                            energy += sortedProcessor[i].CalcEnergy(sortedtasks[j]);

                        }



                    }


                    times[i] = time;
                    energies[i] = energy;

                }

                Array.Sort(times);
                double totalEnergy = energies.Sum();
                double[] val = new double[] { times[times.Length - 1], totalEnergy };

                allocations.Add(new KeyValuePair<Allocation, double[]>(allocation, val));


            }


            return allocations;

        }

        public string[] printAllocationMap(Allocation allocation)
        {
            string map = "";
            string[] maps = new string[allocation.map.GetLength(0)];
            for (int i = 0; i < allocation.map.GetLength(0); i++)
            {

                map = "";
                for (int j = 0; j < allocation.map.GetLength(1); j++)
                {

                    map += allocation.map[i, j] + " ";

                }



                maps[i] = map;

            }

            return maps;
        }
        public string PrintAllocationUnits(String[] map, string[] limit)
        {
            string line = "";

            for (int i = 0; i < map.Length; i++)
            {
                line += map[i] + "&nbsp; &nbsp;  &nbsp;  &nbsp;  &nbsp;" + limit[i]+"<br>";
            }

            return line;
        }

      

    }
}
