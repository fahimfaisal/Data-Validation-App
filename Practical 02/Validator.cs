
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

        public IDictionary<Allocation,double[]> TimeValidation(TaskAllocation TaffFile, Configuration CffFile)
        {

            double energy;
            double[] times;
            double time;
            double[] energies;
           

            Processor[] sortedProcessor = CffFile.Processors.OrderBy(x => x.ID).ToArray();
            
            Task[] sortedtasks = CffFile.Tasks.OrderBy(x=> x.ID).ToArray();

          



            IDictionary<Allocation, double[]> allocations= new Dictionary<Allocation, double[]>();

            

            foreach (Allocation allocation in TaffFile.allocations.allocations)
            {
                
                times = new double[allocation.map.GetLength(0)];
                energies = new double[allocation.map.GetLength(0)];
                
                for (int  i = 0;  i < allocation.map.GetLength(0);  i++)
                {
                    time = 0d;
                    
                    energy = 0d;

                    for (int j = 0; j < allocation.map.GetLength(1); j++)
                    {
                        if (allocation.map[i,j].Equals(1))
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
                
                allocations.Add(new KeyValuePair<Allocation,double[]>(allocation,val));
               

            }


            return allocations;
           
        }



       
    }
}
