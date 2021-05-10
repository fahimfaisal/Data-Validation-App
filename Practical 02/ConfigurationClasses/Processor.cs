using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataValidation.ConfigurationClasses
{
    public class Processor
    {
       public int ID { get; set; }
       public ProcessorType Type { get; set; }
        public Double Frequency { get; set; }
        public int Ram { get; set; }
        public int Download { get; set; }
        
       public int Upload { get; set; }

        

        public Processor()
        {

        }
        public Processor(int Id, ProcessorType type, Double frequency, int ram, int download, int upload)
        {
            this.ID = Id;
            this.Type = type;
            this.Frequency = frequency;
            this.Ram = ram;
            this.Download = download;
            this.Upload = upload;
        }


        /// <summary>
        /// Method to calculate runtime based on the task and processor
        /// </summary>
        /// <param name="task"> The task asscociated with the processor</param>
        /// <returns>the calculated time</returns>
       
        public double CalcTime(Task task)
        {

            double val = task.Runtime * (task.ReferenceFrequency / Frequency);

            return val;

            
        }

        /// <summary>
        /// Method to calculate Energy based on the task and processor
        /// </summary>
        /// <param name="task"> The task asscociated with the processor</param>
        /// <returns>the calculated energy</returns>

        public double CalcEnergy(Task task)
        {
            double val = CalcTime(task) * (Type.C2 * Frequency * Frequency + Type.C1 * Frequency + Type.C0);

            return val;
        }


        /// <summary>
        /// Method to Check if ram requirement of the task is higher
        /// </summary>
        /// <param name="task"> The task asscociated with the processor</param>
        /// <returns>true if ram requriement is higher</returns>

        public bool CheckRam(Task task)
        {
            return Ram >= task.Ram ? true : false;
   
        }

        /// <summary>
        /// Method to Check if download speed requirement of the task is higher
        /// </summary>
        /// <param name="task"> The task asscociated with the processor</param>
        /// <returns>true if upload requriement is higher</returns>


        public bool CheckDownloadSpeed(Task task)
        {
            return Download >= task.Download ? true : false;
        }

        /// <summary>
        /// Method to Check if upload speed requirement of the task is higher
        /// </summary>
        /// <param name="task"> The task asscociated with the processor</param>
        /// <returns>true if download requriement is higher</returns>

        public bool CheckUploadSpeed(Task task)
        {
            return Upload >= task.Upload ? true : false;
        }


    }
}