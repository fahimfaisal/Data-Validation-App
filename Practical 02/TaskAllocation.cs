using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataValidation.TaskAllocationClasses;

namespace DataValidation
{
    class TaskAllocation
    {
        public string cffFilename { get; set; }

        List<string> Errors { get; set; }

        public Allocations allocations { get; set; }


        
        
        
        
        /// <summary>
        /// Method to get cff filename
        /// </summary>
        /// <param name="taffFilename">The path of the taff file</param>
        /// <returns>boolean value if cff file name found : true</returns>
        
        
        public Boolean getCffFilename(string taffFilename)
        {
            cffFilename = null;
            bool valid = false;
            
            //open the TAff file and extract the cff

            StreamReader sr = new StreamReader(taffFilename);

            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                line = line.Trim();

                if (line.StartsWith(Keywords.filename))
                {
                    String[] data = line.Split('=');

                    if (data.Length == 2)
                    {
                        if (data[1].StartsWith("\"") && data[1].EndsWith("\""))
                        {
                            line = data[1].Trim('"');

                            if (line.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
                            {
                                String path = Path.GetDirectoryName(taffFilename);
                                cffFilename = path + Path.DirectorySeparatorChar +line;
                                valid = true;
                            }
                          
                        }
                        
                    }
                   
                }
            }
            sr.Close();

            return valid;

        }



        /// <summary>
        /// Method to create an Allocation object by reading the taff file
        /// </summary>
        /// <param name="taffFilename"> path of taff file</param>
     

        public void GetAllocations(String taffFilename)
        {
            StreamReader sr = new StreamReader(taffFilename);

            while (!sr.EndOfStream)
            {


                String line = sr.ReadLine();

                line = line.Trim();

             
                if (line.StartsWith(Keywords.allocations_start))
                {

                    allocations = new Allocations();

                    line = sr.ReadLine();
                    line = line.Trim();

                    while (!line.StartsWith(Keywords.allocations_end))
                    {

                        line = sr.ReadLine();
                        line = line.Trim();

                        if (line.StartsWith(Keywords.count))
                        {
                            String[] data = line.Split('=');

                            allocations.Count = int.Parse(data[1]);
                        }

                        if (line.StartsWith(Keywords.tasks))
                        {
                            String[] data = line.Split('=');

                            allocations.Taks = int.Parse(data[1]);
                        }

                        if (line.StartsWith(Keywords.processors))
                        {
                            String[] data = line.Split('=');

                            allocations.Processor = int.Parse(data[1]);
                        }

                        if (line.StartsWith(Keywords.allocation_start))
                        {
                            Allocation allocation = new Allocation();

                            while (!line.StartsWith(Keywords.allocation_end))
                            {
                                line = sr.ReadLine();
                                line = line.Trim();

                                if (line.StartsWith("ID"))
                                {


                                    String[] data = line.Split('=');

                                    allocation.ID = int.Parse(data[1]);

                                }

                                if (line.StartsWith(Keywords.map))
                                {
                                    String[] data = line.Split('=');

                                    String[] processors = data[1].Split(';');

                                    String[] tasks = processors[0].Split(',');
                                
                                    allocation.Map = new int[processors.Length, tasks.Length];


                                    try
                                    {
                                        for (int i = 0; i < allocation.Map.GetLength(0); i++)
                                        {
                                            String[] number = processors[i].Split(',');


                                            for (int j = 0; j < allocation.Map.GetLength(1); j++)
                                            {

                                                allocation.Map[i, j] = int.Parse(number[j]);

                                              


                                            }
                                        }

                                    }
                                    catch (Exception)
                                    {

                                        allocation.Map = null;
                                    }


                                }


                            }

                            if (allocation.Map != null)
                            {

                                List<int[]> activeTasks = new List<int[]>();
                                int[] active;

                                for (int i = 0; i < allocation.Map.GetLength(0); i++)
                                {
                                    
                                    for (int j = 0; j < allocation.Map.GetLength(1); j++)
                                    {
                                        
                                        if (allocation.Map[i,j].Equals(1))
                                        {
                                            active = new int[]{ i, j };
                                            activeTasks.Add(active);
                                        }

                                      
                                    }

                                    
                                }
                                var activeTasksArray = activeTasks.ToArray();

                                foreach (int[] element in activeTasksArray)
                                {
                                    for (int i = 0; i < activeTasksArray.Length; i++)
                                    {
                                        if (element[1] == activeTasksArray[i][1] && element[0] != activeTasksArray[i][0])
                                        {
                                            allocation.Map = new int[1,1];

                                        }
                                    }

                                }
                            }

                            allocations.allocations.Add(allocation);

                        }

                    }

                }
            }
        }          
        
    }





    
}
