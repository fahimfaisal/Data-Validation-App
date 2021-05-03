
using System;
using System.Collections.Generic;
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

        public double[] TimeValidation(TaskAllocation TaffFile, Configuration CffFile)
        {

            
            double[] times = new double[0];
            double time;
            Processor[] sortedProcessor = CffFile.Processors.OrderBy(x => x.ID).ToArray();
            
            Task[] sortedtasks = CffFile.Tasks.OrderBy(x=> x.ID).ToArray();

            Allocation[] sortedAllocation = TaffFile.allocations.allocations.OrderBy(x => x.ID).ToArray();


            double[] allocations = new double[sortedAllocation.Length];

            int all = 0;

            foreach (Allocation allocation in sortedAllocation)
            {
                
                times = new double[allocation.map.GetLength(0)];
                
                for (int  i = 0;  i < allocation.map.GetLength(0);  i++)
                {
                    time = 0d;

                    for (int j = 0; j < allocation.map.GetLength(1); j++)
                    {
                        if (allocation.map[i,j].Equals(1))
                        {

                             
                                 time += sortedProcessor[i].CalcTime(sortedtasks[j]);

                            

                        }

                        

                    }

                    times[i] = time;
                    
                }

                Array.Sort(times);


                allocations[all] = times[times.Length - 1];

                all++;

            }


            return allocations;
           
        }

    }
}
