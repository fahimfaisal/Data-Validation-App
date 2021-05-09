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

                if (line.StartsWith(Keywords.config))
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
     

        public void GetAllocations(String taffFilename)
        {
            StreamReader sr = new StreamReader(taffFilename);

            while (!sr.EndOfStream)
            {


                String line = sr.ReadLine();

                line = line.Trim();

             
                if (line.StartsWith("ALLOCATIONS"))
                {

                    allocations = new Allocations();

                    line = sr.ReadLine();
                    line = line.Trim();

                    while (!line.StartsWith("END-ALLOCATIONS"))
                    {

                        line = sr.ReadLine();
                        line = line.Trim();

                        if (line.StartsWith("COUNT"))
                        {
                            String[] data = line.Split('=');

                            allocations.count = int.Parse(data[1]);
                        }

                        if (line.StartsWith("TASKS"))
                        {
                            String[] data = line.Split('=');

                            allocations.taks = int.Parse(data[1]);
                        }

                        if (line.StartsWith("PROCESSORS"))
                        {
                            String[] data = line.Split('=');

                            allocations.processor = int.Parse(data[1]);
                        }

                        if (line.StartsWith("ALLOCATION"))
                        {
                            Allocation allocation = new Allocation();

                            while (!line.StartsWith("END-ALLOCATION"))
                            {
                                line = sr.ReadLine();
                                line = line.Trim();

                                if (line.StartsWith("ID"))
                                {


                                    String[] data = line.Split('=');

                                    allocation.ID = int.Parse(data[1]);

                                }

                                if (line.StartsWith("MAP"))
                                {
                                    String[] data = line.Split('=');

                                    String[] processors = data[1].Split(';');

                                    String[] tasks = processors[0].Split(',');

                                   

                                    allocation.map = new int[processors.Length, tasks.Length];



                                    try
                                    {
                                        for (int i = 0; i < allocation.map.GetLength(0); i++)
                                        {
                                            String[] number = processors[i].Split(',');


                                            for (int j = 0; j < allocation.map.GetLength(1); j++)
                                            {

                                                allocation.map[i, j] = int.Parse(number[j]);

                                              


                                            }
                                        }

                                    }
                                    catch (Exception)
                                    {

                                        allocation.map = null;
                                    }


                                }


                            }

                            if (allocation.map != null)
                            {

                                List<int[]> activeTasks = new List<int[]>();
                                int[] active;

                                for (int i = 0; i < allocation.map.GetLength(0); i++)
                                {
                                    

                                    for (int j = 0; j < allocation.map.GetLength(1); j++)
                                    {
                                        
                                        if (allocation.map[i,j].Equals(1))
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
                                            allocation.map = new int[1,1];

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
